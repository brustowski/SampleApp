IF OBJECT_ID(N'dbo.v_Pipeline_DEFValues_Manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_Pipeline_DEFValues_Manual
GO

IF OBJECT_ID('dbo.v_Pipeline_DEFValues', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.v_Pipeline_DEFValues
END
GO

IF OBJECT_ID(N'dbo.v_Pipeline_Tables', 'V') IS NOT NULL
  DROP VIEW dbo.v_Pipeline_Tables
GO
