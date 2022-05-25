INSERT dbo.App_Permissions(id, description, name) VALUES (50, 'View Inbond record', 'InbondViewRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (51, 'Delete Inbond Records Permission', 'InbondDeleteRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (52, 'File Inbond Records Permission', 'InbondFileRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (53, 'Import Inbond records permission', 'InbondImportRecord')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (16, 'InbondUser', 'The role with following permissions: View, Edit, and File Inbond data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (50, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (51, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (52, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (53, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (50, 16)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (51, 16)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (52, 16)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (53, 16)
GO

CREATE OR ALTER VIEW inbond.v_inbound_grid
AS
SELECT DISTINCT
  z.id
 ,cfc.firms_code
 ,zfh.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,z.port_of_arrival
 ,z.entry_date
 ,z.conveyance
 ,consignee.ClientCode AS consignee_code
 ,z.value
 ,z.manifest_qty
 ,z.manifest_qty_unit
 ,z.weight
 ,z.description
 ,z.marks_and_numbers
 ,z.created_user
 ,'' AS filing_number
 ,'' AS job_link
 ,z.created_date
 ,ISNULL(zfh.mapping_status, 0) AS mapping_status
 ,ISNULL(zfh.filing_status, 0) AS filing_status
 ,z.deleted AS is_deleted
FROM inbond.inbound z
LEFT JOIN cw_firms_codes cfc
  ON z.firms_code_id = cfc.id
LEFT JOIN inbond.filing_detail zfd
  ON zfd.Z_FK = z.id
LEFT JOIN inbond.filing_header zfh
  ON zfh.id = zfd.Filing_Headers_FK
LEFT JOIN Clients importer
  ON z.importer_id = importer.id
LEFT JOIN Clients consignee
  ON z.consignee_id = consignee.id
GO