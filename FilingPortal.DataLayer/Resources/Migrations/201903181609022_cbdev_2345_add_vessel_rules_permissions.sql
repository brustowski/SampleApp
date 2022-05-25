INSERT dbo.App_Permissions(id, description, name) VALUES (27, 'View Vessel Import Rule Records Permission', 'VesselViewImportRecordRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (28, 'Edit Vessel Import Rule Records Permission', 'VesselEditImportRecordRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (29, 'Delete Vessel Import Rule Records Permission', 'VesselDeleteImportRecordRules')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (9, 'VesselPowerUser', 'The role with following permissions: View, Edit, and File Vessel Imports and Rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (27, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (28, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (29, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (27, 9)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (28, 9)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (29, 9)