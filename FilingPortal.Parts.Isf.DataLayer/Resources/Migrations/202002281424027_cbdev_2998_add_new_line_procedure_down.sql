DROP PROCEDURE isf.sp_add_empty_line;
GO

UPDATE isf.form_section_configuration
SET title = 'Lines'
   ,name = 'lines'
WHERE id = 3;
UPDATE isf.form_section_configuration
SET title = 'line'
   ,name = 'line'
   ,procedure_name = 'sp_add_line'
WHERE id = 4;
UPDATE isf.form_section_configuration
SET title = 'Manufacturers'
   ,name = 'manufacturers'
WHERE id = 5;
UPDATE isf.form_section_configuration
SET title = 'Manufacturer'
   ,name = 'manufacturer'
WHERE id = 6;
GO