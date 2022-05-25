--- update pipeline tables view ---
ALTER VIEW dbo.v_Pipeline_Tables 
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN pipeline_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(i.COLUMN_NAME) NOT IN ('ID', 'PI_FK', 'FILING_HEADERS_FK', 'PIPELINE_INBOUNDS_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

--- update Pipeline DefValues Manual view ---
ALTER VIEW dbo.v_Pipeline_DEFValues_Manual 
AS SELECT
  v.id
 ,v.Filing_Headers_FK AS filing_header_id
 ,v.CreatedDate AS created_date
 ,v.CreatedUser AS created_user
 ,v.Display_on_UI AS display_on_ui
 ,CAST(v.FEditable AS BIT) AS editable
 ,CAST(v.FHasDefaultVal AS BIT) AS has_default_value
 ,CAST(v.FMandatory AS BIT) AS mandatory
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