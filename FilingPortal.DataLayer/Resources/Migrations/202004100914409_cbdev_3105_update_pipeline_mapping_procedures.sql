-- add pipeline import invoice header record --
ALTER PROCEDURE dbo.sp_imp_pipeline_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_invoice_header AS invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_pipeline_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,supplier
       ,invoice_total
       ,origin
       ,consignee
       ,transaction_related
       ,manufacturer
       ,seller
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,supplier_address
       ,export
       ,manufacturer_address
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.batch
       ,rule_importer.supplier
       ,inbnd.quantity * COALESCE(rule_price_exact_facility.pricing, rule_price_exact.pricing, rule_price.pricing)
       ,rule_importer.origin
       ,rule_importer.consignee
       ,rule_importer.transaction_related
       ,rule_importer.manufacturer
       ,rule_importer.supplier
       ,@importerCode
       ,rule_importer.consignee
       ,rule_importer.consignee
       ,rule_importer.seller
       ,rule_importer.country_of_export
       ,rule_importer.mid
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail AS detail
      JOIN dbo.imp_pipeline_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_importer AS rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients AS client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_batch_code AS rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.Batch) = rule_batch.batch_code
      LEFT JOIN dbo.imp_pipeline_rule_facility AS rule_facility
        ON inbnd.facility = rule_facility.facility

      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price
        ON rule_price.importer_id = client.id
          AND rule_price.crude_type_id IS NULL
          AND rule_price.facility_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price_exact
        ON rule_price_exact.importer_id = client.id
          AND rule_price_exact.crude_type_id = rule_batch.id
          AND rule_price_exact.facility_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price_exact_facility
        ON rule_price_exact_facility.importer_id = client.id
          AND rule_price_exact_facility.crude_type_id = rule_batch.id
          AND rule_price_exact_facility.facility_id = rule_facility.id

      WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_imp_pipeline_add_invoice_line @filingHeaderId
                                             ,@recordId
                                             ,@filingUser
                                             ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END;
END;
GO

-- add pipeline import invoice line record --
ALTER PROCEDURE dbo.sp_imp_pipeline_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_invoice_line';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get current tariff ---------------
  DECLARE @tariffs TABLE (
    inbound_id INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (
      inbound_id
     ,tariff)
    SELECT
      detail.inbound_id
     ,CASE
        WHEN inbnd.api < 25 THEN 1
        WHEN inbnd.api >= 25 THEN 2
      END
    FROM dbo.imp_pipeline_filing_detail AS detail
    JOIN dbo.imp_pipeline_inbound AS inbnd
      ON detail.inbound_id = inbnd.id
    WHERE detail.filing_header_id = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_invoice_line AS invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_pipeline_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,transaction_related
       ,tariff
       ,customs_qty
       ,goods_description
       ,spi
       ,gr_weight
       ,price_unit
       ,attribute1
       ,attribute2
       ,attribute3
       ,invoice_qty
       ,org
       ,line_price
       ,amount
       ,manufacturer
       ,consignee
       ,sold_to_party
       ,origin
       ,export
       ,dest_state
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.batch -- ?? is it ok?
       ,rule_importer.transaction_related
       ,rule_api.tariff
       ,inbnd.quantity
       ,CONCAT(rule_batch.product, ' - ', rule_batch.batch_code)
       ,rule_importer.spi
       ,CONVERT(DECIMAL(18, 3), dbo.fn_imp_pipeline_weight(inbnd.quantity, inbnd.api))
       ,COALESCE(rule_price_exact_facility.pricing, rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.batch
       ,inbnd.api
       ,CONCAT(rule_facility.pipeline, ' P/L')
       ,inbnd.quantity
       ,rule_importer.origin
       ,inbnd.quantity * COALESCE(rule_price_exact_facility.pricing, rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.quantity * COALESCE(rule_price_exact_facility.freight, rule_price_exact.freight, rule_price.freight)
       ,rule_importer.manufacturer
       ,rule_importer.consignee
       ,rule_importer.consignee
       ,rule_importer.origin
       ,rule_importer.country_of_export
       ,rule_facility.destination_state
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail AS detail
      JOIN dbo.imp_pipeline_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_facility AS rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer AS rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients AS client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_batch_code AS rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code
      LEFT JOIN @tariffs AS tariff
        ON inbnd.id = tariff.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_api AS rule_api
        ON tariff.tariff = rule_api.id

      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price
        ON rule_price.importer_id = client.id
          AND rule_price.crude_type_id IS NULL
          AND rule_price.facility_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price_exact
        ON rule_price_exact.importer_id = client.id
          AND rule_price_exact.crude_type_id = rule_batch.id
          AND rule_price_exact.facility_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_price AS rule_price_exact_facility
        ON rule_price_exact_facility.importer_id = client.id
          AND rule_price_exact_facility.crude_type_id = rule_batch.id
          AND rule_price_exact_facility.facility_id = rule_facility.id

      WHERE detail.filing_header_id = @filingHeaderId;
  END;
END;
GO