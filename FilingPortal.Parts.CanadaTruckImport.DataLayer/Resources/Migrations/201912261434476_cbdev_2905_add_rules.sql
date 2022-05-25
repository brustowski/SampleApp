INSERT dbo.App_Permissions(id, description, name) VALUES (20005, 'View Canada Truck Rules Permission', 'CanadaTruckImportViewRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (20006, 'Edit Canada Truck Rules Permission', 'CanadaTruckImportEditRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (20007, 'Delete Canada Truck Rules Permission', 'CanadaTruckImportDeleteRules')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (20001, 'CanadaTruckImportPowerUser', 'The role with following permissions: View and Edit Canada Truck Import rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20005, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20006, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20007, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20005, 20001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20006, 20001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20007, 20001)
GO