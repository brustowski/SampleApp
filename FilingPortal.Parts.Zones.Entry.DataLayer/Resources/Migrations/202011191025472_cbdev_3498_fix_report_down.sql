ALTER VIEW zones_entry.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_id

 ,declaration.main_supplier AS declaration_main_supplier
 ,declaration.importer AS declaration_importer
 ,declaration.shipment_type AS declaration_shipment_type
 ,declaration.entry_type AS declaration_entry_type
 ,declaration.message AS declaration_message
 ,declaration.rlf AS declaration_rlf
 ,declaration.live AS declaration_live
 ,declaration.enable_entry_sum AS declaration_enable_entry_sum
 ,declaration.enable_cargo_rel AS declaration_enable_cargo_rel
 ,declaration.type AS type
 ,declaration.pga_expedited_release AS declaration_pga_expedited_release
 ,declaration.stand_alone_prior_notice AS declaration_stand_alone_prior_notice
 ,declaration.immediate_delivery AS immediate_delivery
 ,declaration.consolidated_summary AS declaration_consolidated_summary
 ,declaration.issuer AS declaration_issuer
 ,declaration.ftz_number AS declaration_ftz_number
 ,declaration.trip_id AS declaration_trip_id
 ,declaration.carrier_scac AS declaration_carrier_scac
 ,declaration.loading_port AS declaration_loading_port
 ,declaration.discharge_port AS declaration_discharge_port
 ,declaration.entry_port AS declaration_entry_port
 ,declaration.dep AS declaration_dep
 ,declaration.arr AS declaration_arr
 ,declaration.arr2 AS declaration_arr2
 ,declaration.hmf AS declaration_hmf
 ,declaration.est_entry_date AS declaration_est_entry_date
 ,declaration.it_no AS declaration_it_no
 ,declaration.it_date AS declaration_it_date
 ,declaration.issuer_scac AS declaration_issuer_scac
 ,declaration.house_bill AS declaration_house_bill
 ,declaration.origin AS declaration_origin
 ,declaration.destination AS declaration_destination
 ,declaration.destination_state AS declaration_destination_state
 ,declaration.country_of_export AS declaration_country_of_export
 ,declaration.etd AS declaration_etd
 ,declaration.eta AS declaration_eta
 ,declaration.export_date AS declaration_export_date
 ,declaration.description AS declaration_description
 ,declaration.owner_ref AS declaration_owner_ref
 ,declaration.inco AS declaration_inco
 ,declaration.total_weight AS declaration_total_weight
 ,declaration.total_weight_unit AS declaration_total_weight_unit
 ,declaration.total_volume AS declaration_total_volume
 ,declaration.total_volume_unit AS declaration_total_volume_unit
 ,declaration.screening AS declaration_screening
 ,declaration.no_packages AS declaration_no_packages
 ,declaration.firms_code AS declaration_firms_code
 ,declaration.centralized_exam_site AS declaration_centralized_exam_site
 ,declaration.entry_number AS declaration_entry_number
 ,declaration.psc AS declaration_psc
 ,declaration.purchased AS declaration_purchased
 ,declaration.manual_entry AS declaration_manual_entry
 ,declaration.importer_of_record AS declaration_importer_of_record
 ,declaration.consignee AS declaration_consignee
 ,declaration.application_begin_date AS declaration_application_begin_date
 ,declaration.application_end_date AS declaration_application_end_date
 ,declaration.cbp_form_29_date AS declaration_cbp_form_29_date
 ,declaration.decl_3461_box_29 AS declaration_decl_3461_box_29
 ,declaration.auth_agent AS declaration_Kauth_agent

 ,packing.bill_type AS packing_bill_type
 ,packing.bill_num AS packing_bill_num
 ,packing.manifest_qty AS packing_manifest_qty
 ,packing.uq AS packing_uq
 ,packing.bill_issuer_scac AS packing_bill_issuer_scac
 ,packing.it_number AS packing_it_number
 ,packing.bill_number AS packing_bill_number
 ,packing.container_number AS packing_container_number
 ,packing.pack_qty AS packing_pack_qty
 ,packing.pack_type AS packing_pack_type
 ,packing.marks_and_numbers AS packing_marks_and_numbers
 ,packing.shipping_symbol AS packing_shipping_symbol

 ,invoice.invoice_no AS invoice_invoice_no
 ,invoice.supplier AS invoice_supplier
 ,invoice.supplier_address AS invoice_supplier_address
 ,invoice.inco AS invoice_inco
 ,invoice.agreed_place AS invoice_agreed_place
 ,invoice.invoice_total AS invoice_invoice_total
 ,invoice.currency AS invoice_currency
 ,invoice.origin AS invoice_origin
 ,invoice.payment_date AS invoice_payment_date
 ,invoice.consignee AS invoice_consignee
 ,invoice.consignee_address AS invoice_consignee_address
 ,invoice.inv_gross_weight AS invoice_inv_gross_weight
 ,invoice.inv_gross_weight_unit AS invoice_inv_gross_weight_unit
 ,invoice.net_weight AS invoice_net_weight
 ,invoice.net_weight_unit AS invoice_net_weight_unit
 ,invoice.export AS invoice_export
 ,invoice.export_date AS invoice_export_date
 ,invoice.first_sale AS invoice_first_sale
 ,invoice.transaction_related AS invoice_transaction_related
 ,invoice.packages AS invoice_packages
 ,invoice.packages_unit AS invoice_packages_unit
 ,invoice.manufacturer AS invoice_manufacturer
 ,invoice.manufacturer_address AS invoice_manufacturer_address
 ,invoice.seller AS invoice_seller
 ,invoice.seller_address AS invoice_seller_address
 ,invoice.importer AS invoice_importer
 ,invoice.sold_to_party AS invoice_sold_to_party
 ,invoice.sold_to_party_address AS invoice_sold_to_party_address
 ,invoice.ship_to_party AS invoice_ship_to_party
 ,invoice.ship_to_party_address AS invoice_ship_to_party_address
 ,invoice.broker_pga_contact_name AS invoice_broker_pga_contact_name
 ,invoice.broker_pga_contact_phone AS invoice_broker_pga_contact_phone
 ,invoice.broker_pga_contact_email AS invoice_broker_pga_contact_email
 ,invoice.epa_pst_cert_date AS invoice_epa_pst_cert_date
 ,invoice.epa_tsca_cert_date AS invoice_epa_tsca_cert_date
 ,invoice.epa_vne_cert_date AS invoice_epa_vne_cert_date
 ,invoice.fsis_cert_date AS invoice_fsis_cert_date
 ,invoice.fws_cert_date AS invoice_fws_cert_date
 ,invoice.lacey_act_cert_date AS invoice_lacey_act_cert_date
 ,invoice.nhtsa_cert_date AS invoice_nhtsa_cert_date
 ,invoice.landed_costing_ex_rate AS invoice_landed_costing_ex_rate

 ,line.invoice_no AS line_invoice_no
 ,line.invoice_line_number AS line_invoice_line_number
 ,line.tariff AS line_tariff
 ,line.customs_qty AS line_customs_qty
 ,line.customs_qty_unit AS line_customs_qty_unit
 ,line.prov_prog_tariff AS line_prov_prog_tariff
 ,line.export AS line_export
 ,line.origin AS line_origin
 ,line.spi AS line_spi
 ,line.product_claim AS line_product_claim
 ,line.details_set AS line_details_set
 ,line.first_sale AS line_first_sale
 ,line.dest_state AS line_dest_state
 ,line.transaction_related AS line_transaction_related
 ,line.zone_status AS line_zone_status
 ,line.goods_description AS line_goods_description
 ,line.invoice_qty AS line_invoice_qty
 ,line.invoice_qty_unit AS line_invoice_qty_unit
 ,line.line_price AS line_line_price
 ,line.gross_weight AS line_gross_weight
 ,line.gross_weight_unit AS line_gross_weight_unit
 ,line.net_weight AS line_net_weight
 ,line.net_weight_unit AS line_net_weight_unit
 ,line.volume AS line_volume
 ,line.volume_unit AS line_volume_unit
 ,line.ftz_pack_qty AS line_ftz_pack_qty
 ,line.ftz_pack_qty_unit AS line_ftz_pack_qty_unit
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.consignee AS line_consignee
 ,line.consignee_address AS line_consignee_address
 ,line.foreign_exporter AS line_foreign_exporter
 ,line.foreign_exporter_address AS line_foreign_exporter_address
 ,line.sold_to_party AS line_sold_to_party
 ,line.sold_to_party_address AS line_sold_to_party_address
 ,line.code AS line_code
 ,line.amount AS line_amount
 ,line.currency AS line_currency
 ,line.dutiable AS line_dutiable
 ,line.cif_component AS line_cif_component
 ,line.aii_charge_description AS line_aii_charge_description
 ,line.epa_tsca AS line_epa_tsca
 ,line.charges AS line_charges
 ,line.tsca_cert_indicator AS line_tsca_cert_indicator
 ,line.certifying_individual AS line_certifying_individual

 ,misc.branch AS misc_branch
 ,misc.broker AS misc_broker
 ,misc.service AS misc_service
 ,misc.merge_by AS misc_merge_by
 ,misc.missing_document_1 AS misc_missing_document_1
 ,misc.missing_document_2 AS misc_missing_document_2
 ,misc.cbp_import_specialist_team_number AS misc_cbp_import_specialist_team_number
 ,misc.oga_line_release_indicator AS misc_oga_line_release_indicator
 ,misc.entry_date_election_code AS misc_entry_date_election_code
 ,misc.entry_date AS misc_entry_date
 ,misc.estimated_entry_value AS misc_estimated_entry_value
 ,misc.shipment_usage_type_indicator AS misc_shipment_usage_type_indicator
 ,misc.general_order_number AS misc_general_order_number
 ,misc.is_express_consignment AS misc_is_express_consignment
 ,misc.tax_deferrable_indicator AS misc_tax_deferrable_indicator
 ,misc.preparer_district_port AS misc_preparer_district_port
 ,misc.designated_exam_port AS misc_designated_exam_port
 ,misc.designated_exam_site AS misc_designated_exam_site
 ,misc.preparer_office_code AS misc_preparer_office_code
 ,misc.recon_issue AS misc_recon_issue
 ,misc.fta_recon AS misc_fta_recon
 ,misc.lock_indicators AS misc_lock_indicators
 ,misc.recon_job AS misc_recon_job
 ,misc.prior_disclosure AS misc_prior_disclosure
 ,misc.nafta_303 AS misc_nafta_303
 ,misc.posted_filed AS misc_posted_filed
 ,misc.payment_type AS misc_payment_type
 ,misc.broker_to_pay AS misc_broker_to_pay
 ,misc.periodic_statement_month AS misc_periodic_statement_month
 ,misc.preliminary_statement_print_date AS misc_preliminary_statement_print_date
 ,misc.lock_psd AS misc_lock_psd
 ,misc.client_branch_designation AS misc_client_branch_designation
 ,misc.check_no AS misc_check_no
 ,misc.bond_type AS misc_bond_type
 ,misc.submitter AS misc_submitter
 ,misc.broker_pga_contact_name AS misc_broker_pga_contact_name
 ,misc.broker_pga_contact_phone AS misc_broker_pga_contact_phone
 ,misc.broker_pga_contact_email AS misc_broker_pga_contact_email
 ,misc.arrival_date_time AS misc_arrival_date_time
 ,misc.goods_from_ftz AS misc_goods_from_ftz
 ,misc.inspection_firms AS misc_inspection_firms
 ,misc.fsis_inspection AS misc_fsis_inspection
 ,misc.inspection_port AS misc_inspection_port
 ,misc.req_inspection_date AS misc_req_inspection_date
 ,misc.type AS misc_type
 ,misc.surety_code AS misc_surety_code

FROM zones_entry.filing_header AS header
JOIN zones_entry.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN zones_entry.declaration AS declaration
  ON header.id = declaration.filing_header_id
JOIN zones_entry.invoice_header AS invoice
  ON header.id = invoice.filing_header_id
JOIN zones_entry.invoice_line AS line
  ON invoice.id = line.parent_record_id
JOIN zones_entry.packing AS packing
  ON header.id = packing.filing_header_id
JOIN zones_entry.misc AS misc
  ON header.id = misc.filing_header_id
WHERE header.mapping_status = 2
GO