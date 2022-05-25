UPDATE dbo.truck_export_sections SET procedure_name = 'truck_export_add_declaration_record' WHERE table_name = 'truck_export_declarations'
UPDATE dbo.truck_export_sections SET procedure_name = 'truck_export_add_invoice_header_record' WHERE table_name = 'truck_export_invoice_headers'
UPDATE dbo.truck_export_sections SET procedure_name = 'truck_export_add_invoice_line_record' WHERE table_name = 'truck_export_invoice_lines'
go

 -- add truck_export_delete_record procedure
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_delete_record') AND type in ('P', 'PC'))
	DROP PROCEDURE dbo.truck_export_delete_record
GO

CREATE PROCEDURE dbo.truck_export_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.truck_export_sections tes
      WHERE tes.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE THROW 51000, 'Invalid table name', 1
END
GO

 -- add delete trigger for truck_export_declarations table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_declarations_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_declarations_befor_delete
GO

CREATE TRIGGER dbo.truck_export_declarations_befor_delete
    ON dbo.truck_export_declarations
    FOR DELETE
AS
    DELETE FROM dbo.truck_export_def_values_manual
    WHERE table_name='truck_export_declarations' AND record_id IN(SELECT deleted.id FROM deleted)
GO

-- add delete trigger for truck_export_invoice_headers table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_invoice_headers_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_invoice_headers_befor_delete
GO

CREATE TRIGGER dbo.truck_export_invoice_headers_befor_delete
    ON dbo.truck_export_invoice_headers
    FOR DELETE
AS
    DELETE FROM dbo.truck_export_def_values_manual
    WHERE table_name='truck_export_invoice_headers' AND record_id IN(SELECT deleted.id FROM deleted)
GO

-- add delete trigger for truck_export_invoice_lines table
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_invoice_lines_befor_delete') AND type in ('TR', 'TA'))
	DROP TRIGGER dbo.truck_export_invoice_lines_befor_delete
GO

CREATE TRIGGER dbo.truck_export_invoice_lines_befor_delete
    ON dbo.truck_export_invoice_lines
    FOR DELETE
AS
    DELETE FROM dbo.truck_export_def_values_manual
    WHERE table_name='truck_export_invoice_lines' AND record_id IN(SELECT deleted.id FROM deleted)
GO