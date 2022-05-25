--- Update functions ---
ALTER FUNCTION dbo.fn_getUnitByTariff (@tariff VARCHAR(35), @tarifftype VARCHAR(128))
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

--- Add Vessel export resulting tables ---
-- Vessel Export Declaration table --
CREATE TABLE dbo.Vessel_Export_Declarations (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL
 ,transport VARCHAR(128) NULL
 ,container VARCHAR(128) NULL
 ,transaction_related VARCHAR(10) NULL
 ,hazardous VARCHAR(10) NULL
 ,routed_tran VARCHAR(10) NULL
 ,filing_option VARCHAR(10) NULL
 ,tariff_type VARCHAR(4) NULL
 ,sold_en_route VARCHAR(10) NULL
 ,service VARCHAR(128) NULL
 ,vessel VARCHAR(128) NULL
 ,voyage VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,port_of_loading VARCHAR(128) NULL
 ,dep DATE NULL
 ,discharge VARCHAR(128) NULL
 ,arr DATE NULL
 ,export VARCHAR(128) NULL
 ,exp DATE NULL
 ,house_bill VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,etd DATE NULL
 ,destination VARCHAR(128) NULL
 ,eta DATE NULL
 ,country_of_export VARCHAR(10) NULL
 ,export_date DATE NULL
 ,description VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL
 ,transport_ref VARCHAR(128) NULL
 ,country_of_dest VARCHAR(128) NULL
 ,state_of_origin VARCHAR(2) NULL
 ,inbond_type VARCHAR(10) NULL
 ,license_type VARCHAR(10) NULL
 ,license_number VARCHAR(128) NULL
 ,export_code VARCHAR(10) NULL
 ,eccn VARCHAR(128) NULL
 ,intermediate_consignee VARCHAR(128) NULL
 ,carrier VARCHAR(128) NULL
 ,forwader VARCHAR(128) NULL
 ,check_local_client VARCHAR(10) NULL
 ,export_adjustment_value VARCHAR(10) NULL
 ,original_itn VARCHAR(128) NULL
 ,CONSTRAINT PK_vessel_export_declaration_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

ALTER TABLE dbo.Vessel_Export_Declarations
ADD CONSTRAINT FK__vessel_export_declarations__vessel_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Export_Filing_Headers (id) ON DELETE CASCADE
GO

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
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

CREATE TABLE dbo.Vessel_Export_Invoice_Headers (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,usppi VARCHAR(128) NULL
 ,usppi_address VARCHAR(128) NULL
 ,usppi_contact VARCHAR(128) NULL
 ,usppi_phone VARCHAR(128) NULL
 ,ultimate_consignee VARCHAR(128) NULL
 ,ultimate_consignee_address VARCHAR(128) NULL
 ,invoice_number AS (dbo.vessel_export_invoice_header_number(filing_header_id, id))
 ,origin_indicator VARCHAR(1) NULL
 ,export_date DATE NULL
 ,ultimate_consignee_type VARCHAR(10) NULL
 ,invoice_inco_term VARCHAR(10) NULL
 ,invoice_total_amount NUMERIC(18, 6) NULL
 ,invoice_total_amount_currency VARCHAR(5) NULL
 ,exchange_rate NUMERIC(18, 6) NULL
 ,CONSTRAINT PK_vessel_export_invoice_headers_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

ALTER TABLE dbo.Vessel_Export_Invoice_Headers
ADD CONSTRAINT FK__vessel_export_invoice_headers__vessel_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Export_Filing_Headers (id) ON DELETE CASCADE
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

CREATE TABLE dbo.Vessel_Export_Invoice_Lines (
  id INT IDENTITY
 ,invoice_header_id INT NOT NULL
 ,line_number AS (dbo.vessel_export_invoice_line_number(invoice_header_id, id))
 ,export_code VARCHAR(128) NULL
 ,tariff_type VARCHAR(4) NULL
 ,tariff VARCHAR(35) NULL
 ,customs_qty NUMERIC(18, 6) NULL
 ,customs_qty_unit VARCHAR(10) NULL
 ,second_qty NUMERIC(18, 6) NULL
 ,price NUMERIC(18, 6) NULL
 ,price_currency VARCHAR(10) NULL
 ,gross_weight NUMERIC(18, 6) NULL
 ,gross_weight_unit VARCHAR(10) NULL
 ,goods_description VARCHAR(512) NULL
 ,license_type VARCHAR(10) NULL
 ,license_number VARCHAR(128) NULL
 ,license_value VARCHAR(128) NULL
 ,unit_price NUMERIC(16, 6) NULL
 ,goods_origin VARCHAR(10) NULL
 ,invoice_qty_unit VARCHAR(10) NULL
 ,CONSTRAINT PK_vessel_export_invoice_lines_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

ALTER TABLE dbo.Vessel_Export_Invoice_Lines
ADD CONSTRAINT FK__vessel_export_invoice_lines__vessel_export_invoice_headers__invoice_header_id FOREIGN KEY (invoice_header_id) REFERENCES dbo.Vessel_Export_Invoice_Headers (id) ON DELETE CASCADE
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

--- Vessel export report view ---
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

--- Add Vessel export configuration ---
SET IDENTITY_INSERT dbo.Vessel_Export_Sections ON
INSERT INTO dbo.Vessel_Export_Sections (id, [name], title, table_name, [procedure_name], is_array, parent_id) VALUES
  (1, 'root', 'Root', NULL, NULL, CONVERT(bit, 'False'), NULL)
  ,(2, 'declaration', 'Declaration', 'Vessel_Export_Declarations', 'vessel_export_add_declaration_record', CONVERT(bit, 'False'), 1)
  ,(3, 'invoice', 'Invoices', NULL, NULL, CONVERT(bit, 'False'), 1)
  ,(4, 'invoice_header', 'Invoice', 'Vessel_Export_Invoice_Headers', 'vessel_export_add_invoice_header_record', CONVERT(bit, 'True'), 3)
  ,(5, 'invoice_line', 'Line', 'Vessel_Export_Invoice_Lines', 'vessel_export_add_invoice_line_record', CONVERT(bit, 'True'), 4)
SET IDENTITY_INSERT dbo.Vessel_Export_Sections OFF
GO

INSERT dbo.Vessel_Export_Def_Values
	(section_id, column_name, label, [description], default_value, editable, has_default_value, mandatory, display_on_ui, [manual], single_filing_order, paired_field_table, paired_field_column, handbook_name, created_date, created_user) VALUES 
	(2, 'main_supplier', 'Main supplier', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'importer', 'Importer', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'shpt_type', 'Shipment type', NULL, 'EXP', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'transport', 'Transport', NULL, 'SEA', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'container', 'Container', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'transaction_related', 'Transaction related', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'hazardous', 'Hazardous', NULL, 'Y', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'routed_tran', 'Routed tran', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'filing_option', 'Filing option', NULL, '2', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'tariff_type', 'Tariff type', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'sold_en_route', 'Sold En Route', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'service', 'Service', NULL, 'STD', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'vessel', 'Vessel', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'voyage', 'Voyage', NULL, '001', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'carrier_scac', 'Carrier SCAC', NULL, 'UNKN', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'port_of_loading', 'Port of loading', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'dep', 'Dep', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'discharge', 'Discharge', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'arr', 'Arr', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'export', 'Export', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'exp', 'Exp', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'house_bill', 'House bill', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'origin', 'Origin', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'etd', 'Etd', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
	,(2, 'destination', 'Destination', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
	,(2, 'eta', 'Eta', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'country_of_export', 'Country of export', NULL, 'US', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'export_date', 'Export date', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'description', 'Description', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'owner_ref', 'Owner ref', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'inco', 'INCO', NULL, 'FOB', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'transport_ref', 'Transport ref', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'country_of_dest', 'Country of dest', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'state_of_origin', 'State of origin', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'inbond_type', 'Inbond type', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'license_type', 'License type', NULL, 'C33', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'license_number', 'License number', NULL,'NLR', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'export_code', 'Export code', NULL, 'OS', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'eccn', 'ECCN', NULL, 'EAR99', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'intermediate_consignee', 'Intermediate consignee', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'carrier', 'Carrier', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'forwader', 'Forwader', NULL, 'CHARBRONYC', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'check_local_client', 'Check local client', NULL, 'OK', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'export_adjustment_value', 'Export adjustment value', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(2, 'original_itn', 'Original ITN', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())

  ,(4, 'usppi', 'USPPI', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'usppi_address', 'USPPI address', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'usppi_contact', 'USPPI contact', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'usppi_phone', 'USPPI phone', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'ultimate_consignee', 'Ultimate consignee', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'ultimate_consignee_address', 'Consignee address', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'origin_indicator', 'Origin indicator', NULL, 'D', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'export_date', 'Export date', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'ultimate_consignee_type', 'Consignee type', NULL, 'R', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'invoice_inco_term', 'INCO term', NULL, 'FOB', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'invoice_total_amount', 'Total amount', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'invoice_total_amount_currency', 'Total amount currency', NULL, 'USD', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(4, 'exchange_rate', 'Exchange rate', NULL, '1', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  
  ,(5, 'export_code', 'Export code', NULL, 'OS', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'tariff_type', 'Tariff type', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'tariff', 'Tariff', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'customs_qty', 'Customs qty', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'customs_qty_unit', 'Customs qty unit', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'second_qty', 'Second qty', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'price', 'Price', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'price_currency', 'Price currency', NULL, 'USD', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'gross_weight', 'Gross weight', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'gross_weight_unit', 'Gross weight unit', NULL, 'T', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'goods_description', 'Goods description', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'license_type', 'License type', NULL, 'C33', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'license_number', 'License number', NULL, 'NLR', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'license_value', 'License value', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'unit_price', 'Unit price', NULL, '0', CONVERT(bit, 'True'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'goods_origin', 'Goods origin', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
  ,(5, 'invoice_qty_unit', 'Invoice qty unit', NULL, NULL, CONVERT(bit, 'True'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 0, NULL, NULL, NULL, NULL, GETDATE(), SUSER_NAME())
GO

--- Add Vessel export mapping procedures ---
-- Vessel export add def value manual values --
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
  OR v.display_on_ui > 0)
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

  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Def_Values_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
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
  END
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

-- Vessel export add declarations record --
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
       ,arr
       ,export
       ,exp
       ,origin
       ,etd
       ,export_date
       ,description
       ,owner_ref
       ,transport_ref
       ,country_of_dest
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
       ,intake.export_date
       ,intake.load_port
       ,intake.export_date
       ,load_port.unloco_code
       ,intake.export_date
       ,intake.export_date
       ,intake.scheduler + ' - ' + intake.reference_number + ' - ' + intake.goods_description
       ,intake.transport_ref + ' - ' + intake.scheduler
       ,intake.transport_ref
       ,intake.country_of_destination
       ,load_port.state_of_origin
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
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee
        ON usppi_consignee.usppi_id = intake.usppi_id
          AND usppi_consignee.consignee_id = intake.importer_id
      LEFT JOIN Vessel_Export_Rule_LoadPort load_port
        ON intake.load_port = load_port.load_port
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

-- Vessel export add invoice line record --
CREATE PROCEDURE dbo.vessel_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Invoice_Lines veil
      WHERE veil.invoice_header_id = @parentId)
    OR @allowMultiple = 1
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
       ,invoice_qty_unit)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,intake.tariff_type
       ,intake.tariff
       ,intake.quantity
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
       ,intake.[value]
       ,COALESCE(intake.weight, dbo.weightToTon(intake.quantity, dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)))
       ,intake.goods_description
       ,dbo.fn_getUnitByTariff(intake.tariff, intake.tariff_type)
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

    PRINT @recordId

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

-- Vessel export add invoice header record --
CREATE PROCEDURE dbo.vessel_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Export_Invoice_Headers veih
      WHERE veih.filing_header_id = @parentId)
    OR @allowMultiple = 1
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
       ,export_date
       ,invoice_total_amount)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,usppi.ClientCode
       ,usppi_rule.address
       ,usppi_rule.contact
       ,usppi_rule.phone
       ,consignee.ClientCode
       ,usppi_consignee_rule.consignee_address
       ,usppi_consignee_rule.ultimate_consignee_type
       ,intake.origin_indicator
       ,intake.export_date
       ,intake.invoice_total
      FROM dbo.Vessel_Export_Filing_Details detail
      INNER JOIN dbo.Vessel_Exports intake
        ON intake.id = detail.vessel_export_id
      LEFT JOIN dbo.Clients usppi
        ON intake.usppi_id = usppi.id
      LEFT JOIN dbo.Clients consignee
        ON intake.importer_id = consignee.id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI usppi_rule
        ON usppi_rule.usppi_id = intake.usppi_id
      LEFT JOIN dbo.Vessel_Export_Rule_USPPI_Consignee usppi_consignee_rule
        ON usppi_consignee_rule.usppi_id = intake.usppi_id
          AND usppi_consignee_rule.consignee_id = intake.importer_id
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