DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (50, 51, 52, 53)
DELETE FROM dbo.App_Permissions WHERE id IN (50, 51, 52, 53)
DELETE FROM dbo.App_Roles WHERE id IN (16)
GO

DROP VIEW IF EXISTS inbond.v_inbound_grid
GO