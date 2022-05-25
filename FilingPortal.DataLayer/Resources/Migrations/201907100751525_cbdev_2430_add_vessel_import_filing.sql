IF OBJECT_ID('dbo.v_Vessel_Import_Def_Values', 'V') IS NOT NULL
  DROP VIEW dbo.v_Vessel_Import_Def_Values
GO

CREATE VIEW dbo.v_Vessel_Import_Def_Values
AS
SELECT
  vdv.id
 ,vdv.column_name
 ,vdv.created_date
 ,vdv.created_user
 ,vdv.default_value
 ,vdv.display_on_ui
 ,vdv.editable
 ,vdv.has_default_value
 ,vdv.mandatory
 ,vdv.[manual]
 ,vdv.[description]
 ,vdv.label
 ,vdv.single_filing_order
 ,vdv.paired_field_table
 ,vdv.paired_field_column
 ,vdv.handbook_name
 ,vs.table_name
 ,vs.[name] AS section_name
 ,vs.title AS section_title
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.vessel_import_def_values vdv
INNER JOIN dbo.vessel_import_sections vs
  ON vdv.section_id = vs.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(vdv.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(vs.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

IF OBJECT_ID('dbo.v_Vessel_Import_Def_Values_Manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_Vessel_Import_Def_Values_Manual
GO

CREATE VIEW dbo.v_Vessel_Import_Def_Values_Manual
AS
SELECT
  vdvm.id
 ,vdvm.filing_header_id
 ,vdvm.parent_record_id
 ,vdvm.record_id
 ,vdvm.section_name
 ,vdvm.section_title
 ,vdvm.table_name
 ,vdvm.column_name
 ,vdvm.label
 ,vdvm.[value]
 ,vdvm.description
 ,vdvm.editable
 ,vdvm.has_default_value
 ,vdvm.mandatory
 ,vdvm.paired_field_table
 ,vdvm.paired_field_column
 ,vdvm.handbook_name
 ,vdvm.display_on_ui
 ,vdvm.manual
 ,vdvm.modification_date
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.vessel_import_def_values_manual vdvm
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(vdvm.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(vdvm.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

IF OBJECT_ID('dbo.v_Vessel_Import_Tables', 'V') IS NOT NULL
  DROP VIEW dbo.v_Vessel_Import_Tables
GO

CREATE VIEW dbo.v_Vessel_Import_Tables
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM INFORMATION_SCHEMA.COLUMNS i
INNER JOIN vessel_import_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
GO