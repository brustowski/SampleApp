--- rename view for Vessel Import read model ---
IF OBJECT_ID('dbo.Vessel_Import_Grid', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.Vessel_Import_Grid
END
GO

CREATE VIEW dbo.Vessel_Inbound_Grid
AS
SELECT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,imports.user_id AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.created_date AS created_date
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
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
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
GO

IF OBJECT_ID('dbo.vessel_import_add_declaration_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_declaration_record
END

IF OBJECT_ID('dbo.vessel_import_add_invoice_header_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_invoice_header_record
END

IF OBJECT_ID('dbo.vessel_import_add_invoice_line_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_invoice_line_record
END

IF OBJECT_ID('dbo.vessel_import_add_packing_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_packing_record
END

IF OBJECT_ID('dbo.vessel_import_add_misc_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_misc_record
END

IF OBJECT_ID('dbo.vessel_import_add_def_values_manual', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_add_def_values_manual
END

IF OBJECT_ID('dbo.vessel_import_apply_def_values', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_apply_def_values
END

IF OBJECT_ID('dbo.vessel_import_delete_record', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_delete_record
END

IF OBJECT_ID('dbo.vessel_import_filing', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_filing
END

IF OBJECT_ID('dbo.vessel_import_filing_del', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_filing_del
END

IF OBJECT_ID('dbo.vessel_import_filing_param', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_filing_param
END

IF OBJECT_ID('dbo.vessel_import_del', 'P') IS NOT NULL
BEGIN
  DROP PROCEDURE dbo.vessel_import_del
END