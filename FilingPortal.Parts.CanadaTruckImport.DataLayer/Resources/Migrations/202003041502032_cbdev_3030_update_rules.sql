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
       ,rule_carrier.gross_weight_unit
       ,rule_carrier.invoice_qty
       ,rule_carrier.invoice_uq
       ,inbnd.line_price
       ,inbnd.line_price
       ,rule_importer.org_state
       ,product_code.code
       ,@invoiceNumber
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_importer AS rule_importer
        ON rule_importer.importer_id = inbnd.importer_id
      LEFT JOIN canada_imp_truck.rule_carrier AS rule_carrier
        ON rule_carrier.carrier = inbnd.carrier_at_import
      LEFT JOIN canada_imp_truck.handbook_product_code AS product_code
        ON rule_importer.product_code_id = product_code.id
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
      ,cid_country_of_origin
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
       ,rule_carrier.gross_weight_unit
       ,dbo.fn_get_client_code(rule_importer.consignee_id)
       ,dbo.fn_get_client_code(rule_importer.exporter_id)
       ,rule_importer.no_packages
       ,rule_importer.packages_unit
       ,inbnd.line_price
       ,rule_importer.direct_ship_place
        ,rule_importer.country_of_origin
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
       ,rule_carrier.gross_weight
       ,rule_carrier.gross_weight_unit
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

-- set default value for Transport Mode and Service Option
DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.declaration';
DECLARE @columnName VARCHAR(128) = 'st_transport';
DECLARE @value VARCHAR(128) = 'ROA'; 

DECLARE @command VARCHAR(MAX);

SELECT
  @command = 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d
  ON d.parent_object_id = c.object_id
    AND d.parent_column_id = c.column_id
WHERE c.name = @columnName
AND c.object_id = OBJECT_ID(@tableName, 'U');

SET @command = COALESCE(@command, '') + 'ALTER TABLE ' + @tableName + ' ADD DEFAULT (''' + @value + ''') FOR ' + @columnName + ';' + CHAR(10);


SET @columnName = 'ro_service';
SET @value = 'IID';

SELECT
  @command = @command + 'ALTER TABLE ' + @tableName + ' DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d
  ON d.parent_object_id = c.object_id
    AND d.parent_column_id = c.column_id
WHERE c.name = @columnName
AND c.object_id = OBJECT_ID(@tableName, 'U');

SET @command = COALESCE(@command, '') + 'ALTER TABLE ' + @tableName + ' ADD DEFAULT (''' + @value + ''') FOR ' + @columnName + ';' + CHAR(10);

EXEC (@command);
GO