IF OBJECT_ID(N'dbo.Pipeline_ContainersTab', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_ContainersTab
GO

IF OBJECT_ID(N'dbo.Pipeline_DeclarationTab', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_DeclarationTab
GO

IF OBJECT_ID(N'dbo.Pipeline_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_InvoiceHeaders
GO

IF OBJECT_ID(N'dbo.Pipeline_InvoiceLines', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_InvoiceLines
GO

IF OBJECT_ID(N'dbo.Pipeline_MISC', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_MISC
GO

IF OBJECT_ID(N'dbo.Pipeline_Packing', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_Packing
GO

IF OBJECT_ID(N'dbo.Pipeline_Organizations', 'U') IS NOT NULL
  DROP TABLE dbo.Pipeline_Organizations
GO

IF OBJECT_ID(N'dbo.v_Pipeline_Filing_Data', 'V') IS NOT NULL
  DROP VIEW dbo.v_Pipeline_Filing_Data
GO
-- drop extractBatchCode
IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.extractBatchCode')
    AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.extractBatchCode
GO