INSERT dbo.App_Permissions(id, description, name) VALUES (20001, 'View Canada Truck Import Records Permission', 'CanadaTruckImportViewInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (20002, 'Delete Canada Truck Import Records Permission', 'CanadaTruckImportDeleteInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (20003, 'File Canada Truck Import Records Permission', 'CanadaTruckImportFileInboundRecord')
INSERT dbo.App_Permissions(id, description, name) VALUES (20004, 'Import Canada Truck Import Records Permission', 'CanadaTruckImportImportInboundRecord')

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (20000, 'CanadaTruckImportUser', 'The role with following permissions: View, Edit, and File Canada Truck Import inbound data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20001, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20002, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20003, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20004, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20001, 20000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20002, 20000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20003, 20000)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (20004, 20000)
GO


--
-- Create view canada_imp_truck.v_inbound_grid
--
GO
PRINT (N'Create view canada_imp_truck.v_inbound_grid')
GO
CREATE OR ALTER VIEW canada_imp_truck.v_inbound_grid
AS
SELECT DISTINCT
  inbound.id
 ,fh.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbound.carrier_at_import
 ,inbound.port
 ,inbound.pars_number
 ,inbound.eta
 ,inbound.owners_reference
 ,inbound.direct_ship_date
 ,inbound.created_user
 ,'' AS filing_number
 ,'' AS job_link
 ,inbound.created_date
 ,ISNULL(fh.mapping_status, 0) AS mapping_status
 ,ISNULL(fh.filing_status, 0) AS filing_status
 ,inbound.deleted AS is_deleted
FROM canada_imp_truck.inbound inbound
LEFT JOIN canada_imp_truck.filing_detail fd
  ON fd.inbound_id = inbound.id
LEFT JOIN canada_imp_truck.filing_header fh
  ON fh.id = fd.filing_header_id
LEFT JOIN Clients importer
  ON inbound.importer_id = importer.id
GO