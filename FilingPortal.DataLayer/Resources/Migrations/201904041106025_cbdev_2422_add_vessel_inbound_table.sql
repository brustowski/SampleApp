INSERT dbo.App_Permissions(id, description, name) VALUES (30, 'View Vessel Import Records Permission', 'VesselViewImportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (31, 'Delete Vessel Import Records Permission', 'VesselDeleteImportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (32, 'File Vessel Import Records Permission', 'VesselFileImportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (33, 'Add Vessel Import Records Permission', 'VesselAddRecordRules')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (10, 'VesselUser', 'The role with following permissions: View, Edit, and File Vessel Import data; View Vessel Rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (30, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (31, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (32, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (33, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (27, 10)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (30, 10)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (31, 10)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (32, 10)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (33, 10)

IF OBJECT_ID(N'dbo.Vessel_Inbound_Grid', 'V') IS NOT NULL
  DROP VIEW dbo.Vessel_Inbound_Grid
GO

CREATE VIEW dbo.Vessel_Inbound_Grid 
AS SELECT
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