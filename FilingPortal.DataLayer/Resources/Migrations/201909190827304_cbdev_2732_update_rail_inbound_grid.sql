ALTER VIEW dbo.Rail_Inbound_Grid
AS SELECT DISTINCT
  bd_parsed.BDP_PK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.[Port] Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.[name] Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.[name] Filing_Headers_FilingStatus_Title
 ,rail_description.[Description]

FROM dbo.Rail_BD_Parsed bd_parsed
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
LEFT JOIN dbo.Rail_Filing_Details filing_details
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers filing_headers
  ON filing_headers.id = filing_details.Filing_Headers_FK
    AND filing_headers.MappingStatus <> 0
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.MappingStatus, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.FilingStatus, 0) = filing_status.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Rail_Filing_Headers filing_headers
  INNER JOIN dbo.Rail_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.MappingStatus > 0
  AND bd_parsed.BDP_PK = filing_details.BDP_FK)

UNION ALL

SELECT
  filing_details.BDP_FK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.[Port] Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.[name] Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.[name] Filing_Headers_FilingStatus_Title
 ,rail_description.[Description]

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.[Port]
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
WHERE filing_headers.MappingStatus > 0
GO