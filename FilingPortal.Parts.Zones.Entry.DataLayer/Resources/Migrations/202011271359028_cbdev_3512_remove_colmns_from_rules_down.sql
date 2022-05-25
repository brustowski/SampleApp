-- add invoice line record --
ALTER PROCEDURE zones_entry.sp_add_invoice_line (@filingHeaderId INT,
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
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO zones_entry.invoice_line (filing_header_id
    , parent_record_id
    , operation_id
    , tariff
    , zone_status
    , origin
    , export
    , customs_qty
    , customs_qty_unit
    , invoice_qty
    , invoice_qty_unit
    , line_price
    , gross_weight
    , manufacturer
    , goods_description
    , epa_tsca
    , tsca_cert_indicator
    , certifying_individual
    , invoice_no
    , amount
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd_lines.hts
       ,inbnd_lines.ftz_status
       ,inbnd_lines.country_of_origin
       ,inbnd_lines.country_of_origin
       ,inbnd_lines.ftz_manifest_qty
       ,rule_importer.uq
       ,inbnd_lines.ftz_manifest_qty
       ,COALESCE(sb_tariff.Unit, tariff.Unit)
       ,inbnd_lines.item_value
       ,dbo.fn_app_weight_to_ton(inbnd_lines.ftz_manifest_qty, tariff.Unit)
       ,inbnd_lines.manufacturers_id_no
       ,rule_importer.goods_description
       ,rule_importer.epa_tsca
       ,rule_importer.tsca_cert_indicator
       ,rule_importer.certifying_individual
       ,invoice_header.invoice_no
       ,inbnd_lines.charges
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN zones_entry.inbound_lines inbnd_lines
        ON inbnd.id = inbnd_lines.inbound_record_id
      LEFT JOIN dbo.Tariff tariff
        ON inbnd_lines.hts = tariff.USC_Tariff
      LEFT JOIN dbo.SchB_Tariff sb_tariff
        ON inbnd_lines.hts = sb_tariff.UB_Tariff
      LEFT JOIN zones_entry.rule_importer rule_importer
        ON inbnd.importer_id = rule_importer.importer_id
      INNER JOIN zones_entry.invoice_header invoice_header
        ON invoice_header.id = @parentId
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO

-- add truck import misc record --
ALTER PROCEDURE zones_entry.sp_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM zones_entry.form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.misc (filing_header_id
    , parent_record_id
    , operation_id
    , branch
    , [broker]
    , preliminary_statement_print_date
    , recon_issue
    , entry_date_election_code
    , created_date
    , created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
       ,inbnd.statement_date
       ,rule_importer.recon_issue
       ,rule_importer.entry_date_election_code
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      LEFT JOIN zones_entry.rule_importer rule_importer
        ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO