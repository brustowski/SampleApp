DELETE FROM dbo.App_Permissions_Roles
WHERE App_Roles_FK = 20001
  AND App_Permissions_FK IN (20001, 20002, 20003, 20004);
GO