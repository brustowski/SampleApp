DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (23001, 23002, 23003, 23004, 23005, 23006, 23007)
DELETE FROM dbo.App_Permissions WHERE id IN (23001, 23002, 23003, 23004, 23005, 23006, 23007)
DELETE FROM dbo.App_Roles WHERE id IN (23000, 23001)
GO


DROP VIEW IF EXISTS us_exp_rail.v_field_configuration
GO

DROP VIEW IF EXISTS us_exp_rail.v_form_configuration
GO

DROP VIEW IF EXISTS us_exp_rail.v_inbound_grid
GO

DROP PROCEDURE IF EXISTS us_exp_rail.sp_delete_entry_records
GO

DROP PROCEDURE IF EXISTS us_exp_rail.sp_delete_inbound
GO

DROP PROCEDURE IF EXISTS us_exp_rail.sp_recalculate
GO

DROP PROCEDURE IF EXISTS us_exp_rail.sp_review_entry
GO

DROP PROCEDURE IF EXISTS us_exp_rail.sp_update_entry
GO

TRUNCATE TABLE us_exp_rail.form_configuration
GO

DELETE FROM us_exp_rail.form_section_configuration
GO

DROP PROCEDURE us_exp_rail.sp_create_entry_records
GO

