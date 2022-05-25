ALTER TABLE inbond.commodities
DROP COLUMN template_type;
GO
DELETE FROM inbond.form_configuration 
WHERE section_id = 6 AND column_name = 'template_type';
GO