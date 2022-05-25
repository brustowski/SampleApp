ALTER VIEW inbond.v_form_configuration 
AS SELECT
  form.id
 ,form.label
 ,constr.[value] AS default_value
 ,form.description
 ,sections.table_name
 ,form.column_name AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,form.handbook_name
 ,form.paired_field_table
 ,form.paired_field_column
 ,form.display_on_ui
 ,form.manual
 ,form.single_filing_order
 ,CAST(form.has_default_value AS BIT) AS has_default_value
 ,CAST(form.editable AS BIT) AS editable
 ,CAST(form.mandatory AS BIT) AS mandatory
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM inbond.form_configuration AS form
JOIN inbond.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.columns AS c
  JOIN sys.default_constraints AS df
    ON df.parent_object_id = c.object_id
      AND df.parent_column_id = c.column_id
  WHERE c.object_id = OBJECT_ID(sections.table_name, 'U')
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW inbond.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns AS i
INNER JOIN inbond.form_section_configuration AS s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

UPDATE cfg
SET display_on_ui = 1
FROM inbond.form_configuration AS cfg
JOIN inbond.form_section_configuration AS section
  ON cfg.section_id = section.id
  AND section.table_name = 'inbond.commodities'
WHERE cfg.column_name = 'description';

UPDATE cfg
SET display_on_ui = 3
FROM inbond.form_configuration AS cfg
JOIN inbond.form_section_configuration AS section
  ON cfg.section_id = section.id
  AND section.table_name = 'inbond.commodities'
WHERE cfg.column_name = 'marks_and_numbers';

UPDATE cfg
SET display_on_ui = 9
FROM inbond.form_configuration AS cfg
JOIN inbond.form_section_configuration AS section
  ON cfg.section_id = section.id
  AND section.table_name = 'inbond.commodities'
WHERE cfg.column_name = 'template_type';
GO
