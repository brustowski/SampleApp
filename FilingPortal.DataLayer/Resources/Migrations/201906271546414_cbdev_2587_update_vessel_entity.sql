ALTER VIEW dbo.Vessel_Inbound_Grid 
AS SELECT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,Clients.ClientCode AS importer_code
 ,imports.vessel AS vessel
 ,states.StateCode AS state
 ,imports.discharge_terminal AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
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
LEFT OUTER JOIN dbo.Clients
  ON imports.importer_id = Clients.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
GO