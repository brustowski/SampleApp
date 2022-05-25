ALTER TABLE canada_imp_truck.filing_header
DROP COLUMN error_description;

ALTER TABLE canada_imp_truck.filing_header
DROP COLUMN request_xml;

ALTER TABLE canada_imp_truck.filing_header
DROP COLUMN response_xml;

ALTER TABLE canada_imp_truck.documents
DROP COLUMN [status];
GO

DELETE FROM canada_imp_truck.form_configuration
WHERE column_name IN ('unit_price', 'line_number', 'product_code', 'line_price', 'tariff', 'invoice_no', 'place_of_direct_shipment', 'direct_shipment_date');
GO

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN unit_price;

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN line_number;

EXEC SP_RENAME 'canada_imp_truck.invoice_lines.ld_origin_state'
              ,'ld_state'
              ,'COLUMN';
UPDATE canada_imp_truck.form_configuration
SET column_name = 'ld_state'
   ,label = 'State'
WHERE column_name = 'ld_origin_state';

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN product_code;

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN line_price;

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN tariff;

ALTER TABLE canada_imp_truck.invoice_lines
DROP COLUMN invoice_no;

ALTER TABLE canada_imp_truck.invoice_headers
DROP COLUMN place_of_direct_shipment;

ALTER TABLE canada_imp_truck.invoice_headers
DROP COLUMN direct_shipment_date;
GO

DROP FUNCTION canada_imp_truck.fn_invoice_line_number;
GO

-- add invoice line record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_lines'
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
      FROM canada_imp_truck.invoice_lines AS line
      WHERE line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines (filing_header_id
    , parent_record_id
    , operation_id
    , ld_gross_weight
    , ld_gross_weight_uq
    , ld_invoice_qty
    , created_date
    , created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_carrier.invoice_qty
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
    SELECT
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,GETDATE()
     ,@filingUser
    FROM canada_imp_truck.filing_detail detail
    JOIN canada_imp_truck.inbound inbnd
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

-- add declaration record --
ALTER PROCEDURE canada_imp_truck.sp_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'declaration'
  DECLARE @allowMultiple BIT = 0;

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
      FROM canada_imp_truck.declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO canada_imp_truck.declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,importer
       ,td_cust_port_of_clearance
       ,td_carrier_at_import
       ,main_vendor
       ,st_transport
       ,st_service
       ,ro_service
       ,sd_total_gross_weight
       ,sd_total_gross_weight_uq
       ,sd_no_packages
       ,sd_no_packages_uq
       ,td_sub_location
       ,td_first_port_arr
       ,sd_final_destination
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,dbo.fn_get_client_code(inbnd.importer_id)
       ,inbnd.port
       ,inbnd.carrier_at_import
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_importer.transport
       ,rule_importer.service_option
       ,rule_importer.service_option
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_importer.no_packages
       ,rule_importer.packages_unit
       ,rule_port.sub_location
       ,rule_port.first_port_of_arrival
       ,rule_port.final_destination
       ,GETDATE()
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_port rule_port
        ON rule_port.port_of_clearance = inbnd.port
      LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

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

-- recalculate rail fileds
ALTER PROCEDURE canada_imp_truck.sp_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN canada_imp_truck.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN canada_imp_truck.form_section_configuration section
      ON conf.section_id = section.id;

  DECLARE @tblUpdatedFields field_update_list;
  -- calculate new values

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

DROP VIEW canada_imp_truck.v_report;
GO