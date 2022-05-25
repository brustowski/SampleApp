DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (20001, 20002, 20003, 20004)
DELETE FROM dbo.App_Permissions WHERE id IN (20001, 20002, 20003, 20004)
DELETE FROM dbo.App_Roles WHERE id IN (20000)
GO

DROP VIEW IF EXISTS canada_imp_truck.v_inbound_grid
GO