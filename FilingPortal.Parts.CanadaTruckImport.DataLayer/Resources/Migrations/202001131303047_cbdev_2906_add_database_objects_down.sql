DROP VIEW IF EXISTS canada_imp_truck.v_form_configuration
GO

DROP VIEW IF EXISTS canada_imp_truck.v_field_configuration
GO

DROP TABLE IF EXISTS canada_imp_truck.declaration
GO

DROP TABLE IF EXISTS canada_imp_truck.invoice_lines_charges
GO

DROP TABLE IF EXISTS canada_imp_truck.invoice_lines_duties_and_taxes
GO

DROP TABLE IF EXISTS canada_imp_truck.invoice_lines
GO

DROP TABLE IF EXISTS canada_imp_truck.invoice_headers
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_add_declaration
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_add_invoice_header
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_add_invoice_line
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_add_invoice_line_charge
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_add_invoice_line_duties_and_taxes
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_create_entry_records
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_delete_entry_records
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_delete_inbound
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_recalculate
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_review_entry
GO

DROP PROCEDURE IF EXISTS canada_imp_truck.sp_update_entry
GO

TRUNCATE TABLE canada_imp_truck.form_configuration
GO

DELETE FROM canada_imp_truck.form_section_configuration
GO