ALTER VIEW canada_imp_truck.v_report
AS
SELECT
  header.id AS filing_header_id
 ,detail.inbound_id AS inbound_id
 ,aud.Broker AS broker

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
LEFT JOIN app_users_data aud
  ON aud.UserAccount = header.created_user
WHERE header.mapping_status = 2;
GO