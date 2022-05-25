 -- drop truck_export_delete_declaration_record procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_delete_befor_record') AND type in ('P', 'PC'))
	DROP PROCEDURE dbo.truck_export_delete_befor_record
GO

 -- drop delete trigger for truck_export_declarations table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_declarations_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_declarations_befor_delete
GO

-- drop delete trigger for truck_export_invoice_headers table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_invoice_headers_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_invoice_headers_befor_delete
GO

-- drop delete trigger for truck_export_invoice_lines table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_invoice_lines_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_invoice_lines_befor_delete
GO