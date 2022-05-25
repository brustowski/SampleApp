INSERT INTO dbo.App_Permissions(id, description, name) VALUES(41, 'Add Vessel Export Records Permission', 'VesselAddExportRecord')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(42, 'View Vessel Export Records Permission', 'VesselViewExportRecord')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(43, 'Delete Vessel Export Records Permission', 'VesselDeleteExportRecord')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(44, 'File Vessel Export Records Permission', 'VesselFileExportRecord')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(45, 'View Vessel Export Rule Records Permission', 'VesselViewExportRecordRules')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(46, 'Edit Vessel Export Rule Records Permission', 'VesselEditExportRecordRules')
INSERT INTO dbo.App_Permissions(id, description, name) VALUES(47, 'Delete Vessel Export Rule Records Permission', 'VesselDeleteExportRecordRules')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (13, 'VesselExportUser', 'The role with following permissions: Import, View, Edit, and File Vessel Export data; View Vessel Export Rules data.')
INSERT dbo.App_Roles(id, title, description) VALUES (14, 'VesselExportPowerUser', 'The role with following permissions: View, Edit, and File Vessel Export and Rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (41, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (42, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (43, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (44, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (45, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (46, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (47, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (41, 13)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (42, 13)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (43, 13)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (44, 13)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (45, 13)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (47, 13)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (41, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (42, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (43, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (44, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (45, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (46, 14)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (47, 14)

GO

CREATE VIEW dbo.v_vessel_export_tables 
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
  ,tes.id AS section_id
 ,tes.title AS section_title
FROM information_schema.columns i
  INNER JOIN dbo.vessel_export_sections tes
  ON i.TABLE_NAME = tes.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
GO

CREATE VIEW dbo.vessel_export_grid
AS
SELECT DISTINCT
  ve.id
 ,vefh.id AS filing_header_id
 ,usppi.ClientCode AS usppi
 ,importer.ClientCode AS importer
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.description
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,'' AS filing_number
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
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
WHERE NOT EXISTS (SELECT
    *
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
 ,vessel.name AS vessel
 ,ve.export_date
 ,ve.load_port
 ,ve.discharge_port
 ,ve.country_of_destination
 ,ve.tariff_type
 ,ve.tariff
 ,ve.goods_description
 ,ve.origin_indicator
 ,ve.quantity
 ,ve.weight
 ,ve.value
 ,ve.invoice_total
 ,ve.scheduler
 ,ve.transport_ref
 ,ve.description
 ,ve.container
 ,ve.in_bond
 ,ve.sold_en_route
 ,ve.export_adjustment_value
 ,ve.original_itn
 ,ve.routed_transaction
 ,ve.reference_number
 ,'' AS filing_number
 ,ve.created_date
 ,ISNULL(vefh.mapping_status, 0) AS mapping_status
 ,ISNULL(vefh.filing_status, 0) AS filing_status
 ,vefh.error_description
 ,ve.deleted
FROM dbo.vessel_exports ve
LEFT JOIN dbo.vessel_export_filing_details vefd
  ON vefd.vessel_export_id = ve.id
INNER JOIN dbo.vessel_export_filing_headers vefh
  ON vefh.id = vefd.filing_header_id
INNER JOIN Clients importer
  ON ve.importer_id = importer.id
INNER JOIN Clients usppi
  ON ve.usppi_id = usppi.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
GO

CREATE VIEW dbo.v_vessel_export_def_values_manual
AS
SELECT
  v.id
 ,v.filing_header_id
 ,v.parent_record_id
 ,v.section_name
 ,v.section_title
 ,v.table_name
 ,v.column_name
 ,v.record_id
 ,v.modification_date
 ,v.label
 ,v.description
 ,v.value
 ,v.editable
 ,v.has_default_value
 ,v.mandatory
 ,v.display_on_ui
 ,v.manual
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.vessel_export_def_values_manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATED_DATE', 'CREATED_USER', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_Vessel_Export_Def_Values AS
SELECT
  vdv.id
 ,vdv.column_name
 ,vdv.created_date
 ,vdv.created_user
 ,vdv.default_value
 ,vdv.display_on_ui
 ,vdv.editable
 ,vdv.has_default_value
 ,vdv.mandatory
 ,vdv.[manual]
 ,vdv.[description]
 ,vdv.label
 ,vdv.single_filing_order
 ,vdv.paired_field_table
 ,vdv.paired_field_column
 ,vdv.handbook_name
 ,vs.table_name
 ,vs.[name] AS section_name
 ,vs.title AS section_title
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.vessel_export_def_values vdv
INNER JOIN dbo.vessel_export_sections vs
  ON vdv.section_id = vs.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(vdv.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(vs.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO