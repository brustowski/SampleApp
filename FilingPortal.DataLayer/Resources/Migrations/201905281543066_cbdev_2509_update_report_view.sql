ALTER VIEW dbo.Truck_Export_Report
AS SELECT
  headers.id
 ,detailes.truck_export_id as TEI_ID
 ,declaration.main_supplier as Declarationtab_Main_Supplier 
 ,declaration.importer as Declarationtab_Importer
 ,declaration.shpt_type as Declarationtab_shpt_type 
 ,declaration.transport as Declarationtab_transport
 ,declaration.container as Declarationtab_container
 ,declaration.tran_related as Declarationtab_tran_related
 ,declaration.hazardous as Declarationtab_hazardous
 ,declaration.routed_tran as Declarationtab_routed_tran
 ,declaration.filing_option as Declarationtab_filing_option
 ,declaration.tariff_type as Declarationtab_TariffType
 ,declaration.sold_en_route as Declarationtab_sold_en_route
 ,declaration.service as Declarationtab_service
 ,declaration.master_bill as Declarationtab_master_bill
 ,declaration.port_of_loading as Declarationtab_port_of_loading
 ,declaration.dep as Declarationtab_dep
 ,declaration.discharge as Declarationtab_discharge            
 ,declaration.export as Declarationtab_export
 ,declaration.exp_date as Declarationtab_exp_date
 ,declaration.house_bill as Declarationtab_house_bill
 ,declaration.origin as Declarationtab_destination
 ,declaration.destination as Declarationtab_
 ,declaration.owner_ref as Declarationtab_owner_ref
 ,declaration.transport_ref as Declarationtab_transport_ref
 ,declaration.inbond_type as Declarationtab_Inbond_type
 ,declaration.license_type as Declarationtab_License_type
 ,declaration.license_number as Declarationtab_License_number
 ,declaration.export_code as Declarationtab_ExportCode
 ,declaration.eccn as Declarationtab_Eccn
 ,declaration.country_of_dest as Declarationtab_Country_of_dest
 ,declaration.state_of_origin as Declarationtab_State_of_origin
 ,declaration.intermediate_consignee as Declarationtab_Intermediate_consignee
 ,declaration.carrier as Declarationtab_carrier
 ,declaration.forwader as Declarationtab_forwader
 ,declaration.arr_date as Declarationtab_arr_date
 ,declaration.check_local_client as Declarationtab_check_local_client

 ,invheaders.usppi as Invheaderstab_Usppi
 ,invheaders.usppi_address  as Invheaderstab_usppi_address
 ,invheaders.usppi_contact  as Invheaderstab_usppi_contact
 ,invheaders.usppi_phone  as Invheaderstab_usppi_phone
 ,invheaders.origin_indicator  as Invheaderstab_origin_indicator
 ,invheaders.ultimate_consignee  as Invheaderstab_ultimate_consignee
 ,invheaders.ultimate_consignee_type  as Invheaderstab_ultimate_consignee_type
 ,invheaders.invoice_number  as Invheaderstab_invoice_number
 ,invheaders.invoice_total_amount  as Invheaderstab_invoice_total_amount
 ,invheaders.invoice_total_amount_currency  as Invheaderstab_invoice_total_amount_currency
 ,invheaders.ex_rate_date  as Invheaderstab_ex_rate_date
 ,invheaders.exchange_rate  as Invheaderstab_exchange_rate
 ,invheaders.invoice_inco_term  as Invheaderstab_invoice_inco_term

 ,invlines.invoice_line_number as Invlinestab_lno
 ,invlines.tariff as Invlinestab_tariff
 ,invlines.customs_qty as Invlinestab_customs_qty
 ,invlines.export_code as Invlinestab_export_code
 ,invlines.goods_description as Invlinestab_goods_description
 ,invlines.customs_qty_unit  as Invlinestab_customs_qty_unit
 ,invlines.second_qty as Invlinestab_second_qty
 ,invlines.price as Invlinestab_price
 ,invlines.price_currency as Invlinestab_price_currency
 ,invlines.gross_weight as Invlinestab_gross_weight
 ,invlines.gross_weight_unit as Invlinestab_gross_weight_unit
 ,invlines.license_value as Invlinestab_license_value
 ,invlines.unit_price as Invlinestab_unit_price
 ,invlines.tariff_type as Invlinestab_tariff_type
FROM dbo.truck_export_filing_headers headers
INNER JOIN dbo.truck_export_filing_details detailes
  ON headers.id = detailes.filing_header_id
LEFT OUTER JOIN dbo.truck_export_declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_lines invlines
  ON invlines.invoice_header_id= invheaders.id

WHERE headers.mapping_status = 2
GO