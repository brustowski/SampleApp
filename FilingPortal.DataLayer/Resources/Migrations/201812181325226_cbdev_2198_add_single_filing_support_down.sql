IF INDEXPROPERTY(OBJECT_ID('dbo.Rail_DEFValues'), 'UK_Rail_DEFValues_SingleFilingOrder', 'IndexId') IS NOT NULL
BEGIN
  DROP INDEX UK_Rail_DEFValues_SingleFilingOrder ON dbo.Rail_DEFValues
END

IF OBJECT_ID('dbo.v_Rail_DEFValues', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.v_Rail_DEFValues
END
GO
