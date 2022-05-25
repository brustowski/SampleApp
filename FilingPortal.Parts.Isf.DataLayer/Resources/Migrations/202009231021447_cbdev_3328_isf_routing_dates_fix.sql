/****** Object:  StoredProcedure [isf].[sp_add_routing]    Script Date: 9/9/2020 5:05:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- add routing record --
ALTER PROCEDURE [isf].[sp_add_routing] (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.routing'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM isf.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM isf.routing AS routing
      WHERE routing.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.routing (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,etd
       ,eta)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.etd
       ,inbnd.eta
      FROM isf.filing_detail AS detail
      INNER JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
