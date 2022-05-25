IF OBJECT_ID(N'app_create_handbook', 'P') IS NOT NULL
  DROP PROCEDURE app_create_handbook
GO

IF OBJECT_ID(N'app_get_handbook', 'P') IS NOT NULL
  DROP PROCEDURE app_get_handbook
GO

IF OBJECT_ID(N'v_Handbooks', 'V') IS NOT NULL
  DROP VIEW v_Handbooks
GO