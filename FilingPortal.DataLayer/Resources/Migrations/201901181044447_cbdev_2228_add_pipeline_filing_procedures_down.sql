-- drop extractBatchCode
IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.extractBatchCode')
    AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.extractBatchCode
GO

-- drop fn_pipeline_weight
IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_pipeline_weight')
    AND type IN ('IF', 'FN', 'TF'))
  DROP FUNCTION dbo.fn_pipeline_weight
GO

IF OBJECT_ID(N'dbo.pipeline_filing', 'P') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_filing
GO

IF OBJECT_ID(N'dbo.pipeline_filing_param', 'P') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_filing_param
GO

IF OBJECT_ID(N'dbo.pipeline_filing_del', 'P') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_filing_del
GO