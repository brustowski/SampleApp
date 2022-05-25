-- add invoice line record --
ALTER PROCEDURE zones_entry.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'zones_entry.invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.invoice_line
    WHERE filing_header_id = @filingHeaderId
  END

  INSERT INTO zones_entry.invoice_line (filing_header_id
  , parent_record_id
  , operation_id
  , line_no
  , tariff
  , zone_status
  , origin
  , export
  , customs_qty
  , invoice_qty
  , invoice_qty_unit
  , line_price
  , gross_weight
  , manufacturer
  , goods_description
  , invoice_no
  , amount
  , charges
  , ftz_date
  , transaction_related
  , created_date
  , created_user)
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,inbnd_lines.item_no
     ,inbnd_lines.hts
     ,inbnd_lines.ftz_status
     ,inbnd_lines.country_of_origin
     ,inbnd_lines.country_of_origin
     ,inbnd_lines.ftz_manifest_qty
     ,inbnd_lines.ftz_manifest_qty
     ,COALESCE(sb_tariff.Unit, tariff.Unit)
     ,inbnd_lines.item_value
     ,dbo.fn_app_weight_to_ton(inbnd_lines.ftz_manifest_qty, tariff.Unit)
     ,inbnd_lines.manufacturers_id_no
     ,rule_importer.goods_description
     ,invoice_header.invoice_no
     ,inbnd_lines.charges
     ,inbnd_lines.charges
     ,inbnd_lines.ftz_date
     ,inbnd_lines.transaction_related
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail detail
    JOIN zones_entry.inbound inbnd
      ON inbnd.id = detail.inbound_id
    LEFT JOIN zones_entry.inbound_lines inbnd_lines
      ON inbnd.id = inbnd_lines.inbound_record_id
    LEFT JOIN dbo.Tariff tariff
      ON inbnd_lines.hts = tariff.USC_Tariff
    LEFT JOIN dbo.SchB_Tariff sb_tariff
      ON inbnd_lines.hts = sb_tariff.UB_Tariff
    LEFT JOIN zones_entry.rule_importer rule_importer
      ON inbnd.importer_id = rule_importer.importer_id
    INNER JOIN zones_entry.invoice_header invoice_header
      ON invoice_header.id = @parentId
    WHERE detail.filing_header_id = @filingHeaderId
END;
GO

-- add invoice header record --
ALTER PROCEDURE zones_entry.sp_add_invoice_header (@filingHeaderId INT,
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
  FROM zones_entry.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 0
  BEGIN
    DELETE FROM zones_entry.invoice_header
    WHERE filing_header_id = @filingHeaderId
  END
  INSERT INTO zones_entry.invoice_header (filing_header_id
  , parent_record_id
  , operation_id
  , consignee
  , supplier
  , supplier_address
  , invoice_total
  , seller
  , manufacturer
  , importer
  , sold_to_party
  , ship_to_party
  , created_date
  , created_user)
  OUTPUT INSERTED.ID INTO @IDs (ID)
    SELECT DISTINCT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,rule_importer.consignee
     ,rule_importer.supplier
     ,(SELECT TOP (1)
          lines.manufacturers_id_no
        FROM zones_entry.inbound_lines lines
        WHERE lines.inbound_record_id = inbnd.id)
     ,parsed_data.total_entered_value
     ,rule_importer.seller
     ,rule_importer.manufacturer
     ,c.ClientCode
     ,rule_importer.sold_to_party
     ,rule_importer.ship_to_party
     ,GETDATE()
     ,@filingUser
    FROM zones_entry.filing_detail detail
    JOIN zones_entry.inbound inbnd
      ON inbnd.id = detail.inbound_id
    LEFT JOIN Clients c
      ON inbnd.importer_id = c.id
    LEFT JOIN zones_entry.rule_importer rule_importer
      ON inbnd.importer_id = rule_importer.importer_id
    LEFT JOIN zones_entry.inbound_parsed_data parsed_data
      ON inbnd.id = parsed_data.id
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
  EXEC zones_entry.sp_add_invoice_line @filingHeaderId
                                      ,@recordId
                                      ,@filingUser
                                      ,@operationId

  FETCH NEXT FROM cur INTO @recordId

  END

  CLOSE cur
  DEALLOCATE cur

END;
GO