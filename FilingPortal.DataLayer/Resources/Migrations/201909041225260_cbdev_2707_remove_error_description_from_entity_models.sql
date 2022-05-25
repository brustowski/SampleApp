ALTER VIEW dbo.Rail_Inbound_Grid
AS
SELECT DISTINCT
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
 ,rail_port.Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,rail_description.Description

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
 ,declaration_tab.Entry_Port BD_Parsed_PortOfUnlading
 ,declaration_tab.Description BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,containers_tab.Container_Number AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,declaration_tab.Importer Rule_ImporterSupplier_Importer
 ,declaration_tab.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,invoice_lines.Tariff Rule_Desc1_Desc2_Tariff
 ,declaration_tab.Entry_Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,invoice_lines.Description

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_DeclarationTab declaration_tab
  ON declaration_tab.Filing_Headers_FK = filing_headers.id
LEFT JOIN dbo.Rail_ContainersTab containers_tab
  ON containers_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = containers_tab.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invoice_lines
  ON invoice_lines.Filing_Headers_FK = filing_headers.id
WHERE filing_headers.MappingStatus > 0
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

ALTER VIEW dbo.Vessel_Import_Grid
AS
SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,imports.eta AS eta
 ,user_data.[Broker] AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.code AS country_of_origin
 ,imports.created_date AS created_date
 ,'' AS filing_number
 ,'' AS job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,imports.deleted AS is_deleted
 ,CASE
    WHEN rules_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
 ,CASE
    WHEN rules_product.id IS NULL THEN 0
    ELSE 1
  END AS has_product_rule
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
    AND headers.mapping_status <> 0
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.cw_firms_codes firms_codes
  ON imports.firms_code_id = firms_codes.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
LEFT JOIN Vessel_Rule_Port rules_port
  ON imports.firms_code_id = rules_port.firms_code_id
LEFT JOIN Vessel_Rule_Product rules_product
  ON tariffs.USC_Tariff = rules_product.tariff

WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Vessel_Import_Filing_Headers filing_headers
  INNER JOIN dbo.Vessel_Import_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.mapping_status > 0
  AND imports.id = filing_details.VI_FK)

UNION ALL

SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,imports.eta AS eta
 ,user_data.[Broker] AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.code AS country_of_origin
 ,imports.created_date AS created_date
 ,headers.filing_number
 ,headers.job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,imports.deleted AS is_deleted
 ,1 AS has_port_rule
 ,1 AS has_product_rule
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.cw_firms_codes firms_codes
  ON imports.firms_code_id = firms_codes.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE headers.mapping_status > 0
GO

ALTER VIEW dbo.vessel_export_grid
AS
SELECT DISTINCT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,country.code AS country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.[weight]
 ,ve.[value]
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.[description]
 ,'' AS filing_number
 ,'' AS job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,ve.deleted
 ,CASE
    WHEN rules_usppi.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_rule
 ,CASE
    WHEN rules_usppi_consignee.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_consignee_rule
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
LEFT JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
    AND vefh.mapping_status <> 0
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
LEFT JOIN Vessel_Export_Rule_USPPI rules_usppi
  ON ve.usppi_id = rules_usppi.usppi_id
LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee rules_usppi_consignee
  ON rules_usppi_consignee.usppi_id = ve.usppi_id
    AND rules_usppi_consignee.consignee_id = ve.importer_id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.vessel_export_filing_headers fh
  INNER JOIN dbo.vessel_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND ve.id = fd.vessel_export_id)
AND ve.deleted = 0

UNION

SELECT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,country.code AS country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.[weight]
 ,ve.[value]
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,ve.[description]
 ,vefh.filing_number
 ,vefh.job_link
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status 
 ,ve.deleted
 ,1 AS has_usppi_rule
 ,1 AS has_usppi_consignee_rule
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
INNER JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
GO