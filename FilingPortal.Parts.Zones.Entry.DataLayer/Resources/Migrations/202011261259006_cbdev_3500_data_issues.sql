-- add declaration record --
ALTER PROCEDURE zones_entry.sp_add_declaration (@filingHeaderId INT,
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
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.declaration (filing_header_id
    , parent_record_id
    , operation_id
    , importer
    , consignee
    , entry_type
    , discharge_port
    , entry_port
    , arr
    , est_entry_date
    , destination_state
    , description
    , owner_ref
    , firms_code
    , centralized_exam_site
    , entry_number
    , application_begin_date
    , application_end_date
    , rlf
    , ftz_number
    , decl_3461_box_29
    , total_weight
    , enable_entry_sum
    , enable_cargo_rel
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,importer.ClientCode
       ,rule_importer.consignee
       ,inbnd.entry_type
       ,inbnd.entry_port
       ,inbnd.entry_port
       ,inbnd.arrival_date
       ,inbnd.arrival_date
       ,inbnd.ultimate_destination_state
       ,inbnd.merchandise_description3461
       ,inbnd.owner_ref
       ,inbnd.firms_code
       ,inbnd.firms_code
       ,CONCAT(inbnd.entry_no, inbnd.check_digit)
       ,inbnd.application_begin_date
       ,inbnd.application_end_date
       ,rule_importer.rlf
       ,inbnd.ftz_number
       ,rule_importer.f3461_box29
       ,inbnd.gross_wgt
       ,~inbnd.is_update
       ,~inbnd.is_update
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail AS detail
      JOIN zones_entry.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN zones_entry.rule_importer rule_importer
        ON inbnd.importer_id = rule_importer.importer_id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

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
      ,amount
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

-- add invoice header record --
ALTER PROCEDURE zones_entry.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_entry.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_entry.invoice_header (filing_header_id
    , parent_record_id
    , operation_id
    , consignee
    , supplier
    , supplier_address
    , invoice_total
    , seller
    , manufacturer
    , importer
    , sold_to_party
    , ship_to_party
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.consignee
       ,rule_importer.supplier
       ,(SELECT TOP (1)
            lines.manufacturers_id_no
          FROM zones_entry.inbound_lines lines
          WHERE lines.inbound_record_id = inbnd.id)
       ,inbnd.total_entered_value
       ,rule_importer.seller
       ,rule_importer.manufacturer
       ,c.ClientCode
       ,rule_importer.sold_to_party
       ,rule_importer.ship_to_party
       ,GETDATE()
       ,@filingUser
      FROM zones_entry.filing_detail detail
      JOIN zones_entry.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN Clients c
        ON inbnd.importer_id = c.id
      LEFT JOIN zones_entry.rule_importer rule_importer
        ON inbnd.importer_id = rule_importer.importer_id
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
    EXEC zones_entry.sp_add_invoice_line @filingHeaderId
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

DECLARE @decl_id INT = NULL;
SELECT
  @decl_id = fsc.id
FROM zones_entry.form_section_configuration fsc
WHERE fsc.table_name = 'zones_entry.declaration'

DECLARE @header_id INT = NULL;
SELECT
  @header_id = fsc.id
FROM zones_entry.form_section_configuration fsc
WHERE fsc.table_name = 'zones_entry.invoice_header'

DECLARE @lines_id INT = NULL;
SELECT
  @lines_id = fsc.id
FROM zones_entry.form_section_configuration fsc
WHERE fsc.table_name = 'zones_entry.invoice_line'

INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@decl_id, 'total_number_of_packages_uq', SUSER_NAME(), 252, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), NULL, 'Total Number Of Packages UQ', 'units', CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@header_id, 'pga_contact_email', SUSER_NAME(), 15, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), NULL, 'PGA Contact Email', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@header_id, 'pga_contact_phone', SUSER_NAME(), 13, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), NULL, 'PGA Contact Phone', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@header_id, 'pga_contact_name', SUSER_NAME(), 14, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'False'), NULL, 'PGA Contact Name', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@lines_id, 'code', SUSER_NAME(), 22, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), NULL, 'Code', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@lines_id, 'amount', SUSER_NAME(), 21, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), NULL, 'Amount', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
INSERT zones_entry.form_configuration (section_id, column_name, created_user, display_on_ui, manual, editable, mandatory, description, label, handbook_name, confirmation_needed, has_default_value)
  VALUES (@decl_id, 'enable_entry_sum', SUSER_NAME(), 70, 0, CONVERT(BIT, 'True'), CONVERT(BIT, 'True'), NULL, 'Enable Entry Sum', NULL, CONVERT(BIT, 'False'), CONVERT(BIT, 'False'))
GO