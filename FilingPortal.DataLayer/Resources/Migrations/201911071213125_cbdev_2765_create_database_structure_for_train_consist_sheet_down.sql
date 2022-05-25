DROP PROCEDURE IF EXISTS dbo.sp_imp_rail_audit_train_consist_sheet_verify
GO

DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (48)
DELETE FROM dbo.App_Permissions WHERE id IN (48)
DELETE FROM dbo.App_Roles WHERE id IN (15)