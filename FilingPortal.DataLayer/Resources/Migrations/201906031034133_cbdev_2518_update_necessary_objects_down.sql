--- update pipeline tables view ---
ALTER VIEW dbo.v_Pipeline_Tables 
AS SELECT
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
GO

--- update Pipeline DefValues Manual view ---
ALTER VIEW dbo.v_Pipeline_DEFValues_Manual 
AS SELECT
  v.id
 ,v.Filing_Headers_FK AS filing_header_id
 ,v.CreatedDate AS created_date
 ,v.CreatedUser AS created_user
 ,v.Display_on_UI AS display_on_ui
 ,v.FEditable AS editable
 ,v.FHasDefaultVal AS has_default_value
 ,v.FMandatory AS mandatory
 ,v.FManual AS manual
 ,v.ModifiedDate AS modification_date
 ,v.ValueLabel AS label
 ,v.parent_record_id
 ,v.section_name
 ,v.section_title
 ,v.record_id
 ,v.description
 ,v.table_name
 ,v.column_name
 ,v.value
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.Pipeline_DEFValues_Manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL