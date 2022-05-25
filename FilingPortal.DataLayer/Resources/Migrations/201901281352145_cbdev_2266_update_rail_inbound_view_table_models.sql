ALTER VIEW dbo.Rail_Inbound_Grid
AS
SELECT DISTINCT
  p.BDP_PK BD_Parsed_Id
 ,p.BDP_EM BD_Parsed_EDIMessage_Id
 ,h.id Filing_Headers_id
 ,p.Importer BD_Parsed_Importer
 ,p.Supplier BD_Parsed_Supplier
 ,p.PortOfUnlading BD_Parsed_PortOfUnlading
 ,p.Description1 BD_Parsed_Description1
 ,p.Description2 BD_Parsed_Description2
 ,p.BillofLading BD_Parsed_BillofLading
 ,p.CreatedDate BD_Parsed_CreatedDate
 ,CONCAT(p.EquipmentInitial, p.EquipmentNumber) AS BD_Parsed_Container_Number
 ,p.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,p.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(p.DuplicateOf,0)) BD_Parsed_Is_Duplicated
 ,rn.Importer Rule_ImporterSupplier_Importer
 ,rn.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rd.Tariff Rule_Desc1_Desc2_Tariff
 ,RP.Port Rule_Port_Port
 ,h.FilingNumber Filing_Headers_FilingNumber
 ,ISNULL(h.MappingStatus, 0) Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(h.FilingStatus, 0) Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,rd.Description
 ,h.ErrorDescription

FROM dbo.Rail_BD_Parsed p
LEFT JOIN dbo.Rail_Rule_Port rp
  ON p.PortOfUnlading = RP.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
  ON p.Importer = rn.Importer_Name
    AND (p.Supplier = rn.Supplier_Name
      OR (p.Supplier IS NULL
        AND rn.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rd
  ON rd.Description1 = p.Description1
    AND (p.Description2 = rd.Description2
      OR (p.Description2 IS NULL
        AND rd.Description2 IS NULL))
LEFT JOIN dbo.Rail_Filing_Details f
  ON f.BDP_FK = p.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers h
  ON h.id = f.Filing_Headers_FK
    AND h.MappingStatus <> 0
LEFT JOIN MappingStatus ms
  ON ISNULL(h.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(h.FilingStatus, 0) = fs.id
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Rail_Filing_Headers h
  INNER JOIN dbo.Rail_Filing_Details f
    ON h.id = f.Filing_Headers_FK
  WHERE h.MappingStatus > 0
  AND p.BDP_PK = f.BDP_FK)

UNION ALL

SELECT
  f.BDP_FK BD_Parsed_Id
 ,p.BDP_EM BD_Parsed_EDIMessage_Id
 ,h.id Filing_Headers_id
 ,p.Importer BD_Parsed_Importer
 ,p.Supplier BD_Parsed_Supplier
 ,d.Entry_Port BD_Parsed_PortOfUnlading
 ,d.Description BD_Parsed_Description1
 ,l.Attribute_1 BD_Parsed_Description2
 ,d.Master_Bill BD_Parsed_BillofLading
 ,p.CreatedDate BD_Parsed_CreatedDate
 ,c.Container_Number AS BD_Parsed_Container_Number
 ,p.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,p.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(p.DuplicateOf,0)) BD_Parsed_Is_Duplicated
 ,d.Importer Rule_ImporterSupplier_Importer
 ,d.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,l.Tariff Rule_Desc1_Desc2_Tariff
 ,d.Entry_Port Rule_Port_Port
 ,h.FilingNumber Filing_Headers_FilingNumber
 ,ISNULL(h.MappingStatus, 0) Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(h.FilingStatus, 0) Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,l.Description
 ,h.ErrorDescription

FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT JOIN dbo.MappingStatus ms
  ON h.MappingStatus = ms.id
LEFT JOIN dbo.FilingStatus fs
  ON h.FilingStatus = fs.id
LEFT JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
    AND f.BDP_FK = d.BDP_FK
LEFT JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines l
  ON l.Filing_Headers_FK = h.id
    AND f.BDP_FK = l.BDP_FK
WHERE h.MappingStatus > 0