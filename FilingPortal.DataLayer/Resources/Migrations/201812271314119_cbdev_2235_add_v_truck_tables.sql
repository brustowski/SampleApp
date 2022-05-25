CREATE VIEW dbo.v_Truck_Tables
AS
SELECT
  ROW_NUMBER() OVER (ORDER BY table_name, column_name) AS id
 ,table_name AS TableName
 ,column_name AS ColumnName
FROM information_schema.columns i
WHERE i.table_schema = 'dbo'
AND i.table_name LIKE 'Truck_%'
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