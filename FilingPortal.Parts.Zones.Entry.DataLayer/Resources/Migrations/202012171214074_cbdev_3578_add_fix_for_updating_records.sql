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
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.declaration
    WHERE declaration.filing_header_id = @filingHeaderId
  END

  INSERT INTO zones_entry.declaration (filing_header_id
  , parent_record_id
  , operation_id
  , importer
  , consignee
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
  , importer_id
  , filer_code
  , created_date
  , created_user)
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,importer.ClientCode
     ,rule_importer.consignee
     ,inbnd.entry_port
     ,inbnd.entry_port
     ,inbnd.arrival_date
     ,inbnd.arrival_date
     ,parsed_data.ultimate_destination_state
     ,rule_importer.goods_description
     ,inbnd.owner_ref
     ,inbnd.firms_code
     ,inbnd.firms_code
     ,CONCAT(inbnd.entry_no, parsed_data.check_digit)
     ,parsed_data.application_begin_date
     ,parsed_data.application_end_date
     ,rule_importer.rlf
     ,parsed_data.ftz_number
     ,rule_importer.f3461_box29
     ,parsed_data.gross_wgt
     ,inbnd.is7501
     ,~inbnd.is7501
     ,inbnd.importer_id
     ,inbnd.filer_code
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail AS detail
    JOIN zones_entry.inbound AS inbnd
      ON inbnd.id = detail.inbound_id
    LEFT JOIN dbo.Clients AS importer
      ON inbnd.importer_id = importer.id
    LEFT JOIN zones_entry.rule_importer rule_importer
      ON inbnd.importer_id = rule_importer.importer_id
    LEFT JOIN zones_entry.inbound_parsed_data parsed_data
      ON inbnd.id = parsed_data.id
    WHERE detail.filing_header_id = @filingHeaderId
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
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.misc
    WHERE filing_header_id = @filingHeaderId
  END
  INSERT INTO zones_entry.misc (filing_header_id
  , parent_record_id
  , operation_id
  , branch
  , [broker]
  , recon_issue
  , fta_recon
  , team_no
  , created_date
  , created_user)
    SELECT DISTINCT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,user_data.Branch
     ,user_data.[Broker]
     ,COALESCE(parsed_data.recon_issue, rule_importer.recon_issue)
     ,parsed_data.nafta_recon
     ,parsed_data.team_no
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail detail
    JOIN zones_entry.inbound inbnd
      ON inbnd.id = detail.inbound_id
    LEFT JOIN dbo.app_users_data user_data
      ON user_data.UserAccount = @filingUser
    LEFT JOIN zones_entry.rule_importer rule_importer
      ON inbnd.importer_id = rule_importer.importer_id
    LEFT JOIN zones_entry.inbound_parsed_data parsed_data
      ON inbnd.id = parsed_data.id
    WHERE detail.filing_header_id = @filingHeaderId
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
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.invoice_header
    WHERE filing_header_id = @filingHeaderId
  END
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
     ,parsed_data.total_entered_value
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
    LEFT JOIN zones_entry.inbound_parsed_data parsed_data
      ON inbnd.id = parsed_data.id
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
  , manufacturer
  , goods_description
  , invoice_no
  , amount
  , charges
  , ftz_date
  , transaction_related
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
     ,COALESCE(sb_tariff.Unit, tariff.Unit)
     ,inbnd_lines.item_value
     ,dbo.fn_app_weight_to_ton(inbnd_lines.ftz_manifest_qty, tariff.Unit)
     ,inbnd_lines.manufacturers_id_no
     ,rule_importer.goods_description
     ,invoice_header.invoice_no
     ,inbnd_lines.charges
     ,inbnd_lines.charges
     ,inbnd_lines.ftz_date
     ,inbnd_lines.transaction_related
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
GO

-- add notes record --
ALTER PROCEDURE zones_entry.sp_add_notes (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_entry.notes';
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

  -- add notes data and apply rules
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.notes
    WHERE filing_header_id = @filingHeaderId
  END
  INSERT INTO zones_entry.notes (filing_header_id
  , parent_record_id
  , operation_id
  , title
  , date
  , message
  , created_date
  , created_user)
  OUTPUT INSERTED.ID INTO @IDs (ID)
    SELECT DISTINCT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,inbound_notes.title
     ,inbound_notes.date
     ,inbound_notes.message
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail detail
    JOIN zones_entry.inbound_notes inbound_notes
      ON detail.inbound_id = inbound_notes.inbound_record_id
    WHERE detail.filing_header_id = @filingHeaderId
END;
GO

-- add packing record --
ALTER PROCEDURE zones_entry.sp_add_packing (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'packing';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add packing tab data and apply rules
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.packing
    WHERE filing_header_id = @filingHeaderId
  END
  INSERT INTO zones_entry.packing (filing_header_id
  , parent_record_id
  , operation_id
  , bill_num
  , created_date
  , created_user)
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,inbnd.vessel_name
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
END;
GO

CREATE PROCEDURE zones_entry.sp_refile_entry (@filingHeaderId INT, @filingUser VARCHAR(128) = NULL)
AS
BEGIN
  EXEC zones_entry.sp_create_entry_records @filingHeaderId, @filingUser
END
GO