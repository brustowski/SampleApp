set ansi_warnings off
go
ALTER TABLE [dbo].[Truck_Filing_Headers] Alter column [job_link] varchar(128)
GO
ALTER TABLE [dbo].[truck_export_filing_headers] Alter column [job_link] varchar(128)
GO
ALTER TABLE [dbo].[Pipeline_Filing_Headers] Alter column [job_link] varchar(128)
GO
ALTER TABLE [dbo].[Vessel_Export_Filing_Headers] Alter column [job_link] varchar(128)
GO
ALTER TABLE [dbo].[Vessel_Import_Filing_Headers] Alter column [job_link] varchar(128)
GO