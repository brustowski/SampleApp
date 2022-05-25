--- Rail calculated gross weight unit fn ---
ALTER TABLE dbo.Rail_Filing_Headers
DROP COLUMN GrossWeightSummaryUnit
GO

ALTER FUNCTION dbo.fn_Rail_CalculateGrossWtUnit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = (dbo.fn_GetRailCarCount(@filingHeaderId))

  SELECT TOP (1)
    @result =
    CASE
      WHEN @count > 1 THEN 'T'
      ELSE ril.Gr_Weight_Unit
    END
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId
  RETURN @result
END
GO

ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummaryUnit AS (dbo.fn_Rail_CalculateGrossWtUnit(id))
GO

--- Rail weight summary fn ---
ALTER TABLE dbo.Rail_Filing_Headers
DROP COLUMN GrossWeightSummary
GO

ALTER FUNCTION dbo.fn_RailWeightSummary (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;
  DECLARE @calcwt DECIMAL(18, 9) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = (dbo.fn_GetRailCarCount(@filingHeaderId))

  SELECT
    @calcwt = SUM(ril.Gr_Weight)
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId

  SELECT TOP (1)
    @result =
    CASE
      WHEN @count > 1 THEN dbo.weightToTon(@calcwt, ril.Gr_Weight_Unit)
      ELSE ril.Gr_Weight
    END

  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId
  RETURN @result
END
GO

ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummary AS (dbo.fn_RailWeightSummary(id))
GO