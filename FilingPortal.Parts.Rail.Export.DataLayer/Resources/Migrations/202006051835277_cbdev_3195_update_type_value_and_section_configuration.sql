UPDATE us_exp_rail.inbound_containers
SET type = DEFAULT
WHERE type IS NULL
OR type = '';
GO

UPDATE us_exp_rail.form_section_configuration
SET is_hidden = 1
WHERE name = 'container';
GO

ALTER TABLE us_exp_rail.inbound_containers
ADD CONSTRAINT DF__inbound__gross_weight
DEFAULT 0.00 FOR gross_weight;
GO

UPDATE us_exp_rail.inbound_containers
SET gross_weight = DEFAULT
WHERE gross_weight IS NULL;
GO
