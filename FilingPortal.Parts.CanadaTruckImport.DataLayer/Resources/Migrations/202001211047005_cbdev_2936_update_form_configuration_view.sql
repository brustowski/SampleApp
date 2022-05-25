ALTER VIEW canada_imp_truck.v_form_configuration 
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
FROM canada_imp_truck.form_configuration AS form
JOIN canada_imp_truck.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS clmn
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