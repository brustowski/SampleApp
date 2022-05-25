IF OBJECT_ID('dbo.truck_export_invoice_lines', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_invoice_lines
GO

IF OBJECT_ID('dbo.truck_export_invoice_headers', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_invoice_headers
GO

IF OBJECT_ID('dbo.truck_export_declarations', 'U') IS NOT NULL
  DROP TABLE dbo.truck_export_declarations
GO