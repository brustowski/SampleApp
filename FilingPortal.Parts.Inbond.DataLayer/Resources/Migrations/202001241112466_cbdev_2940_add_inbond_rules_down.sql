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

-- update existing user roles
UPDATE dbo.App_Users_Roles 
SET App_Roles_FK = 16
WHERE App_Roles_FK = 21000;

-- delete new permissions and roles
DELETE FROM dbo.App_Permissions
WHERE id BETWEEN 21000 AND 21006;

DELETE FROM dbo.App_Roles
WHERE id IN (21000, 21001);
GO