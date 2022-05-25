UPDATE inbnd
SET vendor_id = rule_importer.vendor_id
   ,product_code_id = rule_importer.product_code_id
   ,consignee_id = rule_importer.consignee_id
FROM canada_imp_truck.inbound AS inbnd
JOIN canada_imp_truck.rule_importer AS rule_importer
  ON inbnd.importer_id = rule_importer.importer_id;

UPDATE inbnd
SET gross_weight = COALESCE(rule_carrier.gross_weight, 0)
   ,invoice_qty = COALESCE(rule_carrier.invoice_qty, 0)
   ,line_price = COALESCE(inbnd.line_price, 0)
FROM canada_imp_truck.inbound AS inbnd
LEFT JOIN canada_imp_truck.rule_carrier AS rule_carrier
  ON inbnd.carrier_at_import = rule_carrier.carrier;
GO

INSERT INTO canada_imp_truck.rule_vendor (
    vendor_id
   ,importer_id
   ,exporter_id
   ,export_state
   ,direct_ship_place
   ,no_packages
   ,country_of_origin
   ,org_state
   ,created_date
   ,created_user)
  SELECT
    rule_importer.vendor_id
   ,rule_importer.importer_id
   ,rule_importer.exporter_id
   ,rule_importer.export_state
   ,rule_importer.direct_ship_place
   ,rule_importer.no_packages
   ,rule_importer.country_of_origin
   ,rule_importer.org_state
   ,rule_importer.created_date
   ,rule_importer.created_user
  FROM canada_imp_truck.rule_importer AS rule_importer;

;
WITH cte
AS
  (SELECT DISTINCT
      inbnd.product_code_id
    FROM canada_imp_truck.inbound AS inbnd
	where inbnd.product_code_id IS NOT NULL)
INSERT INTO canada_imp_truck.rule_product (
    product_code_id
   ,gross_weight_unit
   ,packages_unit
   ,invoice_uq
   ,created_date
   ,created_user)
  SELECT
    product_code_id
   ,''
   ,''
   ,''
   ,GETDATE()
   ,SUSER_NAME()
  FROM cte;
GO

ALTER VIEW canada_imp_truck.v_inbound_grid 
AS SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,vendor.ClientCode AS vendor
 ,inbnd.port
 ,inbnd.pars_number
 ,inbnd.eta
 ,inbnd.owners_reference
 ,inbnd.gross_weight
 ,inbnd.direct_ship_date
 ,consignee.ClientCode AS consignee
 ,product.code AS product
 ,inbnd.invoice_qty
 ,inbnd.line_price
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,has_vendor_rule.value AS has_vendor_rule
 ,has_port_rule.value AS has_port_rule
 ,has_product_rule.value AS has_product_rule
 ,CAST(has_vendor_rule.value & has_port_rule.value & has_product_rule.value AS BIT) AS has_all_required_rules
 ,inbnd.deleted AS is_deleted
FROM canada_imp_truck.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
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
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import_truck_canada'

LEFT JOIN dbo.Clients AS vendor
  ON inbnd.vendor_id = vendor.id
LEFT JOIN dbo.Clients AS consignee
  ON inbnd.consignee_id = consignee.id
LEFT JOIN canada_imp_truck.handbook_product_code AS product
  ON inbnd.product_code_id = product.id
LEFT JOIN canada_imp_truck.rule_vendor AS rule_vendor
  ON inbnd.vendor_id = rule_vendor.vendor_id
LEFT JOIN canada_imp_truck.rule_port AS rule_port
  ON inbnd.port = rule_port.port_of_clearance
LEFT JOIN canada_imp_truck.rule_product AS rule_product
  ON inbnd.product_code_id = rule_product.product_code_id

CROSS APPLY (SELECT
    IIF(rule_vendor.id IS NULL, 0, 1) AS value) AS has_vendor_rule
CROSS APPLY (SELECT
    IIF(rule_port.id IS NULL, 0, 1) AS value) AS has_port_rule
CROSS APPLY (SELECT
    IIF(rule_product.id IS NULL, 0, 1) AS value) AS has_product_rule

WHERE inbnd.deleted = 0
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
       ,dbo.fn_get_client_code(rule_vendor.importer_id)
       ,inbnd.port
       ,SUBSTRING(inbnd.pars_number, 1, 4)
       ,dbo.fn_get_client_code(rule_vendor.vendor_id)
       ,inbnd.gross_weight
       ,rule_product.gross_weight_unit
       ,rule_vendor.no_packages
       ,rule_product.packages_unit
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
      LEFT JOIN canada_imp_truck.rule_vendor AS rule_vendor
        ON rule_vendor.vendor_id = inbnd.vendor_id
      LEFT JOIN canada_imp_truck.rule_port AS rule_port
        ON rule_port.port_of_clearance = inbnd.port
      LEFT JOIN canada_imp_truck.rule_product AS rule_product
        ON rule_product.product_code_id = inbnd.product_code_id

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
       ,dbo.fn_get_client_code(rule_vendor.vendor_id)
       ,inbnd.gross_weight
       ,rule_product.gross_weight_unit
       ,dbo.fn_get_client_code(inbnd.consignee_id)
       ,dbo.fn_get_client_code(rule_vendor.exporter_id)
       ,rule_vendor.no_packages
       ,rule_product.packages_unit
       ,inbnd.line_price
       ,rule_vendor.direct_ship_place
       ,rule_vendor.country_of_origin
       ,rule_vendor.org_state
       ,rule_vendor.export_state
       ,inbnd.direct_ship_date
      FROM canada_imp_truck.filing_detail AS detail
      JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_vendor AS rule_vendor
        ON rule_vendor.vendor_id = inbnd.vendor_id
      LEFT JOIN canada_imp_truck.rule_product AS rule_product
        ON rule_product.product_code_id = inbnd.product_code_id
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
       ,inbnd.gross_weight
       ,rule_product.gross_weight_unit
       ,inbnd.invoice_qty
       ,rule_product.invoice_uq
       ,inbnd.line_price
       ,inbnd.line_price
       ,rule_vendor.org_state
       ,product_code.code
       ,@invoiceNumber
      FROM canada_imp_truck.filing_detail AS detail
      INNER JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN canada_imp_truck.rule_vendor  AS rule_vendor
        ON rule_vendor.vendor_id = inbnd.vendor_id
      LEFT JOIN canada_imp_truck.rule_product AS rule_product
        ON rule_product.product_code_id = inbnd.product_code_id
      LEFT JOIN canada_imp_truck.handbook_product_code AS product_code
        ON inbnd.product_code_id = product_code.id
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

-- add invoice line record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line_charge (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_lines_charges'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration section
  WHERE section.table_name = @tableName

  -- add charges data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines_charges AS charge
      WHERE charge.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines_charges (
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
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO

-- add invoice line record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line_duties_and_taxes (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'canada_imp_truck.invoice_lines_duties_and_taxes'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM canada_imp_truck.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add duties and tax data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM canada_imp_truck.invoice_lines_duties_and_taxes AS tax
      WHERE tax.parent_record_id = @parentId)
  BEGIN
    INSERT INTO canada_imp_truck.invoice_lines_duties_and_taxes (
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
       ,@filingUser
      FROM canada_imp_truck.filing_detail AS detail
      JOIN canada_imp_truck.inbound AS inbnd
        ON inbnd.id = detail.inbound_id

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO