-- add Zones In-Bond permissions 
INSERT dbo.App_Permissions(id, description, name) VALUES (21000, 'View Zones In-Bond Records Permission', 'ViewZonesInBondRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (21001, 'Delete Zones In-Bond Records Permission', 'DeleteZonesInBondRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (21002, 'File Zones In-Bond Records Permission', 'FileZonesInBondRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (21003, 'Import Zones In-Bond Records Permission', 'ImportZonesInBondRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (21004, 'View Zones In-Bond Rules Permission', 'ViewZonesInBondRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (21005, 'Edit Zones In-Bond Rules Permission', 'EditZonesInBondRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (21006, 'Delete Zones In-Bond Rules Permission', 'DeleteZonesInBondRules')

-- add Zones Roles
SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (21000, 'ZonesInBondUser', 'The role with following permissions: View, Edit, and File In-Bond data.')
INSERT dbo.App_Roles(id, title, description) VALUES (21001, 'ZonesInBondPowerUser', 'The role with following permissions: View, Edit, and File In-Bond and Rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

-- set roles permissions
-- admin
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21000, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21001, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21002, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21003, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21004, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21005, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21006, 1)
-- user
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21000, 21000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21001, 21000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21002, 21000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21003, 21000)
-- power user
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21000, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21001, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21002, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21003, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21004, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21005, 21001)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21006, 21001)

-- update existing user roles
UPDATE dbo.App_Users_Roles
SET App_Roles_FK = 21000
WHERE App_Roles_FK = 16;

-- delete old roles and permissions
DELETE FROM dbo.App_Permissions
WHERE id IN (50, 51, 52, 53);

DELETE FROM dbo.App_Roles
WHERE id = 16;
GO