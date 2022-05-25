--
-- add rail export declaration record
--
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'us_exp_rail.declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM us_exp_rail.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO us_exp_rail.declaration (filing_header_id
    , parent_record_id
    , operation_id
    , destination
    , country_of_dest
    , tran_related
    , importer
    , tariff_type
    , master_bill
    , main_supplier
    , carrier
    , port_of_loading)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbound.importer
       ,inbound.tariff_type
       ,inbound.master_bill
       ,inbound.exporter
       ,inbound.carrier
       ,inbound.load_port
      FROM us_exp_rail.filing_detail detail
      JOIN us_exp_rail.inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN us_exp_rail.rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN us_exp_rail.rule_exporter_consignee trule_exporter
        ON inbound.importer = trule_exporter.consignee_code
          AND inbound.exporter = trule_exporter.exporter
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Create column [invoice_qty] on table [us_exp_rail].[invoice_line]
--
ALTER TABLE us_exp_rail.invoice_line
  ADD invoice_qty numeric(18, 6) NULL
GO

--
-- Create column [description] on table [us_exp_rail].[declaration]
--
ALTER TABLE us_exp_rail.declaration
  ADD description varchar(500) NULL
GO

-- add rail export invoice line record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO invoice_line (filing_header_id
    , parent_record_id
    , operation_id
    , tariff
    , customs_qty
    , invoice_qty
    , price
    , gross_weight
    , gross_weight_unit
    , goods_description
    , tariff_type
    , invoice_qty_unit)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbound.tariff
       ,container.customs_qty
       ,container.customs_qty
       ,container.price
       ,container.gross_weight
       ,inbound.gross_weight_uom
       ,inbound.goods_description
       ,inbound.tariff_type
       ,dbo.fn_app_unit_by_tariff(inbound.tariff, inbound.tariff_type) AS invoice_qty_unit
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN (SELECT
          ic.inbound_record_id
         ,SUM(ic.price) AS price
         ,SUM(ic.customs_qty) AS customs_qty
         ,SUM(ic.gross_weight) AS gross_weight
        FROM us_exp_rail.inbound_containers ic
        GROUP BY ic.inbound_record_id) AS container
        ON container.inbound_record_id = inbound.id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

--
-- Add new columns mappings if not exists
--
IF (NOT EXISTS (SELECT
      0
    FROM us_exp_rail.form_configuration
    WHERE column_name = 'description'
    AND section_id = 2)
  )
  INSERT INTO us_exp_rail.form_configuration (section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label)
    VALUES (2, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 'description', GETDATE(), SUSER_NAME(), NULL, 35, 0, NULL, 'Description', 'Description')

IF (NOT EXISTS (SELECT
      0
    FROM us_exp_rail.form_configuration
    WHERE column_name = 'invoice_qty'
    AND section_id = 5)
  )
INSERT INTO us_exp_rail.form_configuration (section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label)
  VALUES (5, CONVERT(BIT, 'False'), CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), 'invoice_qty', GETDATE(), SUSER_NAME(), NULL, 13, 0, NULL, 'Invoice QTY', 'Invoice Qty')
GO

