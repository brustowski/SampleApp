IF INDEXPROPERTY(OBJECT_ID('dbo.Truck_DEFValues'), 'UK_Truck_DEFValues_SingleFilingOrder', 'IndexId') IS NULL
BEGIN
  CREATE UNIQUE INDEX UK_Truck_DEFValues_SingleFilingOrder
  ON dbo.Truck_DEFValues (SingleFilingOrder)
  WHERE SingleFilingOrder IS NOT NULL
END
GO

IF OBJECT_ID(N'dbo.v_Truck_DEFValues_Manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_Truck_DEFValues_Manual
GO

CREATE VIEW dbo.v_Truck_DEFValues_Manual
  AS
  SELECT
    v.*
   ,i.DATA_TYPE AS ValueType
   ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
  FROM dbo.Truck_DEFValues_Manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
  WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
  AND i.TABLE_SCHEMA = 'dbo')
  OR i.COLUMN_NAME IS NULL
GO