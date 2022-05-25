ALTER VIEW dbo.Pipeline_Inbound_Grid
AS
SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
FROM dbo.Pipeline_Inbound pi
LEFT OUTER JOIN dbo.Pipeline_Filing_Details fd
  ON pi.Id = fd.Pipeline_Inbounds_FK
LEFT OUTER JOIN dbo.Pipeline_Filing_Headers fh
  ON fd.Filing_Headers_FK = fh.id
    AND fh.MappingStatus <> 0
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Pipeline_Filing_Headers fh
  INNER JOIN dbo.Pipeline_Filing_Details fd
    ON fh.id = fd.Filing_Headers_FK
  WHERE fh.MappingStatus > 0
  AND pi.Id = fd.Pipeline_Inbounds_FK)
AND pi.FDeleted = 0

UNION

SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
FROM dbo.Pipeline_Filing_Headers fh
INNER JOIN dbo.Pipeline_Filing_Details fd
  ON fh.id = fd.Filing_Headers_FK
INNER JOIN dbo.Pipeline_Inbound pi
  ON fd.Pipeline_Inbounds_FK = pi.Id
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE fh.MappingStatus > 0
AND pi.FDeleted = 0
GO

ALTER VIEW dbo.truck_export_grid
AS
SELECT DISTINCT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter
 ,te.importer
 ,te.tariff_type
 ,te.tariff
 ,te.routed_tran
 ,te.sold_en_route
 ,te.master_bill
 ,te.origin
 ,te.export
 ,te.export_date
 ,te.eccn
 ,te.goods_description
 ,te.customs_qty
 ,te.price
 ,te.gross_weight
 ,te.gross_weight_uom
 ,te.hazardous
 ,te.origin_indicator
 ,te.goods_origin
 ,'' AS filing_number
 ,'' AS job_link
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
LEFT JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
    AND tefh.mapping_status <> 0
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.truck_export_filing_headers fh
  INNER JOIN dbo.truck_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND te.id = fd.truck_export_id)
AND te.deleted = 0

UNION

SELECT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter
 ,te.importer
 ,te.tariff_type
 ,te.tariff
 ,te.routed_tran
 ,te.sold_en_route
 ,te.master_bill
 ,te.origin
 ,te.export
 ,te.export_date
 ,te.eccn
 ,te.goods_description
 ,te.customs_qty
 ,te.price
 ,te.gross_weight
 ,te.gross_weight_uom
 ,te.hazardous
 ,te.origin_indicator
 ,te.goods_origin
 ,tefh.filing_number
 ,tefh.job_link
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
INNER JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
WHERE tefh.mapping_status <> 0
AND te.deleted = 0
GO

ALTER VIEW dbo.Truck_Inbound_Grid
AS
SELECT
  ti.Id AS ID
 ,tfh.id AS Filing_Headers_Id
 ,ti.Importer AS BaseImporter
 ,tri.cw_ior AS Importer
 ,ti.PAPs
 ,'' AS FilingNumber
 ,'' AS job_link
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,ti.FDeleted
FROM dbo.Truck_Inbound ti
LEFT JOIN dbo.Truck_Rule_Importers tri
  ON ti.Importer = tri.ior
LEFT JOIN dbo.Truck_Filing_Details tfd
  ON tfd.BDP_FK = ti.Id
LEFT JOIN dbo.Truck_Filing_Headers tfh
  ON tfh.id = tfd.Filing_Headers_FK
    AND tfh.MappingStatus <> 0
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Truck_Filing_Headers tfh
  INNER JOIN dbo.Truck_Filing_Details tfd
    ON tfh.id = tfd.Filing_Headers_FK
  WHERE tfh.MappingStatus > 0
  AND ti.Id = tfd.BDP_FK)
AND ti.FDeleted = 0

UNION

SELECT
  ti.Id AS ID
 ,tfh.id AS Filing_Headers_Id
 ,ti.Importer AS BaseImporter
 ,declaration.Importer AS Importer
 ,ti.PAPs
 ,tfh.FilingNumber AS FilingNumber
 ,tfh.job_link
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,ti.FDeleted
FROM dbo.Truck_Filing_Headers tfh
INNER JOIN dbo.Truck_Filing_Details tfd
  ON tfh.id = tfd.Filing_Headers_FK
INNER JOIN dbo.Truck_Inbound ti
  ON tfd.BDP_FK = ti.Id
LEFT JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = tfh.id
    AND tfd.BDP_FK = declaration.TI_FK
WHERE tfh.MappingStatus > 0
AND ti.FDeleted = 0
GO