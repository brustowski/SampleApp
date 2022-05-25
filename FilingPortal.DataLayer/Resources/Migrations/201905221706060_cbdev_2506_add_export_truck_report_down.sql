 ALTER TABLE [dbo].[truck_export_filing_headers]
   DROP COLUMN [response_xml] ,[request_xml] 
   GO

   ALTER TABLE [dbo].[truck_export_documents]
   DROP COLUMN [status] 
   GO
IF OBJECT_ID(N'dbo.Truck_Export_Report', 'V') IS NOT NULL
  DROP VIEW dbo.Truck_Export_Report
GO