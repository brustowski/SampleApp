IF INDEXPROPERTY(OBJECT_ID('dbo.Rail_DEFValues'), 'UK_Rail_DEFValues_SingleFilingOrder', 'IndexId') IS NULL
BEGIN
  CREATE UNIQUE INDEX UK_Rail_DEFValues_SingleFilingOrder
  ON dbo.Rail_DEFValues (SingleFilingOrder)
  WHERE SingleFilingOrder IS NOT NULL
END
GO

IF OBJECT_ID('dbo.v_Rail_DEFValues', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.v_Rail_DEFValues
END
GO

CREATE VIEW dbo.v_Rail_DEFValues
AS
SELECT
  v.*
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Rail_DEFValues v
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
WHERE (UPPER(i.column_name) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO
