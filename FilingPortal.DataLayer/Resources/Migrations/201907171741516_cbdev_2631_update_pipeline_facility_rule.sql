-- add declaration tab record --
ALTER PROCEDURE [dbo].[pipeline_add_declaration_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ruleImporters.Supplier AS Main_Supplier
         ,@ImporterCode AS Importer
         ,ruleFacility.Issuer AS Issuer
         ,REPLACE(inbound.TicketNumber, '-', '') AS Batch_Ticket
         ,ruleFacility.Pipeline AS Pipeline
         ,ruleFacility.Issuer AS Carrier_SCAC
         ,ruleFacility.port AS Discharge
         ,ruleFacility.port AS Entry_Port
         ,inbound.ExportDate AS Dep
         ,inbound.ImportDate AS Arr
         ,inbound.ImportDate AS Arr_2
         ,ruleFacility.Origin AS Origin
         ,ruleFacility.Destination AS Destination
         ,ruleFacility.Destination_State AS Destination_State
         ,inbound.ImportDate AS ETA
         ,inbound.ImportDate AS Export_Date
         ,CONCAT(ruleFacility.pipeline,' P/L', ': ', inbound.Batch) AS Description
         ,inbound.TicketNumber AS Owner_Ref
         ,ruleFacility.FIRMs_Code AS FIRMs_Code
         ,REPLACE(inbound.TicketNumber, '-', '') AS Master_Bill
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
       
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_DeclarationTab (
        Filing_Headers_FK
       ,PI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Batch_Ticket
       ,Pipeline
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Dep
       ,Arr
       ,Arr_2
       ,Origin
       ,Destination
       ,Destination_State
       ,ETA
       ,Export_Date
       ,Description
       ,Owner_Ref
       ,FIRMs_Code
       ,Master_Bill
       ,Importer_of_record)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Batch_Ticket
       ,Pipeline
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Dep
       ,Arr
       ,Arr_2
       ,Origin
       ,Destination
       ,Destination_State
       ,ETA
       ,Export_Date
       ,Description
       ,Owner_Ref
       ,FIRMs_Code
       ,Master_Bill
       ,@ImporterCode
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_DeclarationTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
END;
Go
ALTER PROCEDURE [dbo].[pipeline_add_container_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ROUND(inbound.Quantity, 0) AS Manifest_QTY
         ,REPLACE(inbound.TicketNumber, '-', '') AS Bill_Num
         ,rulefacility.Issuer AS Bill_Issuer_SCAC
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))        
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_ContainersTab (
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_ContainersTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
END;
Go
ALTER PROCEDURE [dbo].[pipeline_add_invoice_line_record] (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

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

  INSERT INTO @tariffs (
      pi
     ,tariff)
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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,inbound.Batch AS Invoice_No -- ?? is it ok?
         ,ruleImporters.Transaction_Related AS Transaction_Related
         ,ruleAPI.Tariff AS Tariff
         ,inbound.Quantity AS Customs_QTY
         ,CONCAT(ruleBatch.product, ' - ', ruleBatch.batch_code) AS Goods_Description
         ,ruleImporters.SPI AS SPI
         ,CONVERT(DECIMAL(18, 3), dbo.fn_pipeline_weight(inbound.Quantity, inbound.API)) AS Gr_weight
         ,ruleImporters.value AS PriceUnit
         ,inbound.Batch AS Attribute_1
         ,'API @ 60° F = ' + CONVERT(VARCHAR(128), inbound.API) AS Attribute_2
         ,CONCAT(ruleFacility.pipeline,' P/L') AS Attribute_3
         ,inbound.Quantity AS Invoice_Qty
         ,ruleImporters.origin AS ORG
         ,inbound.Quantity * ruleImporters.value AS Line_Price
         ,inbound.Quantity * ruleImporters.freight AS Amount
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.origin AS Origin
         ,ruleImporters.country_of_export AS Export
         ,ruleFacility.Destination_State AS Destination_State
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        
        LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
          ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
        LEFT JOIN @tariffs t
          ON inbound.id = t.pi
        LEFT JOIN Pipeline_Rule_API ruleAPI
          ON t.Tariff = ruleAPI.id
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,tariff
       ,Customs_QTY
       ,Goods_Description
       ,spi
       ,Gr_Weight
       ,PriceUnit
       ,Attribute_1
       ,Attribute_2
       ,Attribute_3
       ,Invoice_Qty
       ,ORG
       ,Line_Price
       ,Amount
       ,Manufacturer
       ,Consignee
       ,Sold_to_party
       ,Origin
       ,Export
       ,Dest_State)
      SELECT
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,Gr_weight
       ,PriceUnit
       ,Attribute_1
       ,Attribute_2
       ,Attribute_3
       ,Invoice_Qty
       ,ORG
       ,Line_Price
       ,Amount
       ,Manufacturer
       ,Consignee
       ,Sold_To_Party
       ,Origin
       ,Export
       ,Destination_State
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_InvoiceLines pil
    WHERE pil.Filing_Headers_FK = @parentId
  END

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

  RETURN @recordId
END;
Go

ALTER TABLE [dbo].[Pipeline_Documents]
  ADD [Status] [nvarchar](50) NULL
  GO
   


