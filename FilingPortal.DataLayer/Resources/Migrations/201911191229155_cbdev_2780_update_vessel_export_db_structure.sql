EXECUTE sp_rename @objname = N'dbo.Vessel_Export_Declarations'
                 ,@newname = N'exp_vessel_declaration'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK_vessel_export_declaration_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK_vessel_export_declaration_id'
                   ,@newname = N'PK__exp_vessel_declaration__id'
                   ,@objtype = N'OBJECT'
END
IF OBJECT_ID('FK__vessel_export_declarations__vessel_export_filing_headers__filing_header_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'FK__vessel_export_declarations__vessel_export_filing_headers__filing_header_id'
                   ,@newname = N'FK__exp_vessel_declaration__exp_vessel_filing_header__filing_header_id'
                   ,@objtype = N'OBJECT'
END
EXECUTE sp_rename @objname = N'dbo.exp_vessel_declaration.Idx_VesselExportDeclarations_filingHeadersId'
                 ,@newname = N'Idx__filing_header_id'
                 ,@objtype = N'INDEX'

ALTER TABLE dbo.exp_vessel_declaration
ADD parent_record_id INT NULL;
ALTER TABLE dbo.exp_vessel_declaration
ADD operation_id UNIQUEIDENTIFIER NULL;
ALTER TABLE dbo.exp_vessel_declaration
ADD created_date DATETIME NOT NULL DEFAULT (GETDATE());
ALTER TABLE dbo.exp_vessel_declaration
ADD created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME());

UPDATE dbo.exp_vessel_declaration
SET parent_record_id = filing_header_id;
ALTER TABLE dbo.exp_vessel_declaration
ALTER COLUMN parent_record_id INT NOT NULL;
GO

-- Invoice Header table
EXECUTE sp_rename @objname = N'dbo.Vessel_Export_Invoice_Headers'
                 ,@newname = N'exp_vessel_invoice_header'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK_vessel_export_invoice_headers_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK_vessel_export_invoice_headers_id'
                   ,@newname = N'PK__exp_vessel_invoice_header__id'
                   ,@objtype = N'OBJECT'
END
IF OBJECT_ID('FK__vessel_export_invoice_headers__vessel_export_filing_headers__filing_header_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'FK__vessel_export_invoice_headers__vessel_export_filing_headers__filing_header_id'
                   ,@newname = N'FK__exp_vessel_invoice_header__exp_vessel_filing_header__filing_header_id'
                   ,@objtype = N'OBJECT'
END
EXECUTE sp_rename @objname = N'dbo.exp_vessel_invoice_header.Idx_VesselExportInvoiceHeaders_filingHeadersId'
                 ,@newname = N'Idx__filing_header_id'
                 ,@objtype = N'INDEX'

ALTER TABLE dbo.exp_vessel_invoice_header
ADD parent_record_id INT NULL;
ALTER TABLE dbo.exp_vessel_invoice_header
ADD operation_id UNIQUEIDENTIFIER NULL;
ALTER TABLE dbo.exp_vessel_invoice_header
ADD created_date DATETIME NOT NULL DEFAULT (GETDATE());
ALTER TABLE dbo.exp_vessel_invoice_header
ADD created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME());

UPDATE dbo.exp_vessel_invoice_header
SET parent_record_id = filing_header_id;
ALTER TABLE dbo.exp_vessel_invoice_header
ALTER COLUMN parent_record_id INT NOT NULL;

ALTER TABLE dbo.exp_vessel_invoice_header
DROP COLUMN invoice_number;
DROP FUNCTION dbo.vessel_export_invoice_header_number;
GO
-- Vessel Export Invoice Headers table --
CREATE FUNCTION dbo.fn_exp_vessel_invoice_header_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      header.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY header.id)
    FROM dbo.exp_vessel_invoice_header header
    WHERE header.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO
ALTER TABLE dbo.exp_vessel_invoice_header
ADD invoice_number AS (dbo.fn_exp_vessel_invoice_header_number(filing_header_id, id));
GO

-- Invoice Line table
EXECUTE sp_rename @objname = N'dbo.Vessel_Export_Invoice_Lines'
                 ,@newname = N'exp_vessel_invoice_line'
                 ,@objtype = N'OBJECT'
IF OBJECT_ID('PK_vessel_export_invoice_lines_id') IS NOT NULL
BEGIN
  EXECUTE sp_rename @objname = N'PK_vessel_export_invoice_lines_id'
                   ,@newname = N'PK__exp_vessel_invoice_line__id'
                   ,@objtype = N'OBJECT'
END

ALTER TABLE dbo.exp_vessel_invoice_line
ADD filing_header_id INT NULL;
UPDATE line
SET filing_header_id = header.filing_header_id
FROM dbo.exp_vessel_invoice_line AS line
JOIN dbo.exp_vessel_invoice_header header
  ON line.invoice_header_id = header.id;
ALTER TABLE dbo.exp_vessel_invoice_line
ALTER COLUMN filing_header_id INT NOT NULL;
ALTER TABLE dbo.exp_vessel_invoice_line
ADD CONSTRAINT FK__exp_vessel_invoice_line__exp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_vessel_filing_header (id);
CREATE INDEX Idx__filing_header_id
ON dbo.exp_vessel_invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY];

ALTER TABLE dbo.exp_vessel_invoice_line
DROP COLUMN line_number;
DROP FUNCTION dbo.vessel_export_invoice_line_number;
GO

DROP INDEX Idx_VesselExportInvoiceLines_invoiceHeadersId ON dbo.exp_vessel_invoice_line;
GO
ALTER TABLE dbo.exp_vessel_invoice_line
DROP CONSTRAINT FK__vessel_export_invoice_lines__vessel_export_invoice_headers__invoice_header_id;
GO
ALTER TABLE dbo.exp_vessel_invoice_line
ADD parent_record_id INT NULL
GO
UPDATE dbo.exp_vessel_invoice_line
SET parent_record_id = invoice_header_id;
ALTER TABLE dbo.exp_vessel_invoice_line
ALTER COLUMN parent_record_id INT NOT NULL;
ALTER TABLE dbo.exp_vessel_invoice_line
DROP COLUMN invoice_header_id;
ALTER TABLE dbo.exp_vessel_invoice_line
ADD CONSTRAINT FK__exp_vessel_invoice_line__exp_vessel_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.exp_vessel_invoice_header (id) ON DELETE CASCADE;
GO
CREATE INDEX Idx__parent_record_id
ON dbo.exp_vessel_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY];
GO

ALTER TABLE dbo.exp_vessel_invoice_line
ADD operation_id UNIQUEIDENTIFIER NULL;
ALTER TABLE dbo.exp_vessel_invoice_line
ADD created_date DATETIME NOT NULL DEFAULT (GETDATE());
ALTER TABLE dbo.exp_vessel_invoice_line
ADD created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME());
GO

-- get export vessel invoice line number --
CREATE FUNCTION dbo.fn_exp_vessel_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY line.id)
    FROM dbo.exp_vessel_invoice_line line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId;
  RETURN @result;
END
GO
ALTER TABLE dbo.exp_vessel_invoice_line
ADD line_number AS (dbo.fn_exp_vessel_invoice_line_number(parent_record_id, id));
GO

-- drop triggers
DROP TRIGGER dbo.vessel_epport_declarations_befor_delete;
DROP TRIGGER dbo.vessel_export_invoice_headers_befor_delete;
DROP TRIGGER dbo.vessel_export_invoice_lines_befor_delete;
GO

-- update configuration
UPDATE dbo.exp_vessel_form_section_configuration
SET table_name = 'exp_vessel_declaration'
   ,procedure_name = 'sp_exp_vessel_add_declaration'
WHERE table_name = 'Vessel_Export_Declarations';
UPDATE dbo.exp_vessel_form_section_configuration
SET table_name = 'exp_vessel_invoice_header'
   ,procedure_name = 'sp_exp_vessel_add_invoice_header'
WHERE table_name = 'Vessel_Export_Invoice_Headers';
UPDATE dbo.exp_vessel_form_section_configuration
SET table_name = 'exp_vessel_invoice_line'
   ,procedure_name = 'sp_exp_vessel_add_invoice_line'
WHERE table_name = 'Vessel_Export_Invoice_Lines';
GO

-- update vessel link
ALTER TABLE dbo.exp_vessel_inbound
DROP CONSTRAINT [FK_dbo.Vessel_Exports_dbo.Vessels_vessel_id]
GO
ALTER TABLE dbo.exp_vessel_inbound
ADD CONSTRAINT [FK__exp_vessel_inbound__handbook_vessel__vessel_id] FOREIGN KEY (vessel_id) REFERENCES dbo.handbook_vessel (id)
GO
DROP TABLE dbo.Vessels
GO

-- drop unnecessary functions
DROP FUNCTION dbo.fn_getUnitByTariff;

-- drop old views
DROP VIEW dbo.v_Vessel_Export_Def_Values;
DROP VIEW dbo.v_vessel_export_def_values_manual;
DROP VIEW dbo.v_vessel_export_tables;
DROP VIEW dbo.vessel_export_grid;
DROP VIEW dbo.Vessel_Export_Report;
GO

-- create views 
CREATE VIEW dbo.v_exp_vessel_inbound_grid
AS
SELECT DISTINCT
  inbnd.id
 ,usppi.ClientCode AS usppi
 ,usppi_address.code AS [address]
 ,inbnd.contact
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
OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
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
WHERE inbnd.deleted = 0
GO

CREATE VIEW dbo.v_exp_vessel_form_configuration
AS
SELECT
  form.id
 ,form.label
 ,form.[value] AS default_value
 ,form.description
 ,sections.table_name
 ,form.column_name AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,form.handbook_name
 ,form.paired_field_table
 ,form.paired_field_column
 ,form.display_on_ui
 ,form.manual
 ,form.single_filing_order
 ,form.has_default_value
 ,form.editable
 ,form.mandatory
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.exp_vessel_form_configuration form
INNER JOIN dbo.exp_vessel_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL
GO

CREATE VIEW dbo.v_exp_vessel_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN dbo.exp_vessel_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

CREATE VIEW dbo.v_exp_vessel_report
AS
SELECT
  header.id AS Filing_Header_Id
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

 ,invoice.usppi AS Invoice_Headers_usppi
 ,invoice.usppi_address AS Invoice_Headers_usppi_address
 ,invoice.usppi_contact AS Invoice_Headers_usppi_contact
 ,invoice.usppi_phone AS Invoice_Headers_usppi_phone
 ,invoice.ultimate_consignee AS Invoice_Headers_ultimate_consignee
 ,invoice.ultimate_consignee_address AS Invoice_Headers_ultimate_consignee_address
 ,invoice.invoice_number AS Invoice_Headers_invoice_number
 ,invoice.origin_indicator AS Invoice_Headers_origin_indicator
 ,invoice.export_date AS Invoice_Headers_export_date
 ,invoice.ultimate_consignee_type AS Invoice_Headers_ultimate_consignee_type
 ,invoice.invoice_inco_term AS Invoice_Headers_invoice_inco_term
 ,invoice.invoice_total_amount AS Invoice_Headers_invoice_total_amount
 ,invoice.invoice_total_amount_currency AS Invoice_Headers_invoice_total_amount_currency
 ,invoice.exchange_rate AS Invoice_Headers_exchange_rate

 ,line.line_number AS Invoice_Lines_line_number
 ,line.export_code AS Invoice_Lines_export_code
 ,line.tariff_type AS Invoice_Lines_tariff_type
 ,line.tariff AS Invoice_Lines_tariff
 ,line.customs_qty AS Invoice_Lines_customs_qty
 ,line.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,line.second_qty AS Invoice_Lines_second_qty
 ,line.price AS Invoice_Lines_price
 ,line.price_currency AS Invoice_Lines_price_currency
 ,line.gross_weight AS Invoice_Lines_gross_weight
 ,line.gross_weight_unit AS Invoice_Lines_gross_weight_unit
 ,line.goods_description AS Invoice_Lines_goods_description
 ,line.license_type AS Invoice_Lines_license_type
 ,line.license_number AS Invoice_Lines_license_number
 ,line.license_value AS Invoice_Lines_license_value
 ,line.unit_price AS Invoice_Lines_unit_price
 ,line.goods_origin AS Invoice_Lines_goods_origin
 ,line.invoice_qty_unit AS Invoice_Lines_invoice_qty_unit

FROM dbo.exp_vessel_filing_header AS header
INNER JOIN dbo.exp_vessel_filing_detail AS detaile
  ON header.id = detaile.filing_header_id
LEFT JOIN dbo.exp_vessel_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.exp_vessel_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.exp_vessel_invoice_line line
  ON line.parent_record_id = invoice.id

WHERE header.mapping_status = 2
GO

-- prcedoures
DROP PROCEDURE dbo.vessel_export_filing_post_save;
DROP PROCEDURE dbo.vessel_export_filing_param;
DROP PROCEDURE dbo.vessel_export_filing_del;
DROP PROCEDURE dbo.vessel_export_filing;
DROP PROCEDURE dbo.vessel_export_add_declaration_record;
DROP PROCEDURE dbo.vessel_export_add_invoice_header_record;
DROP PROCEDURE dbo.vessel_export_add_invoice_line_record;
DROP PROCEDURE dbo.vessel_export_add_def_values_manual;
DROP PROCEDURE dbo.vessel_export_apply_def_values;
DROP PROCEDURE dbo.vessel_export_del;
DROP PROCEDURE dbo.vessel_export_delete_record;
DROP PROCEDURE dbo.update_def_values_manual;
GO

-- add declaration record --
CREATE PROCEDURE dbo.sp_exp_vessel_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_vessel_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.exp_vessel_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
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
       ,original_itn
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,usppi.ClientCode
       ,importer.ClientCode
       ,inbnd.container
       ,rule_usppi_consignee.transaction_related
       ,inbnd.routed_transaction
       ,inbnd.tariff_type
       ,inbnd.sold_en_route
       ,vessel.name
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.discharge_port
       ,inbnd.load_port
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.export_date
       ,inbnd.description
       ,inbnd.reference_number
       ,inbnd.transport_ref
       ,country.code
       ,foreign_port.unloco
       ,domestic_port.unloco
       ,domestic_port.state
       ,inbnd.in_bond
       ,inbnd.export_adjustment_value
       ,inbnd.original_itn
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail AS detail
      INNER JOIN dbo.exp_vessel_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS usppi
        ON inbnd.usppi_id = usppi.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.handbook_vessel AS vessel
        ON vessel.id = inbnd.vessel_id
      LEFT JOIN dbo.Countries AS country
        ON inbnd.country_of_destination_id = country.id
      LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS rule_usppi_consignee
        ON rule_usppi_consignee.usppi_id = inbnd.usppi_id
          AND rule_usppi_consignee.consignee_id = inbnd.importer_id
      LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
        ON inbnd.load_port = domestic_port.port_code
      LEFT JOIN dbo.CW_Foreign_Ports AS foreign_port
        ON inbnd.discharge_port = foreign_port.port_code
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add invoice line record --
CREATE PROCEDURE dbo.sp_exp_vessel_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_invoice_line'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_vessel_invoice_line AS line
      WHERE line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO dbo.exp_vessel_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,tariff_type
       ,tariff
       ,customs_qty
       ,customs_qty_unit
       ,price
       ,gross_weight
       ,goods_description
       ,invoice_qty_unit
       ,goods_origin
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.tariff_type
       ,inbnd.tariff
       ,inbnd.quantity
       ,dbo.fn_app_unit_by_tariff(inbnd.tariff, inbnd.tariff_type)
       ,inbnd.[value]
       ,COALESCE(inbnd.weight, dbo.fn_app_weight_to_ton(inbnd.quantity, dbo.fn_app_unit_by_tariff(inbnd.tariff, inbnd.tariff_type)))
       ,inbnd.goods_description
       ,dbo.fn_app_unit_by_tariff(inbnd.tariff, inbnd.tariff_type)
       ,IIF(inbnd.origin_indicator = 'D', 'US', NULL)
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail detail
      JOIN dbo.exp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id

      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add invoice header record --
CREATE PROCEDURE dbo.sp_exp_vessel_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_vessel_invoice_header'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_vessel_invoice_header AS header
      WHERE header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.exp_vessel_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,usppi
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee
       ,ultimate_consignee_address
       ,ultimate_consignee_type
       ,origin_indicator
       ,export_date
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,usppi.ClientCode
       ,usppi_address.code
       ,inbnd.contact
       ,inbnd.phone
       ,consignee.ClientCode
       ,consignee_address.code
       ,usppi_consignee_rule.ultimate_consignee_type
       ,inbnd.origin_indicator
       ,inbnd.export_date
       ,GETDATE()
       ,@filingUser
      FROM dbo.exp_vessel_filing_detail AS detail
      INNER JOIN dbo.exp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS usppi
        ON inbnd.usppi_id = usppi.id
      LEFT JOIN dbo.Client_Addresses AS usppi_address
        ON inbnd.address_id = usppi_address.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.importer_id = consignee.id
      LEFT JOIN dbo.exp_vessel_rule_usppi_consignee AS usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = inbnd.usppi_id
          AND usppi_consignee_rule.consignee_id = inbnd.importer_id
      LEFT JOIN dbo.Client_Addresses AS consignee_address
        ON consignee_address.id = usppi_consignee_rule.consignee_address_id
      WHERE detail.filing_header_id = @filingHeaderId;

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_exp_vessel_add_invoice_line @filingHeaderId
                                           ,@recordId
                                           ,@filingUser
                                           ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO
-- add filing records --
CREATE PROCEDURE dbo.sp_exp_vessel_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_exp_vessel_add_declaration @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add invoice header
  EXEC dbo.sp_exp_vessel_add_invoice_header @filingHeaderId
                                           ,@filingHeaderId
                                           ,@filingUser
                                           ,@operationId
END;
GO
-- review mapped data
CREATE PROCEDURE dbo.sp_exp_vessel_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.exp_vessel_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.exp_vessel_form_configuration defValue
    INNER JOIN dbo.exp_vessel_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO
-- delete filing entry
CREATE PROCEDURE dbo.sp_exp_vessel_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.exp_vessel_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.exp_vessel_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.exp_vessel_form_section_configuration ps
        WHERE ps.table_name = @tableName)
    BEGIN
      DECLARE @str VARCHAR(MAX)
      SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
      EXEC (@str)
    END
    ELSE
      THROW 51000, 'Invalid table name', 1
  END
END
GO
-- update filing entry
CREATE PROCEDURE dbo.sp_exp_vessel_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,[value] VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
   ,row_num INT NOT NULL
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (
      id
     ,record_id
     ,[value]
     ,table_name
     ,column_name
     ,row_num)
    SELECT
      field.id
     ,field.record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name, field.record_id ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN dbo.exp_vessel_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.exp_vessel_form_section_configuration section
      ON config.section_id = section.id
    WHERE config.editable = 1;

  DECLARE @recordId INT;
  DECLARE @tableName VARCHAR(128);
  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    table_name
   ,record_id
  FROM @result
  GROUP BY table_name
          ,record_id;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  SET @columns = NULL;

  SELECT
    @columns = COALESCE(@columns + ', ', '') + field.column_name + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
  FROM @result AS field
  WHERE field.record_id = @recordId
  AND field.table_name = @tableName
  AND field.row_num = 1;

  SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@recordId AS VARCHAR(10)) + ';' + CHAR(10);

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  END;

  CLOSE cur;
  DEALLOCATE cur

  EXEC (@command);
END
GO
-- soft delete inbound record
CREATE PROCEDURE dbo.sp_exp_vessel_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_exp_vessel_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE exp_vessel_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE exp_vessel_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM exp_vessel_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- recalculate rail fileds
CREATE PROCEDURE dbo.sp_exp_vessel_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN dbo.exp_vessel_form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN dbo.exp_vessel_form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values

  -- price
  DECLARE @tbl AS TABLE (
    parent_record_id INT NOT NULL
   ,record_id INT NOT NULL
   ,price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value)
    FROM @config a
    WHERE a.table_name = 'exp_vessel_invoice_line'
    AND a.column_name = 'price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice header invoice total
  DECLARE @total DECIMAL(18, 6);
  SELECT @total = SUM(price) FROM @tbl;
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(@total, '0.##############')
    FROM @config AS fields
    WHERE table_name = 'exp_vessel_invoice_header'
    AND column_name = 'invoice_total_amount';

  -- declaration destination
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,foreign_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'discharge'
    LEFT JOIN dbo.CW_Foreign_Ports AS foreign_port
      ON field2.value = foreign_port.port_code
    WHERE field.table_name = 'exp_vessel_declaration'
    AND field.column_name = 'destination';

  -- declaration origin
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,domestic_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
      ON field2.value = domestic_port.port_code
    WHERE field.table_name = 'exp_vessel_declaration'
    AND field.column_name = 'origin';

  -- declaration state_of_origin
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      field.id
     ,field.record_id
     ,field.parent_record_id
     ,domestic_port.state
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Domestic_Ports AS domestic_port
      ON field2.value = domestic_port.port_code
    WHERE field.table_name = 'exp_vessel_declaration'
    AND field.column_name = 'state_of_origin';

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO