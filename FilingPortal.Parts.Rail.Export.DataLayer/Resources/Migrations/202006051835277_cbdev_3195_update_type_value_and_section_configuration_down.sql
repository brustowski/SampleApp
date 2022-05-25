UPDATE us_exp_rail.form_section_configuration
SET is_hidden = 0
WHERE name = 'container';
GO

DECLARE @command VARCHAR(MAX);
SELECT
  @command = 'ALTER TABLE us_exp_rail.inbound_containers DROP CONSTRAINT [' + d.name + '];' + CHAR(10)
FROM sys.columns AS c
JOIN sys.default_constraints AS d
  ON d.parent_object_id = c.object_id
    AND d.parent_column_id = c.column_id
WHERE c.name = 'gross_weight'
AND c.object_id = OBJECT_ID('us_exp_rail.inbound_containers', 'U');

EXEC(@command);
GO