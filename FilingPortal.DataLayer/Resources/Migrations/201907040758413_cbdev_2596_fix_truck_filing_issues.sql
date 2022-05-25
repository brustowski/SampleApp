--- update fields ---
ALTER TABLE dbo.Truck_InvoiceHeaders
DROP COLUMN Invoice_Total
GO

IF OBJECT_ID('dbo.truck_invoice_total', 'FN') IS NOT NULL
  DROP FUNCTION dbo.truck_invoice_total
GO

CREATE FUNCTION dbo.truck_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(til.Invoice_Qty * til.PriceUnit)
  FROM dbo.Truck_InvoiceLines til
  WHERE til.InvoiceHeaders_FK = @invoiceHeaderId
  GROUP BY til.InvoiceHeaders_FK

  RETURN @result
END
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
ADD Invoice_Total AS (dbo.truck_invoice_total(id))
GO

-- remove unnecessary values
DELETE tdv
  FROM dbo.truck_def_values tdv
  INNER JOIN truck_sections ts
    ON tdv.section_id = ts.id
WHERE (tdv.column_name = 'Invoice_Total'
  AND ts.table_name = 'Truck_InvoiceHeaders')

  OR (tdv.column_name = 'Line_Price'
  AND tdv.label <> 'Line Price'
  AND ts.table_name = 'Truck_InvoiceLines')

  OR (tdv.column_name = 'Gr_Weight'
  AND tdv.label <> 'Gr. Weight'
  AND ts.table_name = 'Truck_InvoiceLines')
GO

DELETE FROM dbo.truck_def_values_manual
WHERE (column_name = 'Invoice_Total'
  AND table_name = 'Truck_InvoiceHeaders')

  OR (column_name = 'Line_Price'
  AND label <> 'Line Price'
  AND table_name = 'Truck_InvoiceLines')

  OR (column_name = 'Gr_Weight'
  AND label <> 'Gr. Weight'
  AND table_name = 'Truck_InvoiceLines')
GO

UPDATE tdv
SET tdv.has_default_value = 0
FROM dbo.truck_def_values tdv
INNER JOIN dbo.truck_sections AS ts
  ON tdv.section_id = ts.id
WHERE tdv.column_name = 'Line_Price'
AND ts.table_name = 'Truck_InvoiceLines'
GO


--- Truck apply dafault values procedure for specified table---
ALTER PROCEDURE dbo.truck_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.[value] IS NULL THEN 'NULL '
      ELSE 'try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
        i.DATA_TYPE +
        CASE
          WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
            CASE
              WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
              ELSE CONVERT(VARCHAR(4),
                CASE
                  WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
                  ELSE CHARACTER_MAXIMUM_LENGTH
                END)
            END + ')'
          WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
            + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
          ELSE ''
        END
        + ') '
    END
    + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.truck_def_values_manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.has_default_value = 1
  AND v.record_id = @recordId

  EXEC (@str);
END
GO

--- Truck apply values to result table---
ALTER PROCEDURE dbo.truck_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.[value] IS NULL THEN 'NULL '
      ELSE 'try_cast(''' + ISNULL(v.value, '') + ''' as ' +
        data_type +
        CASE
          WHEN data_type IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
            CASE
              WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
              ELSE CONVERT(VARCHAR(4),
                CASE
                  WHEN data_type IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
                  ELSE CHARACTER_MAXIMUM_LENGTH
                END)
            END + ')'
          WHEN data_type IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
            + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
          ELSE ''
        END
        + ') '
    END
    + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.truck_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO

-- add invoice line record --
ALTER PROCEDURE dbo.truck_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add Invoice Lines data and apply rules  
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_InvoiceLines invoiceLines
      WHERE invoiceLines.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
         ,details.BDP_FK AS TI_FK
         ,inbound.PAPs AS Invoice_No
         ,ruleImporters.transactions_related AS Transaction_Related
         ,ruleImporters.Tariff AS Tariff
         ,ruleImporters.custom_quantity AS Customs_QTY
         ,ruleImporters.Goods_Description AS Goods_Description
         ,ruleImporters.SPI AS SPI
         ,ruleImporters.co AS ORG
         ,ruleimporters.ce AS Export
         ,ruleImporters.gross_weight AS Gr_Weight
         ,ruleImporters.gross_weight_uq AS Gr_Weight_Unit
         ,ruleImporters.custom_uq AS UQ
         ,ruleImporters.Line_Price AS PriceUnit
         ,ruleImporters.product_id AS Prod_ID_1
         ,ruleImporters.custom_attrib1 AS Attribute_1
         ,ruleImporters.custom_attrib2 AS Attribute_2
         ,ruleImporters.Invoice_Qty AS Invoice_Qty
         ,ruleImporters.invoice_uq AS Invoice_Qty_Unit
         ,ruleImporters.charges AS Amount
         ,ruleImporters.invoice_qty * ruleImporters.line_price AS Line_Price
        FROM dbo.Truck_Filing_Details details
        INNER JOIN dbo.Truck_Inbound inbound
          ON inbound.id = details.BDP_FK
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,Gr_Weight_Unit
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price)
      SELECT
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,Gr_Weight_Unit
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_InvoiceLines invoiceLines
    WHERE invoiceLines.Filing_Headers_FK = @parentId
  END

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

--- Truck report view---
ALTER VIEW dbo.Truck_Report
AS
SELECT
  headers.id
 ,declaration.Main_Supplier
 ,declaration.Importer
 ,declaration.Shipment_Type
 ,declaration.Transport
 ,declaration.Entry_Type
 ,declaration.RLF
 ,declaration.Enable_Entry_Sum
 ,declaration.Type
 ,declaration.Certify_Cargo_Release
 ,declaration.Service
 ,declaration.Issuer
 ,declaration.Master_Bill
 ,declaration.Carrier_SCAC
 ,declaration.Discharge
 ,declaration.Entry_Port
 ,declaration.Dep
 ,declaration.Arr
 ,declaration.Arr_2
 ,declaration.HMF
 ,declaration.Origin
 ,declaration.Destination
 ,declaration.Destination_State
 ,declaration.Country_of_Export
 ,declaration.ETA
 ,declaration.Export_Date
 ,declaration.Description
 ,declaration.Owner_Ref
 ,declaration.INCO
 ,declaration.Total_Weight
 ,declaration.Total_Volume
 ,declaration.No_Packages
 ,declaration.FIRMs_Code
 ,declaration.Centralized_Exam_Site
 ,declaration.Purchased
 ,declaration.Manual_Entry
 ,declaration.Importer_of_record
 ,declaration.Split_Shipment_Release
 ,declaration.Check_Local_Client
 ,containers.Bill_Type
 ,containers.Bill_Num
 ,containers.Bill_Number
 ,containers.UQ AS Containers_UQ
 ,containers.Manifest_QTY
 ,containers.Packing_UQ
 ,containers.Bill_Issuer_SCAC
 ,invheaders.Invoice_No
 ,invheaders.Consignee_Address
 ,invheaders.Invoice_Total
 ,invheaders.Curr
 ,invheaders.Payment_Date
 ,invheaders.Consignee
 ,invheaders.Inv_Date
 ,invheaders.Agreed_Place
 ,invheaders.Inv_Gross_Weight
 ,invheaders.Net_Weight
 ,invheaders.Manufacturer
 ,invheaders.Seller
 ,invheaders.Sold_to_party
 ,invheaders.Ship_to_party
 ,invheaders.Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone
 ,invheaders.Broker_PGA_Contact_Email
 ,invlines.invoice_line_number AS LNO
 ,invlines.Tariff
 ,invlines.Customs_QTY
 ,invlines.Line_Price
 ,invlines.Goods_Description
 ,invlines.ORG
 ,invlines.SPI
 ,invlines.Gr_Weight
 ,invlines.Gr_Weight_Unit
 ,invlines.Gr_Weight_Tons
 ,invlines.UQ
 ,invlines.PriceUnit
 ,invlines.Prod_ID_1
 ,invlines.Attribute_1
 ,invlines.Attribute_2
 ,invlines.Export
 ,invlines.Invoice_Qty
 ,invlines.Invoice_Qty_Unit
 ,invlines.Code
 ,invlines.Amount
 ,invlines.CIF_Component
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,invlines.TSCA_Indicator
 ,invlines.Certifying_Individual
 ,misc.Branch
 ,misc.Broker
 ,misc.Merge_By
 ,misc.Tax_Deferrable_Ind
 ,misc.Preparer_Dist_Port
 ,misc.Recon_Issue
 ,misc.FTA_Recon
 ,misc.Bond_Type
 ,misc.Payment_Type
 ,misc.Broker_to_Pay
 ,misc.Prelim_Statement_Date
 ,misc.Submitter
 ,invlines.Manufacturer AS Truck_InvoiceLines_Manufacturer
 ,invheaders.Transaction_Related AS Truck_InvoiceHeaders_Transaction_Related
 ,invlines.Transaction_Related AS Truck_InvoiceLines_Transaction_Related
 ,invheaders.EPA_TSCA_Cert_Date AS Truck_InvoiseHeaders_EPA_TSCA_Cert_Date
 ,declaration.Container AS Truck_DeclarationTab_Container
 ,invheaders.Supplier AS Truck_InvoiceHeaders_Supplier
 ,invheaders.Origin AS Truck_InvoiceHeaders_Origin
FROM dbo.Truck_Filing_Headers headers
LEFT OUTER JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO