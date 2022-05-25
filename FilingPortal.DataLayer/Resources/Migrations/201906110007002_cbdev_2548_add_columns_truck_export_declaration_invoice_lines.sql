ALTER TABLE [dbo].[truck_export_declarations]
  ADD [country_of_export] [varchar](10) NULL
  GO
 
  ALTER TABLE [dbo].[truck_export_invoice_lines]
  ADD  [goods_origin] [varchar](10) NULL
  GO


