ALTER VIEW dbo.Truck_Export_Report 
AS SELECT
  headers.id
 ,detailes.truck_export_id AS TEI_ID
 ,declaration.[main_supplier] AS Declarationtab_Main_Supplier
 ,declaration.[importer] AS Declarationtab_Importer
 ,declaration.[shpt_type] AS Declarationtab_shpt_type
 ,declaration.[transport] AS Declarationtab_transport
 ,declaration.[container] AS Declarationtab_container
 ,declaration.[tran_related] AS Declarationtab_tran_related
 ,declaration.[hazardous] AS Declarationtab_hazardous
 ,declaration.[routed_tran] AS Declarationtab_routed_tran
 ,declaration.[filing_option] AS Declarationtab_filing_option
 ,declaration.[tariff_type] AS Declarationtab_TariffType
 ,declaration.[sold_en_route] AS Declarationtab_sold_en_route
 ,declaration.[service] AS Declarationtab_service
 ,declaration.[master_bill] AS Declarationtab_master_bill
 ,declaration.[port_of_loading] AS Declarationtab_port_of_loading
 ,declaration.[dep] AS Declarationtab_dep
 ,declaration.[discharge] AS Declarationtab_discharge
 ,declaration.[export] AS Declarationtab_export
 ,declaration.[exp_date] AS Declarationtab_exp_date
 ,declaration.[house_bill] AS Declarationtab_house_bill
 ,declaration.[origin] AS Declarationtab_origin
 ,declaration.[destination] AS Declarationtab_destination
 ,declaration.[owner_ref] AS Declarationtab_owner_ref
 ,declaration.[transport_ref] AS Declarationtab_transport_ref
 ,declaration.[inbond_type] AS Declarationtab_Inbond_type
 ,declaration.[license_type] AS Declarationtab_License_type
 ,declaration.[license_number] AS Declarationtab_License_number
 ,declaration.[export_code] AS Declarationtab_ExportCode
 ,declaration.[eccn] AS Declarationtab_Eccn
 ,declaration.[country_of_dest] AS Declarationtab_Country_of_dest
 ,declaration.[state_of_origin] AS Declarationtab_State_of_origin
 ,declaration.[intermediate_consignee] AS Declarationtab_Intermediate_consignee
 ,declaration.[carrier] AS Declarationtab_carrier
 ,declaration.[forwader] AS Declarationtab_forwader
 ,declaration.[arr_date] AS Declarationtab_arr_date
 ,declaration.[check_local_client] AS Declarationtab_check_local_client

 ,invheaders.[usppi] AS Invheaderstab_Usppi
 ,invheaders.[usppi_address] AS Invheaderstab_usppi_address
 ,invheaders.[usppi_contact] AS Invheaderstab_usppi_contact
 ,invheaders.[usppi_phone] AS Invheaderstab_usppi_phone
 ,invheaders.[origin_indicator] AS Invheaderstab_origin_indicator
 ,invheaders.[ultimate_consignee] AS Invheaderstab_ultimate_consignee
 ,invheaders.[ultimate_consignee_type] AS Invheaderstab_ultimate_consignee_type
 ,invheaders.[invoice_number] AS Invheaderstab_invoice_number
 ,invheaders.[invoice_total_amount] AS Invheaderstab_invoice_total_amount
 ,invheaders.[invoice_total_amount_currency] AS Invheaderstab_invoice_total_amount_currency
 ,invheaders.[ex_rate_date] AS Invheaderstab_ex_rate_date
 ,invheaders.[exchange_rate] AS Invheaderstab_exchange_rate
 ,invheaders.[invoice_inco_term] AS Invheaderstab_invoice_inco_term

 ,invlines.[invoice_line_number] AS Invlinestab_lno
 ,invlines.[tariff] AS Invlinestab_tariff
 ,invlines.[customs_qty] AS Invlinestab_customs_qty
 ,invlines.[export_code] AS Invlinestab_export_code
 ,invlines.[goods_description] AS Invlinestab_goods_description
 ,invlines.[customs_qty_unit] AS Invlinestab_customs_qty_unit
 ,invlines.[second_qty] AS Invlinestab_second_qty
 ,invlines.[price] AS Invlinestab_price
 ,invlines.[price_currency] AS Invlinestab_price_currency
 ,invlines.[gross_weight] AS Invlinestab_gross_weight
 ,invlines.[gross_weight_unit] AS Invlinestab_gross_weight_unit
 ,invlines.[license_value] AS Invlinestab_license_value
 ,invlines.[unit_price] AS Invlinestab_unit_price
 ,invlines.[tariff_type] AS Invlinestab_tariff_type
FROM dbo.truck_export_filing_headers headers
INNER JOIN dbo.truck_export_filing_details detailes
  ON headers.id = detailes.filing_header_id
LEFT OUTER JOIN dbo.truck_export_declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_lines invlines
  ON invlines.invoice_header_id = invheaders.id

WHERE headers.mapping_status = 2
