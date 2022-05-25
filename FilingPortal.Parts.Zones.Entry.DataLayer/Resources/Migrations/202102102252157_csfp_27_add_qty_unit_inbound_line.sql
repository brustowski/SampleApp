-- add invoice line record --
ALTER PROCEDURE [zones_entry].[sp_add_invoice_line] (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_entry.invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.invoice_line
    WHERE filing_header_id = @filingHeaderId
  END

  INSERT INTO zones_entry.invoice_line (filing_header_id
  , parent_record_id
  , operation_id
  , line_no
  , tariff
  , zone_status
  , origin
  , export
  , customs_qty
  , invoice_qty
  , invoice_qty_unit
  , line_price
  , gross_weight
  , gross_weight_unit
  , manufacturer
  , goods_description
  , invoice_no
  , amount
  , charges
  , ftz_date
  , transaction_related
  , spi
  , epa_pst
  , epa_pst_disclaimer
	,ftz_pack_qty
	,ftz_pack_qty_unit
  , created_date
  , created_user)
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,inbnd_lines.item_no
     ,inbnd_lines.hts
     ,inbnd_lines.ftz_status
     ,inbnd_lines.country_of_origin
     ,inbnd_lines.country_of_origin
     ,inbnd_lines.ftz_manifest_qty
     ,inbnd_lines.ftz_manifest_qty
     ,IIF(inbnd_lines.qty_1_uom IS NULL, dbo.fn_app_unit_by_tariff(inbnd_lines.hts, 'HTS'), inbnd_lines.qty_1_uom)
     ,inbnd_lines.item_value
     ,inbnd_lines.gross_weight
     ,inbnd_lines.gross_weight_unit
     ,inbnd_lines.manufacturers_id_no
     ,rule_importer.goods_description
     ,invoice_header.invoice_no
     ,inbnd_lines.charges
     ,inbnd_lines.charges
     ,inbnd_lines.ftz_date
     ,inbnd_lines.transaction_related
     ,IIF(inbnd_lines.spi = '', 'N/A', inbnd_lines.spi)
	 ,IIF(inbnd_lines.program_code like '%PS3%', 'C',null)
	 ,IIF(inbnd_lines.program_code like '%PS3%', inbnd_lines.disclaimer,null)
	 ,inbnd_lines.ftz_manifest_qty
	 ,IIF(inbnd_lines.qty_1_uom IS NULL, dbo.fn_app_unit_by_tariff(inbnd_lines.hts, 'HTS'), inbnd_lines.qty_1_uom)
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail detail
    JOIN zones_entry.inbound inbnd
      ON inbnd.id = detail.inbound_id
    LEFT JOIN zones_entry.inbound_lines inbnd_lines
      ON inbnd.id = inbnd_lines.inbound_record_id   
    LEFT JOIN zones_entry.rule_importer rule_importer
      ON inbnd.importer_id = rule_importer.importer_id
    INNER JOIN zones_entry.invoice_header invoice_header
      ON invoice_header.id = @parentId
    WHERE detail.filing_header_id = @filingHeaderId
END;
GO