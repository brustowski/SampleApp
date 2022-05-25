set ansi_warnings off
go
ALTER TABLE [dbo].[truck_export_declarations]
  DROP COLUMN [country_of_export] 
  GO
 
  ALTER TABLE [dbo].[truck_export_invoice_lines]
  DROP COLUMN  [goods_origin]
  GO
 

