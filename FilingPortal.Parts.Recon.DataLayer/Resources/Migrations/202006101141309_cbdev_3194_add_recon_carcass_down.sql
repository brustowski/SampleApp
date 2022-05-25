DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (24001, 24002)
DELETE FROM dbo.App_Permissions WHERE id IN (24001, 24002)
DELETE FROM dbo.App_Roles WHERE id IN (24000)
GO

DROP PROCEDURE IF EXISTS recon.sp_get_cw_data
GO