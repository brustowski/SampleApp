

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
		,created_user
		,invoice_no
		,zone_status
		,origin
		,export
		,tariff
		,goods_description
		,customs_qty
		,invoice_qty
		,line_price
		,manufacturer
		,gross_weight
		,gross_weight_unit
		,charges
		,loading_port)
      SELECT
		 @filingHeaderId
		,@parentId
		,@operationId
        ,GETDATE()
       ,@filingUser
	   ,parsed_line.item_no
	   ,parsed_line.zone_status
	   ,parsed_line.co
	   ,parsed_data.ce
	   ,parsed_line.hts
	   ,parsed_line.description
	   ,parsed_line.qty1
	   ,parsed_line.qty1
	   ,parsed_line.value
	   ,parsed_line.mid
	   ,parsed_line.gross_wgt
	   ,parsed_line.gross_lbs
	   ,parsed_line.charges
	   ,parsed_data.foreign_port
	 FROM zones_ftz214.filing_detail detail
      JOIN zones_ftz214.inbound inbnd
        ON inbnd.id = detail.inbound_id
	  LEFT JOIN zones_ftz214.inbound_parsed_data parsed_data
		ON inbnd.id = parsed_data.id

	  LEFT JOIN zones_ftz214.inbound_parsed_line parsed_line
		ON inbnd.id = parsed_line.id

      LEFT JOIN dbo.Clients AS clients
		ON inbnd.applicant_id = clients.id
	WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO


