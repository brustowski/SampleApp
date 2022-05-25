INSERT dbo.App_Permissions(id, description, name) VALUES (50, 'Work with Auto-Create configurations', 'AdminAutoCreateConfiguration')
GO
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (50, 1)
GO
