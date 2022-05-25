-- add line record --
ALTER PROCEDURE isf.sp_add_line (@filingHeaderId INT,
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
       ,created_user
       ,source_record_id
       ,product
       ,country_of_origin
       ,tariff_number
       ,manufacturer
       ,manufacturer_address)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,manufacturer_line.id
       ,manufacturer_line.item_number
       ,manufacturer_line.country_of_origin
       ,manufacturer_line.hts_numbers
       ,manufacturer.ClientCode
       ,manufacturer_address.code
      FROM isf.filing_detail AS detail
      JOIN isf.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN isf.inbound_manufacturers AS manufacturer_line
        ON inbnd.id = manufacturer_line.inbound_record_id
      LEFT JOIN dbo.Client_Addresses AS manufacturer_address
        ON manufacturer_address.id = manufacturer_line.manufacturer_address_id
      LEFT JOIN dbo.Clients AS manufacturer
        ON manufacturer_line.manufacturer_id = manufacturer.id
      WHERE detail.filing_header_id = @filingHeaderId;
  END
END;
GO