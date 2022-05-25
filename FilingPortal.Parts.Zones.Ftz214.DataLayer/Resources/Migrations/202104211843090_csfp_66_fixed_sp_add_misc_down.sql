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
		,created_user
		,[service]
		,submitter)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
       ,GETDATE()
       ,@filingUser
	   ,transportMode.service_code
	   ,Clients.ClientCode
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data
		ON inbnd.id = parsed_data.id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
	  JOIN handbook_transport_mode AS transportMode
		ON transportMode.code_number = parsed_data.mot
				LEFT JOIN dbo.Clients AS clients
		ON inbnd.applicant_id = clients.id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;