ALTER TABLE inbond.main_detail
ADD port_of_presentation VARCHAR(128) NULL;
GO

DECLARE @dou INT;
SELECT
  @dou = MAX(display_on_ui) + 1
FROM inbond.form_configuration AS config
WHERE config.section_id = 2;

INSERT INTO inbond.form_configuration (section_id, column_name, label, has_default_value, editable, mandatory, created_date, created_user, display_on_ui, manual)
VALUES (2, 'port_of_presentation', 'Port of Presenation', 0, 1, 0, GETDATE(), SUSER_NAME(),@dou, 0);
GO

-- add main detail record --
ALTER PROCEDURE inbond.sp_add_main_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.main_detail'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.main_detail AS main_detail
      WHERE main_detail.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.main_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,importer
       ,firms_code
       ,transport_mode
       ,carrier_code
       ,conveyance
       ,importing_carrier_port_of_arrival
       ,voyage_trip_no
       ,branch
       ,authorized_agent
       ,entry_date
       ,eta
       ,port_of_presentation)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,importer.ClientCode
       ,cfc.firms_code
       ,rule_transport_mode.transport_mode
       ,rule_carrier.carrier
       ,inbnd.export_conveyance
       ,inbnd.port_of_arrival
       ,IIF(rule_transport_mode.transport_mode = '10', NULL, 1)
       ,SUBSTRING(user_data.Branch, 1, 5)
       ,user_data.Broker
       ,inbnd.created_date
       ,GETDATE()
       ,rule_firms_code.port_of_presentation
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN cw_firms_codes AS cfc
        ON inbnd.firms_code_id = cfc.id
      LEFT JOIN Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_carrier AS rule_carrier
        ON rule_carrier.importer_id = inbnd.importer_id
          AND rule_carrier.firms_code_id = inbnd.firms_code_id
          AND rule_carrier.consignee_id = inbnd.consignee_id
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.importer_id = inbnd.importer_id
          AND rule_transport_mode.firms_code_id = inbnd.firms_code_id
          AND rule_transport_mode.consignee_id = inbnd.consignee_id
          AND rule_transport_mode.carrier = rule_carrier.carrier
      LEFT JOIN inbond.rule_firms_code AS rule_firms_code
        ON rule_firms_code.firms_code_id = inbnd.firms_code_id
          AND rule_firms_code.carrier = rule_carrier.carrier
          AND rule_firms_code.consignee_id = inbnd.consignee_id
          AND rule_firms_code.manifest_qty = inbnd.manifest_qty
          AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode
      LEFT JOIN dbo.app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

ALTER VIEW inbond.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.Z_FK AS inbound_record_id

 ,main_detail.importer AS main_detail_importer
 ,main_detail.supplier AS main_detail_supplier
 ,main_detail.branch AS main_detail_branch
 ,main_detail.mode AS main_detail_mode
 ,main_detail.move_from_whs_ftz AS main_detail_move_from_whs_ftz
 ,main_detail.firms_code AS main_detail_firms_code
 ,main_detail.transport_mode AS main_detail_transport_mode
 ,main_detail.carrier_code AS main_detail_carrier_code
 ,main_detail.conveyance AS main_detail_conveyance
 ,main_detail.voyage_trip_no AS main_detail_voyage_trip_no
 ,main_detail.carrier_country AS main_detail_carrier_country
 ,main_detail.port_of_loading AS main_detail_port_of_loading
 ,main_detail.country_of_export AS main_detail_country_of_export
 ,main_detail.importing_carrier_port_of_arrival AS main_detail_importing_carrier_port_of_arrival
 ,main_detail.date_of_sailing AS main_detail_date_of_sailing
 ,main_detail.date_of_export AS main_detail_date_of_export
 ,main_detail.eta AS main_detail_eta
 ,main_detail.port_of_presentation AS main_detail_port_of_presentation

 ,bill.issuer_code AS bill_issuer_code
 ,bill.master_bill AS bill_master_bill
 ,bill.manifest_qty AS bill_manifest_qty
 ,bill.manifest_qty_unit AS bill_manifest_qty_unit
 ,bill.weight AS bill_weight
 ,bill.weight_unit AS bill_weight_unit
 ,bill.port_of_lading_schedule_k AS bill_port_of_lading_schedule_k
 ,bill.shipper AS bill_shipper
 ,bill.consignee AS bill_consignee
 ,bill.notify_party AS bill_notify_party

 ,movement_heder.in_bond_number AS movement_heder_in_bond_number
 ,movement_heder.in_bond_entry_type AS movement_heder_in_bond_entry_type
 ,movement_heder.us_port_of_destination AS movement_heder_us_port_of_destination
 ,movement_heder.foreign_destination AS movement_heder_foreign_destination
 ,movement_heder.in_bond_carrier AS movement_heder_in_bond_carrier
 ,movement_heder.bta_indicator AS movement_heder_bta_indicator
 ,movement_heder.value_in_whole_dollars AS movement_heder_value_in_whole_dollars

 ,movement_detail.in_bond_number AS movement_detail_in_bond_number
 ,movement_detail.in_bond_qty AS movement_detail_in_bond_qty
 ,movement_detail.master_bill AS movement_detail_master_bill

 ,commodities.tariff AS commodities_tariff
 ,commodities.monetary_value AS commodities_monetary_value
 ,commodities.piece_count AS commodities_piece_count
 ,commodities.manifest_unit AS commodities_manifest_unit
 ,commodities.description AS commodities_description
 ,commodities.marks_and_numbers AS commodities_marks_and_numbers
 ,commodities.weight AS commodities_weight
 ,commodities.weight_unit AS commodities_weight_unit

FROM inbond.filing_header AS header
JOIN inbond.filing_detail AS detail
  ON header.id = detail.Filing_Headers_FK
JOIN inbond.main_detail AS main_detail
  ON header.id = main_detail.filing_header_id
JOIN inbond.bill AS bill
  ON header.id = bill.filing_header_id
JOIN inbond.movement_header AS movement_heder
  ON header.id = movement_heder.filing_header_id
JOIN inbond.movement_detail AS movement_detail
  ON header.id = movement_detail.filing_header_id
JOIN inbond.commodities AS commodities
  ON movement_detail.id = commodities.parent_record_id
WHERE header.mapping_status = 2;
GO