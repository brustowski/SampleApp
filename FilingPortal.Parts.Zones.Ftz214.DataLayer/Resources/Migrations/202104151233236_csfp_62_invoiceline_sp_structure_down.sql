
-- add invoice line record --
ALTER   PROCEDURE [zones_ftz214].[sp_add_invoice_line] (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_ftz214.invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO zones_ftz214.invoice_line (
		 filing_header_id
		,parent_record_id
		,operation_id
		,created_date
		,created_user)
      SELECT
		 @filingHeaderId
		,@parentId
		,@operationId
        ,GETDATE()
       ,@filingUser
      FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
		WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO


