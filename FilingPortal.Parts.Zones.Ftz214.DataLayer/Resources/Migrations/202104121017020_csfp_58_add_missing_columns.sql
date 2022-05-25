IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'declaration' AND COLUMN_NAME = 'admission_no')
BEGIN
ALTER TABLE [zones_ftz214].[declaration]
           ADD [admission_no] [varchar](8) NULL

END

IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'declaration' AND COLUMN_NAME = 'it_no')
BEGIN
ALTER TABLE [zones_ftz214].[declaration]
          ADD it_no [varchar](35) NULL

END
IF NOT EXISTS (
  SELECT
    *
  FROM
    INFORMATION_SCHEMA.COLUMNS
  WHERE
   TABLE_SCHEMA = 'zones_ftz214' AND TABLE_NAME = 'declaration' AND COLUMN_NAME = 'it_date')
BEGIN
ALTER TABLE [zones_ftz214].[declaration]
          ADD it_date [varchar](35) NULL

END

GO
ALTER VIEW [zones_ftz214].[v_report]
AS
SELECT
  header.id AS filing_header_id
 ,inbound.is_update as is_update
 ,detail.inbound_id AS inbound_id
 ,header.filing_number as job_number
 ,declaration.importer AS declaration_importer
 ,declaration.shipment_type AS declaration_shipment_type
 ,declaration.stand_alone_prior_notice AS declaration_stand_alone_prior_notice
 ,declaration.issuer AS declaration_issuer
 ,declaration.carrier_scac AS declaration_carrier_scac
 ,declaration.loading_port AS declaration_loading_port
 ,declaration.discharge_port AS declaration_discharge_port
 ,declaration.dep AS declaration_dep
 ,declaration.arr AS declaration_arr
 ,declaration.arr2 AS declaration_arr2
 ,declaration.hmf AS declaration_hmf
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
 ,declaration.firms_code AS declaration_firms_code
 ,declaration.transport AS declaration_transport
 ,declaration.carrier AS declaration_carrier
 ,declaration.container AS declaration_container
 ,declaration.admission_type AS declaration_admission_type
 ,declaration.direct_delivery AS declaration_direct_delivery
 ,declaration.include_ptt AS declaration_include_ptt
 ,declaration.ptt_without_exception AS declaration_ptt_without_exception
 ,declaration.ocean_bill AS declaration_ocean_bill
 ,declaration.vessel AS declaration_vessel
 ,declaration.voyage AS declaration_voyage
 ,declaration.ultimate_consignee AS declaration_ultimate_consignee
 ,declaration.ftz_port AS declaration_ftz_port
 ,declaration.first_arr_date AS declaration_first_arr_date
 ,declaration.zone_id AS declaration_zone_id
 ,declaration.year AS declaration_year
 ,declaration.delivery_code AS declaration_delivery_code
 ,declaration.applicant AS declaration_applicant
 ,declaration.ftz_operator AS declaration_ftz_operator
 ,declaration.check_local_client AS declaration_check_local_client
 ,declaration.entry_finalized AS declaration_entry_finalized
 ,declaration.it_no as declaration_it_no
 ,declaration.it_date as declaration_it_date
 ,declaration.admission_no as declaration_adminssion_no
 ,packing.bill_type AS packing_bill_type
 ,packing.manifest_qty AS packing_manifest_qty
 ,packing.manifest_uq AS packing_uq
 ,packing.bill_issuer_scac AS packing_bill_issuer_scac
 ,packing.it_number AS packing_it_number
 ,packing.bill_number AS packing_bill_number
 ,packing.manifest_uq AS packing_manifest_uq
 ,packing.it_manifest_qty AS packing_it_manifest_qty
 ,packing.foreign_port AS packing_foreign_port
 ,packing.export_country AS packing_export_country
 ,packing.firms AS packing_firms
 ,packing.bta_indicator AS packing_bta_indicator
 ,packing.ptt_carrier AS packing_ptt_carrier
 ,invoice.invoice_no AS invoice_invoice_no
 ,invoice.supplier AS invoice_supplier
 ,invoice.supplier_address AS invoice_supplier_address
 ,invoice.inco AS invoice_inco
 ,invoice.invoice_total AS invoice_invoice_total
 ,invoice.currency AS invoice_currency
 ,invoice.consignee AS invoice_consignee
 ,invoice.consignee_address AS invoice_consignee_address
 ,invoice.export AS invoice_export
 ,invoice.manufacturer AS invoice_manufacturer
 ,invoice.manufacturer_address AS invoice_manufacturer_address
 ,invoice.seller AS invoice_seller
 ,invoice.seller_address AS invoice_seller_address
 ,invoice.importer AS invoice_importer
 ,invoice.sold_to_party AS invoice_sold_to_party
 ,invoice.ship_to_party AS invoice_ship_to_party
 ,invoice.broker_pga_contact_name AS invoice_broker_pga_contact_name
 ,invoice.broker_pga_contact_phone AS invoice_broker_pga_contact_phone
 ,invoice.broker_pga_contact_email AS invoice_broker_pga_contact_email
 ,invoice.related_bill as invoice_related_bill
 ,line.invoice_no AS line_invoice_no
 ,line.invoice_line_number AS line_invoice_line_number
 ,line.tariff AS line_tariff
 ,line.customs_qty AS line_customs_qty
 ,line.export AS line_export
 ,line.origin AS line_origin
 ,line.zone_status AS line_zone_status
 ,line.goods_description AS line_goods_description
 ,line.invoice_qty AS line_invoice_qty
 ,line.line_price AS line_line_price
 ,line.gross_weight AS line_gross_weight
 ,line.gross_weight_unit AS line_gross_weight_unit
 ,line.manufacturer AS line_manufacturer
 ,line.manufacturer_address AS line_manufacturer_address
 ,line.consignee AS line_consignee
 ,line.consignee_address AS line_consignee_address
 ,line.code AS line_code
 ,line.amount AS line_amount
 ,line.currency AS line_currency
 ,line.charges AS line_charges
 ,line.spi AS line_spi
 ,line.loading_port AS line_loading_port
 ,line.invoice_qty_uq AS line_invoice_qty_uq
 ,misc.branch AS misc_branch
 ,misc.broker AS misc_broker
 ,misc.service AS misc_service
 ,misc.merge_by AS misc_merge_by
 ,misc.submitter AS misc_submitter
 ,misc.broker_pga_contact_name AS misc_broker_pga_contact_name
 ,misc.broker_pga_contact_phone AS misc_broker_pga_contact_phone
 ,misc.broker_pga_contact_email AS misc_broker_pga_contact_email

FROM zones_ftz214.filing_header AS header
JOIN zones_ftz214.filing_detail AS detail
  ON header.id = detail.filing_header_id
JOIN zones_ftz214.declaration AS declaration
  ON header.id = declaration.filing_header_id
JOIN zones_ftz214.invoice_header AS invoice
  ON header.id = invoice.filing_header_id
JOIN zones_ftz214.invoice_line AS line
  ON invoice.id = line.parent_record_id
JOIN zones_ftz214.packing AS packing
  ON header.id = packing.filing_header_id
JOIN zones_ftz214.misc AS misc
  ON header.id = misc.filing_header_id
  JOIN zones_ftz214.inbound AS inbound
  ON detail.inbound_id = inbound.id
WHERE header.job_status = 2
GO