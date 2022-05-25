ALTER TABLE canada_imp_truck.filing_header
  ADD error_description VARCHAR(MAX) NULL;

ALTER TABLE canada_imp_truck.filing_header
ADD request_xml VARCHAR(MAX) NULL;

ALTER TABLE canada_imp_truck.filing_header
ADD response_xml VARCHAR(MAX) NULL;

ALTER TABLE canada_imp_truck.documents
ADD [status] VARCHAR(128) NULL;
GO

CREATE FUNCTION canada_imp_truck.fn_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY line.id)
    FROM canada_imp_truck.invoice_lines AS line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO

ALTER TABLE canada_imp_truck.invoice_lines
ADD unit_price DECIMAL(18, 6) NULL;

ALTER TABLE canada_imp_truck.invoice_lines
ADD line_number AS canada_imp_truck.fn_invoice_line_number(parent_record_id, id);

EXEC SP_RENAME 'canada_imp_truck.invoice_lines.ld_state'
              ,'ld_origin_state'
              ,'COLUMN';
UPDATE canada_imp_truck.form_configuration
SET column_name = 'ld_origin_state'
   ,label = 'Origin State'
WHERE column_name = 'ld_state';

ALTER TABLE canada_imp_truck.invoice_lines
ADD product_code VARCHAR(128) NULL;

ALTER TABLE canada_imp_truck.invoice_lines
ADD line_price DECIMAL(18, 6) NULL;

ALTER TABLE canada_imp_truck.invoice_lines
ADD tariff VARCHAR(128) NULL;

ALTER TABLE canada_imp_truck.invoice_lines
ADD invoice_no INT NULL;

ALTER TABLE canada_imp_truck.invoice_headers
ADD place_of_direct_shipment VARCHAR(128) NULL;

ALTER TABLE canada_imp_truck.invoice_headers
ADD direct_shipment_date DATE NULL;
GO

INSERT INTO canada_imp_truck.form_configuration (section_id, has_default_value, editable, mandatory, column_name, created_date, created_user, value, display_on_ui, manual, single_filing_order, description, label, paired_field_table, paired_field_column, handbook_name) VALUES 
(6, 0, 1, 0, 'unit_price', GETDATE(), SUSER_NAME(), NULL, 130, 0, NULL, NULL, 'Unit Price', NULL, NULL, NULL)
,(6, 0, 0, 0, 'line_number', GETDATE(), SUSER_NAME(), NULL, 131, 0, NULL, NULL, 'Line Number', NULL, NULL, NULL)
,(6, 0, 1, 0, 'product_code', GETDATE(), SUSER_NAME(), NULL, 132, 0, NULL, NULL, 'Product Code', NULL, NULL, NULL)
,(6, 0, 1, 0, 'line_price', GETDATE(), SUSER_NAME(), NULL, 133, 0, NULL, NULL, 'Line Price', NULL, NULL, NULL)
,(6, 0, 1, 0, 'tariff', GETDATE(), SUSER_NAME(), NULL, 134, 0, NULL, NULL, 'Tariff', NULL, NULL, NULL)
,(6, 0, 1, 0, 'invoice_no', GETDATE(), SUSER_NAME(), NULL, 135, 0, NULL, NULL, 'Invoice No', NULL, NULL, NULL)
,(5, 0, 1, 0, 'place_of_direct_shipment', GETDATE(), SUSER_NAME(), NULL, 136, 0, NULL, NULL, 'Place of Direct Shipment', NULL, NULL, NULL)
,(5, 0, 1, 0, 'direct_shipment_date', GETDATE(), SUSER_NAME(), NULL, 137, 0, NULL, NULL, 'Direct Shipment Date', NULL, NULL, NULL);
GO

-- add invoice line record --
ALTER PROCEDURE canada_imp_truck.sp_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'invoice_lines';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

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
       ,ld_gross_weight
       ,ld_gross_weight_uq
       ,ld_invoice_qty
       ,ld_customs_qty
       ,line_price
       ,dt_line_value
       ,unit_price
       ,ld_origin_state
       ,product_code
       ,invoice_no
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_carrier.gross_weight
       ,rule_importer.gross_weight_unit
       ,rule_carrier.invoice_qty
       ,rule_carrier.invoice_qty
       ,rule_carrier.invoice_qty * rule_importer.unit_price
       ,rule_carrier.invoice_qty * rule_importer.unit_price
       ,rule_importer.unit_price
       ,rule_importer.org_state
       ,rule_importer.product_code
       ,rule_importer.inv_number
       ,GETDATE()
       ,@filingUser
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
    INSERT INTO canada_imp_truck.invoice_headers (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,oa_vendor
       ,cid_inv_gross_weight
       ,cid_inv_gross_weight_uq
       ,cid_invoice_no
       ,cid_packs
       ,oa_consignee
       ,oa_exporter
       ,cid_packs_uq
       ,cid_inv_total_amount
       ,place_of_direct_shipment
       ,created_date
       ,created_user)
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

  -- quantity, unit_price
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,quantity
     ,unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(28, 15), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
        AND a.table_name = 'canada_imp_truck.invoice_lines'
        AND a.column_name = 'ld_invoice_qty'
        AND b.column_name = 'unit_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line customs qty
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity, '0.#####')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'canada_imp_truck.invoice_lines'
    AND column_name IN ('ld_customs_qty');
  -- invoice line line_price
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * unit_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'canada_imp_truck.invoice_lines'
    AND column_name IN ('line_price', 'dt_line_value');

  -- invoice header invoice total
  DECLARE @total DECIMAL(28, 15);
  SELECT
    @total = SUM(quantity * unit_price)
  FROM @tbl;
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

CREATE VIEW canada_imp_truck.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_id

 ,declaration.main_vendor AS declaration_main_vendor
 ,declaration.importer AS declaration_importer
 ,declaration.shipment_type AS declaration_shipment_type
 ,declaration.st_transport AS declaration_st_transport
 ,declaration.st_container AS declaration_st_container
 ,declaration.st_b3_entry_t AS declaration_st_b3_entry_t
 ,declaration.st_service AS declaration_st_service
 ,declaration.ro_service AS declaration_ro_service
 ,declaration.ro_assessment AS declaration_ro_assessment
 ,declaration.ro_validate_release AS declaration_ro_validate_release
 ,declaration.ro_priority_ind AS declaration_ro_priority_ind
 ,declaration.ro_validate_cadex AS declaration_ro_validate_cadex
 ,declaration.dd_transacion_number AS declaration_dd_transacion_number
 ,declaration.dd_b3_status AS declaration_dd_b3_status
 ,declaration.dd_release_status AS declaration_dd_release_status
 ,declaration.msi_rel_submitted_time AS declaration_msi_rel_submitted_time
 ,declaration.msi_b3c_submitted_time AS declaration_msi_b3c_submitted_time
 ,declaration.td_carrier_at_import AS declaration_td_carrier_at_import
 ,declaration.td_cargo_control_no AS declaration_td_cargo_control_no
 ,declaration.td_house_bill AS declaration_td_house_bill
 ,declaration.td_registration AS td_registration
 ,declaration.td_first_port_arr AS declaration_td_first_port_arr
 ,declaration.td_eta AS declaration_td_eta
 ,declaration.td_cust_port_of_clearance AS declaration_td_cust_port_of_clearance
 ,declaration.td_sub_location AS declaration_td_sub_location
 ,declaration.td_exam_location AS declaration_td_exam_location
 ,declaration.td_estimated_release_date AS declaration_td_estimated_release_date
 ,declaration.td_actual_release_date AS declaration_td_actual_release_date
 ,declaration.sd_owners_reference AS declaration_sd_owners_reference
 ,declaration.sd_final_destination AS declaration_sd_final_destination
 ,declaration.sd_eta AS declaration_sd_eta
 ,declaration.sd_goods_descripion AS declaration_sd_goods_descripion
 ,declaration.sd_inco_term AS declaration_sd_inco_term
 ,declaration.sd_total_gross_weight AS declaration_sd_total_gross_weight
 ,declaration.sd_total_gross_weight_uq AS declaration_sd_total_gross_weight_uq
 ,declaration.sd_no_packages AS declaration_sd_no_packages
 ,declaration.sd_no_packages_uq AS declaration_sd_no_packages_uq

 ,invoice.cid_invoice_no AS invoice_cid_invoice_no
 ,invoice.cid_group_invoice AS invoice_cid_group_invoice
 ,invoice.cid_inv_total_amount AS invoice_cid_inv_total_amount
 ,invoice.cid_inv_total_amount_curr AS invoice_cid_inv_total_amount_curr
 ,invoice.cid_exchange_rate AS invoice_cid_exchange_rate
 ,invoice.cid_lc_xr AS invoice_cid_lc_xr
 ,invoice.cid_inco_term AS invoice_cid_inco_term
 ,invoice.cid_agreed_place AS invoice_cid_agreed_place
 ,invoice.cid_inv_gross_weight AS invoice_cid_inv_gross_weight
 ,invoice.cid_inv_gross_weight_uq AS invoice_cid_inv_gross_weight_uq
 ,invoice.cid_packs AS invoice_cid_packs
 ,invoice.cid_packs_uq AS invoice_cid_packs_uq
 ,invoice.cid_inv_net_weight AS invoice_cid_inv_net_weight
 ,invoice.cid_inv_net_weight_uq AS invoice_cid_inv_net_weight_uq
 ,invoice.cid_country_of_origin AS invoice_cid_country_of_origin
 ,invoice.cid_country_of_origin_state AS invoice_cid_country_of_origin_state
 ,invoice.cid_country_of_export AS invoice_cid_country_of_export
 ,invoice.cid_country_of_export_state AS invoice_cid_country_of_export_state
 ,invoice.cid_country_of_source AS invoice_cid_country_of_source
 ,invoice.cid_country_of_source_state AS invoice_cid_country_of_source_state
 ,invoice.cid_region AS invoice_cid_region
 ,invoice.cid_tranship_country AS invoice_cid_tranship_country
 ,invoice.oa_vendor AS invoice_oa_vendor
 ,invoice.oa_shipper AS invoice_oa_shipper
 ,invoice.oa_originator AS invoice_oa_originator
 ,invoice.oa_exporter AS invoice_oa_exporter
 ,invoice.oa_purchaser AS invoice_oa_purchaser
 ,invoice.oa_consignee AS invoice_oa_consignee
 ,invoice.oa_manufacturer AS invoice_oa_manufacturer
 ,invoice.place_of_direct_shipment AS invoice_place_of_direct_shipment
 ,invoice.direct_shipment_date AS invoice_direct_shipment_date

 ,line.ld_class_tariff AS line_ld_class_tariff
 ,line.ld_customs_qty AS line_ld_customs_qty
 ,line.ld_customs_qty_uq AS line_ld_customs_qty_uq
 ,line.ld_tariff_treatment_code AS line_ld_tariff_treatment_code
 ,line.ld_customs_qty_2 AS line_ld_customs_qty_2
 ,line.ld_customs_qty_2_uq AS line_ld_customs_qty_2_uq
 ,line.ld_value_for_duty_code AS line_ld_value_for_duty_code
 ,line.ld_customs_qty_3 AS line_ld_customs_qty_3
 ,line.ld_customs_qty_3_uq AS line_ld_customs_qty_3_uq
 ,line.ld_sima_measure AS line_ld_sima_measure
 ,line.ld_goods_desc AS line_ld_goods_desc
 ,line.ld_commodity_code AS line_ld_commodity_code
 ,line.ld_invoice_qty AS line_ld_invoice_qty
 ,line.ld_invoice_qty_uq AS line_ld_invoice_qty_uq
 ,line.ld_gross_weight AS line_ld_gross_weight
 ,line.ld_gross_weight_uq AS line_ld_gross_weight_uq
 ,line.ld_volume AS line_ld_volume
 ,line.ld_volume_uq AS line_ld_volume_uq
 ,line.ld_price AS line_ld_price
 ,line.ld_price_curr AS line_ld_price_curr
 ,line.ld_goods_origin AS line_ld_goods_origin
 ,line.ld_origin_state AS line_ld_origin_state
 ,line.ld_invoice_bill AS line_ld_invoice_bill
 ,line.ld_region AS line_ld_region
 ,line.ld_manufacturer AS line_ld_manufacturer
 ,line.dt_customs_qty AS line_dt_customs_qty
 ,line.dt_customs_qty_uq AS line_dt_customs_qty_uq
 ,line.dt_customs_qty_2 AS line_dt_customs_qty_2
 ,line.dt_customs_qty_2_uq AS line_dt_customs_qty_2_uq
 ,line.dt_customs_qty_3 AS line_dt_customs_qty_3
 ,line.dt_customs_qty_3_uq AS line_dt_customs_qty_3_uq
 ,line.dt_line_value AS line_dt_line_value
 ,line.dt_value_or_percent AS line_dt_value_or_percent
 ,line.dt_adjustment AS line_dt_adjustment
 ,line.dt_vfcc AS line_dt_vfcc
 ,line.dt_vfcc_override AS line_dt_vfcc_override
 ,line.dt_exchange_rate AS line_dt_exchange_rate
 ,line.dt_vfd AS line_dt_vfd
 ,line.dt_vfd_override AS line_dt_vfd_override
 ,line.dt_vft AS line_dt_vft
 ,line.unit_price AS line_unit_price
 ,line.line_number AS line_line_number
 ,line.product_code AS line_product_code
 ,line.line_price AS line_line_price
 ,line.tariff AS line_tariff
 ,line.invoice_no AS line_invoice_no

 ,charge.code AS charge_code
 ,charge.[desc] AS charge_desc
 ,charge.amount AS charge_amount
 ,charge.curr AS charge_curr
 ,charge.dutiable AS charge_dutiable
 ,charge.cif_component AS charge_cif_component
 ,charge.percent_of_line_price AS charge_percent_of_line_price
 ,charge.included_in_invoice AS charge_included_in_invoice

 ,duty_tax.type AS duty_tax_type
 ,duty_tax.code AS code
 ,duty_tax.description AS duty_tax_description
 ,duty_tax.exempt AS duty_tax_exempt
 ,duty_tax.ovr AS duty_tax_ovr
 ,duty_tax.rate AS duty_tax_rate
 ,duty_tax.rate_type AS duty_tax_rate_type
 ,duty_tax.quantity AS duty_tax_quantity
 ,duty_tax.uom AS duty_tax_uom
 ,duty_tax.amount AS duty_tax_amount
 ,duty_tax.normal_value_per_unit AS duty_tax_normal_value_per_unit
 ,duty_tax.normal_value_curr AS duty_tax_normal_value_curr
 ,duty_tax.foreign_rate AS duty_tax_foreign_rate
 ,duty_tax.foreign_curr AS duty_tax_foreign_curr
 ,duty_tax.foreign_curr_exchange_rate AS duty_tax_foreign_curr_exchange_rate

FROM canada_imp_truck.filing_header AS header
JOIN canada_imp_truck.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN canada_imp_truck.declaration AS declaration
  ON header.id = declaration.filing_header_id
JOIN canada_imp_truck.invoice_headers AS invoice
  ON header.id = invoice.filing_header_id
JOIN canada_imp_truck.invoice_lines AS line
  ON invoice.id = line.parent_record_id
JOIN canada_imp_truck.invoice_lines_charges AS charge
  ON line.id = charge.parent_record_id
JOIN canada_imp_truck.invoice_lines_duties_and_taxes AS duty_tax
  ON line.id = duty_tax.parent_record_id
WHERE header.mapping_status = 2;
GO