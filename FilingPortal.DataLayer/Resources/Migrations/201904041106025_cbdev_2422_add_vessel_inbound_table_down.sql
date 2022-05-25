DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (30, 31, 32, 33)
DELETE FROM dbo.App_Permissions WHERE id IN (30, 31, 32, 33)
DELETE FROM dbo.App_Roles WHERE id IN (10)

IF OBJECT_ID(N'dbo.Vessel_Inbound_Grid', 'V') IS NOT NULL
  DROP VIEW dbo.Vessel_Inbound_Grid
GO