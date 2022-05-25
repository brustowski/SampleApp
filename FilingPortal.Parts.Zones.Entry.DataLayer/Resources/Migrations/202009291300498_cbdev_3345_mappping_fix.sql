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
    , enable_cargo_rel
    , ftz_number
    , decl_3461_box_29
    , created_date
    , created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,importer.ClientCode
       ,importer.ClientCode
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
       ,inbnd.entry_no
       ,inbnd.application_begin_date
       ,inbnd.application_end_date
       ,rule_importer.rlf
       ,rule_importer.enable_cargo_release
       ,rule_importer.ftz_no
       ,rule_importer.f3461_box29
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