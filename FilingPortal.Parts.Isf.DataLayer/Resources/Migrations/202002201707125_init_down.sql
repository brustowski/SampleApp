DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (22001, 22002, 22003, 22004)
DELETE FROM dbo.App_Permissions WHERE id IN (22001, 22002, 22003, 22004)
DELETE FROM dbo.App_Roles WHERE id IN (22000)
GO


DROP VIEW IF EXISTS isf.v_field_configuration
GO

DROP VIEW IF EXISTS isf.v_form_configuration
GO

DROP VIEW IF EXISTS isf.v_inbound_grid
GO

DROP PROCEDURE IF EXISTS isf.sp_delete_entry_records
GO

DROP PROCEDURE IF EXISTS isf.sp_delete_inbound
GO

DROP PROCEDURE IF EXISTS isf.sp_recalculate
GO

DROP PROCEDURE IF EXISTS isf.sp_review_entry
GO

DROP PROCEDURE IF EXISTS isf.sp_update_entry
GO

TRUNCATE TABLE isf.form_configuration;
GO

DELETE FROM isf.form_section_configuration;
GO

DROP PROCEDURE isf.sp_create_entry_records;
GO

DROP PROCEDURE isf.sp_add_main_detail;
GO

DROP PROCEDURE isf.sp_add_line;
GO

DROP PROCEDURE isf.sp_add_container;
GO

DROP PROCEDURE isf.sp_add_routing;
GO

DROP VIEW isf.v_report;
GO

DROP TABLE isf.main_detail;
GO

DROP TABLE isf.line;
GO

DROP TABLE isf.container;
GO

DROP TABLE isf.routing;
GO