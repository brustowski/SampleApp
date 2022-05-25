-- create string attribute2 column
ALTER TABLE Pipeline_InvoiceLines
  ADD Attribute_2_s VARCHAR(128) NULL
GO
-- update new column value
UPDATE Pipeline_InvoiceLines
  SET Attribute_2_s = 'API @ 60° F = ' + FORMAT(Attribute_2, '0.######')
GO

-- replace columns
EXEC sp_rename 'Pipeline_InvoiceLines.Attribute_2', 'Attribute_2_d', 'COLUMN';
EXEC sp_rename 'Pipeline_InvoiceLines.Attribute_2_s', 'Attribute_2', 'COLUMN';

-- remove previous column
ALTER TABLE Pipeline_InvoiceLines
	drop column Attribute_2_d
GO

-- Add Pipeline Invoice Line record --
ALTER PROCEDURE dbo.pipeline_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  -- Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- get current tariff ---------------
  DECLARE @tariffs TABLE (
    pi INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (pi
  , tariff)
    SELECT
      details.Pipeline_Inbounds_FK
     ,TariffId =
      CASE
        WHEN pi.API < 25 THEN 1
        WHEN pi.API >= 25 THEN 2
      END
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound pi
      ON details.Pipeline_Inbounds_FK = pi.id
    WHERE details.Filing_Headers_FK = @filingHeaderId

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_InvoiceLines (Filing_Headers_FK
    , InvoiceHeaders_FK
    , PI_FK
    , Invoice_No
    , Transaction_Related
    , tariff
    , Customs_QTY
    , Goods_Description
    , spi
    , Gr_Weight
    , PriceUnit
    , Attribute_1
    , Attribute_2
    , Attribute_3
    , Invoice_Qty
    , ORG
    , Line_Price
    , Amount
    , Manufacturer
    , Consignee
    , Sold_to_party
    , Origin
    , Export
    , Dest_State)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,details.Pipeline_Inbounds_FK
       ,inbound.Batch -- ?? is it ok?
       ,ruleImporters.Transaction_Related
       ,ruleAPI.Tariff
       ,inbound.Quantity
       ,CONCAT(ruleBatch.product, ' - ', ruleBatch.batch_code)
       ,ruleImporters.SPI
       ,CONVERT(DECIMAL(18, 3), dbo.fn_pipeline_weight(inbound.Quantity, inbound.API))
       ,COALESCE(rulePriceExact.pricing, rulePrice.pricing)
       ,inbound.Batch
       ,'API @ 60° F = ' + FORMAT(inbound.API, '0.######')
       ,CONCAT(ruleFacility.pipeline, ' P/L')
       ,inbound.Quantity
       ,ruleImporters.origin
       ,inbound.Quantity * COALESCE(rulePriceExact.pricing, rulePrice.pricing)
       ,inbound.Quantity * COALESCE(rulePriceExact.freight, rulePrice.freight)
       ,ruleImporters.manufacturer
       ,ruleImporters.Consignee
       ,ruleImporters.Consignee
       ,ruleImporters.origin
       ,ruleImporters.country_of_export
       ,ruleFacility.Destination_State
      FROM dbo.Pipeline_Filing_Details details
      INNER JOIN dbo.Pipeline_Inbound inbound
        ON inbound.id = details.Pipeline_Inbounds_FK
      LEFT JOIN Pipeline_Rule_Facility ruleFacility
        ON inbound.facility = ruleFacility.facility
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
      LEFT JOIN Clients clients
        ON clients.ClientCode = @ImporterCode
      LEFT JOIN Pipeline_Rule_Price rulePrice
        ON clients.id = rulePrice.importer_id
          AND rulePrice.crude_type_id IS NULL
      LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
        ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
      LEFT JOIN Pipeline_Rule_Price rulePriceExact
        ON clients.id = rulePriceExact.importer_id
          AND ruleBatch.id = rulePriceExact.crude_type_id
      LEFT JOIN @tariffs t
        ON inbound.id = t.pi
      LEFT JOIN Pipeline_Rule_API ruleAPI
        ON t.Tariff = ruleAPI.id
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO

--- Pipeline post save action ---
ALTER PROCEDURE dbo.pipeline_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @importerCode VARCHAR(128) = dbo.fn_pipeline_GetImporterCode(@filingHeaderId);
  DECLARE @api DECIMAL(18, 6)
         ,@freight DECIMAL(18, 6)

  SELECT
    @api = inbound.API
   ,@freight = COALESCE(rulePriceExact.freight, rulePrice.freight)

  FROM dbo.Pipeline_Filing_Details details
  INNER JOIN dbo.Pipeline_Inbound inbound
    ON inbound.id = details.Pipeline_Inbounds_FK
  LEFT JOIN Clients clients
    ON clients.ClientCode = @ImporterCode
  LEFT JOIN Pipeline_Rule_Price rulePrice
    ON clients.id = rulePrice.importer_id
      AND rulePrice.crude_type_id IS NULL
  LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
    ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
  LEFT JOIN Pipeline_Rule_Price rulePriceExact
    ON clients.id = rulePriceExact.importer_id
      AND ruleBatch.id = rulePriceExact.crude_type_id
  WHERE details.Filing_Headers_FK = @filingHeaderId

  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
  )

  INSERT INTO @tbl (record_id
  , parent_record_id
  , filing_header_id
  , quantity
  , unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,a.Filing_Headers_FK
     ,CONVERT(DECIMAL(18, 6), a.value) AS Quantity
     ,CONVERT(DECIMAL(28, 15), b.value) AS UnitPrice
    FROM Pipeline_DEFValues_Manual a
    JOIN Pipeline_DEFValues_Manual b
      ON a.record_id = b.record_id
    WHERE a.Filing_Headers_FK = @filingHeaderId
    AND b.Filing_Headers_FK = @filingHeaderId
    AND a.table_name = 'Pipeline_InvoiceLines'
    AND a.column_name = 'Invoice_Qty'
    AND b.column_name = 'PriceUnit'

  DECLARE @total DECIMAL(28, 15)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  -- update invoice customs qty
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Customs_QTY'
  -- update invoice line price
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Line_Price'
  -- update invoice line amount
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * @freight, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Amount'
  -- update invoice line gross weight
  UPDATE defValues
  SET defValues.value = FORMAT(dbo.fn_pipeline_weight(t.Quantity, @api), '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Gr_Weight'

  -- update invoice header invoice total
  UPDATE defValues
  SET value = FORMAT(@total, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl
    ON defValues.record_id = [@tbl].parent_record_id
  WHERE defValues.Filing_Headers_FK = @filingHeaderId
  AND table_name = 'Pipeline_InvoiceHeaders'
  AND column_name = 'Invoice_Total'
END
GO

ALTER VIEW dbo.Pipeline_Report
AS
SELECT
  header.id AS Pipeline_Filing_Headers_id
 ,details.Pipeline_Inbounds_FK AS PI_ID
 ,declaration.Arr AS Pipeline_DeclarationTab_Arr
 ,declaration.Arr_2 AS Pipeline_DeclarationTab_Arr_2
 ,declaration.Carrier_SCAC AS Pipeline_DeclarationTab_Carrier_SCAC
 ,declaration.Centralized_Exam_Site AS Pipeline_DeclarationTab_Centralized_Exam_Site
 ,declaration.Certify_Cargo_Release AS Pipeline_DeclarationTab_Certify_Cargo_Release
 ,declaration.Check_Local_Client AS Pipeline_DeclarationTab_Check_Local_Client
 ,declaration.Container AS Pipeline_DeclarationTab_Container
 ,declaration.Country_of_Export AS Pipeline_DeclarationTab_Country_of_Export
 ,declaration.Dep AS Pipeline_DeclarationTab_Dep
 ,declaration.Description AS Pipeline_DeclarationTab_Description
 ,declaration.Destination AS Pipeline_DeclarationTab_Destination
 ,declaration.Destination_State AS Pipeline_DeclarationTab_Destination_State
 ,declaration.Discharge AS Pipeline_DeclarationTab_Discharge
 ,declaration.Enable_Entry_Sum AS Pipeline_DeclarationTab_Enable_Entry_Sum
 ,declaration.Entry_Port AS Pipeline_DeclarationTab_Entry_Port
 ,declaration.Entry_Type AS Pipeline_DeclarationTab_Entry_Type
 ,declaration.ETA AS Pipeline_DeclarationTab_ETA
 ,declaration.Export_Date AS Pipeline_DeclarationTab_Export_Date
 ,declaration.FIRMs_Code AS Pipeline_DeclarationTab_FIRMs_Code
 ,declaration.HMF AS Pipeline_DeclarationTab_HMF
 ,declaration.Importer AS Pipeline_DeclarationTab_Importer
 ,declaration.Importer_of_record AS Pipeline_DeclarationTab_Importer_of_record
 ,declaration.INCO AS Pipeline_DeclarationTab_INCO
 ,declaration.Issuer AS Pipeline_DeclarationTab_Issuer
 ,declaration.Main_Supplier AS Pipeline_DeclarationTab_Main_Supplier
 ,declaration.Manual_Entry AS Pipeline_DeclarationTab_Manual_Entry
 ,declaration.Master_Bill AS Pipeline_DeclarationTab_Master_Bill
 ,declaration.No_Packages AS Pipeline_DeclarationTab_No_Packages
 ,declaration.Origin AS Pipeline_DeclarationTab_Origin
 ,declaration.Owner_Ref AS Pipeline_DeclarationTab_Owner_Ref
 ,declaration.Purchased AS Pipeline_DeclarationTab_Purchased
 ,declaration.RLF AS Pipeline_DeclarationTab_RLF
 ,declaration.Service AS Pipeline_DeclarationTab_Service
 ,declaration.Shipment_Type AS Pipeline_DeclarationTab_Shipment_Type
 ,declaration.Split_Shipment_Release AS Pipeline_DeclarationTab_Split_Shipment_Release
 ,declaration.Total_Volume AS Pipeline_DeclarationTab_Total_Volume
 ,declaration.Total_Weight AS Pipeline_DeclarationTab_Total_Weight
 ,declaration.Transport AS Pipeline_DeclarationTab_Transport
 ,declaration.Type AS Pipeline_DeclarationTab_Type
 ,containers.Bill_Issuer_SCAC AS Pipeline_Packing_Bill_Issuer_SCAC
 ,containers.Bill_Number AS Pipeline_Packing_Bill_Number
 ,containers.Bill_Type AS Pipeline_Packing_Bill_Type
 ,containers.Container_Number AS Pipeline_Packing_Container_Number
 ,containers.Is_Split AS Pipeline_Packing_Is_Split
 ,containers.IT_Number AS Pipeline_Packing_IT_Number
 ,containers.Manifest_QTY AS Pipeline_Packing_Manifest_QTY
 ,containers.Marks_and_Numbers AS Pipeline_Packing_Marks_and_Numbers
 ,containers.Pack_QTY AS Pipeline_Packing_Pack_QTY
 ,containers.Pack_Type AS Pipeline_Packing_Pack_Type
 ,containers.Shipping_Symbol AS Pipeline_Packing_Shipping_Symbol
 ,containers.UQ AS Pipeline_Packing_UQ
 ,containers.Packing_UQ AS Pipeline_Packing_Container_Packing_UQ
 ,containers.Seal_Number AS Pipeline_Packing_Seal_Number
 ,containers.Type AS Pipeline_Packing_Type
 ,containers.Mode AS Pipeline_Packing_Mode
 ,containers.Goods_Weight AS Pipeline_Packing_Goods_Weight
 ,containers.Bill_Num AS Pipeline_Packing_Bill_Num
 ,invheaders.Agreed_Place AS Pipeline_InvoiceHeaders_Agreed_Place
 ,invheaders.Broker_PGA_Contact_Email AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.Broker_PGA_Contact_Name AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.Consignee AS Pipeline_InvoiceHeaders_Consignee
 ,invheaders.Consignee_Address AS Pipeline_InvoiceHeaders_Consignee_Address
 ,invheaders.Curr AS Pipeline_InvoiceHeaders_Curr
 ,invheaders.EPA_PST_Cert_Date AS Pipeline_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.EPA_TSCA_Cert_Date AS Pipeline_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.EPA_VNE_Cert_Date AS Pipeline_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.Export AS Pipeline_InvoiceHeaders_Export
 ,invheaders.Export_Date AS Pipeline_InvoiceHeaders_Export_Date
 ,invheaders.First_Sale AS Pipeline_InvoiceHeaders_First_Sale
 ,invheaders.FSIS_Cert_Date AS Pipeline_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.FWS_Cert_Date AS Pipeline_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.Importer AS Pipeline_InvoiceHeaders_Importer
 ,invheaders.INCO AS Pipeline_InvoiceHeaders_INCO
 ,invheaders.Inv_Date AS Pipeline_InvoiceHeaders_Inv_Date
 ,invheaders.Inv_Gross_Weight AS Pipeline_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.Invoice_No AS Pipeline_InvoiceHeaders_Invoice_No
 ,invheaders.Invoice_Total AS Pipeline_InvoiceHeaders_Invoice_Total
 ,invheaders.LACEY_ACT_Cert_Date AS Pipeline_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.Landed_Costing_Ex_Rate AS Pipeline_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.Manufacturer AS Pipeline_InvoiceHeaders_Manufacturer
 ,invheaders.Net_Weight AS Pipeline_InvoiceHeaders_Net_Weight
 ,invheaders.NHTSA_Cert_Date AS Pipeline_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.Origin AS Pipeline_InvoiceHeaders_Origin
 ,invheaders.Packages AS Pipeline_InvoiceHeaders_Packages
 ,invheaders.Payment_Date AS Pipeline_InvoiceHeaders_Payment_Date
 ,invheaders.Seller AS Pipeline_InvoiceHeaders_Seller
 ,invheaders.Ship_to_party AS Pipeline_InvoiceHeaders_Ship_to_party
 ,invheaders.Sold_to_party AS Pipeline_InvoiceHeaders_Sold_to_party
 ,invheaders.Supplier AS Pipeline_InvoiceHeaders_Supplier
 ,invheaders.Supplier_Address AS Pipeline_InvoiceHeaders_Supplier_Address
 ,invheaders.Transaction_Related AS Pipeline_InvoiceHeaders_Transaction_Related
 ,invlines.Attribute_1 AS Pipeline_InvoiceLines_Attribute_1
 ,invlines.Attribute_2 AS Pipeline_InvoiceLines_Attribute_2
 ,invlines.Certifying_Individual AS Pipeline_InvoiceLines_Certifying_Individual
 ,invlines.CIF_Component AS Pipeline_InvoiceLines_CIF_Component
 ,invlines.Code AS Pipeline_InvoiceLines_Code
 ,invlines.Consignee AS Pipeline_InvoiceLines_Consignee
 ,invlines.Curr AS Pipeline_InvoiceLines_Curr
 ,invlines.Customs_QTY AS Pipeline_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Pipeline_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Pipeline_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Pipeline_InvoiceLines_Export
 ,invlines.Goods_Description AS Pipeline_InvoiceLines_Goods_Description
 ,invlines.Gr_Weight AS Pipeline_InvoiceLines_Gr_Weight
 ,invlines.Invoice_No AS Pipeline_InvoiceLines_Invoice_No
 ,invlines.Invoice_Qty AS Pipeline_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Pipeline_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Pipeline_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Pipeline_InvoiceLines_LNO
 ,invlines.Manufacturer AS Pipeline_InvoiceLines_Manufacturer
 ,invlines.ORG AS Pipeline_InvoiceLines_ORG
 ,invlines.Origin AS Pipeline_InvoiceLines_Origin
 ,invlines.PGA_Contact_Email AS Pipeline_InvoiceLines_PGA_Contact_Email
 ,invlines.PGA_Contact_Name AS Pipeline_InvoiceLines_PGA_Contact_Name
 ,invlines.PGA_Contact_Phone AS Pipeline_InvoiceLines_PGA_Contact_Phone
 ,invlines.PriceUnit AS Pipeline_InvoiceLines_PriceUnit
 ,invlines.Prod_ID_1 AS Pipeline_InvoiceLines_Prod_ID_1
 ,invlines.Sold_to_party AS Pipeline_InvoiceLines_Sold_To_Party
 ,invlines.SPI AS Pipeline_InvoiceLines_SPI
 ,invlines.Tariff AS Pipeline_InvoiceLines_Tariff
 ,invlines.Transaction_Related AS Pipeline_InvoiceLines_Transaction_Related
 ,invlines.TSCA_Indicator AS Pipeline_InvoiceLines_TSCA_Indicator
 ,invlines.UQ AS Pipeline_InvoiceLines_UQ
 ,invlines.Amount AS Pipeline_InvoiceLines_Amount
 ,misc.Bond_Type AS Pipeline_MISC_Bond_Type
 ,misc.Branch AS Pipeline_MISC_Branch
 ,misc.Broker AS Pipeline_MISC_Broker
 ,misc.Broker_to_Pay AS Pipeline_MISC_Broker_to_Pay
 ,misc.FTA_Recon AS Pipeline_MISC_FTA_Recon
 ,misc.Merge_By AS Pipeline_MISC_Merge_By
 ,misc.Payment_Type AS Pipeline_MISC_Payment_Type
 ,misc.Prelim_Statement_Date AS Pipeline_MISC_Prelim_Statement_Date
 ,misc.Preparer_Dist_Port AS Pipeline_MISC_Preparer_Dist_Port
 ,misc.Recon_Issue AS Pipeline_MISC_Recon_Issue
 ,misc.Submitter AS Pipeline_MISC_Submitter
 ,misc.Tax_Deferrable_Ind AS Pipeline_MISC_Tax_Deferrable_Ind
 ,declaration.Pipeline AS Pipeline_DeclarationTab_Pipeline
 ,invlines.Attribute_3 AS Pipeline_InvoiceLines_Attribute_3
 ,invheaders.Manufacturer_Address AS Pipeline_InvoiceHeaders_Manufacturer_Address
 ,declaration.EntryNumber AS Pipeline_DeclarationTab_EntryNumber
FROM dbo.Pipeline_Filing_Headers AS header
INNER JOIN dbo.Pipeline_Filing_Details AS details
  ON header.id = details.Filing_Headers_FK
LEFT OUTER JOIN dbo.Pipeline_DeclarationTab AS declaration
  ON declaration.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = declaration.PI_FK
LEFT OUTER JOIN dbo.Pipeline_ContainersTab AS containers
  ON containers.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = containers.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceLines AS invlines
  ON invlines.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = invlines.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceHeaders AS invheaders
  ON invheaders.Filing_Headers_FK = header.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT OUTER JOIN dbo.Pipeline_MISC AS misc
  ON misc.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = misc.PI_FK
WHERE (header.MappingStatus = 2)
GO