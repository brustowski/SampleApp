IF OBJECT_ID('dbo.truck_export_grid', 'V') IS NOT NULL
  DROP VIEW dbo.truck_export_grid
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_del') AND type in ('P', 'PC'))
	DROP PROCEDURE dbo.truck_export_del
GO

DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (34, 35, 36, 37, 38, 39, 40)
DELETE FROM dbo.App_Permissions WHERE id IN (34, 35, 36, 37, 38, 39, 40)
DELETE FROM dbo.App_Roles WHERE id IN (11, 12)
GO
