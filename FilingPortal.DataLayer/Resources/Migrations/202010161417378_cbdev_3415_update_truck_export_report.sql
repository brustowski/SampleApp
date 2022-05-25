ALTER VIEW dbo.v_exp_truck_report 
AS SELECT
  header.id
 ,header.is_updated
 ,header.filing_number as Filing_Number 
 ,detail.inbound_id AS TEI_ID
 ,declaration.main_supplier AS Declarationtab_Main_Supplier
 ,declaration.importer AS Declarationtab_Importer
 ,declaration.shpt_type AS Declarationtab_shpt_type
 ,declaration.transport AS Declarationtab_transport
 ,declaration.container AS Declarationtab_container
 ,declaration.tran_related AS Declarationtab_tran_related
 ,declaration.hazardous AS Declarationtab_hazardous
 ,declaration.routed_tran AS Declarationtab_routed_tran
 ,declaration.filing_option AS Declarationtab_filing_option
 ,declaration.tariff_type AS Declarationtab_TariffType
 ,declaration.sold_en_route AS Declarationtab_sold_en_route
 ,declaration.[service] AS Declarationtab_service
 ,declaration.master_bill AS Declarationtab_master_bill
 ,declaration.port_of_loading AS Declarationtab_port_of_loading
 ,declaration.dep AS Declarationtab_dep
 ,declaration.discharge AS Declarationtab_discharge
 ,declaration.export AS Declarationtab_export
 ,declaration.exp_date AS Declarationtab_exp_date
 ,declaration.house_bill AS Declarationtab_house_bill
 ,declaration.origin AS Declarationtab_origin
 ,declaration.destination AS Declarationtab_destination
 ,declaration.owner_ref AS Declarationtab_owner_ref
 ,declaration.transport_ref AS Declarationtab_transport_ref
 ,declaration.inbond_type AS Declarationtab_Inbond_type
 ,declaration.license_type AS Declarationtab_License_type
 ,declaration.license_number AS Declarationtab_License_number
 ,declaration.export_code AS Declarationtab_ExportCode
 ,declaration.eccn AS Declarationtab_Eccn
 ,declaration.country_of_dest AS Declarationtab_Country_of_dest
 ,declaration.state_of_origin AS Declarationtab_State_of_origin
 ,declaration.intermediate_consignee AS Declarationtab_Intermediate_consignee
 ,declaration.carrier AS Declarationtab_carrier
 ,declaration.forwader AS Declarationtab_forwader
 ,declaration.arr_date AS Declarationtab_arr_date
 ,declaration.check_local_client AS Declarationtab_check_local_client
 ,declaration.country_of_export AS Declarationtab_Country_of_export

 ,invoice.usppi AS Invheaderstab_Usppi
 ,invoice.usppi_address AS Invheaderstab_usppi_address
 ,invoice.usppi_contact AS Invheaderstab_usppi_contact
 ,invoice.usppi_phone AS Invheaderstab_usppi_phone
 ,invoice.origin_indicator AS Invheaderstab_origin_indicator
 ,invoice.ultimate_consignee AS Invheaderstab_ultimate_consignee
 ,invoice.ultimate_consignee_type AS Invheaderstab_ultimate_consignee_type
 ,invoice.invoice_number AS Invheaderstab_invoice_number
 ,invoice.invoice_total_amount AS Invheaderstab_invoice_total_amount
 ,invoice.invoice_total_amount_currency AS Invheaderstab_invoice_total_amount_currency
 ,invoice.ex_rate_date AS Invheaderstab_ex_rate_date
 ,invoice.exchange_rate AS Invheaderstab_exchange_rate
 ,invoice.invoice_inco_term AS Invheaderstab_invoice_inco_term

 ,invoice_line.invoice_line_number AS Invlinestab_lno
 ,invoice_line.tariff AS Invlinestab_tariff
 ,invoice_line.customs_qty AS Invlinestab_customs_qty
 ,invoice_line.export_code AS Invlinestab_export_code
 ,invoice_line.goods_description AS Invlinestab_goods_description
 ,invoice_line.customs_qty_unit AS Invlinestab_customs_qty_unit
 ,invoice_line.second_qty AS Invlinestab_second_qty
 ,invoice_line.price AS Invlinestab_price
 ,invoice_line.price_currency AS Invlinestab_price_currency
 ,invoice_line.gross_weight AS Invlinestab_gross_weight
 ,invoice_line.gross_weight_unit AS Invlinestab_gross_weight_unit
 ,invoice_line.license_value AS Invlinestab_license_value
 ,invoice_line.unit_price AS Invlinestab_unit_price
 ,invoice_line.tariff_type AS Invlinestab_tariff_type
 ,invoice_line.goods_origin AS Invlinestab_goods_origin
 ,invoice_line.invoice_qty_unit AS Invlinestab_invoice_qty_unit

FROM dbo.exp_truck_filing_header header
JOIN dbo.exp_truck_filing_detail detail
  ON header.id = detail.filing_header_id
LEFT JOIN dbo.exp_truck_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_line invoice_line
  ON invoice_line.parent_record_id = invoice.id

WHERE header.mapping_status = 2
GO