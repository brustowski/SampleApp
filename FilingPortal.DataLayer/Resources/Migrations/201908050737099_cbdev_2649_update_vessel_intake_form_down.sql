alter VIEW dbo.vessel_export_grid 
AS SELECT DISTINCT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.description
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,'' AS filing_number
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
LEFT JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
    AND vefh.mapping_status <> 0
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.vessel_export_filing_headers fh
  INNER JOIN dbo.vessel_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND ve.id = fd.vessel_export_id)
AND ve.deleted = 0

UNION

SELECT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.description
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,'' AS filing_number
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
INNER JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
GO