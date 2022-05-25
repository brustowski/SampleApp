-- recalculate pipeline fileds
ALTER PROCEDURE dbo.sp_imp_pipeline_recalculate (@filingHeaderId INT
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
  -- invoice customs qty
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
  -- invoice line price
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