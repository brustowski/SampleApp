ALTER VIEW dbo.Pipeline_Inbound_Grid 
AS SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Port
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.EntryNumber
 ,pi.CreatedDate 
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.ErrorDescription
FROM dbo.Pipeline_Inbound pi
LEFT OUTER JOIN dbo.Pipeline_Filing_Details fd
  ON pi.Id = fd.Pipeline_Inbounds_FK
LEFT OUTER JOIN dbo.Pipeline_Filing_Headers fh
  ON fd.Filing_Headers_FK = fh.id AND fh.MappingStatus <> 0
LEFT JOIN MappingStatus ms 
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs 
  ON isnull(fh.FilingStatus, 0) = fs.id
WHERE 
  NOT EXISTS (SELECT
      *
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
 ,pi.Port
 ,pi.Quantity
 ,pi.API
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.EntryNumber
 ,pi.CreatedDate 
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.ErrorDescription
FROM dbo.Pipeline_Filing_Headers fh
INNER JOIN dbo.Pipeline_Filing_Details fd
  ON fh.id = fd.Filing_Headers_FK
INNER JOIN dbo.Pipeline_Inbound pi
  ON fd.Pipeline_Inbounds_FK = pi.Id
LEFT JOIN MappingStatus ms 
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs 
  ON isnull(fh.FilingStatus, 0) = fs.id
WHERE 
  fh.MappingStatus > 0 AND pi.FDeleted = 0
GO

ALTER VIEW dbo.Truck_Inbound_Grid 
AS SELECT
  ti.Id AS ID
 ,tfh.id AS Filing_Headers_Id
 ,ti.Importer AS BaseImporter
 ,tri.cw_ior AS Importer
 ,ti.PAPs
 ,'' AS FilingNumber
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,tfh.ErrorDescription
FROM dbo.Truck_Inbound ti
LEFT JOIN dbo.Truck_Rule_Importers tri
  ON ti.Importer = tri.ior
LEFT JOIN dbo.Truck_Filing_Details tfd
  ON tfd.BDP_FK = ti.Id
LEFT JOIN dbo.Truck_Filing_Headers tfh
  ON tfh.id = tfd.Filing_Headers_FK
    AND tfh.MappingStatus <> 0
WHERE NOT EXISTS (SELECT
    *
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
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,tfh.ErrorDescription

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
