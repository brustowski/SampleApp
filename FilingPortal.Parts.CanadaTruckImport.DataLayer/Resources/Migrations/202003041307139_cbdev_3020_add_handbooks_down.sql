UPDATE rule_importer
SET product_code = COALESCE(product_code.code,product_code)
FROM canada_imp_truck.rule_importer AS rule_importer
JOIN canada_imp_truck.handbook_product_code AS product_code
  ON rule_importer.product_code_id = product_code.id;
GO

-- add invoice line record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_lines';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  DECLARE @invoiceNumber INT;
  SELECT
    @invoiceNumber = header.cid_invoice_no
  FROM canada_imp_truck.invoice_headers AS header
  WHERE header.id = header.parent_record_id;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines AS line
      WHERE line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,ld_gross_weight
       ,ld_gross_weight_uq
       ,ld_invoice_qty      
       ,ld_invoice_qty_uq
       ,line_price
       ,dt_line_value
       ,ld_origin_state
       ,product_code
       ,invoice_no)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_carrier.invoice_qty
       ,rule_importer.invoice_uq
       ,inbnd.line_price
       ,inbnd.line_price
       ,rule_importer.org_state
       ,rule_importer.product_code
       ,@invoiceNumber
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer AS rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_carrier AS rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import
      WHERE detail.filing_header_id = @filingHeaderId;

    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,GETDATE()
     ,@filingUser
    FROM canada_imp_truck.filing_detail AS detail
    JOIN canada_imp_truck.inbound AS inbnd
      ON inbnd.id = detail.inbound_id
    WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT;
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs;

    OPEN cur;

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    EXEC canada_imp_truck.sp_add_invoice_line_charge @filingHeaderId
                                                    ,@recordId
                                                    ,@filingUser
                                                    ,@operationId

    EXEC canada_imp_truck.sp_add_invoice_line_duties_and_taxes @filingHeaderId
                                                              ,@recordId
                                                              ,@filingUser
                                                              ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END
  END
END;
GO