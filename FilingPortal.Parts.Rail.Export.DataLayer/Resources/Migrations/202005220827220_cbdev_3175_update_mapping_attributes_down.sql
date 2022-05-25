-- add rail export declaration record --
CREATE OR ALTER PROCEDURE us_exp_rail.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM us_exp_rail.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO declaration (filing_header_id
    , parent_record_id
    , operation_id
    , destination
    , country_of_dest
    , tran_related
    , importer
    , tariff_type
    , master_bill
    , main_supplier)
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
      FROM filing_detail detail
      JOIN inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN rule_exporter_consignee trule_exporter
        ON inbound.importer = trule_exporter.consignee_code
          AND inbound.exporter = trule_exporter.exporter
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
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
-- Drop column [invoice_qty] from table [us_exp_rail].[invoice_line]
--
ALTER TABLE us_exp_rail.invoice_line
  DROP COLUMN IF EXISTS invoice_qty
GO

--
-- Drop column [description] from table [us_exp_rail].[declaration]
--
ALTER TABLE us_exp_rail.declaration
  DROP COLUMN IF EXISTS description
GO