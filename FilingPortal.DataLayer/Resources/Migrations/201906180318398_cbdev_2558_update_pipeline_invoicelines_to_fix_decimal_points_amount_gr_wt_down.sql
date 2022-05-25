  ALTER TABLE [dbo].[Pipeline_InvoiceLines] ALTER COLUMN Amount INT NULL
  GO
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
         ,dbo.fn_pipeline_weight(inbound.Quantity, inbound.API) AS Gr_weight
         ,ruleImporters.value AS PriceUnit
         ,inbound.Batch AS Attribute_1
         ,'API Gravity @ 60 F° = "' + CONVERT(VARCHAR(128), inbound.API) + '"' AS Attribute_2
         ,rulePorts.customs_attribute3 AS Attribute_3
         ,inbound.Quantity AS Invoice_Qty
         ,ruleImporters.origin AS ORG
         ,inbound.Quantity * ruleImporters.value AS Line_Price
         ,inbound.Quantity * ruleImporters.freight AS Amount
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.origin AS Origin
         ,ruleImporters.country_of_export AS Export
         ,rulePorts.Destination_State AS Destination_State
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON (
              --we have both tofacility-port and port-importer rule associated with record
              (
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)
                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
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

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO