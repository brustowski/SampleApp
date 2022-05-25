ALTER VIEW canada_imp_truck.v_inbound_grid 
AS SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbnd.carrier_at_import
 ,inbnd.port
 ,inbnd.pars_number
 ,inbnd.eta
 ,inbnd.owners_reference
 ,inbnd.direct_ship_date
 ,inbnd.line_price
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_importer.id IS NULL THEN 0
    ELSE 1
  END AS has_importer_rule
 ,CASE
    WHEN rule_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
 ,CASE
    WHEN rule_carrier.id IS NULL THEN 0
    ELSE 1
  END AS has_carrier_rule
 ,inbnd.deleted AS is_deleted
FROM canada_imp_truck.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.mapping_status
   ,fh.filing_status
  FROM canada_imp_truck.filing_header AS fh
  JOIN canada_imp_truck.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

LEFT JOIN Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN canada_imp_truck.rule_importer AS rule_importer
  ON inbnd.importer_id = rule_importer.importer_id
LEFT JOIN canada_imp_truck.rule_port AS rule_port
  ON inbnd.port = rule_port.port_of_clearance
LEFT JOIN canada_imp_truck.rule_carrier AS rule_carrier
  ON inbnd.carrier_at_import = rule_carrier.carrier

WHERE inbnd.deleted = 0
GO

-- add invoice header record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_headers'
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
    INSERT INTO canada_imp_truck.invoice_headers (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,oa_vendor
       ,cid_inv_gross_weight
       ,cid_inv_gross_weight_uq
       ,oa_consignee
       ,oa_exporter
       ,cid_packs
       ,cid_packs_uq
       ,cid_inv_total_amount
       ,place_of_direct_shipment
       ,cid_country_of_origin_state
       ,cid_country_of_export_state
       ,direct_shipment_date)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,dbo.fn_get_client_code(rule_importer.consignee_id)
       ,dbo.fn_get_client_code(rule_importer.exporter_id)
       ,rule_importer.no_packages
       ,rule_importer.packages_unit
       ,inbnd.line_price
       ,rule_importer.direct_ship_place
       ,rule_importer.org_state
       ,rule_importer.export_state
       ,inbnd.direct_ship_date
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

  -- line price
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,line_price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,line_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS line_price
    FROM @config a
    WHERE a.table_name = 'canada_imp_truck.invoice_lines'
    AND a.column_name = 'line_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line line_value
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(line_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'canada_imp_truck.invoice_lines'
    AND column_name = 'dt_line_value';

  -- invoice header invoice total
  DECLARE @total DECIMAL(28, 15);
  SELECT @total = SUM(line_price) FROM @tbl;
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'canada_imp_truck.invoice_headers'
    AND column_name = 'cid_inv_total_amount';

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
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

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.declaration'
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
       ,created_date
       ,created_user
       ,importer
       ,td_cust_port_of_clearance
       ,td_carrier_at_import
       ,main_vendor
       ,st_transport
       ,ro_service
       ,sd_total_gross_weight
       ,sd_total_gross_weight_uq
       ,sd_no_packages
       ,sd_no_packages_uq
       ,td_sub_location
       ,td_first_port_arr
       ,sd_final_destination
       ,sd_owners_reference
       ,td_exam_location
       ,td_cargo_control_no
       ,sd_eta
       ,td_eta)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,dbo.fn_get_client_code(inbnd.importer_id)
       ,inbnd.port
       ,inbnd.carrier_at_import
       ,dbo.fn_get_client_code(rule_importer.vendor_id)
       ,rule_importer.transport
       ,rule_importer.service_option
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_importer.no_packages
       ,rule_importer.packages_unit
       ,rule_port.sub_location
       ,rule_port.first_port_of_arrival
       ,rule_port.final_destination
       ,inbnd.owners_reference
       ,rule_port.sub_location
       ,inbnd.pars_number
       ,inbnd.eta
       ,inbnd.eta
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

UPDATE canada_imp_truck.form_section_configuration
SET procedure_name = 'sp_add_declaration'
WHERE name = 'declaration';
UPDATE canada_imp_truck.form_section_configuration
SET procedure_name = 'sp_add_invoice_header'
WHERE name = 'invoice_header';
GO