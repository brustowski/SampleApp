INSERT INTO dbo.handbook_entry_status (code, description, status_type) VALUES 
 ('ADR','Awaiting Delete Response','Export')
,('AOR','Awaiting Original Response','Export')
,('ARR','Awaiting Replacement Response','Export')
,('COM','Compliance Alert - A potential reporting compliance violation may have occured','Export')
,('DSC','Delete SED Clear','Export')
,('ERR','Error - Invalid data, resolve and re-transmit','Export')
,('MES','Multiple Entry Status - Check Messages Tab for more details','Export')
,('OSC','Original SED Clear','Export')
,('RSC','Replacement SED Clear','Export')
,('RSR','Replacement SEF Requierd','Export')
,('UNK','Unknown Status - Check Messages Tab for more details','Export')
,('VRN','Verification requested - Please correct the shipment if data reported is incorrect','Export')
,('WRN','Warning - Check And Retransmit If Needed','Export')
,('WCR','Warning - Filer must correct the shipment data and retransmit whithi 4 days','Export')
,('NOT','Not Sent - only valid for exact match','Export');
GO

ALTER VIEW dbo.v_exp_truck_inbound_grid 
AS SELECT
  inbound.id
 ,inbound.exporter
 ,inbound.importer
 ,inbound.tariff_type
 ,inbound.tariff
 ,inbound.routed_tran
 ,inbound.sold_en_route
 ,inbound.master_bill
 ,inbound.origin
 ,inbound.export
 ,inbound.export_date
 ,inbound.eccn
 ,inbound.goods_description
 ,inbound.customs_qty
 ,inbound.price
 ,inbound.gross_weight
 ,inbound.gross_weight_uom
 ,inbound.hazardous
 ,inbound.origin_indicator
 ,inbound.goods_origin
 ,inbound.deleted
 ,inbound.created_date
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM exp_truck_rule_consignee rule_consignee
        WHERE RTRIM(LTRIM(rule_consignee.consignee_code)) = RTRIM(LTRIM(inbound.importer))) THEN 1
    ELSE 0
  END AS has_consignee_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM exp_truck_rule_exporter_consignee rule_exporter_consignee
        WHERE RTRIM(LTRIM(rule_exporter_consignee.consignee_code)) = RTRIM(LTRIM(inbound.importer))
        AND RTRIM(LTRIM(rule_exporter_consignee.exporter)) = RTRIM(LTRIM(inbound.exporter))) THEN 1
    ELSE 0
  END AS has_exporter_consignee_rule
FROM dbo.exp_truck_inbound inbound

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.exp_truck_filing_header etfh
  JOIN dbo.exp_truck_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbound.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

WHERE inbound.deleted = 0;
GO

ALTER VIEW dbo.v_imp_truck_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.Importer AS base_importer
 ,inbnd.paps
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,rule_importer.cw_ior AS importer
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,IIF(rule_importer.ior IS NULL, 0, 1) AS has_importer_rule
 ,IIF(rule_port.entry_port IS NULL, 0, 1) AS has_port_rule
FROM dbo.imp_truck_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_truck_filing_header etfh
  JOIN dbo.imp_truck_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.imp_truck_rule_importer rule_importer
  ON inbnd.importer = rule_importer.ior
LEFT JOIN dbo.imp_truck_rule_port rule_port
  ON rule_port.entry_port = rule_importer.entry_port
    AND rule_port.arrival_port = rule_importer.arrival_port
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

WHERE inbnd.deleted = 0;
GO

ALTER VIEW dbo.v_exp_vessel_inbound_grid 
AS SELECT DISTINCT
  inbnd.id
 ,usppi.ClientCode AS usppi
 ,usppi_address.code AS [address]
 ,contact.contact_name AS [contact]
 ,inbnd.phone
 ,importer.ClientCode AS importer
 ,vessel.[name] AS vessel
 ,inbnd.export_date
 ,inbnd.load_port
 ,inbnd.discharge_port
 ,country.code AS country_of_destination
 ,inbnd.tariff_type
 ,inbnd.tariff
 ,inbnd.goods_description
 ,inbnd.origin_indicator
 ,inbnd.quantity
 ,inbnd.[weight]
 ,inbnd.[value]
 ,inbnd.transport_ref
 ,inbnd.container
 ,inbnd.in_bond
 ,inbnd.sold_en_route
 ,inbnd.export_adjustment_value
 ,inbnd.original_itn
 ,inbnd.routed_transaction
 ,inbnd.reference_number
 ,inbnd.[description]
 ,inbnd.created_date
 ,inbnd.deleted
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_usppi_consignee.id IS NULL THEN 0
    ELSE 1
  END AS has_usppi_consignee_rule
FROM dbo.exp_vessel_inbound inbnd
JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id
JOIN dbo.Clients AS usppi
  ON inbnd.usppi_id = usppi.id
LEFT JOIN dbo.Client_Addresses AS usppi_address
  ON inbnd.address_id = usppi_address.id
LEFT JOIN dbo.handbook_vessel AS vessel
  ON inbnd.vessel_id = vessel.id
LEFT JOIN dbo.Countries AS country
  ON inbnd.country_of_destination_id = country.id
LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
  ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
    AND rule_usppi_consignee.consignee_id = inbnd.importer_id
LEFT JOIN handbook_cw_client_contacts contact ON inbnd.contact_id = contact.id
OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.exp_vessel_filing_header AS etfh
  JOIN dbo.exp_vessel_filing_detail AS etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN dbo.FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

WHERE inbnd.deleted = 0;
GO

ALTER VIEW dbo.v_imp_vessel_inbound_grid 
AS SELECT
  inbnd.id AS id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,vessel.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,inbnd.eta
 ,user_data.[Broker] AS filer_id
 ,inbnd.container
 ,inbnd.entry_type
 ,inbnd.owner_ref
 ,inbnd.unit_price
 ,inbnd.customs_qty
 ,country.code AS country_of_origin
 ,inbnd.created_date
 ,inbnd.deleted
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rules_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
 ,CASE
    WHEN rules_product.id IS NULL THEN 0
    ELSE 1
  END AS has_product_rule
FROM dbo.imp_vessel_inbound inbnd
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN dbo.Clients AS supplier
  ON inbnd.supplier_id = supplier.id
LEFT JOIN dbo.US_States states
  ON inbnd.state_id = states.id
LEFT JOIN dbo.Tariff tariffs
  ON inbnd.classification_id = tariffs.id
LEFT JOIN dbo.handbook_vessel vessel
  ON inbnd.vessel_id = vessel.id
LEFT JOIN dbo.cw_firms_codes firms_codes
  ON inbnd.firms_code_id = firms_codes.id
LEFT JOIN dbo.handbook_tariff_product_description descriptions
  ON inbnd.product_description_id = descriptions.id
LEFT JOIN dbo.Countries country
  ON inbnd.country_of_origin_id = country.id
LEFT JOIN dbo.app_users_data user_data
  ON inbnd.user_id = user_data.UserAccount
LEFT JOIN dbo.imp_vessel_rule_port rules_port
  ON inbnd.firms_code_id = rules_port.firms_code_id
LEFT JOIN dbo.imp_vessel_rule_product rules_product
  ON tariffs.USC_Tariff = rules_product.tariff

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_vessel_filing_header etfh
  JOIN dbo.imp_vessel_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

WHERE inbnd.deleted = 0;
GO

ALTER VIEW dbo.v_imp_pipeline_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.importer
 ,inbnd.batch
 ,inbnd.ticket_number
 ,inbnd.facility
 ,inbnd.site_name
 ,inbnd.quantity
 ,inbnd.api
 ,inbnd.entry_number
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_importer rule_importer
        WHERE inbnd.importer = rule_importer.importer) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_batch_code rule_batch
        WHERE dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code) THEN 1
    ELSE 0
  END AS has_batch_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_facility rule_facility
        WHERE inbnd.facility = rule_facility.facility) THEN 1
    ELSE 0
  END AS has_facility_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_price rule_price
        INNER JOIN dbo.Clients clients
          ON rule_price.importer_id = clients.id
        WHERE inbnd.importer = clients.ClientCode
        AND clients.id = rule_price.importer_id) THEN 1
    ELSE 0
  END AS has_price_rule
FROM dbo.imp_pipeline_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_pipeline_filing_header etfh
  JOIN dbo.imp_pipeline_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

WHERE inbnd.deleted = 0;
GO

ALTER VIEW dbo.v_imp_rail_inbound_grid 
AS SELECT
  inbound.id AS BD_Parsed_Id
 ,inbound.broker_download_id AS BD_Parsed_EDIMessage_Id
 ,filing_header.id AS Filing_Headers_id
 ,inbound.importer AS BD_Parsed_Importer
 ,inbound.supplier AS BD_Parsed_Supplier
 ,inbound.port_of_unlading AS BD_Parsed_PortOfUnlading
 ,inbound.description1 AS BD_Parsed_Description1
 ,inbound.bill_of_lading AS BD_Parsed_BillofLading
 ,inbound.issuer_code AS BD_Parsed_Issuer_Code
 ,CONCAT(inbound.equipment_initial, inbound.equipment_number) AS BD_Parsed_Container_Number
 ,inbound.reference_number1 AS BD_Parsed_ReferenceNumber1
 ,inbound.created_date AS BD_Parsed_CreatedDate
 ,inbound.deleted AS BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(inbound.duplicate_of, 0)) AS BD_Parsed_Is_Duplicated
 ,COALESCE(importer_supplier_exact.importer, importer_supplier.importer) AS Rule_ImporterSupplier_Importer
 ,COALESCE(importer_supplier_exact.main_supplier, importer_supplier.main_supplier) AS Rule_ImporterSupplier_Main_Supplier
 ,rail_description.tariff AS Rule_Desc1_Desc2_Tariff
 ,rail_port.[port] AS Rule_Port_Port
 ,filing_header.filing_number AS Filing_Headers_FilingNumber
 ,filing_header.job_hyperlink AS Filing_Headers_JobLink
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS Filing_Headers_MappingStatus
 ,mapping_status.[name] AS Filing_Headers_MappingStatus_Title
 ,filing_status.id AS Filing_Headers_FilingStatus
 ,filing_status.[name] AS Filing_Headers_FilingStatus_Title
 ,rail_description.[description]

FROM dbo.imp_rail_inbound inbound
LEFT JOIN dbo.imp_rail_rule_port rail_port
  ON inbound.port_of_unlading = rail_port.Port
LEFT JOIN dbo.imp_rail_rule_importer_supplier importer_supplier
  ON inbound.importer = importer_supplier.importer_name
    AND importer_supplier.product_description IS NULL
    AND (inbound.supplier = importer_supplier.supplier_name
      OR (inbound.supplier IS NULL
        AND importer_supplier.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_importer_supplier importer_supplier_exact
  ON inbound.importer = importer_supplier_exact.importer_name
    AND inbound.description1 = importer_supplier_exact.product_description
    AND (inbound.supplier = importer_supplier_exact.supplier_name
      OR (inbound.supplier IS NULL
        AND importer_supplier_exact.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_product rail_description
  ON rail_description.description1 = inbound.description1

OUTER APPLY (SELECT
    irfh.id
   ,irfh.filing_number
   ,irfh.job_hyperlink
   ,irfh.entry_status
   ,irfh.mapping_status
   ,irfh.filing_status
  FROM dbo.imp_rail_filing_header irfh
  JOIN dbo.imp_rail_filing_detail irfd
    ON irfh.id = irfd.filing_header_id
  WHERE irfd.inbound_id = inbound.id
  AND irfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code;
GO