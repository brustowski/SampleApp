-- add declaration record --
ALTER PROCEDURE [zones_entry].[sp_add_declaration] (@filingHeaderId INT,
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
     ,inbnd.entry_no
     ,parsed_data.application_begin_date
     ,parsed_data.application_end_date
     ,rule_importer.rlf
     ,parsed_data.ftz_number
     ,''
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