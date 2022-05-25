ALTER VIEW dbo.Vessel_Import_Grid
AS
SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,user_data.Broker AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.name AS country_of_origin
 ,imports.created_date AS created_date
 ,'' AS filing_number
 ,'' AS job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
    AND headers.mapping_status <> 0
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Vessel_Import_Filing_Headers filing_headers
  INNER JOIN dbo.Vessel_Import_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.mapping_status > 0
  AND imports.id = filing_details.VI_FK)

UNION ALL

SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,user_data.Broker AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.name AS country_of_origin
 ,imports.created_date AS created_date
 ,headers.filing_number
 ,headers.job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE headers.mapping_status > 0
GO


ALTER VIEW dbo.vessel_export_grid
AS
SELECT DISTINCT
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
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,'' AS filing_number
 ,'' AS job_link
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
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,vefh.filing_number
 ,vefh.job_link
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