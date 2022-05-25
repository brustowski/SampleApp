
-- add truck import invoice header record --
ALTER   PROCEDURE [zones_ftz214].[sp_add_invoice_header] (@filingHeaderId INT,
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
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO zones_ftz214.invoice_header (
	 filing_header_id
    ,parent_record_id
    ,operation_id
    ,created_date
    ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
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
    EXEC zones_ftz214.sp_add_invoice_line @filingHeaderId
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


