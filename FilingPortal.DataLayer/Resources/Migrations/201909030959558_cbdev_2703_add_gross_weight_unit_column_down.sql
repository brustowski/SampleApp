ALTER TABLE dbo.Vessel_Import_Invoice_Lines
DROP COLUMN gross_weight_unit
GO

ALTER VIEW dbo.Vessel_Import_Report
AS
SELECT
  headers.id AS Filing_Header_Id
 ,detailes.VI_FK AS VI_ID

 ,declaration.main_supplier AS Declarations_main_supplier
 ,declaration.importer AS Declarations_importer
 ,declaration.shpt_type AS Declarations_shpt_type
 ,declaration.transport AS Declarations_transport
 ,declaration.container AS Declarations_container
 ,declaration.ent_type AS Declarations_ent_type
 ,declaration.rlf AS Declarations_rlf
 ,declaration.message AS Declarations_message
 ,declaration.enable_ent_sum AS Declarations_enable_ent_sum
 ,declaration.enable_cargo_rel AS Declarations_enable_cargo_rel
 ,declaration.type AS Declarations_type
 ,declaration.certify_cargo_release AS Declarations_certify_cargo_release
 ,declaration.service AS Declarations_service
 ,declaration.issuer AS Declarations_issuer
 ,declaration.ocean_bill AS Declarations_ocean_bill
 ,declaration.vessel AS Declarations_vessel
 ,declaration.voyage AS Declarations_voyage
 ,declaration.carrier_scac AS Declarations_carrier_scac
 ,declaration.port_of_discharge AS Declarations_port_of_discharge
 ,declaration.port_of_loading AS Declarations_port_of_loading
 ,declaration.entry_port AS Declarations_entry_port
 ,declaration.dep AS Declarations_dep
 ,declaration.arr AS Declarations_arr
 ,declaration.loading_unloco AS Declarations_loading_unloco
 ,declaration.discharage_unloco AS Declarations_discharage_unloco
 ,declaration.hmf AS Declarations_hmf
 ,declaration.origin AS Declarations_origin
 ,declaration.destination AS Declarations_destination
 ,declaration.etd AS Declarations_etd
 ,declaration.eta AS Declarations_eta
 ,declaration.dest_state AS Declarations_dest_state
 ,declaration.country_of_export AS Declarations_country_of_export
 ,declaration.export_date AS Declarations_export_date
 ,declaration.description AS Declarations_description
 ,declaration.owner_ref AS Declarations_owner_ref
 ,declaration.inco AS Declarations_inco
 ,declaration.firms_code AS Declarations_firms_code
 ,declaration.entry_number AS Declarations_entry_number
 ,declaration.purchased AS Declarations_purchased
 ,declaration.check_local_client AS Declarations_check_local_client

 ,packings.manifest_qty AS Packings_manifest_qty
 ,packings.uq AS Packings_uq
 ,packings.bill_type AS Packings_bill_type
 ,packings.bill_issuer_scac AS Packings_bill_issuer_scac
 ,packings.bill_num AS Packings_bill_num

 ,misc.branch AS Miscs_branch
 ,misc.broker AS Miscs_broker
 ,misc.merge_by AS Miscs_merge_by
 ,misc.preparer_dist_port AS Miscs_preparer_dist_port
 ,misc.recon_issue AS Miscs_recon_issue
 ,misc.payment_type AS Miscs_payment_type
 ,misc.fta_recon AS Miscs_fta_recon
 ,misc.broker_to_pay AS Miscs_broker_to_pay
 ,misc.inbond_type AS Miscs_inbond_type

 ,invheaders.invoice_number AS Invoice_Headers_invoice_number
 ,invheaders.supplier AS Invoice_Headers_supplier
 ,invheaders.supplier_address AS Invoice_Headers_supplier_address
 ,invheaders.inco AS Invoice_Headers_inco
 ,invheaders.invoice_total AS Invoice_Headers_invoice_total
 ,invheaders.invoice_total_currency AS Invoice_Headers_invoice_total_currency
 ,invheaders.line_total AS Invoice_Headers_line_total
 ,invheaders.country_of_origin AS Invoice_Headers_country_of_origin
 ,invheaders.country_of_export AS Invoice_Headers_country_of_export
 ,invheaders.consignee AS Invoice_Headers_consignee
 ,invheaders.consignee_address AS Invoice_Headers_consignee_address
 ,invheaders.export_date AS Invoice_Headers_export_date
 ,invheaders.transaction_related AS Invoice_Headers_transaction_related
 ,invheaders.seller AS Invoice_Headers_seller
 ,invheaders.sold_to_party AS Invoice_Headers_sold_to_party
 ,invheaders.ship_to_party AS Invoice_Headers_ship_to_party
 ,invheaders.broker_pga_contact_name AS Invoice_Headers_broker_pga_contact_name
 ,invheaders.broker_pga_contact_phone AS Invoice_Headers_broker_pga_contact_phone
 ,invheaders.importer AS Invoice_Headers_importer
 ,invheaders.manufacturer AS Invoice_Headers_manufacturer
 ,invheaders.manufacturer_id AS Invoice_Headers_manufacturer_id
 ,invheaders.broker_pga_contact_email AS Invoice_Headers_broker_pga_contact_email

 ,invlines.line_no AS Invoice_Lines_line_no
 ,invlines.entry_no AS Invoice_Lines_entry_no
 ,invlines.product AS Invoice_Lines_product
 ,invlines.classification AS Invoice_Lines_classification
 ,invlines.tariff AS Invoice_Lines_tariff
 ,invlines.customs_qty AS Invoice_Lines_customs_qty
 ,invlines.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,invlines.goods_description AS Invoice_Lines_goods_description
 ,invlines.spi AS Invoice_Lines_spi
 ,invlines.country_of_origin AS Invoice_Lines_country_of_origin
 ,invlines.country_of_export AS Invoice_Lines_country_of_export
 ,invlines.destination_state AS Invoice_Lines_destination_state
 ,invlines.manufacturer AS Invoice_Lines_manufacturer
 ,invlines.consignee AS Invoice_Lines_consignee
 ,invlines.sold_to_party AS Invoice_Lines_sold_to_party
 ,invlines.seller AS Invoice_Lines_seller
 ,invlines.seller_address AS Invoice_Lines_seller_address
 ,invlines.gross_weight AS Invoice_Lines_gross_weight
 ,invlines.price AS Invoice_Lines_price
 ,invlines.prod_id AS Invoice_Lines_prod_id
 ,invlines.attribute1 AS Invoice_Lines_attribute1
 ,invlines.attribute2 AS Invoice_Lines_attribute2
 ,invlines.attribute3 AS Invoice_Lines_attribute3
 ,invlines.transaction_related AS Invoice_Lines_transaction_related
 ,invlines.invoice_qty AS Invoice_Lines_invoice_qty
 ,invlines.code AS Invoice_Lines_code
 ,invlines.amount AS Invoice_Lines_amount
 ,invlines.epa_tsca_toxic_substance_control_act_indicator AS Invoice_Lines_epa_tsca_toxic_substance_control_act_indicator
 ,invlines.tsca_indicator AS Invoice_Lines_tsca_indicator
 ,invlines.certifying_individual AS Invoice_Lines_certifying_individual
 ,invlines.mid AS Invoice_Lines_mid
 ,invlines.hmf AS Invoice_Lines_hmf
 ,invlines.unit_price AS Invoice_Lines_unit_price

FROM dbo.Vessel_Import_Filing_Headers headers
INNER JOIN dbo.Vessel_Import_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT OUTER JOIN dbo.Vessel_Import_Declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Packings AS packings
  ON packings.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Miscs AS misc
  ON misc.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Lines invlines
  ON invlines.invoice_header_id = invheaders.id

WHERE headers.mapping_status = 2
GO

UPDATE dbo.Vessel_Import_Def_Values
SET paired_field_table = NULL
   ,paired_field_column = NULL
WHERE column_name = 'gross_weight'
GO

DELETE FROM dbo.Vessel_Import_Def_Values
WHERE column_name = 'gross_weight_unit'
GO