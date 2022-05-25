ALTER TABLE dbo.truck_export_invoice_headers
  DROP COLUMN invoice_number
GO
IF OBJECT_ID('dbo.truck_export_invoice_header_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.truck_export_invoice_header_number
GO
CREATE FUNCTION dbo.truck_export_invoice_header_number (
  @filingHeaderId INT
  , @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      teih.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY teih.id)
    FROM dbo.truck_export_invoice_headers teih
    WHERE teih.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
ALTER TABLE dbo.truck_export_invoice_headers
  ADD invoice_number AS (dbo.truck_export_invoice_header_number(filing_header_id, id))
GO

ALTER TABLE dbo.truck_export_invoice_lines
  DROP COLUMN lno
GO
IF OBJECT_ID('dbo.truck_export_invoice_line_number', 'FN') IS NOT NULL
  DROP FUNCTION dbo.truck_export_invoice_line_number
GO
CREATE FUNCTION dbo.truck_export_invoice_line_number (
  @invoiceHeaderId INT
  , @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      teil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY teil.id)
    FROM dbo.truck_export_invoice_lines teil
    WHERE teil.invoice_header_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
ALTER TABLE dbo.truck_export_invoice_lines
  ADD invoice_line_number AS (dbo.truck_export_invoice_line_number(invoice_header_id, id))
GO

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
GO

DELETE FROM dbo.truck_export_def_values
WHERE column_name='invoice_number'
DELETE FROM dbo.truck_export_def_values_manual
WHERE column_name='invoice_number'
GO