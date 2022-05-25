DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (41, 42, 43, 44, 45, 46, 47)
DELETE FROM dbo.App_Permissions WHERE id IN (41, 42, 43, 44, 45, 46, 47)
DELETE FROM dbo.App_Roles WHERE id IN (13, 14)
GO

IF OBJECT_ID('vessel_export_grid', 'V') IS NOT NULL
  DROP VIEW vessel_export_grid
GO
IF OBJECT_ID('v_vessel_export_def_values_manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_vessel_export_def_values_manual
GO
IF OBJECT_ID('v_vessel_export_tables', 'V') IS NOT NULL
DROP VIEW dbo.v_vessel_export_tables
GO
IF OBJECT_ID('v_Vessel_Export_Def_Values', 'V') IS NOT NULL
  DROP VIEW v_Vessel_Export_Def_Values
GO