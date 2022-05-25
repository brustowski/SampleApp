IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_declaration_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_declaration_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_def_values_manual') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_def_values_manual
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_invoice_header_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_invoice_header_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_invoice_line_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_invoice_line_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_apply_def_values') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_apply_def_values
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing_del') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing_del
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing_param') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing_param
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.update_def_values_manual') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.update_def_values_manual
GO