IF OBJECT_ID('dbo.truck_export_grid', 'V') IS NOT NULL
  DROP VIEW dbo.truck_export_grid
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

CREATE VIEW dbo.truck_export_grid
AS
SELECT
  te.id
 ,tefh.id AS filing_header_id
 ,te.exporter_code
 ,te.usppi_code
 ,te.consignee_code
 ,te.carrier
 ,te.scac
 ,te.license
 ,te.license_type
 ,te.tariff
 ,te.routed_tran
 ,te.container
 ,te.eccn
 ,te.goods_description
 ,'' AS filing_number
 ,te.created_date
 ,ISNULL(tefh.mapping_status, 0) AS mapping_status
 ,ISNULL(tefh.filing_status, 0) AS filing_status
 ,tefh.error_description
 ,te.deleted
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
LEFT JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
    AND tefh.mapping_status <> 0
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.truck_export_filing_headers fh
  INNER JOIN dbo.truck_export_filing_details fd
    ON fh.id = fd.filing_header_id
  WHERE fh.mapping_status > 0
  AND te.id = fd.truck_export_id)
AND te.deleted = 0
GO

CREATE PROCEDURE dbo.truck_export_del (@id INT,
@deleted BIT)
AS
BEGIN
  UPDATE dbo.truck_exports
  SET deleted = @deleted
  WHERE id = @id
  AND NOT EXISTS (SELECT
      *
    FROM dbo.truck_export_filing_details tefd
    INNER JOIN dbo.truck_export_filing_headers tefh
      ON tefd.filing_header_id = tefh.id
    WHERE tefd.truck_export_id = @id
    AND (ISNULL(mapping_status, 0) > 0
    OR ISNULL(filing_status, 0) > 0))
END
GO

INSERT dbo.App_Permissions(id, description, name) VALUES (34, 'Import Truck Export Records Permission', 'TruckImportExportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (35, 'View Truck Export Records Permission', 'TruckViewExportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (36, 'Delete Truck Export Records Permission', 'TruckDeleteExportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (37, 'File Truck Export Records Permission', 'TruckFileExportRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (38, 'View Truck Export Rule Records Permission', 'TruckViewExportRecordRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (39, 'Edit Truck Export Rule Records Permission', 'TruckEditExportRecordRules')
INSERT dbo.App_Permissions(id, description, name) VALUES (40, 'Delete Truck Export Rule Records Permission', 'TruckDeleteExportRecordRules')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (11, 'TruckExportUser', 'The role with following permissions: Import, View, Edit, and File Truck Export data; View Truck Export Rules data.')
INSERT dbo.App_Roles(id, title, description) VALUES (12, 'TruckExportPowerUser', 'The role with following permissions: Import, View, Edit, and File Truck Export and Rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 34)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 35)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 36)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 37)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 38)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 39)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (1, 40)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (11, 34)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (11, 35)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (11, 36)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (11, 37)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 34)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 35)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 36)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 37)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 38)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 39)
INSERT dbo.App_Permissions_Roles(App_Roles_FK, App_Permissions_FK) VALUES (12, 40)
GO
