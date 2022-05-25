ALTER VIEW dbo.v_Rail_Tables 
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN rail_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO