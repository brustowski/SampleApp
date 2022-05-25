-- add invoice header record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_headers'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_headers AS header
      WHERE header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_headers (filing_header_id
    , parent_record_id
    , operation_id
    , oa_vendor
    , cid_inv_gross_weight
    , cid_inv_gross_weight_uq
    , cid_invoice_no
    , cid_packs
    , oa_consignee
    , oa_exporter
    , cid_packs_uq
    , cid_inv_total_amount
    , place_of_direct_shipment
    , cid_country_of_origin_state
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_importer.inv_number
       ,rule_importer.packs
       ,dbo.fn_get_client_code(rule_importer.consignee_id)
       ,dbo.fn_get_client_code(rule_importer.exporter_id)
       ,rule_importer.packages_unit
       ,rule_carrier.invoice_qty * rule_importer.unit_price
       ,rule_importer.direct_ship_place
       ,rule_importer.org_state
       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import
      WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC canada_imp_truck.sp_add_invoice_line @filingHeaderId
                                             ,@recordId
                                             ,@filingUser
                                             ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO