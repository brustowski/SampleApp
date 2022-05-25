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
     ,rule_importer.recon_issue
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