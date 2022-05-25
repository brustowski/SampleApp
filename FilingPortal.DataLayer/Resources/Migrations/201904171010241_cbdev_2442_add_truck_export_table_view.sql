IF OBJECT_ID('dbo.v_truck_export_tables', 'V') IS NOT NULL
  DROP VIEW dbo.v_truck_export_tables
GO

CREATE VIEW dbo.v_truck_export_tables 
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
  ,tes.id AS section_id
 ,tes.title AS section_title
FROM information_schema.columns i
  INNER JOIN dbo.truck_export_sections tes
  ON i.TABLE_NAME = tes.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
GO

ALTER VIEW dbo.v_Truck_Tables
AS
SELECT
  ROW_NUMBER() OVER (ORDER BY table_name, column_name) AS id
 ,table_name AS TableName
 ,column_name AS ColumnName
FROM information_schema.columns i
WHERE i.table_schema = 'dbo'
AND i.table_name LIKE 'Truck_%'
AND i.TABLE_NAME NOT LIKE 'Truck_export_%'
AND i.TABLE_NAME NOT LIKE 'Truck_Rule_%'
AND i.table_name NOT IN (
'Truck_Inbound',
'Truck_DEFValues',
'Truck_DEFValues_Manual',
'Truck_Documents',
'Truck_Inbound_Grid',
'Truck_Report'
)
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'TI_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO