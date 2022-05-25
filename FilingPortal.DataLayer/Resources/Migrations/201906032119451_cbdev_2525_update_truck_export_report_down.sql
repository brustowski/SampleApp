ALTER VIEW dbo.Truck_Export_Report
AS SELECT
  headers.id
 ,detailes.truck_export_id as TEI_ID
 ,declaration.main_supplier
 ,declaration.importer
 ,declaration.shpt_type
 ,declaration.transport
 ,declaration.container
 ,declaration.tran_related
 ,declaration.hazardous
 ,declaration.routed_tran
 ,declaration.filing_option
 ,declaration.tariff_type as declarationtabTariffType
 ,declaration.sold_en_route
 ,declaration.service
 ,declaration.master_bill
 ,declaration.port_of_loading
 ,declaration.dep
 ,declaration.discharge
 ,declaration.export
 ,declaration.exp_date
 ,declaration.house_bill
 ,declaration.origin
 ,declaration.destination
 ,declaration.owner_ref
 ,declaration.transport_ref
 ,declaration.inbond_type
 ,declaration.license_type
 ,declaration.license_number
 ,declaration.export_code as declarationtabExportCode
 ,declaration.eccn
 ,declaration.country_of_dest
 ,declaration.state_of_origin
 ,declaration.intermediate_consignee
 ,declaration.carrier
 ,declaration.forwader
 ,declaration.arr_date
 ,declaration.check_local_client

 ,invheaders.usppi
 ,invheaders.usppi_address
 ,invheaders.usppi_contact
 ,invheaders.usppi_phone
 ,invheaders.origin_indicator
 ,invheaders.ultimate_consignee
 ,invheaders.ultimate_consignee_type
 ,invheaders.invoice_number
 ,invheaders.invoice_total_amount
 ,invheaders.invoice_total_amount_currency
 ,invheaders.ex_rate_date
 ,invheaders.exchange_rate
 ,invheaders.invoice_inco_term

 ,invlines.invoice_line_number AS lno
 ,invlines.tariff
 ,invlines.customs_qty
 ,invlines.export_code
 ,invlines.goods_description
 ,invlines.customs_qty_unit
 ,invlines.second_qty
 ,invlines.price
 ,invlines.price_currency
 ,invlines.gross_weight
 ,invlines.gross_weight_unit
 ,invlines.license_value
 ,invlines.unit_price
 ,invlines.tariff_type
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
Go
