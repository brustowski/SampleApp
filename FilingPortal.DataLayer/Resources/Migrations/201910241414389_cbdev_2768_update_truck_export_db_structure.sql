PRINT ('create tables')
GO

CREATE TABLE dbo.exp_truck_inbound (
  id INT IDENTITY
 ,tariff VARCHAR(35) NOT NULL
 ,routed_tran VARCHAR(10) NOT NULL
 ,eccn VARCHAR(128) NOT NULL
 ,goods_description VARCHAR(512) NOT NULL
 ,exporter VARCHAR(128) NOT NULL
 ,importer VARCHAR(128) NOT NULL
 ,tariff_type VARCHAR(3) NOT NULL
 ,sold_en_route VARCHAR(128) NOT NULL
 ,master_bill VARCHAR(128) NOT NULL
 ,origin VARCHAR(128) NOT NULL
 ,export VARCHAR(128) NOT NULL
 ,export_date DATETIME NOT NULL
 ,customs_qty DECIMAL(18, 6) NOT NULL
 ,price DECIMAL(18, 6) NOT NULL
 ,gross_weight DECIMAL(18, 6) NOT NULL
 ,gross_weight_uom VARCHAR(3) NOT NULL
 ,hazardous VARCHAR(3) NOT NULL
 ,origin_indicator VARCHAR(1) NOT NULL
 ,goods_origin VARCHAR(10) NOT NULL
 ,deleted BIT NOT NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_filing_detail (
  inbound_id INT NOT NULL
 ,filing_header_id INT NOT NULL
 ,PRIMARY KEY CLUSTERED (inbound_id, filing_header_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_declaration (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
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
 ,[service] VARCHAR(128) NULL
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
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_invoice_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
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
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.exp_truck_invoice_line (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
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
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_rule_consignee (
  id INT IDENTITY
 ,consignee_code VARCHAR(128) NOT NULL
 ,destination VARCHAR(5) NULL
 ,country VARCHAR(2) NULL
 ,ultimate_consignee_type VARCHAR(1) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_rule_exporter_consignee (
  id INT IDENTITY
 ,exporter VARCHAR(128) NOT NULL
 ,consignee_code VARCHAR(128) NOT NULL
 ,[address] VARCHAR(128) NULL
 ,contact VARCHAR(128) NULL
 ,phone VARCHAR(128) NULL
 ,tran_related VARCHAR(1) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_rule_port (
  id INT IDENTITY
 ,port VARCHAR(128) NOT NULL
 ,origin_code VARCHAR(10) NULL
 ,state_of_origin VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_form_section_configuration (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,procedure_name VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.exp_truck_form_configuration (
  id INT IDENTITY
 ,section_id INT NOT NULL
 ,column_name VARCHAR(128) NULL
 ,label VARCHAR(128) NULL
 ,[description] VARCHAR(128) NULL
 ,[value] VARCHAR(512) NULL
 ,has_default_value BIT NOT NULL
 ,editable BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,display_on_ui TINYINT NOT NULL DEFAULT (0)
 ,[manual] TINYINT NOT NULL DEFAULT (0)
 ,single_filing_order TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.exp_truck_inbound ON;
INSERT INTO dbo.exp_truck_inbound (
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
  FROM dbo.truck_exports te;
SET IDENTITY_INSERT dbo.exp_truck_inbound OFF;
GO

PRINT ('copy filing header data')
GO
--
-- Alter column [mapping_status] on table [dbo].[truck_export_filing_headers]
--
ALTER TABLE dbo.truck_export_filing_headers
  ALTER
    COLUMN mapping_status int
GO
ALTER TABLE dbo.truck_export_filing_headers
  ALTER
    COLUMN filing_status int
GO

EXEC sp_rename N'dbo.truck_export_filing_headers'
              ,N'exp_truck_filing_header'
              ,'OBJECT'
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.exp_truck_filing_detail (
    inbound_id
   ,filing_header_id)
  SELECT
    fd.truck_export_id
   ,fd.filing_header_id
  FROM dbo.truck_export_filing_details fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.exp_truck_declaration ON;
INSERT INTO dbo.exp_truck_declaration (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,declaration.filing_header_id
   ,NULL
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
  FROM dbo.truck_export_declarations declaration;
SET IDENTITY_INSERT dbo.exp_truck_declaration OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.exp_truck_invoice_header ON;
INSERT INTO dbo.exp_truck_invoice_header (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,invoice.filing_header_id
   ,NULL
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
  FROM dbo.truck_export_invoice_headers invoice;
SET IDENTITY_INSERT dbo.exp_truck_invoice_header OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.exp_truck_invoice_line ON;
INSERT INTO dbo.exp_truck_invoice_line (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,invoice.filing_header_id
   ,line.invoice_header_id
   ,NULL
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
  FROM dbo.truck_export_invoice_lines line
  LEFT JOIN truck_export_invoice_headers invoice
    ON line.invoice_header_id = invoice.id;
SET IDENTITY_INSERT dbo.exp_truck_invoice_line OFF;
GO

PRINT ('copy documents data')
GO
DECLARE @truck_docs_drop_constraints_sql NVARCHAR(MAX) = N'';

SELECT
  @truck_docs_drop_constraints_sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
  + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
  ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.objects
WHERE OBJECT_NAME(parent_object_id) = 'truck_export_documents' AND (type = 'D' OR type = 'F')

PRINT @truck_docs_drop_constraints_sql;
EXEC sp_executesql @truck_docs_drop_constraints_sql;

DROP INDEX IF EXISTS Idx__filing_header_id ON dbo.truck_export_documents
DROP INDEX IF EXISTS Idx__inbound_record_id ON dbo.truck_export_documents

EXEC sp_rename N'dbo.truck_export_documents.extension'
              ,N'file_extension'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.truck_export_documents.description'
              ,N'file_description'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.truck_export_documents.content'
              ,N'file_content'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.truck_export_documents'
              ,N'exp_truck_document'
              ,'OBJECT'
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.exp_truck_rule_port ON;
INSERT INTO dbo.exp_truck_rule_port (
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
  FROM dbo.Truck_Export_Rule_Port r;
SET IDENTITY_INSERT dbo.exp_truck_rule_port OFF;
GO

PRINT ('copy consignee rule data')
GO
SET IDENTITY_INSERT dbo.exp_truck_rule_consignee ON;
INSERT INTO dbo.exp_truck_rule_consignee (
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
  FROM dbo.Truck_Export_Rule_Consignee r;
SET IDENTITY_INSERT dbo.exp_truck_rule_consignee OFF;
GO

PRINT ('copy exporter-consignee rule data')
GO
SET IDENTITY_INSERT dbo.exp_truck_rule_exporter_consignee ON;
INSERT INTO dbo.exp_truck_rule_exporter_consignee (
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
  FROM dbo.Truck_Export_Rule_Exporter_Consignee r;
SET IDENTITY_INSERT dbo.exp_truck_rule_exporter_consignee OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.exp_truck_form_section_configuration ON;
INSERT INTO dbo.exp_truck_form_section_configuration (
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
  FROM dbo.truck_export_sections section;
SET IDENTITY_INSERT dbo.exp_truck_form_section_configuration OFF;
GO

UPDATE exp_truck_form_section_configuration
SET table_name = 'exp_truck_declaration'
   ,procedure_name = 'sp_exp_truck_add_declaration'
WHERE table_name = 'truck_export_declarations';
UPDATE exp_truck_form_section_configuration
SET table_name = 'exp_truck_invoice_header'
   ,procedure_name = 'sp_exp_truck_add_invoice_header'
WHERE table_name = 'truck_export_invoice_headers';
UPDATE exp_truck_form_section_configuration
SET table_name = 'exp_truck_invoice_line'
   ,procedure_name = 'sp_exp_truck_add_invoice_line'
WHERE table_name = 'truck_export_invoice_lines';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.exp_truck_form_configuration ON;
INSERT INTO dbo.exp_truck_form_configuration (
    id
   ,section_id
   ,column_name
   ,label
   ,description
   ,value
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
   ,dv.default_value
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
  FROM dbo.truck_export_def_values dv;
SET IDENTITY_INSERT dbo.exp_truck_form_configuration OFF;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.exp_truck_filing_header
ADD CONSTRAINT [FK__exp_truck_filing_header__FilingStatus__filing_status] FOREIGN KEY (filing_status) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.exp_truck_filing_header
ADD CONSTRAINT [FK__exp_truck_filing_header__MappingStatus__mapping_status] FOREIGN KEY (mapping_status) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.exp_truck_filing_detail
ADD CONSTRAINT FK__exp_truck_filing_detail__exp_truck_inbound__inbound_id FOREIGN KEY (inbound_id) REFERENCES dbo.exp_truck_inbound (id)
GO

ALTER TABLE dbo.exp_truck_filing_detail
ADD CONSTRAINT FK__exp_truck_filing_detail__exp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_truck_filing_header (id)
GO

ALTER TABLE dbo.exp_truck_declaration
ADD CONSTRAINT FK__exp_truck_declaration__exp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.exp_truck_invoice_header
ADD CONSTRAINT FK__exp_truck_invoice_header__exp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.exp_truck_invoice_line
ADD CONSTRAINT FK__exp_truck_invoice_line__exp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_truck_filing_header (id)
GO

ALTER TABLE dbo.exp_truck_invoice_line
ADD CONSTRAINT FK__exp_truck_invoice_line__exp_truck_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.exp_truck_invoice_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.exp_truck_document
ADD CONSTRAINT [FK__exp_truck_document__exp_truck_inbound__inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.exp_truck_inbound (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.exp_truck_document
ADD CONSTRAINT FK__exp_truck_document__exp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.exp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.exp_truck_form_section_configuration
ADD CONSTRAINT FK_exp_truck_form_section_configuration__exp_truck_form_section_configuration__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.exp_truck_form_section_configuration (id)
GO

ALTER TABLE dbo.exp_truck_form_configuration
ADD CONSTRAINT FK__exp_truck_form_configuration__exp_truck_form_section_configuration__section_id FOREIGN KEY (section_id) REFERENCES dbo.exp_truck_form_section_configuration (id)
GO

PRINT ('add default value constraint')

DECLARE @command VARCHAR(MAX);
SELECT
  @command = COALESCE(@command, '') +
  CASE
    WHEN d.name IS NOT NULL THEN 'ALTER TABLE dbo.[' + section.table_name + '] DROP CONSTRAINT ' + d.name + ';' + CHAR(10)
    ELSE ''
  END
  + 'ALTER TABLE dbo.[' + section.table_name + '] ADD DEFAULT (' +
  CASE
    WHEN tp.name IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '''' + field.[value] + ''''
    ELSE field.[value]
  END
  + ') FOR [' + field.column_name + '];' + CHAR(10)
FROM exp_truck_form_configuration field
JOIN exp_truck_form_section_configuration section
  ON field.section_id = section.id
JOIN sys.tables t
  ON section.table_name = t.name
JOIN sys.columns c
  ON c.object_id = t.object_id
    AND c.name = field.column_name
JOIN sys.types tp
  ON tp.system_type_id = c.system_type_id
LEFT JOIN sys.default_constraints d
  ON d.parent_object_id = t.object_id
    AND d.parent_column_id = c.column_id
WHERE field.has_default_value = 1
ORDER BY section.table_name;

EXEC (@command)

PRINT ('add indexes')
GO
CREATE INDEX Idx__importer
ON dbo.exp_truck_inbound (importer)
ON [PRIMARY]
GO

CREATE INDEX Idx__exporter
ON dbo.exp_truck_inbound (exporter)
ON [PRIMARY]
GO

CREATE INDEX Idx__origin
ON dbo.exp_truck_inbound (origin)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_status
ON dbo.exp_truck_filing_header (filing_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__mapping_status
ON dbo.exp_truck_filing_header (mapping_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.exp_truck_filing_detail (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_id
ON dbo.exp_truck_filing_detail (inbound_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.exp_truck_declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.exp_truck_invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.exp_truck_invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON dbo.exp_truck_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.exp_truck_document (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_record_id
ON dbo.exp_truck_document (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__port
ON dbo.exp_truck_rule_port (port)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__consignee_code
ON dbo.exp_truck_rule_consignee (consignee_code)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__consignee_code__exporter
ON dbo.exp_truck_rule_exporter_consignee (consignee_code, exporter)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__name
ON dbo.exp_truck_form_section_configuration (name)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_id
ON dbo.exp_truck_form_section_configuration (parent_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__section_id
ON dbo.exp_truck_form_configuration (section_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets truck export invoice line number
CREATE FUNCTION dbo.fn_exp_truck_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      invoice_line.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY invoice_line.id)
    FROM dbo.exp_truck_invoice_line invoice_line
    WHERE invoice_line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice line table
ALTER TABLE dbo.exp_truck_invoice_line
ADD invoice_line_number AS (dbo.fn_exp_truck_invoice_line_number(parent_record_id, id));
GO
-- gets truck export invoice header number
CREATE FUNCTION dbo.fn_exp_truck_invoice_header_number (@filingHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.row_num
  FROM (SELECT
      invoice.id
     ,row_num = ROW_NUMBER() OVER (ORDER BY invoice.id)
    FROM dbo.exp_truck_invoice_header invoice
    WHERE invoice.filing_header_id = @filingHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.exp_truck_invoice_header
ADD invoice_number AS (dbo.fn_exp_truck_invoice_header_number(filing_header_id, id));
GO

-- gets unit of measure base on tariff
CREATE FUNCTION dbo.fn_app_unit_by_tariff (@tariff VARCHAR(35), @tarifftype VARCHAR(128))
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

PRINT ('create views')
GO

CREATE VIEW dbo.v_exp_truck_form_configuration
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
 ,CAST(form.has_default_value AS BIT) AS has_default_value
 ,CAST(form.editable AS BIT) AS editable
 ,CAST(form.mandatory AS BIT) AS mandatory
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.exp_truck_form_configuration form
INNER JOIN dbo.exp_truck_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id', 'broker_download_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL
GO

CREATE VIEW dbo.v_exp_truck_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN exp_truck_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id', 'broker_download_id')
GO

CREATE VIEW dbo.v_exp_truck_inbound_grid
AS
SELECT
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
WHERE inbound.deleted = 0
GO

CREATE VIEW dbo.v_exp_truck_report
AS
SELECT
  header.id
 ,detail.inbound_id AS TEI_ID
 ,declaration.main_supplier AS Declarationtab_Main_Supplier
 ,declaration.importer AS Declarationtab_Importer
 ,declaration.shpt_type AS Declarationtab_shpt_type
 ,declaration.transport AS Declarationtab_transport
 ,declaration.container AS Declarationtab_container
 ,declaration.tran_related AS Declarationtab_tran_related
 ,declaration.hazardous AS Declarationtab_hazardous
 ,declaration.routed_tran AS Declarationtab_routed_tran
 ,declaration.filing_option AS Declarationtab_filing_option
 ,declaration.tariff_type AS Declarationtab_TariffType
 ,declaration.sold_en_route AS Declarationtab_sold_en_route
 ,declaration.[service] AS Declarationtab_service
 ,declaration.master_bill AS Declarationtab_master_bill
 ,declaration.port_of_loading AS Declarationtab_port_of_loading
 ,declaration.dep AS Declarationtab_dep
 ,declaration.discharge AS Declarationtab_discharge
 ,declaration.export AS Declarationtab_export
 ,declaration.exp_date AS Declarationtab_exp_date
 ,declaration.house_bill AS Declarationtab_house_bill
 ,declaration.origin AS Declarationtab_origin
 ,declaration.destination AS Declarationtab_destination
 ,declaration.owner_ref AS Declarationtab_owner_ref
 ,declaration.transport_ref AS Declarationtab_transport_ref
 ,declaration.inbond_type AS Declarationtab_Inbond_type
 ,declaration.license_type AS Declarationtab_License_type
 ,declaration.license_number AS Declarationtab_License_number
 ,declaration.export_code AS Declarationtab_ExportCode
 ,declaration.eccn AS Declarationtab_Eccn
 ,declaration.country_of_dest AS Declarationtab_Country_of_dest
 ,declaration.state_of_origin AS Declarationtab_State_of_origin
 ,declaration.intermediate_consignee AS Declarationtab_Intermediate_consignee
 ,declaration.carrier AS Declarationtab_carrier
 ,declaration.forwader AS Declarationtab_forwader
 ,declaration.arr_date AS Declarationtab_arr_date
 ,declaration.check_local_client AS Declarationtab_check_local_client
 ,declaration.country_of_export AS Declarationtab_Country_of_export

 ,invoice.usppi AS Invheaderstab_Usppi
 ,invoice.usppi_address AS Invheaderstab_usppi_address
 ,invoice.usppi_contact AS Invheaderstab_usppi_contact
 ,invoice.usppi_phone AS Invheaderstab_usppi_phone
 ,invoice.origin_indicator AS Invheaderstab_origin_indicator
 ,invoice.ultimate_consignee AS Invheaderstab_ultimate_consignee
 ,invoice.ultimate_consignee_type AS Invheaderstab_ultimate_consignee_type
 ,invoice.invoice_number AS Invheaderstab_invoice_number
 ,invoice.invoice_total_amount AS Invheaderstab_invoice_total_amount
 ,invoice.invoice_total_amount_currency AS Invheaderstab_invoice_total_amount_currency
 ,invoice.ex_rate_date AS Invheaderstab_ex_rate_date
 ,invoice.exchange_rate AS Invheaderstab_exchange_rate
 ,invoice.invoice_inco_term AS Invheaderstab_invoice_inco_term

 ,invoice_line.invoice_line_number AS Invlinestab_lno
 ,invoice_line.tariff AS Invlinestab_tariff
 ,invoice_line.customs_qty AS Invlinestab_customs_qty
 ,invoice_line.export_code AS Invlinestab_export_code
 ,invoice_line.goods_description AS Invlinestab_goods_description
 ,invoice_line.customs_qty_unit AS Invlinestab_customs_qty_unit
 ,invoice_line.second_qty AS Invlinestab_second_qty
 ,invoice_line.price AS Invlinestab_price
 ,invoice_line.price_currency AS Invlinestab_price_currency
 ,invoice_line.gross_weight AS Invlinestab_gross_weight
 ,invoice_line.gross_weight_unit AS Invlinestab_gross_weight_unit
 ,invoice_line.license_value AS Invlinestab_license_value
 ,invoice_line.unit_price AS Invlinestab_unit_price
 ,invoice_line.tariff_type AS Invlinestab_tariff_type
 ,invoice_line.goods_origin AS Invlinestab_goods_origin

FROM dbo.exp_truck_filing_header header
JOIN dbo.exp_truck_filing_detail detail
  ON header.id = detail.filing_header_id
LEFT JOIN dbo.exp_truck_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.exp_truck_invoice_line invoice_line
  ON invoice_line.parent_record_id = invoice.id

WHERE header.mapping_status = 2
GO

PRINT ('create stored procedures')
GO
-- review mapped data
CREATE PROCEDURE dbo.sp_exp_truck_review_entry (@filingHeaderIds VARCHAR(MAX)
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
   ,value VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.exp_truck_form_section_configuration rs
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
     ,section.name AS section_name
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
     ,defValue.manual
     ,defValue.description
     ,defValue.label
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.exp_truck_form_configuration defValue
    INNER JOIN dbo.exp_truck_form_section_configuration section
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

-- add truck export declaration record --
CREATE PROCEDURE dbo.sp_exp_truck_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_truck_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_truck_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO exp_truck_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
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
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_consignee.destination
       ,rule_consignee.country
       ,trule_exporter.tran_related
       ,inbound.routed_tran
       ,inbound.eccn
       ,inbound.export
       ,inbound.importer
       ,inbound.tariff_type
       ,inbound.sold_en_route
       ,inbound.master_bill
       ,inbound.master_bill
       ,inbound.origin
       ,inbound.master_bill
       ,inbound.exporter
       ,inbound.export_date
       ,inbound.export_date
       ,inbound.hazardous
       ,rule_port.origin_code
       ,rule_port.state_of_origin
      FROM exp_truck_filing_detail detail
      JOIN exp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN exp_truck_rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN exp_truck_rule_exporter_consignee trule_exporter
        ON inbound.importer = trule_exporter.consignee_code
          AND inbound.exporter = trule_exporter.exporter
      LEFT JOIN exp_truck_rule_port rule_port
        ON (LTRIM(inbound.origin) = LTRIM(rule_port.port)) -- todo: think about removing functions
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add truck export invoice line record --
CREATE PROCEDURE dbo.sp_exp_truck_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_truck_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_truck_invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO exp_truck_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,tariff
       ,customs_qty
       ,price
       ,gross_weight
       ,gross_weight_unit
       ,goods_description
       ,tariff_type
       ,invoice_qty_unit
       ,goods_origin)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbound.tariff
       ,inbound.customs_qty
       ,inbound.price
       ,inbound.gross_weight
       ,inbound.gross_weight_uom
       ,inbound.goods_description
       ,inbound.tariff_type
       ,dbo.fn_app_unit_by_tariff(inbound.tariff, inbound.tariff_type) AS invoice_qty_unit
       ,inbound.goods_origin
      FROM exp_truck_filing_detail detail
      JOIN exp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add rail invoice header record --
CREATE PROCEDURE dbo.sp_exp_truck_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'exp_truck_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM exp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.exp_truck_invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO exp_truck_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,usppi_address
       ,usppi_contact
       ,usppi_phone
       ,ultimate_consignee_type
       ,usppi
       ,invoice_total_amount
       ,ultimate_consignee
       ,origin_indicator)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_exporter.address
       ,rule_exporter.contact
       ,rule_exporter.phone
       ,rule_consignee.ultimate_consignee_type
       ,inbound.exporter
       ,inbound.price
       ,inbound.importer
       ,inbound.origin_indicator
      FROM exp_truck_filing_detail detail
      JOIN exp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN exp_truck_rule_consignee rule_consignee
        ON inbound.importer = rule_consignee.consignee_code
      LEFT JOIN exp_truck_rule_exporter_consignee rule_exporter
        ON inbound.importer = rule_exporter.consignee_code
          AND inbound.exporter = rule_exporter.exporter
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
    EXEC dbo.sp_exp_truck_add_invoice_line @filingHeaderId
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
CREATE PROCEDURE dbo.sp_exp_truck_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_exp_truck_add_declaration @filingHeaderId
                                       ,@filingHeaderId
                                       ,@filingUser
                                       ,@operationId
  -- add invoice header
  EXEC dbo.sp_exp_truck_add_invoice_header @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
                                          ,@operationId
END;
GO
-- delete filing entry
CREATE PROCEDURE dbo.sp_exp_truck_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.exp_truck_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.exp_truck_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.exp_truck_form_section_configuration ps
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
-- soft delete inbound record
CREATE PROCEDURE dbo.sp_exp_truck_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_exp_truck_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE exp_truck_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE exp_truck_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM exp_truck_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- update rail filing entry
CREATE PROCEDURE dbo.sp_exp_truck_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,value VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
   ,row_num INT NOT NULL
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (
      id
     ,record_id
     ,value
     ,table_name
     ,column_name
    ,row_num)
    SELECT
      field.id
     ,field.record_id
     ,field.value
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN dbo.exp_truck_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.exp_truck_form_section_configuration section
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
-- recalculate rail fileds
CREATE PROCEDURE dbo.sp_exp_truck_recalculate
AS
  RETURN;
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
 ,truck_export_doc.[file_name] AS [filename]
 ,truck_export_doc.file_extension file_extension
 ,truck_export_doc.file_content AS file_Content
 ,truck_export_doc.file_description AS file_desc
 ,truck_export_doc.document_type AS document_type
 ,'Truck_Export' AS transport_shipment_type
FROM exp_truck_document truck_export_doc
INNER JOIN exp_truck_filing_header truck_export_header
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
ALTER TABLE dbo.truck_export_invoice_headers
DROP COLUMN invoice_number
DROP FUNCTION dbo.truck_export_invoice_header_number
ALTER TABLE dbo.truck_export_invoice_lines
DROP COLUMN invoice_line_number
DROP FUNCTION dbo.truck_export_invoice_line_number

PRINT ('drop tables')
GO
DROP TABLE dbo.truck_export_declarations
DROP TABLE dbo.truck_export_invoice_lines
DROP TABLE dbo.truck_export_invoice_headers
DROP TABLE dbo.truck_export_filing_details
DROP TABLE dbo.truck_exports
DROP TABLE dbo.Truck_Export_Rule_Port
DROP TABLE dbo.Truck_Export_Rule_Consignee
DROP TABLE dbo.Truck_Export_Rule_Exporter_Consignee
DROP TABLE dbo.truck_export_def_values
DROP TABLE dbo.truck_export_sections
DROP TABLE dbo.truck_export_def_values_manual

PRINT ('drop views')
GO
DROP VIEW dbo.v_truck_export_def_values_manual
DROP VIEW dbo.v_truck_export_tables
DROP VIEW dbo.truck_export_grid
DROP VIEW dbo.Truck_Export_Report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.truck_export_filing
DROP PROCEDURE dbo.truck_export_add_declaration_record
DROP PROCEDURE dbo.truck_export_add_invoice_header_record
DROP PROCEDURE dbo.truck_export_add_invoice_line_record
DROP PROCEDURE dbo.truck_export_add_def_values_manual
DROP PROCEDURE dbo.truck_export_apply_def_values
DROP PROCEDURE dbo.truck_export_filing_del
DROP PROCEDURE dbo.truck_export_del
DROP PROCEDURE dbo.truck_export_delete_record
DROP PROCEDURE dbo.truck_export_filing_param