DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (21200, 21201, 21202, 21203, 21204, 21205, 21206)
DELETE FROM dbo.App_Permissions WHERE id IN (21200, 21201, 21202, 21203, 21204, 21205, 21206)
DELETE FROM dbo.App_Roles WHERE id IN (21200, 21201)
GO

DROP VIEW IF EXISTS zones_ftz214.v_field_configuration;
GO

DROP VIEW IF EXISTS zones_ftz214.v_form_configuration;
GO

DROP VIEW IF EXISTS zones_ftz214.v_inbound_grid;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_delete_entry_records;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_delete_inbound;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_recalculate;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_review_entry;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_update_entry;
GO

DROP PROCEDURE IF EXISTS zones_ftz214.sp_inbound_validate;
GO
