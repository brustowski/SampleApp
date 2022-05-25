

-- add truck import misc record --
ALTER   PROCEDURE [zones_ftz214].[sp_add_misc] (@filingHeaderId INT,
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
  FROM zones_ftz214.form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_ftz214.misc (filing_header_id
		,parent_record_id
		,operation_id
		,branch
		,[broker]
		,created_date
		,created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
       ,GETDATE()
       ,@filingUser
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO


