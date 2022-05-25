ALTER VIEW dbo.Vessel_Inbound_Grid
AS
SELECT
  imports.id AS Id
 ,headers.id AS Filing_Headers_Id
 ,imports.importer_code AS ImporterCode
 ,imports.vessel AS Vessel
 ,imports.state AS State
 ,imports.discharge_terminal AS DischargeTerminal
 ,imports.classification AS Classification
 ,imports.eta AS Eta
 ,imports.filer_id AS FilerId
 ,imports.container AS Container
 ,imports.entry_type AS EntryType
 ,imports.master_bill AS MasterBill
 ,imports.created_date AS CreatedDate
 ,ISNULL(headers.mapping_status, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(headers.filing_status, 0) AS Filing_Headers_FilingStatus
 ,headers.error_description AS ErrorDescription
 ,imports.deleted AS Deleted
FROM dbo.Vessel_Imports imports
LEFT JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
GO