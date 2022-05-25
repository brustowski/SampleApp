IF INDEXPROPERTY(OBJECT_ID('dbo.Truck_DEFValues'), 'UK_Truck_DEFValues_SingleFilingOrder', 'IndexId') IS NOT NULL
BEGIN
  DROP INDEX UK_Truck_DEFValues_SingleFilingOrder ON dbo.Truck_DEFValues
END

IF OBJECT_ID(N'dbo.v_Truck_DEFValues_Manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_Truck_DEFValues_Manual
GO