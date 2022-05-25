DECLARE @command VARCHAR(MAX);
SELECT
  @command = COALESCE(@command, '') +
  CASE
    WHEN d.name IS NOT NULL THEN 'ALTER TABLE dbo.[' + section.table_name + '] DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
    ELSE ''
  END
  + 'ALTER TABLE dbo.[' + section.table_name + '] ADD DEFAULT (' +
  CASE
    WHEN tp.name IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '''' + field.[value] + ''''
    ELSE field.[value]
  END
  + ') FOR [' + field.column_name + '];' + CHAR(10)
FROM dbo.exp_vessel_form_configuration AS field
JOIN dbo.exp_vessel_form_section_configuration AS section
  ON field.section_id = section.id
JOIN sys.tables AS t
  ON section.table_name = t.name
JOIN sys.columns AS c
  ON c.object_id = t.object_id
    AND c.name = field.column_name
JOIN sys.types AS tp
  ON tp.system_type_id = c.system_type_id
LEFT JOIN sys.default_constraints AS d
  ON d.parent_object_id = t.object_id
    AND d.parent_column_id = c.column_id
WHERE field.has_default_value = 1
AND field.[value] IS NOT NULL
ORDER BY section.table_name;

EXEC (@command)
