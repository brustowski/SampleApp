INSERT dbo.App_Permissions(id, description, name) VALUES (49, 'Working with rail daily audit', 'AuditRailDailyAudit')

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (49, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (49, 15)