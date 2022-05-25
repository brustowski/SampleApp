DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (20005, 20006, 20007)
DELETE FROM dbo.App_Permissions WHERE id IN (20005, 20006, 20007)
DELETE FROM dbo.App_Roles WHERE id IN (20001)
GO