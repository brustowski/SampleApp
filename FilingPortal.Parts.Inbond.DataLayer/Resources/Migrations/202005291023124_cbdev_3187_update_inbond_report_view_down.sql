ALTER VIEW inbond.v_report 
AS SELECT
  header.id AS filing_header_id
 ,detail.Z_FK AS inbound_record_id

 ,main_detail.importer AS main_detail_importer
 ,main_detail.importer_address AS main_detail_importer_address
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
 ,client_address.code AS bill_consignee_address
 ,addr.is_overriden AS bill_consignee_address_overriden
 ,addr.address1 AS bill_consignee_address_address1
 ,addr.address2 AS bill_consignee_address_address2
 ,addr.country_code AS bill_consignee_address_country_code
 ,addr.city AS bill_consignee_address_city
 ,addr.postal_code AS bill_consignee_address_postal_code
 ,addr.state_code AS bill_consignee_address_state_code
 ,addr.company_name AS bill_consignee_address_company_name
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
 ,REPLACE(commodities.description, CHAR(10), CHAR(32)) AS commodities_description
 ,REPLACE(commodities.marks_and_numbers, CHAR(10), CHAR(32)) AS commodities_marks_and_numbers
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
LEFT JOIN dbo.app_addresses AS addr
  ON addr.id = bill.consignee_address_id
LEFT JOIN dbo.Client_Addresses AS client_address
  ON client_address.client_id = addr.cw_address_id
WHERE header.mapping_status = 2
GO