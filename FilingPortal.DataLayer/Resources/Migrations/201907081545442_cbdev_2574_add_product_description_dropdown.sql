ALTER VIEW dbo.Vessel_Inbound_Grid 
AS SELECT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name as product_description
 ,imports.eta AS eta
 ,imports.user_id AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.created_date AS created_date
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
GO