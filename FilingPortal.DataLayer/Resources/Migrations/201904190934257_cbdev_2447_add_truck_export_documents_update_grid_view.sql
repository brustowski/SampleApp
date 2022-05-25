ALTER VIEW dbo.truck_export_grid
AS
SELECT DISTINCT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter_code
 ,te.usppi_code
 ,te.consignee_code
 ,te.carrier
 ,te.scac
 ,te.license
 ,te.license_type
 ,te.tariff
 ,te.routed_tran
 ,te.container
 ,te.eccn
 ,te.goods_description
 ,'' AS filing_number
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,tefh.error_description
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
LEFT JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
    AND tefh.mapping_status <> 0
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.truck_export_filing_headers fh
  INNER JOIN dbo.truck_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND te.id = fd.truck_export_id)
AND te.deleted = 0

UNION

SELECT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter_code
 ,te.usppi_code
 ,te.consignee_code
 ,te.carrier
 ,te.scac
 ,te.license
 ,te.license_type
 ,te.tariff
 ,te.routed_tran
 ,te.container
 ,te.eccn
 ,te.goods_description
 ,tefh.filing_number
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,tefh.error_description
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
INNER JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
WHERE tefh.mapping_status <> 0
AND te.deleted = 0
GO