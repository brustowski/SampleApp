-- recalculate rail fileds
ALTER PROCEDURE canada_imp_truck.sp_recalculate (@filingHeaderId INT
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
    LEFT JOIN canada_imp_truck.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN canada_imp_truck.form_section_configuration section
      ON conf.section_id = section.id;

  -- quantity, unit_price
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
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
     ,CONVERT(DECIMAL(28, 15), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
        AND a.table_name = 'canada_imp_truck.invoice_lines'
        AND a.column_name = 'ld_invoice_qty'
        AND b.column_name = 'unit_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;  
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
    WHERE table_name = 'canada_imp_truck.invoice_lines'
    AND column_name IN ('line_price', 'dt_line_value');

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
    WHERE table_name = 'canada_imp_truck.invoice_headers'
    AND column_name = 'cid_inv_total_amount';

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

ALTER TABLE canada_imp_truck.invoice_headers
ALTER COLUMN cid_exchange_rate DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_headers
ALTER COLUMN cid_lc_xr DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_headers
ALTER COLUMN cid_inv_gross_weight DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_headers
ALTER COLUMN cid_packs DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_headers
ALTER COLUMN cid_inv_net_weight DECIMAL(18, 6) NULL;
GO

ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_customs_qty DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_customs_qty_2 DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_customs_qty_3 DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_invoice_qty DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_gross_weight DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_volume DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN ld_price DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_customs_qty DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_customs_qty_2 DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_customs_qty_3 DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_line_value DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_adjustment DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_vfcc DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_exchange_rate DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_vfd DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines
ALTER COLUMN dt_vft DECIMAL(18, 6) NULL;
GO


ALTER TABLE canada_imp_truck.invoice_lines_charges
ALTER COLUMN amount DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_charges
ALTER COLUMN percent_of_line_price DECIMAL(16, 6) NULL;
GO

ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN rate DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN quantity DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN amount DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN normal_value_per_unit DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN foreign_rate DECIMAL(18, 6) NULL;
ALTER TABLE canada_imp_truck.invoice_lines_duties_and_taxes
ALTER COLUMN foreign_curr_exchange_rate DECIMAL(18, 6) NULL;
GO