PRINT ('create tables')
GO

CREATE TABLE dbo.truck_exports (
  id INT IDENTITY
 ,tariff VARCHAR(35) NOT NULL
 ,routed_tran VARCHAR(10) NOT NULL
 ,eccn VARCHAR(128) NOT NULL
 ,goods_description VARCHAR(512) NOT NULL
 ,deleted BIT NOT NULL DEFAULT (0)
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,exporter VARCHAR(128) NOT NULL DEFAULT ('')
 ,importer VARCHAR(128) NOT NULL DEFAULT ('')
 ,tariff_type VARCHAR(3) NOT NULL DEFAULT ('')
 ,sold_en_route VARCHAR(128) NOT NULL DEFAULT ('')
 ,master_bill VARCHAR(128) NOT NULL DEFAULT ('')
 ,origin VARCHAR(128) NOT NULL DEFAULT ('')
 ,export VARCHAR(128) NOT NULL DEFAULT ('')
 ,export_date DATETIME NOT NULL DEFAULT ('1900-01-01T00:00:00.000')
 ,customs_qty DECIMAL(18, 6) NOT NULL DEFAULT (0)
 ,price DECIMAL(18, 6) NOT NULL DEFAULT (0)
 ,gross_weight DECIMAL(18, 6) NOT NULL DEFAULT (0)
 ,gross_weight_uom VARCHAR(3) NOT NULL DEFAULT ('')
 ,hazardous VARCHAR(3) NOT NULL DEFAULT ('')
 ,origin_indicator VARCHAR(1) NOT NULL DEFAULT ('')
 ,goods_origin VARCHAR(10) NOT NULL DEFAULT ('')
 ,CONSTRAINT [PK_dbo.truck_exports] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_filing_headers (
  id INT IDENTITY
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,error_description VARCHAR(MAX) NULL
 ,filing_number VARCHAR(255) NULL
 ,mapping_status TINYINT NULL
 ,filing_status TINYINT NULL
 ,response_xml VARCHAR(MAX) NULL
 ,request_xml VARCHAR(MAX) NULL
 ,job_link VARCHAR(1024) NULL
 ,CONSTRAINT [PK_dbo.truck_export_filing_headers] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_filing_details (
  filing_header_id INT NOT NULL
 ,truck_export_id INT NOT NULL
 ,CONSTRAINT [PK_dbo.truck_export_filing_details] PRIMARY KEY CLUSTERED (filing_header_id, truck_export_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_declarations (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL
 ,transport VARCHAR(128) NULL
 ,container VARCHAR(128) NULL
 ,tran_related VARCHAR(10) NULL
 ,hazardous VARCHAR(10) NULL
 ,routed_tran VARCHAR(10) NULL
 ,filing_option VARCHAR(10) NULL
 ,tariff_type VARCHAR(128) NULL
 ,sold_en_route VARCHAR(10) NULL
 ,service VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,port_of_loading VARCHAR(128) NULL
 ,dep DATE NULL
 ,discharge VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,exp_date DATE NULL
 ,house_bill VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,transport_ref VARCHAR(128) NULL
 ,inbond_type VARCHAR(10) NULL
 ,license_type VARCHAR(10) NULL
 ,license_number VARCHAR(128) NULL
 ,export_code VARCHAR(2) NULL
 ,eccn VARCHAR(128) NULL
 ,country_of_dest VARCHAR(128) NULL
 ,state_of_origin VARCHAR(2) NULL
 ,intermediate_consignee VARCHAR(128) NULL
 ,carrier VARCHAR(128) NULL
 ,forwader VARCHAR(128) NULL
 ,arr_date DATE NULL
 ,check_local_client VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,CONSTRAINT PK_truck_export_declaration_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_invoice_headers (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,usppi VARCHAR(128) NULL
 ,usppi_address VARCHAR(128) NULL
 ,usppi_contact VARCHAR(128) NULL
 ,usppi_phone VARCHAR(128) NULL
 ,origin_indicator VARCHAR(128) NULL
 ,ultimate_consignee_type VARCHAR(128) NULL
 ,invoice_total_amount NUMERIC(18, 6) NULL
 ,invoice_total_amount_currency VARCHAR(5) NULL
 ,ex_rate_date DATE NULL
 ,exchange_rate NUMERIC(18, 6) NULL
 ,invoice_inco_term VARCHAR(10) NULL
 ,ultimate_consignee VARCHAR(128) NULL
 ,CONSTRAINT PK_truck_export_invoice_headers_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_invoice_lines (
  id INT IDENTITY
 ,invoice_header_id INT NOT NULL
 ,export_code VARCHAR(128) NULL
 ,tariff VARCHAR(35) NULL
 ,customs_qty NUMERIC(18, 6) NULL
 ,customs_qty_unit VARCHAR(10) NULL
 ,second_qty NUMERIC(18, 6) NULL
 ,price NUMERIC(18, 6) NULL
 ,price_currency VARCHAR(128) NULL
 ,gross_weight NUMERIC(18, 6) NULL
 ,gross_weight_unit VARCHAR(128) NULL
 ,goods_description VARCHAR(512) NULL
 ,license_value VARCHAR(128) NULL
 ,unit_price NUMERIC(16, 6) NULL
 ,tariff_type VARCHAR(3) NULL
 ,invoice_qty_unit VARCHAR(10) NULL
 ,goods_origin VARCHAR(10) NULL
 ,CONSTRAINT PK_truck_export_invoice_lines_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_documents (
  id INT IDENTITY
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NOT NULL
 ,document_type VARCHAR(128) NULL
 ,content VARBINARY(MAX) NULL
 ,description VARCHAR(1000) NULL
 ,extension VARCHAR(128) NOT NULL
 ,file_name VARCHAR(255) NOT NULL
 ,filing_header_id INT NULL
 ,status VARCHAR(MAX) NULL
 ,inbound_record_id INT NULL
 ,CONSTRAINT [PK_dbo.truck_export_documents] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Export_Rule_Port (
  id INT IDENTITY
 ,port VARCHAR(128) NOT NULL
 ,origin_code VARCHAR(10) NULL
 ,state_of_origin VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_Export_Rule_Port] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Export_Rule_Consignee (
  id INT IDENTITY
 ,consignee_code VARCHAR(128) NOT NULL
 ,destination VARCHAR(5) NULL
 ,country VARCHAR(2) NULL
 ,ultimate_consignee_type VARCHAR(1) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_Export_Rule_Consignee] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Export_Rule_Exporter_Consignee (
  id INT IDENTITY
 ,exporter VARCHAR(128) NOT NULL
 ,consignee_code VARCHAR(128) NOT NULL
 ,address VARCHAR(128) NULL
 ,contact VARCHAR(128) NULL
 ,phone VARCHAR(128) NULL
 ,tran_related VARCHAR(1) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_Export_Rule_Exporter_Consignee] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_sections (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,procedure_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.truck_export_sections] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_def_values (
  id INT IDENTITY
 ,section_id INT NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,default_value VARCHAR(512) NULL
 ,editable BIT NOT NULL
 ,display_on_ui TINYINT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,manual TINYINT NOT NULL
 ,description VARCHAR(128) NULL
 ,label VARCHAR(128) NOT NULL
 ,single_filing_order TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.truck_export_def_values] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_export_def_values_manual (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,section_name VARCHAR(128) NOT NULL
 ,section_title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,record_id INT NOT NULL
 ,modification_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,label VARCHAR(128) NOT NULL
 ,description VARCHAR(128) NULL
 ,value VARCHAR(512) NULL
 ,editable BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,display_on_ui TINYINT NOT NULL
 ,manual TINYINT NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.truck_export_def_values_manual] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy data')
GO

PRINT ('copy inbound data')
GO

SET IDENTITY_INSERT dbo.truck_exports ON;
INSERT INTO dbo.truck_exports (
    id
   ,tariff
   ,routed_tran
   ,eccn
   ,goods_description
   ,exporter
   ,importer
   ,tariff_type
   ,sold_en_route
   ,master_bill
   ,origin
   ,export
   ,export_date
   ,customs_qty
   ,price
   ,gross_weight
   ,gross_weight_uom
   ,hazardous
   ,origin_indicator
   ,goods_origin
   ,deleted
   ,created_date
   ,created_user)
  SELECT
    te.id
   ,te.tariff
   ,te.routed_tran
   ,te.eccn
   ,te.goods_description
   ,te.exporter
   ,te.importer
   ,te.tariff_type
   ,te.sold_en_route
   ,te.master_bill
   ,te.origin
   ,te.export
   ,te.export_date
   ,te.customs_qty
   ,te.price
   ,te.gross_weight
   ,te.gross_weight_uom
   ,te.hazardous
   ,te.origin_indicator
   ,te.goods_origin
   ,te.deleted
   ,te.created_date
   ,te.created_user
  FROM dbo.exp_truck_inbound te;
SET IDENTITY_INSERT dbo.truck_exports OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.truck_export_filing_headers ON;
INSERT INTO dbo.truck_export_filing_headers (
    id
   ,filing_number
   ,mapping_status
   ,filing_status
   ,error_description
   ,job_link
   ,request_xml
   ,response_xml
   ,created_date
   ,created_user)
  SELECT
    fh.id
   ,fh.filing_number
   ,fh.mapping_status
   ,fh.filing_status
   ,fh.error_description
   ,fh.job_link
   ,fh.request_xml
   ,fh.response_xml
   ,fh.created_date
   ,fh.created_user
  FROM dbo.exp_truck_filing_header fh;
SET IDENTITY_INSERT dbo.truck_export_filing_headers OFF;
GO

PRINT ('copy filing details data')
GO

INSERT INTO dbo.truck_export_filing_details (
    truck_export_id
   ,filing_header_id)
  SELECT
    fd.inbound_id
   ,fd.filing_header_id
  FROM dbo.exp_truck_filing_detail fd;
GO

PRINT ('copy declaration data')
GO

SET IDENTITY_INSERT dbo.truck_export_declarations ON;
INSERT INTO dbo.truck_export_declarations (
    id
   ,filing_header_id
   ,main_supplier
   ,importer
   ,shpt_type
   ,transport
   ,container
   ,tran_related
   ,hazardous
   ,routed_tran
   ,filing_option
   ,tariff_type
   ,sold_en_route
   ,service
   ,master_bill
   ,carrier_scac
   ,port_of_loading
   ,dep
   ,discharge
   ,export
   ,exp_date
   ,house_bill
   ,origin
   ,destination
   ,owner_ref
   ,transport_ref
   ,inbond_type
   ,license_type
   ,license_number
   ,export_code
   ,eccn
   ,country_of_dest
   ,state_of_origin
   ,intermediate_consignee
   ,carrier
   ,forwader
   ,arr_date
   ,check_local_client
   ,country_of_export)
  SELECT
    declaration.id
   ,declaration.filing_header_id
   ,declaration.main_supplier
   ,declaration.importer
   ,declaration.shpt_type
   ,declaration.transport
   ,declaration.container
   ,declaration.tran_related
   ,declaration.hazardous
   ,declaration.routed_tran
   ,declaration.filing_option
   ,declaration.tariff_type
   ,declaration.sold_en_route
   ,declaration.service
   ,declaration.master_bill
   ,declaration.carrier_scac
   ,declaration.port_of_loading
   ,declaration.dep
   ,declaration.discharge
   ,declaration.export
   ,declaration.exp_date
   ,declaration.house_bill
   ,declaration.origin
   ,declaration.destination
   ,declaration.owner_ref
   ,declaration.transport_ref
   ,declaration.inbond_type
   ,declaration.license_type
   ,declaration.license_number
   ,declaration.export_code
   ,declaration.eccn
   ,declaration.country_of_dest
   ,declaration.state_of_origin
   ,declaration.intermediate_consignee
   ,declaration.carrier
   ,declaration.forwader
   ,declaration.arr_date
   ,declaration.check_local_client
   ,declaration.country_of_export
  FROM dbo.exp_truck_declaration declaration;
SET IDENTITY_INSERT dbo.truck_export_declarations OFF;
GO

PRINT ('copy invoice header data')
GO

SET IDENTITY_INSERT dbo.truck_export_invoice_headers ON;
INSERT INTO dbo.truck_export_invoice_headers (
    id
   ,filing_header_id
   ,usppi
   ,usppi_address
   ,usppi_contact
   ,usppi_phone
   ,origin_indicator
   ,ultimate_consignee_type
   ,invoice_total_amount
   ,invoice_total_amount_currency
   ,ex_rate_date
   ,exchange_rate
   ,invoice_inco_term
   ,ultimate_consignee)
  SELECT
    invoice.id
   ,invoice.filing_header_id
   ,invoice.usppi
   ,invoice.usppi_address
   ,invoice.usppi_contact
   ,invoice.usppi_phone
   ,invoice.origin_indicator
   ,invoice.ultimate_consignee_type
   ,invoice.invoice_total_amount
   ,invoice.invoice_total_amount_currency
   ,invoice.ex_rate_date
   ,invoice.exchange_rate
   ,invoice.invoice_inco_term
   ,invoice.ultimate_consignee
  FROM dbo.exp_truck_invoice_header invoice;
SET IDENTITY_INSERT dbo.truck_export_invoice_headers OFF;
GO

PRINT ('copy invoice lines data')
GO

SET IDENTITY_INSERT dbo.truck_export_invoice_lines ON;
INSERT INTO dbo.truck_export_invoice_lines (
    id
   ,invoice_header_id
   ,export_code
   ,tariff
   ,customs_qty
   ,customs_qty_unit
   ,second_qty
   ,price
   ,price_currency
   ,gross_weight
   ,gross_weight_unit
   ,goods_description
   ,license_value
   ,unit_price
   ,tariff_type
   ,invoice_qty_unit
   ,goods_origin)
  SELECT
    line.id
   ,line.parent_record_id
   ,line.export_code
   ,line.tariff
   ,line.customs_qty
   ,line.customs_qty_unit
   ,line.second_qty
   ,line.price
   ,line.price_currency
   ,line.gross_weight
   ,line.gross_weight_unit
   ,line.goods_description
   ,line.license_value
   ,line.unit_price
   ,line.tariff_type
   ,line.invoice_qty_unit
   ,line.goods_origin
  FROM dbo.exp_truck_invoice_line line
SET IDENTITY_INSERT dbo.truck_export_invoice_lines OFF;
GO

PRINT ('copy documents data')
GO

SET IDENTITY_INSERT dbo.truck_export_documents ON;
INSERT INTO dbo.truck_export_documents (
    id
   ,filing_header_id
   ,inbound_record_id
   ,file_name
   ,extension
   ,description
   ,content
   ,document_type
   ,status
   ,created_date
   ,created_user)
  SELECT
    doc.id
   ,doc.filing_header_id
   ,doc.inbound_record_id
   ,doc.file_name
   ,doc.file_extension
   ,doc.file_description
   ,doc.file_content
   ,doc.document_type
   ,doc.status
   ,doc.created_date
   ,doc.created_user
  FROM dbo.exp_truck_document doc;
SET IDENTITY_INSERT dbo.truck_export_documents OFF;
GO

PRINT ('copy port rule data')
GO

SET IDENTITY_INSERT dbo.Truck_Export_Rule_Port ON;
INSERT INTO dbo.Truck_Export_Rule_Port (
    id
   ,port
   ,origin_code
   ,state_of_origin
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.port
   ,r.origin_code
   ,r.state_of_origin
   ,r.created_date
   ,r.created_user
  FROM dbo.exp_truck_rule_port r;
SET IDENTITY_INSERT dbo.Truck_Export_Rule_Port OFF;
GO

PRINT ('copy consignee rule data')
GO

SET IDENTITY_INSERT dbo.Truck_Export_Rule_Consignee ON;
INSERT INTO dbo.Truck_Export_Rule_Consignee (
    id
   ,consignee_code
   ,destination
   ,country
   ,ultimate_consignee_type
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.consignee_code
   ,r.destination
   ,r.country
   ,r.ultimate_consignee_type
   ,r.created_date
   ,r.created_user
  FROM dbo.exp_truck_rule_consignee r;
SET IDENTITY_INSERT dbo.Truck_Export_Rule_Consignee OFF;
GO

PRINT ('copy exporter-consignee rule data')
GO

SET IDENTITY_INSERT dbo.Truck_Export_Rule_Exporter_Consignee ON;
INSERT INTO dbo.Truck_Export_Rule_Exporter_Consignee (
    id
   ,exporter
   ,consignee_code
   ,address
   ,contact
   ,phone
   ,tran_related
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.exporter
   ,r.consignee_code
   ,r.address
   ,r.contact
   ,r.phone
   ,r.tran_related
   ,r.created_date
   ,r.created_user
  FROM dbo.exp_truck_rule_exporter_consignee r;
SET IDENTITY_INSERT dbo.Truck_Export_Rule_Exporter_Consignee OFF;
GO

PRINT ('copy sections data')
GO

SET IDENTITY_INSERT dbo.truck_export_sections ON;
INSERT INTO dbo.truck_export_sections (
    id
   ,name
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,procedure_name)
  SELECT
    section.id
   ,section.name
   ,section.title
   ,section.table_name
   ,section.is_array
   ,section.parent_id
   ,section.procedure_name
  FROM dbo.exp_truck_form_section_configuration section;
SET IDENTITY_INSERT dbo.truck_export_sections OFF;
GO

UPDATE exp_truck_form_section_configuration
SET table_name = 'truck_export_declarations'
   ,procedure_name = 'truck_export_add_declaration_record'
WHERE table_name = 'exp_truck_declaration';
UPDATE exp_truck_form_section_configuration
SET table_name = 'truck_export_invoice_headers'
   ,procedure_name = 'truck_export_add_invoice_header_record'
WHERE table_name = 'exp_truck_invoice_header';
UPDATE exp_truck_form_section_configuration
SET table_name = 'truck_export_invoice_lines'
   ,procedure_name = 'truck_export_add_invoice_line_record'
WHERE table_name = 'exp_truck_invoice_line';
GO

PRINT ('copy def-values data')
GO

SET IDENTITY_INSERT dbo.truck_export_def_values ON;
INSERT INTO dbo.truck_export_def_values (
    id
   ,section_id
   ,column_name
   ,label
   ,description
   ,default_value
   ,has_default_value
   ,editable
   ,mandatory
   ,display_on_ui
   ,manual
   ,single_filing_order
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,created_date
   ,created_user)
  SELECT
    dv.id
   ,dv.section_id
   ,dv.column_name
   ,dv.label
   ,dv.description
   ,dv.value
   ,dv.has_default_value
   ,dv.editable
   ,dv.mandatory
   ,dv.display_on_ui
   ,dv.manual
   ,dv.single_filing_order
   ,dv.paired_field_table
   ,dv.paired_field_column
   ,dv.handbook_name
   ,dv.created_date
   ,dv.created_user
  FROM dbo.exp_truck_form_configuration dv;
SET IDENTITY_INSERT dbo.truck_export_def_values OFF;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.truck_export_filing_details
ADD CONSTRAINT [FK_dbo.truck_export_filing_details_dbo.truck_export_filing_headers_filing_header_id] FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.truck_export_filing_details
ADD CONSTRAINT [FK_dbo.truck_export_filing_details_dbo.truck_exports_truck_export_id] FOREIGN KEY (truck_export_id) REFERENCES dbo.truck_exports (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.truck_export_declarations
ADD CONSTRAINT FK__truck_export_declarations__truck_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.truck_export_invoice_headers
ADD CONSTRAINT FK__truck_export_invoice_headers__truck_export_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.truck_export_invoice_lines
ADD CONSTRAINT FK__truck_export_invoice_lines__truck_export_invoice_headers__invoice_header_id FOREIGN KEY (invoice_header_id) REFERENCES dbo.truck_export_invoice_headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.truck_export_documents
ADD CONSTRAINT [FK_dbo.truck_export_documents_dbo.truck_export_filing_headers_filing_header_id] FOREIGN KEY (filing_header_id) REFERENCES dbo.truck_export_filing_headers (id)
GO

ALTER TABLE dbo.truck_export_documents
ADD CONSTRAINT [FK_dbo.truck_export_documents_dbo.truck_exports_inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.truck_exports (id)
GO

ALTER TABLE dbo.truck_export_def_values
ADD CONSTRAINT [FK_dbo.truck_export_def_values_dbo.truck_export_sections_section_id] FOREIGN KEY (section_id) REFERENCES dbo.truck_export_sections (id)
GO

ALTER TABLE dbo.truck_export_sections
ADD CONSTRAINT [FK_dbo.truck_export_sections_dbo.truck_export_sections_parent_id] FOREIGN KEY (parent_id) REFERENCES dbo.truck_export_sections (id)
GO

PRINT ('add indexes')
GO

CREATE INDEX IX_filing_header_id
ON dbo.truck_export_filing_details (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX IX_truck_export_id
ON dbo.truck_export_filing_details (truck_export_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckExportDeclarationTab_filingHeadersId
ON dbo.truck_export_declarations (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckExportInvoiceHeaders_filingHeadersId
ON dbo.truck_export_invoice_headers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckExportInvoiceLines_invoiceHeadersId
ON dbo.truck_export_invoice_lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX IX_filing_header_id
ON dbo.truck_export_documents (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX IX_inbound_record_id
ON dbo.truck_export_documents (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_consignee_code
ON dbo.Truck_Export_Rule_Consignee (consignee_code)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_exporter_consignee_code
ON dbo.Truck_Export_Rule_Exporter_Consignee (exporter, consignee_code)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_port
ON dbo.Truck_Export_Rule_Port (port)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.truck_export_def_values (section_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeaderId
ON dbo.truck_export_def_values_manual (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_recordId_tableName_columnName
ON dbo.truck_export_def_values_manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.truck_export_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.truck_export_sections (parent_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO

-- gets truck export invoice line number
CREATE FUNCTION dbo.truck_export_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      teil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY teil.id)
    FROM dbo.truck_export_invoice_lines teil
    WHERE teil.invoice_header_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

-- alter truck export invoice line table
ALTER TABLE dbo.truck_export_invoice_lines
ADD invoice_line_number AS ([dbo].[truck_export_invoice_line_number]([invoice_header_id], [id]));
GO

-- gets truck export invoice header number
CREATE FUNCTION dbo.truck_export_invoice_header_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      teih.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY teih.id)
    FROM dbo.truck_export_invoice_headers teih
    WHERE teih.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO

-- alter truck export invoice header table
ALTER TABLE dbo.truck_export_invoice_headers
ADD invoice_number AS ([dbo].[truck_export_invoice_header_number]([filing_header_id], [id]));
GO

PRINT ('create triggers')
GO

CREATE TRIGGER dbo.truck_export_declarations_befor_delete
ON dbo.truck_export_declarations
FOR DELETE
AS
  DELETE FROM dbo.truck_export_def_values_manual
  WHERE table_name = 'truck_export_declarations'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.truck_export_invoice_headers_befor_delete
ON dbo.truck_export_invoice_headers
FOR DELETE
AS
  DELETE FROM dbo.truck_export_def_values_manual
  WHERE table_name = 'truck_export_invoice_headers'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.truck_export_invoice_lines_befor_delete
ON dbo.truck_export_invoice_lines
FOR DELETE
AS
  DELETE FROM dbo.truck_export_def_values_manual
  WHERE table_name = 'truck_export_invoice_lines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

PRINT ('create views')
GO

CREATE VIEW dbo.truck_export_grid
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
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Truck_Export_Rule_Consignee rule_consignee
        WHERE RTRIM(LTRIM(rule_consignee.consignee_code)) = RTRIM(LTRIM(te.importer))) THEN 1
    ELSE 0
  END AS has_consignee_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Truck_Export_Rule_Exporter_Consignee rule_exporter_consignee
        WHERE RTRIM(LTRIM(rule_exporter_consignee.consignee_code)) = RTRIM(LTRIM(te.importer))
        AND RTRIM(LTRIM(rule_exporter_consignee.exporter)) = RTRIM(LTRIM(te.exporter))) THEN 1
    ELSE 0
  END AS has_exporter_consignee_rule
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
 ,1 AS has_consignee_rule
 ,1 AS has_exporter_consignee_rule
FROM dbo.truck_exports te
LEFT JOIN dbo.truck_export_filing_details tefd
  ON tefd.truck_export_id = te.id
INNER JOIN dbo.truck_export_filing_headers tefh
  ON tefh.id = tefd.filing_header_id
WHERE tefh.mapping_status <> 0
AND te.deleted = 0
GO

CREATE VIEW dbo.Truck_Export_Report
AS
SELECT
  headers.id
 ,detailes.truck_export_id AS TEI_ID
 ,declaration.[main_supplier] AS Declarationtab_Main_Supplier
 ,declaration.[importer] AS Declarationtab_Importer
 ,declaration.[shpt_type] AS Declarationtab_shpt_type
 ,declaration.[transport] AS Declarationtab_transport
 ,declaration.[container] AS Declarationtab_container
 ,declaration.[tran_related] AS Declarationtab_tran_related
 ,declaration.[hazardous] AS Declarationtab_hazardous
 ,declaration.[routed_tran] AS Declarationtab_routed_tran
 ,declaration.[filing_option] AS Declarationtab_filing_option
 ,declaration.[tariff_type] AS Declarationtab_TariffType
 ,declaration.[sold_en_route] AS Declarationtab_sold_en_route
 ,declaration.[service] AS Declarationtab_service
 ,declaration.[master_bill] AS Declarationtab_master_bill
 ,declaration.[port_of_loading] AS Declarationtab_port_of_loading
 ,declaration.[dep] AS Declarationtab_dep
 ,declaration.[discharge] AS Declarationtab_discharge
 ,declaration.[export] AS Declarationtab_export
 ,declaration.[exp_date] AS Declarationtab_exp_date
 ,declaration.[house_bill] AS Declarationtab_house_bill
 ,declaration.[origin] AS Declarationtab_origin
 ,declaration.[destination] AS Declarationtab_destination
 ,declaration.[owner_ref] AS Declarationtab_owner_ref
 ,declaration.[transport_ref] AS Declarationtab_transport_ref
 ,declaration.[inbond_type] AS Declarationtab_Inbond_type
 ,declaration.[license_type] AS Declarationtab_License_type
 ,declaration.[license_number] AS Declarationtab_License_number
 ,declaration.[export_code] AS Declarationtab_ExportCode
 ,declaration.[eccn] AS Declarationtab_Eccn
 ,declaration.[country_of_dest] AS Declarationtab_Country_of_dest
 ,declaration.[state_of_origin] AS Declarationtab_State_of_origin
 ,declaration.[intermediate_consignee] AS Declarationtab_Intermediate_consignee
 ,declaration.[carrier] AS Declarationtab_carrier
 ,declaration.[forwader] AS Declarationtab_forwader
 ,declaration.[arr_date] AS Declarationtab_arr_date
 ,declaration.[check_local_client] AS Declarationtab_check_local_client
 ,declaration.[country_of_export] AS Declarationtab_Country_of_export

 ,invheaders.[usppi] AS Invheaderstab_Usppi
 ,invheaders.[usppi_address] AS Invheaderstab_usppi_address
 ,invheaders.[usppi_contact] AS Invheaderstab_usppi_contact
 ,invheaders.[usppi_phone] AS Invheaderstab_usppi_phone
 ,invheaders.[origin_indicator] AS Invheaderstab_origin_indicator
 ,invheaders.[ultimate_consignee] AS Invheaderstab_ultimate_consignee
 ,invheaders.[ultimate_consignee_type] AS Invheaderstab_ultimate_consignee_type
 ,invheaders.[invoice_number] AS Invheaderstab_invoice_number
 ,invheaders.[invoice_total_amount] AS Invheaderstab_invoice_total_amount
 ,invheaders.[invoice_total_amount_currency] AS Invheaderstab_invoice_total_amount_currency
 ,invheaders.[ex_rate_date] AS Invheaderstab_ex_rate_date
 ,invheaders.[exchange_rate] AS Invheaderstab_exchange_rate
 ,invheaders.[invoice_inco_term] AS Invheaderstab_invoice_inco_term

 ,invlines.[invoice_line_number] AS Invlinestab_lno
 ,invlines.[tariff] AS Invlinestab_tariff
 ,invlines.[customs_qty] AS Invlinestab_customs_qty
 ,invlines.[export_code] AS Invlinestab_export_code
 ,invlines.[goods_description] AS Invlinestab_goods_description
 ,invlines.[customs_qty_unit] AS Invlinestab_customs_qty_unit
 ,invlines.[second_qty] AS Invlinestab_second_qty
 ,invlines.[price] AS Invlinestab_price
 ,invlines.[price_currency] AS Invlinestab_price_currency
 ,invlines.[gross_weight] AS Invlinestab_gross_weight
 ,invlines.[gross_weight_unit] AS Invlinestab_gross_weight_unit
 ,invlines.[license_value] AS Invlinestab_license_value
 ,invlines.[unit_price] AS Invlinestab_unit_price
 ,invlines.[tariff_type] AS Invlinestab_tariff_type
 ,invlines.[goods_origin] AS Invlinestab_goods_origin

FROM dbo.truck_export_filing_headers headers
INNER JOIN dbo.truck_export_filing_details detailes
  ON headers.id = detailes.filing_header_id
LEFT OUTER JOIN dbo.truck_export_declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.truck_export_invoice_lines invlines
  ON invlines.invoice_header_id = invheaders.id

WHERE headers.mapping_status = 2
GO

CREATE VIEW dbo.v_truck_export_def_values_manual
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
FROM dbo.truck_export_def_values_manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATED_DATE', 'CREATED_USER', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_truck_export_tables
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,tes.id AS section_id
 ,tes.title AS section_title
FROM information_schema.columns i
INNER JOIN dbo.truck_export_sections tes
  ON i.TABLE_NAME = tes.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
GO

PRINT ('create stored procedures')
GO

-- Add Truck Export def values manual records --
CREATE PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.truck_export_def_values v
  INNER JOIN dbo.truck_export_sections s
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

  INSERT INTO dbo.truck_export_def_values_manual (
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
     ,tes.name
     ,tes.title
     ,@recordId
     ,tedv.column_name
     ,tes.table_name
     ,GETDATE()
     ,@defValueOut
     ,tedv.editable
     ,tedv.display_on_ui
     ,tedv.has_default_value
     ,tedv.mandatory
     ,tedv.manual
     ,tedv.description
     ,tedv.label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column
    FROM dbo.truck_export_def_values tedv
    INNER JOIN dbo.truck_export_sections tes
      ON tedv.section_id = tes.id
    WHERE tedv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Apply Truck Export def values for specified table --
CREATE PROCEDURE dbo.truck_export_apply_def_values (@tableName VARCHAR(128)
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
  FROM dbo.truck_export_def_values_manual v
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

-- Add Truck Export Declaration Tab record --
CREATE PROCEDURE dbo.truck_export_add_declaration_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.truck_export_declarations ted
      WHERE ted.filing_header_id = @parentId)
  BEGIN

    INSERT INTO dbo.truck_export_declarations (
        filing_header_id
       ,destination
       ,country_of_dest
       ,tran_related
       ,routed_tran
       ,eccn
       ,export
       ,importer
       ,tariff_type
       ,sold_en_route
       ,master_bill
       ,owner_ref
       ,port_of_loading
       ,transport_ref
       ,main_supplier
       ,dep
       ,exp_date
       ,hazardous
       ,origin
       ,state_of_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,terc.destination
       ,terc.country
       ,terec.tran_related
       ,te.routed_tran
       ,te.eccn
       ,te.export
       ,te.importer
       ,te.tariff_type
       ,te.sold_en_route
       ,te.master_bill
       ,te.master_bill
       ,te.origin
       ,te.master_bill
       ,te.exporter
       ,te.export_date
       ,te.export_date
       ,te.hazardous
       ,rulePort.origin_code
       ,rulePort.state_of_origin
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
          AND te.exporter = terec.exporter
      LEFT JOIN truck_export_rule_port rulePort
        ON (RTRIM(LTRIM(te.origin)) = RTRIM(LTRIM(rulePort.port)))
      WHERE tefd.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_export_add_def_values_manual @tableName
                                               ,@filingHeaderId
                                               ,@parentId
                                               ,@recordId

    -- apply default values
    EXEC dbo.truck_export_apply_def_values @tableName
                                          ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO

-- Add Truck Export Invoice Line record --
CREATE PROCEDURE dbo.truck_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.truck_export_invoice_lines t
      WHERE t.invoice_header_id = @parentId)
  BEGIN

    INSERT INTO dbo.truck_export_invoice_lines (
        invoice_header_id
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_unit
       ,goods_description
       ,tariff_type
       ,invoice_qty_unit
       ,goods_origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @parentId
       ,te.tariff
       ,te.customs_qty
       ,te.price
       ,te.gross_weight
       ,te.gross_weight_uom
       ,te.goods_description
       ,te.tariff_type
       ,dbo.fn_getUnitByTariff(te.tariff, te.tariff_type) AS invoice_qty_unit
       ,te.goods_origin
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      WHERE tefd.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_export_add_def_values_manual @tableName
                                               ,@filingHeaderId
                                               ,@parentId
                                               ,@recordId

    -- apply default values
    EXEC dbo.truck_export_apply_def_values @tableName
                                          ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END;
GO

-- Add Truck Export Invoice Header record --
CREATE PROCEDURE dbo.truck_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.truck_export_invoice_headers t
      WHERE t.filing_header_id = @parentId)
  BEGIN

    INSERT INTO dbo.truck_export_invoice_headers (
        filing_header_id
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee_type
       ,usppi
       ,invoice_total_amount
       ,ultimate_consignee
       ,origin_indicator)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,terec.address
       ,terec.contact
       ,terec.phone
       ,terc.ultimate_consignee_type
       ,te.exporter
       ,te.price
       ,te.importer
       ,te.origin_indicator
      FROM truck_export_filing_details tefd
      JOIN truck_exports te
        ON tefd.truck_export_id = te.id
      LEFT JOIN Truck_Export_Rule_Consignee terc
        ON te.importer = terc.consignee_code
      LEFT JOIN Truck_Export_Rule_Exporter_Consignee terec
        ON te.importer = terec.consignee_code
          AND te.exporter = terec.exporter
      WHERE tefd.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.truck_export_add_def_values_manual @tableName
                                               ,@filingHeaderId
                                               ,@parentId
                                               ,@recordId

    -- apply default values
    EXEC dbo.truck_export_apply_def_values @tableName
                                          ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    -- add invoice line
    EXEC dbo.truck_export_add_invoice_line_record @filingHeaderId
                                                 ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO

-- add truck_export_filing procedure
CREATE PROCEDURE dbo.truck_export_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.truck_export_add_declaration_record @filingHeaderId
                                              ,@filingHeaderId

  -- add invoice header
  EXEC dbo.truck_export_add_invoice_header_record @filingHeaderId
                                                 ,@filingHeaderId
END
GO

-- add truck_export_filing_del procedure
CREATE PROCEDURE dbo.truck_export_filing_del (@filingHeaderId INT)
AS
BEGIN

  DELETE FROM dbo.truck_export_def_values_manual
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.truck_export_filing_details
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.truck_export_filing_headers
  WHERE id = @filingHeaderId

END
GO

/****** Object:  StoredProcedure [dbo].[truck_export_del]    Script Date: 24.12.2018 ******/
CREATE PROCEDURE dbo.truck_export_del (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mapping_status = grid.mapping_status
  FROM truck_export_grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE truck_exports
    SET deleted = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE truck_exports
      SET deleted = @deleted
      WHERE Id IN (SELECT
          details.truck_export_id
        FROM truck_export_filing_details details
        WHERE details.filing_header_id = @filingHeaderId)
    END
  END
END
GO

CREATE PROCEDURE dbo.truck_export_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.truck_export_sections tes
      WHERE tes.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE
    THROW 51000, 'Invalid table name', 1
END
GO

-- add truck_export_filing_param procedure
CREATE PROCEDURE dbo.truck_export_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.value, '') + ''' as ' +
    data_type +
    CASE
      WHEN data_type IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN data_type IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN data_type IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.truck_export_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO

PRINT ('update existing objects')
GO

ALTER VIEW dbo.v_Documents
AS
SELECT
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
 ,truck_doc.filename AS filename
 ,truck_doc.file_extension AS file_extension
 ,truck_doc.file_content AS file_Content
 ,truck_doc.file_desc AS file_desc
 ,truck_doc.document_type AS document_type
 ,'Truck_Imp' AS transport_shipment_type
FROM Truck_Documents truck_doc
INNER JOIN truck_Filing_Headers truck_header
  ON truck_doc.Filing_Headers_FK = truck_header.id

UNION ALL
SELECT
  pipeline_header.id AS filing_header_id
 ,pipeline_doc.id AS doc_id
 ,pipeline_doc.filename AS filename
 ,pipeline_doc.file_extension AS file_extension
 ,pipeline_doc.file_content AS file_Content
 ,pipeline_doc.file_desc AS file_desc
 ,pipeline_doc.DocumentType AS document_type
 ,'Pipeline_Imp' AS transport_shipment_type
FROM pipeline_Documents pipeline_doc
INNER JOIN pipeline_Filing_Headers pipeline_header
  ON pipeline_doc.Filing_Headers_FK = pipeline_header.id
UNION ALL
SELECT
  vessel_header.id AS filing_header_id
 ,vessel_doc.id AS doc_id
 ,vessel_doc.file_name AS filename
 ,vessel_doc.extension AS file_extension
 ,vessel_doc.content AS file_Content
 ,vessel_doc.description AS file_desc
 ,vessel_doc.document_type AS document_type
 ,'Vessel_Imp' AS transport_shipment_type
FROM [dbo].[Vessel_Import_Documents] vessel_doc
INNER JOIN [dbo].[Vessel_Import_Filing_Headers] vessel_header
  ON vessel_doc.[filing_header_id] = vessel_header.id
UNION ALL
SELECT
  truck_export_header.id AS filing_header_id
 ,truck_export_doc.id AS doc_id
 ,truck_export_doc.file_name AS filename
 ,truck_export_doc.extension file_extension
 ,truck_export_doc.content AS file_Content
 ,truck_export_doc.description AS file_desc
 ,truck_export_doc.document_type AS document_type
 ,'Truck_Export' AS transport_shipment_type
FROM truck_export_Documents truck_export_doc
INNER JOIN truck_export_Filing_Headers truck_export_header
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

PRINT ('drop old objects')
PRINT ('drop functions and computed columns')
GO

ALTER TABLE dbo.exp_truck_invoice_header
DROP COLUMN invoice_number
DROP FUNCTION dbo.fn_exp_truck_invoice_header_number
ALTER TABLE dbo.exp_truck_invoice_line
DROP COLUMN invoice_line_number
DROP FUNCTION dbo.fn_exp_truck_invoice_line_number
DROP FUNCTION dbo.fn_app_unit_by_tariff

PRINT ('drop tables')
GO

DROP TABLE dbo.exp_truck_declaration
DROP TABLE dbo.exp_truck_document
DROP TABLE dbo.exp_truck_invoice_line
DROP TABLE dbo.exp_truck_invoice_header
DROP TABLE dbo.exp_truck_filing_detail
DROP TABLE dbo.exp_truck_filing_header
DROP TABLE dbo.exp_truck_inbound
DROP TABLE dbo.exp_truck_rule_port
DROP TABLE dbo.exp_truck_rule_consignee
DROP TABLE dbo.exp_truck_rule_exporter_consignee
DROP TABLE dbo.exp_truck_form_configuration
DROP TABLE dbo.exp_truck_form_section_configuration

PRINT ('drop views')
GO

DROP VIEW dbo.v_exp_truck_form_configuration
DROP VIEW dbo.v_exp_truck_field_configuration
DROP VIEW dbo.v_exp_truck_inbound_grid
DROP VIEW dbo.v_exp_truck_report

PRINT ('drop procedures')
GO

DROP PROCEDURE dbo.sp_exp_truck_create_entry_records
DROP PROCEDURE dbo.sp_exp_truck_add_declaration
DROP PROCEDURE dbo.sp_exp_truck_add_invoice_header
DROP PROCEDURE dbo.sp_exp_truck_add_invoice_line
DROP PROCEDURE dbo.sp_exp_truck_review_entry
DROP PROCEDURE dbo.sp_exp_truck_delete_entry_records
DROP PROCEDURE dbo.sp_exp_truck_update_entry
DROP PROCEDURE dbo.sp_exp_truck_recalculate
DROP PROCEDURE dbo.sp_exp_truck_delete_inbound
