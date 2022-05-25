IF OBJECT_ID(N'dbo.v_Pipeline_DEFValues_Manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_Pipeline_DEFValues_Manual
GO

CREATE VIEW dbo.v_Pipeline_DEFValues_Manual
  AS
  SELECT
    v.*
   ,i.DATA_TYPE AS ValueType
   ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
  FROM dbo.Pipeline_DEFValues_Manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
  WHERE (UPPER(i.COLUMN_NAME) 
	NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
	AND i.TABLE_SCHEMA = 'dbo')
  OR i.COLUMN_NAME IS NULL
GO

IF OBJECT_ID('dbo.v_Pipeline_DEFValues', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.v_Pipeline_DEFValues
END
GO

CREATE VIEW dbo.v_Pipeline_DEFValues
AS
SELECT
  v.*
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Pipeline_DEFValues v
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
WHERE (UPPER(i.column_name) 
	NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
	AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO


IF OBJECT_ID(N'dbo.v_Pipeline_Tables', 'V') IS NOT NULL
  DROP VIEW dbo.v_Pipeline_Tables
GO

CREATE VIEW dbo.v_Pipeline_Tables
AS
SELECT
  ROW_NUMBER() OVER (ORDER BY table_name, column_name) AS id
 ,table_name AS TableName
 ,column_name AS ColumnName
FROM information_schema.columns i
WHERE i.table_schema = 'dbo'
AND i.table_name LIKE 'Pipeline_%'
AND i.TABLE_NAME NOT LIKE 'Pipeline_Rule_%'
AND i.table_name NOT IN (
'Pipeline_Inbound',
'Pipeline_DEFValues',
'Pipeline_DEFValues_Manual',
'Pipeline_Documents',
'Pipeline_Inbound_Grid',
'Pipeline_Report'
)
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'Pipeline_Inbounds_FK', 'CREATEDDATE', 'CREATEDUSER')
