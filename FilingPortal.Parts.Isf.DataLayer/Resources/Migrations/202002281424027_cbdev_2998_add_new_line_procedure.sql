-- add line record --
CREATE PROCEDURE isf.sp_add_empty_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'isf.line'
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
      FROM isf.line AS line
      WHERE line.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO isf.line (
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
       ,@filingUser;
  END
END;
GO

UPDATE isf.form_section_configuration
SET title = 'Container details'
   ,name = 'container_details'
WHERE id = 5;
UPDATE isf.form_section_configuration
SET title = 'Container'
   ,name = 'container'
WHERE id = 6;
UPDATE isf.form_section_configuration
SET title = 'Manufacturer details'
   ,name = 'manufacturer_details'
WHERE id = 3;
UPDATE isf.form_section_configuration
SET title = 'Manufacturer'
   ,name = 'manufacturer'
   ,procedure_name = 'sp_add_empty_line'
WHERE id = 4;
GO