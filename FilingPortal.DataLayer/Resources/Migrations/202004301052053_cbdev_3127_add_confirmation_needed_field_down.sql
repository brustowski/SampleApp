ALTER VIEW dbo.v_imp_rail_form_configuration 
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
 ,form.has_default_value
 ,form.editable
 ,form.mandatory
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.imp_rail_form_configuration AS form
JOIN dbo.imp_rail_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.imp_rail_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW dbo.v_imp_pipeline_form_configuration 
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.imp_pipeline_form_configuration AS form
JOIN dbo.imp_pipeline_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.imp_pipeline_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW dbo.v_imp_truck_form_configuration 
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.imp_truck_form_configuration AS form
JOIN dbo.imp_truck_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.imp_truck_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW dbo.v_exp_truck_form_configuration 
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.exp_truck_form_configuration AS form
JOIN dbo.exp_truck_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.exp_truck_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW dbo.v_imp_vessel_form_configuration 
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.imp_vessel_form_configuration AS form
JOIN dbo.imp_vessel_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.imp_vessel_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

ALTER VIEW dbo.v_exp_vessel_form_configuration 
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
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
FROM dbo.exp_vessel_form_configuration AS form
JOIN dbo.exp_vessel_form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
    AND clmn.table_schema = 'dbo'
LEFT JOIN dbo.exp_vessel_form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.default_constraints df
  INNER JOIN sys.tables t
    ON df.parent_object_id = t.object_id
  INNER JOIN sys.columns c
    ON c.object_id = df.parent_object_id
      AND df.parent_column_id = c.column_id
  WHERE t.Name = sections.table_name
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO