DROP VIEW dbo.v_imp_pipeline_rule_price
GO

ALTER VIEW dbo.v_imp_pipeline_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.importer
 ,inbnd.batch
 ,inbnd.ticket_number
 ,inbnd.facility
 ,inbnd.site_name
 ,inbnd.quantity
 ,inbnd.api
 ,inbnd.entry_number
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_importer rule_importer
        WHERE inbnd.importer = rule_importer.importer) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_batch_code rule_batch
        WHERE dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code) THEN 1
    ELSE 0
  END AS has_batch_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_facility rule_facility
        WHERE inbnd.facility = rule_facility.facility) THEN 1
    ELSE 0
  END AS has_facility_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_price rule_price
        INNER JOIN dbo.Clients clients
          ON rule_price.importer_id = clients.id
        WHERE inbnd.importer = clients.ClientCode
        AND clients.id = rule_price.importer_id) THEN 1
    ELSE 0
  END AS has_price_rule
FROM dbo.imp_pipeline_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_pipeline_filing_header etfh
  JOIN dbo.imp_pipeline_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
WHERE inbnd.deleted = 0;
GO

-- add pipeline import invoice header record --
CREATE OR ALTER PROCEDURE dbo.sp_imp_pipeline_add_invoice_header (@filingHeaderId INT,
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
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_invoice_header invoice_header
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
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.batch
       ,rule_importer.supplier
       ,inbnd.quantity * COALESCE(rule_price_exact.pricing, rule_price.pricing)
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
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price
        ON client.id = rule_price.importer_id
          AND rule_price.crude_type_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.Batch) = rule_batch.batch_code
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price_exact
        ON client.id = rule_price_exact.importer_id
          AND rule_batch.id = rule_price_exact.crude_type_id
      WHERE detail.filing_header_id = @filingHeaderId

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
CREATE OR ALTER PROCEDURE dbo.sp_imp_pipeline_add_invoice_line (@filingHeaderId INT,
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
    FROM dbo.imp_pipeline_filing_detail detail
    JOIN dbo.imp_pipeline_inbound inbnd
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
      FROM dbo.imp_pipeline_invoice_line invoice_line
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
       ,COALESCE(rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.batch
       ,inbnd.api
       ,CONCAT(rule_facility.pipeline, ' P/L')
       ,inbnd.quantity
       ,rule_importer.origin
       ,inbnd.quantity * COALESCE(rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.quantity * COALESCE(rule_price_exact.freight, rule_price.freight)
       ,rule_importer.manufacturer
       ,rule_importer.consignee
       ,rule_importer.consignee
       ,rule_importer.origin
       ,rule_importer.country_of_export
       ,rule_facility.destination_state
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price
        ON client.id = rule_price.importer_id
          AND rule_price.crude_type_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price_exact
        ON client.id = rule_price_exact.importer_id
          AND rule_batch.id = rule_price_exact.crude_type_id
      LEFT JOIN @tariffs tariff
        ON inbnd.id = tariff.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_api rule_api
        ON tariff.tariff = rule_api.id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

-- recalculate pipeline fileds
CREATE OR ALTER PROCEDURE dbo.sp_imp_pipeline_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN dbo.imp_pipeline_form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN dbo.imp_pipeline_form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values
  -- importer code
  DECLARE @importerCode VARCHAR(128) = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);
  -- freight
  DECLARE @freight DECIMAL(18, 6);
  SELECT
    @freight = COALESCE(rule_price_exact.freight, rule_price.freight)
  FROM dbo.imp_pipeline_filing_detail detail
  JOIN dbo.imp_pipeline_inbound inbnd
    ON inbnd.id = detail.inbound_id
  LEFT JOIN dbo.Clients clients
    ON clients.ClientCode = @importerCode
  LEFT JOIN dbo.imp_pipeline_rule_price rule_price
    ON clients.id = rule_price.importer_id
      AND rule_price.crude_type_id IS NULL
  LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
    ON dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code
  LEFT JOIN dbo.imp_pipeline_rule_price rule_price_exact
    ON clients.id = rule_price_exact.importer_id
      AND rule_batch.id = rule_price_exact.crude_type_id
  WHERE detail.filing_header_id = @filingHeaderId;

  -- quantity, unit_price, api
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
   ,api DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,quantity
     ,unit_price
     ,api)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(28, 15), b.value) AS unit_price
     ,CONVERT(DECIMAL(18, 6), c.value) AS api
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
    JOIN @config c
      ON a.record_id = c.record_id
        AND a.table_name = 'imp_pipeline_invoice_line'
        AND a.column_name = 'invoice_qty'
        AND b.column_name = 'price_unit'
        AND c.column_name = 'attribute2';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line customs qty
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity, '0.#####')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'imp_pipeline_invoice_line'
    AND column_name = 'customs_qty';
  -- invoice line line_price
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * unit_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'imp_pipeline_invoice_line'
    AND column_name = 'line_price';
  -- invoice line amount
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * @freight, '0.######')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'imp_pipeline_invoice_line'
    AND column_name = 'amount';
  -- invoice line gross weight
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(dbo.fn_imp_pipeline_weight(quantity, api), '0.###')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'imp_pipeline_invoice_line'
    AND column_name = 'gr_weight';
  -- invoice header invoice total
  DECLARE @total DECIMAL(28, 15);
  SELECT
    @total = SUM(quantity * unit_price)
  FROM @tbl;
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'imp_pipeline_invoice_header'
    AND column_name = 'invoice_total';

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO