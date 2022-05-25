
-- add packing record --
ALTER   PROCEDURE [zones_ftz214].[sp_add_packing] (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'packing';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_ftz214.form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add packing tab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM zones_ftz214.packing packing
      WHERE packing.filing_header_id = @parentId)
  BEGIN
    INSERT INTO zones_ftz214.packing (
		filing_header_id
		,parent_record_id
		,operation_id
		,manifest_qty
		,bill_number
		,it_number
		,foreign_port
		,export_country
		,firms
		,created_date
		,created_user
		,bill_issuer_scac)
      SELECT 
		 @filingHeaderId
		,@parentId
		,@operationId
		,parsed_data.qty
		,parsed_data.master
		,parsed_data.it_no
		,parsed_data.foreign_port
		,parsed_data.ce
		,parsed_data.firms
		,GETDATE()
		,@filingUser
		,parsed_data.imp_carrier_code
      FROM zones_ftz214.filing_detail detail
      
	  JOIN zones_ftz214.inbound_parsed_data parsed_data
      ON parsed_data.id = detail.inbound_id
      
	  WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO


