IF OBJECT_ID('dbo.v_truck_export_def_values_manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_truck_export_def_values_manual
GO

CREATE VIEW dbo.v_truck_export_def_values_manual 
AS SELECT
  v.*
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.truck_export_def_values_manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATED_DATE', 'CREATED_USER', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO
