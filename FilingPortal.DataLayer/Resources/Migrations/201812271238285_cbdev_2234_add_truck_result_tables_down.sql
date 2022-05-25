ALTER VIEW dbo.v_Truck_Filing_Data 
AS SELECT
  ti.Id AS id
  ,tfh.id AS Filing_header_id 
  ,tfd.Importer AS Importer
  ,ti.PAPs
FROM dbo.Truck_Filing_Headers tfh
LEFT OUTER JOIN dbo.Truck_FilingData tfd
  ON tfd.FILING_HEADERS_FK = tfh.id
  INNER JOIN Truck_Inbound ti
  ON ti.Id = tfd.TI_FK
GO

IF OBJECT_ID(N'dbo.Truck_ContainersTab', 'U') IS NOT NULL
  DROP TABLE dbo.Truck_ContainersTab
GO

IF OBJECT_ID(N'dbo.Truck_DeclarationTab', 'U') IS NOT NULL
  DROP TABLE dbo.Truck_DeclarationTab
GO

IF OBJECT_ID(N'dbo.Truck_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.Truck_InvoiceHeaders
GO

IF OBJECT_ID(N'dbo.Truck_InvoiceLines', 'U') IS NOT NULL
  DROP TABLE dbo.Truck_InvoiceLines
GO

IF OBJECT_ID(N'dbo.Truck_MISC', 'U') IS NOT NULL
  DROP TABLE dbo.Truck_MISC
GO
