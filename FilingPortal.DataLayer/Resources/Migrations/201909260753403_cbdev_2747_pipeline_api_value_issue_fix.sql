﻿-- Add Pipeline Invoice Line record --
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
        ON clients.id = rulePrice.importer_id AND rulePrice.crude_type_id IS NULL
      LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
        ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
      LEFT JOIN Pipeline_Rule_Price rulePriceExact
        ON clients.id = rulePriceExact.importer_id AND ruleBatch.id = rulePriceExact.crude_type_id
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