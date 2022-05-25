EXECUTE sp_rename @objname = N'dbo.exp_vessel_declaration'
                 ,@newname = N'Vessel_Export_Declarations'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK__exp_vessel_declaration__id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK__exp_vessel_declaration__id'
                   ,@newname = N'PK_vessel_export_declaration_id'
                   ,@objtype = N'OBJECT'
END
IF OBJECT_ID('FK__exp_vessel_declaration__exp_vessel_filing_header__filing_header_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'FK__exp_vessel_declaration__exp_vessel_filing_header__filing_header_id'
                   ,@newname = N'FK__vessel_export_declarations__vessel_export_filing_headers__filing_header_id'
                   ,@objtype = N'OBJECT'
END
EXECUTE sp_rename @objname = N'dbo.Vessel_Export_Declarations.Idx__filing_header_id'
                 ,@newname = N'Idx_VesselExportDeclarations_filingHeadersId'
                 ,@objtype = N'INDEX'

ALTER TABLE dbo.Vessel_Export_Declarations
DROP COLUMN parent_record_id;
ALTER TABLE dbo.Vessel_Export_Declarations
DROP COLUMN operation_id;

DECLARE @cnstrnt NVARCHAR(128);
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Declarations')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_date';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Declarations DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Declarations
DROP COLUMN created_date;
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Declarations')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_user';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Declarations DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Declarations
DROP COLUMN created_user;
GO

-- Invoice Header table
EXECUTE sp_rename @objname = N'dbo.exp_vessel_invoice_header'
                 ,@newname = N'Vessel_Export_Invoice_Headers'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK__exp_vessel_invoice_header__id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK__exp_vessel_invoice_header__id'
                   ,@newname = N'PK_vessel_export_invoice_headers_id'
                   ,@objtype = N'OBJECT'
END
IF OBJECT_ID('FK__exp_vessel_invoice_header__exp_vessel_filing_header__filing_header_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'FK__exp_vessel_invoice_header__exp_vessel_filing_header__filing_header_id'
                   ,@newname = N'FK__vessel_export_invoice_headers__vessel_export_filing_headers__filing_header_id'
                   ,@objtype = N'OBJECT'
END
EXECUTE sp_rename @objname = N'dbo.Vessel_Export_Invoice_Headers.Idx__filing_header_id'
                 ,@newname = N'Idx_VesselExportInvoiceHeaders_filingHeadersId'
                 ,@objtype = N'INDEX'

ALTER TABLE dbo.Vessel_Export_Invoice_Headers
DROP COLUMN parent_record_id;
ALTER TABLE dbo.Vessel_Export_Invoice_Headers
DROP COLUMN operation_id;
DECLARE @cnstrnt NVARCHAR(128);
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Invoice_Headers')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_date';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Invoice_Headers DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Invoice_Headers
DROP COLUMN created_date;
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Invoice_Headers')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_user';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Invoice_Headers DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Invoice_Headers
DROP COLUMN created_user;

ALTER TABLE dbo.Vessel_Export_Invoice_Headers
DROP COLUMN invoice_number;
DROP FUNCTION dbo.fn_exp_vessel_invoice_header_number;
GO
-- Vessel Export Invoice Headers table --
CREATE FUNCTION dbo.vessel_export_invoice_header_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      invoice_header.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY invoice_header.id)
    FROM dbo.Vessel_Export_Invoice_Headers invoice_header
    WHERE invoice_header.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO
ALTER TABLE dbo.Vessel_Export_Invoice_Headers
ADD invoice_number AS ([dbo].[vessel_export_invoice_header_number]([filing_header_id], [id]));
GO

-- Invoice Line table
EXECUTE sp_rename @objname = N'dbo.exp_vessel_invoice_line'
                 ,@newname = N'Vessel_Export_Invoice_Lines'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK__exp_vessel_invoice_line__id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK__exp_vessel_invoice_line__id'
                   ,@newname = N'PK_vessel_export_invoice_lines_id'
                   ,@objtype = N'OBJECT'
END

ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP CONSTRAINT FK__exp_vessel_invoice_line__exp_vessel_filing_header__filing_header_id;
GO
DROP INDEX Idx__filing_header_id ON dbo.Vessel_Export_Invoice_Lines;
GO
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN filing_header_id;

ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN line_number;
DROP FUNCTION dbo.fn_exp_vessel_invoice_line_number
GO

ALTER TABLE dbo.Vessel_Export_Invoice_Lines
ADD invoice_header_id INT NULL
GO
UPDATE dbo.Vessel_Export_Invoice_Lines
SET invoice_header_id = parent_record_id;
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
ALTER COLUMN invoice_header_id INT NOT NULL;
DROP INDEX Idx__parent_record_id ON dbo.Vessel_Export_Invoice_Lines;
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP CONSTRAINT FK__exp_vessel_invoice_line__exp_vessel_invoice_header__parent_record_id;
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN parent_record_id;
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
ADD CONSTRAINT FK__vessel_export_invoice_lines__vessel_export_invoice_headers__invoice_header_id FOREIGN KEY (invoice_header_id) REFERENCES dbo.Vessel_Export_Invoice_Headers (id) ON DELETE CASCADE;
CREATE INDEX Idx_VesselExportInvoiceLines_invoiceHeadersId
ON dbo.Vessel_Export_Invoice_Lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY];
GO

ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN operation_id;
DECLARE @cnstrnt NVARCHAR(128);
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Invoice_Lines')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_date';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Invoice_Lines DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN created_date;
SELECT
  @cnstrnt = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.Vessel_Export_Invoice_Lines')
AND COL_NAME(parent_object_id, parent_column_id) = 'created_user';
IF @cnstrnt IS NOT NULL
  EXECUTE ('ALTER TABLE dbo.Vessel_Export_Invoice_Lines DROP CONSTRAINT [' + @cnstrnt + ']')
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
DROP COLUMN created_user;
GO

-- Vessel Export Invoice Lines table --
CREATE FUNCTION dbo.vessel_export_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      teil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY teil.id)
    FROM dbo.Vessel_Export_Invoice_Lines teil
    WHERE teil.invoice_header_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
ALTER TABLE dbo.Vessel_Export_Invoice_Lines
ADD line_number AS ([dbo].[vessel_export_invoice_line_number]([invoice_header_id], [id]));
GO

-- restore triggers
CREATE TRIGGER dbo.vessel_epport_declarations_befor_delete
ON dbo.Vessel_Export_Declarations
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Export_Def_Values_Manual
  WHERE table_name = 'Vessel_Export_Declarations'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_export_invoice_headers_befor_delete
ON dbo.Vessel_Export_Invoice_Headers
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Export_Def_Values_Manual
  WHERE table_name = 'Vessel_Export_Invoice_Headers'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_export_invoice_lines_befor_delete
ON dbo.Vessel_Export_Invoice_Lines
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Export_Def_Values_Manual
  WHERE table_name = 'Vessel_Export_Invoice_Lines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

-- update configuration
UPDATE dbo.Vessel_Export_Sections
SET table_name = 'Vessel_Export_Declarations'
   ,procedure_name = 'vessel_export_add_declaration_record'
WHERE table_name = 'exp_vessel_declaration';
UPDATE dbo.Vessel_Export_Sections
SET table_name = 'Vessel_Export_Invoice_Headers'
   ,procedure_name = 'vessel_export_add_invoice_header_record'
WHERE table_name = 'exp_vessel_invoice_header';
UPDATE dbo.Vessel_Export_Sections
SET table_name = 'Vessel_Export_Invoice_Lines'
   ,procedure_name = 'vessel_export_add_invoice_line_record'
WHERE table_name = 'exp_vessel_invoice_line';
GO

-- update vessel link
CREATE TABLE dbo.Vessels (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,CONSTRAINT [PK_dbo.Vessels] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Vessels ON;
INSERT INTO dbo.Vessels (
    id
   ,name)
  SELECT id, name FROM dbo.handbook_vessel;
SET IDENTITY_INSERT dbo.Vessels OFF;

ALTER TABLE dbo.Vessel_Exports
DROP CONSTRAINT FK__exp_vessel_inbound__handbook_vessel__vessel_id
GO
ALTER TABLE dbo.Vessel_Exports
ADD CONSTRAINT [FK_dbo.Vessel_Exports_dbo.Vessels_vessel_id] FOREIGN KEY (vessel_id) REFERENCES dbo.Vessels (id)
GO

-- restore functions
CREATE FUNCTION dbo.fn_getUnitByTariff (@tariff VARCHAR(35), @tarifftype VARCHAR(128))
RETURNS VARCHAR(128)
AS
BEGIN
  DECLARE @result VARCHAR(128) = NULL;

  IF UPPER(@tarifftype) = 'SCHB'
    OR UPPER(@tarifftype) = 'SHB'
  BEGIN
    SET @result = (SELECT
        Unit
      FROM SchB_Tariff
      WHERE (UB_Tariff = REPLACE(@tariff, '.', '')))
  END
  ELSE
  BEGIN
    SET @result = (SELECT TOP (1)
        Unit
      FROM Tariff
      WHERE (USC_Tariff = REPLACE(@tariff, '.', '')
      AND [ToDateTime] >= GETDATE())
      ORDER BY FromDateTime DESC)
  END

  RETURN @result
END
GO

-- drop new views 
DROP VIEW dbo.v_exp_vessel_inbound_grid;
DROP VIEW dbo.v_exp_vessel_form_configuration;
DROP VIEW dbo.v_exp_vessel_field_configuration;
DROP VIEW dbo.v_exp_vessel_report;
GO

-- restore views
CREATE VIEW dbo.v_Vessel_Export_Def_Values
AS
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

CREATE VIEW dbo.v_vessel_export_tables
AS
SELECT
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
 ,usppi_address.code AS [address]
 ,ve.contact
 ,ve.phone
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
LEFT JOIN dbo.Client_Addresses usppi_address
  ON ve.address_id = usppi_address.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
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
 ,usppi_address.code AS [address]
 ,ve.contact
 ,ve.phone
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
LEFT JOIN dbo.Client_Addresses usppi_address
  ON ve.address_id = usppi_address.id
LEFT JOIN dbo.Countries country
  ON ve.country_of_destination_id = country.id
LEFT JOIN Vessels vessel
  ON ve.vessel_id = vessel.id
WHERE vefh.mapping_status <> 0
AND ve.deleted = 0
GO

CREATE VIEW dbo.Vessel_Export_Report
AS
SELECT
  headers.id AS Filing_Header_Id
 ,declaration.main_supplier AS Declarations_main_supplier
 ,declaration.importer AS Declarations_importer
 ,declaration.shpt_type AS Declarations_shpt_type
 ,declaration.transport AS Declarations_transport
 ,declaration.container AS Declarations_container
 ,declaration.transaction_related AS Declarations_transaction_related
 ,declaration.hazardous AS Declarations_hazardous
 ,declaration.routed_tran AS Declarations_routed_tran
 ,declaration.filing_option AS Declarations_filing_option
 ,declaration.tariff_type AS Declarations_tariff_type
 ,declaration.sold_en_route AS Declarations_sold_en_route
 ,declaration.service AS Declarations_service
 ,declaration.vessel AS Declarations_vessel
 ,declaration.voyage AS Declarations_voyage
 ,declaration.carrier_scac AS Declarations_carrier_scac
 ,declaration.port_of_loading AS Declarations_port_of_loading
 ,declaration.dep AS Declarations_dep
 ,declaration.discharge AS Declarations_discharge
 ,declaration.arr AS Declarations_arr
 ,declaration.export AS Declarations_export
 ,declaration.exp AS Declarations_exp
 ,declaration.house_bill AS Declarations_house_bill
 ,declaration.origin AS Declarations_origin
 ,declaration.etd AS Declarations_etd
 ,declaration.destination AS Declarations_destination
 ,declaration.eta AS Declarations_eta
 ,declaration.country_of_export AS Declarations_country_of_export
 ,declaration.export_date AS Declarations_export_date
 ,declaration.description AS Declarations_description
 ,declaration.owner_ref AS Declarations_owner_ref
 ,declaration.inco AS Declarations_inco
 ,declaration.transport_ref AS Declarations_transport_ref
 ,declaration.country_of_dest AS Declarations_country_of_dest
 ,declaration.state_of_origin AS Declarations_state_of_origin
 ,declaration.inbond_type AS Declarations_inbond_type
 ,declaration.license_type AS Declarations_license_type
 ,declaration.license_number AS Declarations_license_number
 ,declaration.export_code AS Declarations_export_code
 ,declaration.eccn AS Declarations_eccn
 ,declaration.intermediate_consignee AS Declarations_intermediate_consignee
 ,declaration.carrier AS Declarations_carrier
 ,declaration.forwader AS Declarations_forwader
 ,declaration.check_local_client AS Declarations_check_local_client
 ,declaration.export_adjustment_value AS Declarations_export_adjustment_value
 ,declaration.original_itn AS Declarations_original_itn

 ,invheaders.usppi AS Invoice_Headers_usppi
 ,invheaders.usppi_address AS Invoice_Headers_usppi_address
 ,invheaders.usppi_contact AS Invoice_Headers_usppi_contact
 ,invheaders.usppi_phone AS Invoice_Headers_usppi_phone
 ,invheaders.ultimate_consignee AS Invoice_Headers_ultimate_consignee
 ,invheaders.ultimate_consignee_address AS Invoice_Headers_ultimate_consignee_address
 ,invheaders.invoice_number AS Invoice_Headers_invoice_number
 ,invheaders.origin_indicator AS Invoice_Headers_origin_indicator
 ,invheaders.export_date AS Invoice_Headers_export_date
 ,invheaders.ultimate_consignee_type AS Invoice_Headers_ultimate_consignee_type
 ,invheaders.invoice_inco_term AS Invoice_Headers_invoice_inco_term
 ,invheaders.invoice_total_amount AS Invoice_Headers_invoice_total_amount
 ,invheaders.invoice_total_amount_currency AS Invoice_Headers_invoice_total_amount_currency
 ,invheaders.exchange_rate AS Invoice_Headers_exchange_rate

 ,invlines.line_number AS Invoice_Lines_line_number
 ,invlines.export_code AS Invoice_Lines_export_code
 ,invlines.tariff_type AS Invoice_Lines_tariff_type
 ,invlines.tariff AS Invoice_Lines_tariff
 ,invlines.customs_qty AS Invoice_Lines_customs_qty
 ,invlines.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,invlines.second_qty AS Invoice_Lines_second_qty
 ,invlines.price AS Invoice_Lines_price
 ,invlines.price_currency AS Invoice_Lines_price_currency
 ,invlines.gross_weight AS Invoice_Lines_gross_weight
 ,invlines.gross_weight_unit AS Invoice_Lines_gross_weight_unit
 ,invlines.goods_description AS Invoice_Lines_goods_description
 ,invlines.license_type AS Invoice_Lines_license_type
 ,invlines.license_number AS Invoice_Lines_license_number
 ,invlines.license_value AS Invoice_Lines_license_value
 ,invlines.unit_price AS Invoice_Lines_unit_price
 ,invlines.goods_origin AS Invoice_Lines_goods_origin
 ,invlines.invoice_qty_unit AS Invoice_Lines_invoice_qty_unit

FROM dbo.Vessel_Export_Filing_Headers headers
INNER JOIN dbo.Vessel_Export_Filing_Details detailes
  ON headers.id = detailes.filing_header_id
LEFT OUTER JOIN dbo.Vessel_Export_Declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Export_Invoice_Headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Export_Invoice_Lines invlines
  ON invlines.invoice_header_id = invheaders.id

WHERE headers.mapping_status = 2
GO

ALTER VIEW dbo.v_Documents 
AS SELECT
  rail_header.id AS filing_header_id
 ,rail_doc.id AS doc_id
 ,rail_doc.filename AS filename
 ,rail_doc.file_extension AS file_extension
 ,rail_doc.file_content AS file_Content
 ,rail_doc.file_desc AS file_desc
 ,rail_doc.document_type AS document_type
 ,'Rail_Imp' AS transport_shipment_type
FROM dbo.imp_rail_document rail_doc
INNER JOIN dbo.imp_rail_filing_header rail_header
  ON rail_doc.filing_header_id = rail_header.id
UNION ALL
SELECT
  truck_header.id AS filing_header_id
 ,truck_doc.id AS doc_id
 ,truck_doc.[file_name] AS filename
 ,truck_doc.file_extension AS file_extension
 ,truck_doc.file_content AS file_Content
 ,truck_doc.file_description AS file_desc
 ,truck_doc.document_type AS document_type
 ,'Truck_Imp' AS transport_shipment_type
FROM dbo.imp_truck_document truck_doc
INNER JOIN dbo.imp_truck_filing_header truck_header
  ON truck_doc.filing_header_id = truck_header.id
UNION ALL
SELECT
  pipeline_header.id AS filing_header_id
 ,pipeline_doc.id AS doc_id
 ,pipeline_doc.[file_name] AS filename
 ,pipeline_doc.file_extension AS file_extension
 ,pipeline_doc.file_content AS file_Content
 ,pipeline_doc.file_description AS file_desc
 ,pipeline_doc.document_type AS document_type
 ,'Pipeline_Imp' AS transport_shipment_type
FROM dbo.imp_pipeline_document pipeline_doc
INNER JOIN dbo.imp_pipeline_filing_header pipeline_header
  ON pipeline_doc.filing_header_id = pipeline_header.id
UNION ALL
SELECT
  vessel_header.id AS filing_header_id
 ,vessel_doc.id AS doc_id
 ,vessel_doc.[file_name] AS filename
 ,vessel_doc.file_extension AS file_extension
 ,vessel_doc.file_content AS file_Content
 ,vessel_doc.file_description AS file_desc
 ,vessel_doc.document_type AS document_type
 ,'Vessel_Imp' AS transport_shipment_type
FROM dbo.imp_vessel_document vessel_doc
JOIN dbo.imp_vessel_filing_header AS vessel_header
  ON vessel_doc.filing_header_id = vessel_header.id
UNION ALL
SELECT
  truck_export_header.id AS filing_header_id
 ,truck_export_doc.id AS doc_id
 ,truck_export_doc.[file_name] AS [filename]
 ,truck_export_doc.file_extension file_extension
 ,truck_export_doc.file_content AS file_Content
 ,truck_export_doc.file_description AS file_desc
 ,truck_export_doc.document_type AS document_type
 ,'Truck_Export' AS transport_shipment_type
FROM dbo.exp_truck_document truck_export_doc
INNER JOIN dbo.exp_truck_filing_header truck_export_header
  ON truck_export_doc.filing_header_id = truck_export_header.id
UNION ALL
SELECT
  vessel_export_header.id AS filing_header_id
 ,vessel_export_doc.id AS doc_id
 ,vessel_export_doc.file_name AS filename
 ,vessel_export_doc.extension file_extension
 ,vessel_export_doc.content AS file_Content
 ,vessel_export_doc.description AS file_desc
 ,vessel_export_doc.document_type AS document_type
 ,'Vessel_Export' AS transport_shipment_type
FROM [dbo].[Vessel_Export_Documents] vessel_export_doc
INNER JOIN dbo.vessel_export_Filing_Headers vessel_export_header
  ON vessel_export_doc.filing_header_id = vessel_export_header.id
GO

-- procedures 
DROP PROCEDURE dbo.sp_exp_vessel_add_declaration;
DROP PROCEDURE dbo.sp_exp_vessel_add_invoice_line;
DROP PROCEDURE dbo.sp_exp_vessel_add_invoice_header;
DROP PROCEDURE dbo.sp_exp_vessel_create_entry_records;
DROP PROCEDURE dbo.sp_exp_vessel_review_entry;
DROP PROCEDURE dbo.sp_exp_vessel_delete_entry_records;
DROP PROCEDURE dbo.sp_exp_vessel_update_entry;
DROP PROCEDURE dbo.sp_exp_vessel_delete_inbound;
DROP PROCEDURE dbo.sp_exp_vessel_recalculate;
GO

-- update def values manual data --
CREATE PROCEDURE dbo.update_def_values_manual (@defValuesManualTableName VARCHAR(128) = 'truck_export_def_values_manual'
, @tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  /*
    Update values into def value manual table with data from result table
  */
  DECLARE @selectColumnsList VARCHAR(MAX);
  DECLARE @columnsList VARCHAR(MAX);
  DECLARE @selectStatment VARCHAR(MAX);
  DECLARE @mergeStatment VARCHAR(MAX);

  -- get table column names with type converion for select statment
  SET @selectColumnsList = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.name) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.###############'')'
        WHEN t.name LIKE 'decimal' THEN 'format(' + QUOTENAME(c.name) + ', ''0.###############'')'
        ELSE QUOTENAME(c.name)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(@tableName)
    AND UPPER(c.name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @selectColumnsList

  -- get table column names for UNPIVOT statment
  SET @columnsList = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE objecT_id = OBJECT_ID(@tableName)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @columnsList

  -- set sselect statment
  SET @selectStatment =
  ' SELECT column_name, value
  FROM (SELECT top 1 ' + @selectColumnsList + ' FROM  ' + @tableName + ' where id=' + CAST(@recordId AS VARCHAR(32)) + ') p
  UNPIVOT (value FOR column_name IN (' + @columnsList + ')) as unpvt'
  --PRINT @selectStatment

  --set merge statment
  SET @mergeStatment = '
MERGE ' + @defValuesManualTableName + ' AS t 
USING (' + @selectStatment + ') AS s 
ON (upper(t.column_name) = upper(s.column_name) and upper(t.table_name) = ''' + UPPER(@tableName) + ''' AND t.record_id = ' + CAST(@recordId AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
  UPDATE SET t.value = case when s.value='''' then null else s.value end  ;';
  --PRINT @mergeStatment

  EXEC (@mergeStatment)

END
GO
-- Add Vessel export def value manual values records --
CREATE PROCEDURE dbo.vessel_export_add_def_values_manual (@tableName VARCHAR(128)
, @filingHeaderId INT
, @parentId INT
, @recordId INT)
AS
BEGIN
  DECLARE @str_val NVARCHAR(500)
  DECLARE @parmDefinition NVARCHAR(500)
  DECLARE @defValueOut VARCHAR(128)
  DECLARE @dataType VARCHAR(128)
  DECLARE @defValue VARCHAR(500)
  DECLARE @id INT;
  DECLARE @columnName VARCHAR(128)

  DECLARE c CURSOR FOR SELECT DISTINCT
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,v.default_value
   ,v.id
   ,v.column_name
  FROM dbo.Vessel_Export_Def_Values v
  INNER JOIN dbo.Vessel_Export_Sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.manual > 0
  OR v.has_default_value > 0
  OR v.display_on_ui > 0
  OR v.single_filing_order > 0)
  ORDER BY id

  OPEN c
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  WHILE @@fetch_status = 0
  BEGIN
  SET @defValueOut = NULL;
  SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(''' + @defValue + ''' as ' + @dataType + ') as varchar(500))';
  SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@parmDefinition
                       ,@defValueOut = @defValueOut OUTPUT;
  IF @defValueOut IS NULL
  BEGIN
    SET @str_val = 'SELECT @defValueOut = try_cast(try_cast(' + @defValue + ' as ' + @dataType + ') as varchar(500))';
    SET @parmDefinition = N'@defValueOut varchar(500) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@parmDefinition
                         ,@defValueOut = @defValueOut OUTPUT;
  END TRY
  BEGIN CATCH
    SET @defValueOut = NULL;
  END CATCH;
  END;

  IF @dataType LIKE 'date%'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @dataType LIKE 'numeric'
  BEGIN
    SET @defValueOut = FORMAT(TRY_CAST(@defValueOut AS FLOAT), '0.######');
  END;

  INSERT INTO dbo.Vessel_Export_Def_Values_Manual (
      filing_header_id
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,modification_date
     ,value
     ,editable
     ,display_on_ui
     ,has_default_value
     ,mandatory
     ,manual
     ,description
     ,label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.column_name
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.editable
     ,dv.display_on_ui
     ,dv.has_default_value
     ,dv.mandatory
     ,dv.manual
     ,dv.description
     ,dv.label
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Vessel_Export_Def_Values dv
    INNER JOIN dbo.Vessel_Export_Sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO
-- Vessel export apply filing values for specified resulting table procedure --
CREATE PROCEDURE dbo.vessel_export_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.Vessel_Export_Def_Values_Manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.has_default_value = 1
  AND v.record_id = @recordId

  EXEC (@str);
END
GO
--- Vessel export add declarations record ---
CREATE PROCEDURE dbo.vessel_export_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM dbo.Vessel_Export_Sections section
  WHERE section.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Declarations ved
      WHERE ved.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Export_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,transaction_related
       ,routed_tran
       ,tariff_type
       ,sold_en_route
       ,vessel
       ,port_of_loading
       ,dep
       ,discharge
       ,export
       ,exp
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
       ,destination
       ,origin
       ,state_of_origin
       ,inbond_type
       ,export_adjustment_value
       ,original_itn)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,intake.container
       ,usppi_consignee.transaction_related
       ,intake.routed_transaction
       ,intake.tariff_type
       ,intake.sold_en_route
       ,vessel.name
       ,intake.load_port
       ,intake.export_date
       ,intake.discharge_port
       ,intake.load_port
       ,intake.export_date
       ,intake.export_date
       ,intake.export_date
       ,intake.description
       ,intake.reference_number
       ,intake.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
       ,intake.in_bond
       ,intake.export_adjustment_value
       ,intake.original_itn

      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients importer
        ON intake.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = intake.vessel_id
      LEFT JOIN dbo.Countries country
        ON intake.country_of_destination_id = country.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee
        ON usppi_consignee.usppi_id = intake.usppi_id
          AND usppi_consignee.consignee_id = intake.importer_id
      LEFT JOIN CW_Domestic_Ports domestic_port
        ON intake.load_port = domestic_port.port_code
      LEFT JOIN CW_Foreign_Ports foreign_port
        ON intake.discharge_port = foreign_port.port_code
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO
-- add vessel export invoice line record --
CREATE PROCEDURE dbo.vessel_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Export_Invoice_Lines veil
      WHERE veil.invoice_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Lines (
        invoice_header_id
       ,tariff_type
       ,tariff
       ,customs_qty
       ,customs_qty_unit
       ,price
       ,gross_weight
       ,goods_description
       ,invoice_qty_unit
       ,goods_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,intake.tariff_type
       ,intake.tariff
       ,intake.quantity
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,intake.[value]
       ,COALESCE(intake.weight, dbo.fn_app_weight_to_ton(intake.quantity, dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)))
       ,intake.goods_description
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,IIF(intake.origin_indicator = 'D', 'US', NULL)
      FROM dbo.Vessel_export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id

      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add vessel export invoice header record --
CREATE PROCEDURE dbo.vessel_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Export_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ves.is_array
  FROM dbo.Vessel_Export_Sections ves
  WHERE ves.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Export_Invoice_Headers veih
      WHERE veih.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Export_Invoice_Headers (
        filing_header_id
       ,usppi
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee
       ,ultimate_consignee_address
       ,ultimate_consignee_type
       ,origin_indicator
       ,export_date)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,usppi_address.code
       ,intake.contact
       ,intake.phone
       ,consignee.ClientCode
       ,consignee_address.code
       ,usppi_consignee_rule.ultimate_consignee_type
       ,intake.origin_indicator
       ,intake.export_date
      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Client_Addresses usppi_address
        ON intake.address_id = usppi_address.id
      LEFT JOIN dbo.Clients consignee
        ON intake.importer_id = consignee.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = intake.usppi_id
          AND usppi_consignee_rule.consignee_id = intake.importer_id
      LEFT JOIN dbo.Client_Addresses consignee_address
        ON consignee_address.id = usppi_consignee_rule.consignee_address_id
      WHERE detail.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_export_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Export_Invoice_Headers
    SET invoice_total_amount = line.total
    FROM Vessel_Export_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Export_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId

    -- fill the def value manual table
    EXEC dbo.vessel_export_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_export_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Export_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- Vessel Export filing procedure
CREATE PROCEDURE dbo.vessel_export_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.vessel_export_add_declaration_record @filingHeaderId
                                               ,@filingHeaderId

  -- add invoice header
  EXEC dbo.vessel_export_add_invoice_header_record @filingHeaderId
                                                  ,@filingHeaderId

END;
GO
-- Vessel Export filing records delete procedure ---
CREATE PROCEDURE dbo.vessel_export_filing_del (@filingHeaderId INT)
AS
BEGIN

  DELETE FROM dbo.Vessel_Export_Def_Values_Manual
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.Vessel_Export_Filing_Details
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.Vessel_Export_Filing_Headers
  WHERE id = @filingHeaderId

END
GO
-- Vessel export inbound record soft delete proceure --
CREATE PROCEDURE dbo.vessel_export_del (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mapping_status = grid.mapping_status
  FROM Vessel_Export_Grid grid
  WHERE grid.id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Vessel_Exports
    SET deleted = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Vessel_Exports
      SET deleted = @deleted
      WHERE id IN (SELECT
          details.vessel_export_id
        FROM Vessel_Export_Filing_Details details
        WHERE details.filing_header_id = @filingHeaderId)
    END
  END
END
GO
--- Vessel export delete record from specified resulting table procedure ---
CREATE PROCEDURE dbo.vessel_export_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.Vessel_Export_Sections ves
      WHERE ves.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE
    THROW 51000, 'Invalid table name', 1
END
GO
--- Vessel export apply filing params for all resulting tables procedure ---
CREATE PROCEDURE dbo.vessel_export_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
    i.DATA_TYPE +
    CASE
      WHEN i.DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN i.DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN i.DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Vessel_Export_Def_Values_Manual v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO
--- Vessel Export post save action ---
CREATE PROCEDURE dbo.vessel_export_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,Price DECIMAL(18, 6)
  )

  INSERT INTO @tbl (
      record_id
     ,filing_header_id
     ,Price)
    SELECT
      a.record_id
     ,a.filing_header_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS Price
    FROM Vessel_Export_Def_Values_Manual a
    WHERE a.filing_header_id = @filingHeaderId
    AND a.table_name = 'Vessel_Export_Invoice_Lines'
    AND a.column_name = 'price'

  DECLARE @total DECIMAL(18, 6)
  SELECT
    @total = SUM([@tbl].Price)
  FROM @tbl
  -- update invoice header total amount
  UPDATE Vessel_Export_Def_Values_Manual
  SET value = FORMAT(@total, '0.######')
  WHERE filing_header_id = @filingHeaderId
  AND table_name = 'Vessel_Export_Invoice_Headers'
  AND (column_name = 'invoice_total_amount')
  -- update declaration destination
  UPDATE dest
  SET value = port.unloco
  FROM dbo.Vessel_Export_Def_Values_Manual dest
  JOIN dbo.Vessel_Export_Def_Values_Manual source
    ON dest.record_id = source.record_id
    AND source.column_name = 'discharge'
  LEFT JOIN dbo.CW_Foreign_Ports port
    ON source.value = port.port_code
  WHERE dest.filing_header_id = @filingHeaderId
  AND dest.table_name = 'Vessel_Export_Declarations'
  AND dest.column_name = 'destination'
  -- update declaration origin
  UPDATE dest
  SET value = port.unloco
  FROM dbo.Vessel_Export_Def_Values_Manual dest
  JOIN dbo.Vessel_Export_Def_Values_Manual source
    ON dest.record_id = source.record_id
    AND source.column_name = 'port_of_loading'
  LEFT JOIN dbo.CW_Domestic_Ports port
    ON source.value = port.port_code
  WHERE dest.filing_header_id = 1
  AND dest.table_name = 'Vessel_Export_Declarations'
  AND dest.column_name = 'origin'
  -- update declaration state of origin
  UPDATE dest
  SET value = port.state
  FROM dbo.Vessel_Export_Def_Values_Manual dest
  JOIN dbo.Vessel_Export_Def_Values_Manual source
    ON dest.record_id = source.record_id
    AND source.column_name = 'port_of_loading'
  LEFT JOIN dbo.CW_Domestic_Ports port
    ON source.value = port.port_code
  WHERE dest.filing_header_id = 1
  AND dest.table_name = 'Vessel_Export_Declarations'
  AND dest.column_name = 'state_of_origin'
END
GO