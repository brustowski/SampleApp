DELETE FROM dbo.App_Permissions_Roles WHERE App_Permissions_FK IN (21100, 21101, 21102, 21103, 21104, 21105, 21106)
DELETE FROM dbo.App_Permissions WHERE id IN (21100, 21101, 21102, 21103, 21104, 21105, 21106)
DELETE FROM dbo.App_Roles WHERE id IN (21100, 21101)
GO

DROP VIEW IF EXISTS zones_entry.v_field_configuration;
GO

DROP VIEW IF EXISTS zones_entry.v_form_configuration;
GO

DROP VIEW IF EXISTS zones_entry.v_inbound_grid;
GO

DROP VIEW IF EXISTS zones_entry.v_report;
GO

DROP TABLE IF EXISTS zones_entry.declaration;
GO

DROP TABLE IF EXISTS zones_entry.packing;
GO

DROP TABLE IF EXISTS zones_entry.invoice_line;
GO
DROP FUNCTION IF EXISTS zones_entry.fn_invoice_line_number;
GO

DROP TABLE IF EXISTS zones_entry.invoice_header;
GO
DROP FUNCTION IF EXISTS zones_entry.fn_invoice_number;
GO

DROP TABLE IF EXISTS zones_entry.misc;
GO

