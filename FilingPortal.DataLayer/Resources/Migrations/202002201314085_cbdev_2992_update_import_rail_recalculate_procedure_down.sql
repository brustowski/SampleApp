-- recalculate rail fileds
ALTER PROCEDURE dbo.sp_imp_rail_recalculate (@json VARCHAR(MAX))
AS

  SET NOCOUNT ON;

  DECLARE @InboundList field_update_list;
  INSERT INTO @InboundList (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,record_id
     ,parent_record_id
     ,value
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512));

  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  )
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbound.id
     ,inbound.record_id
     ,inbound.parent_record_id
     ,inbound.value
     ,irfc.column_name
     ,irfsc.table_name
    FROM @InboundList inbound
    LEFT JOIN imp_rail_form_configuration irfc
      ON inbound.Id = irfc.id
    INNER JOIN imp_rail_form_section_configuration irfsc
      ON irfc.section_id = irfsc.id

  -- quantity, unit price
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,quantity
     ,unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
        AND a.table_name = 'imp_rail_invoice_line'
        AND a.column_name = 'invoice_qty'
        AND b.column_name = 'price_unit';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
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
    WHERE table_name = 'imp_rail_invoice_line'
    AND column_name = 'line_price';


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
    WHERE table_name = 'imp_rail_invoice_header'
    AND column_name = 'invoice_total';

  SELECT
    *
  FROM @tblUpdatedFields
  FOR JSON PATH, INCLUDE_NULL_VALUES;

  RETURN 0;
GO