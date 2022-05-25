PRINT ('create tables')
GO

CREATE TABLE dbo.Vessel_Imports (
  id INT IDENTITY
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,deleted BIT NOT NULL DEFAULT (0)
 ,eta DATETIME NOT NULL
 ,container VARCHAR(128) NOT NULL DEFAULT ('BLK')
 ,entry_type VARCHAR(128) NOT NULL DEFAULT ('01')
 ,importer_id UNIQUEIDENTIFIER NOT NULL DEFAULT ('00000000-0000-0000-0000-000000000000')
 ,state_id INT NULL
 ,classification_id INT NOT NULL DEFAULT (0)
 ,user_id VARCHAR(128) NOT NULL
 ,supplier_id UNIQUEIDENTIFIER NULL
 ,vessel_id INT NOT NULL DEFAULT (0)
 ,product_description_id INT NOT NULL DEFAULT (0)
 ,customs_qty DECIMAL(18, 6) NOT NULL DEFAULT (0)
 ,unit_price DECIMAL(18, 6) NOT NULL DEFAULT (0)
 ,country_of_origin_id INT NULL
 ,owner_ref VARCHAR(128) NOT NULL DEFAULT ('')
 ,firms_code_id INT NOT NULL DEFAULT (0)
 ,CONSTRAINT [PK_dbo.Vessel_Imports] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Filing_Headers (
  id INT IDENTITY
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,error_description VARCHAR(MAX) NULL
 ,filing_number VARCHAR(255) NULL
 ,mapping_status TINYINT NULL
 ,filing_status TINYINT NULL
 ,job_link VARCHAR(1024) NULL
 ,request_xml VARCHAR(MAX) NULL
 ,response_xml VARCHAR(MAX) NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Filing_Headers] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Filing_Details (
  Filing_Headers_FK INT NOT NULL
 ,VI_FK INT NOT NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Filing_Details] PRIMARY KEY CLUSTERED (Filing_Headers_FK, VI_FK)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Declarations (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL DEFAULT ('Imp')
 ,transport VARCHAR(10) NULL DEFAULT ('SEA')
 ,container VARCHAR(10) NULL
 ,ent_type VARCHAR(10) NULL
 ,rlf VARCHAR(128) NULL DEFAULT ('RLF')
 ,message VARCHAR(128) NULL DEFAULT ('ACE')
 ,enable_ent_sum VARCHAR(10) NULL DEFAULT ('Y')
 ,enable_cargo_rel VARCHAR(10) NULL
 ,type VARCHAR(128) NULL DEFAULT ('ACE')
 ,certify_cargo_release VARCHAR(128) NULL DEFAULT ('Y')
 ,service VARCHAR(128) NULL DEFAULT ('STD')
 ,issuer VARCHAR(128) NULL
 ,ocean_bill VARCHAR(128) NULL
 ,vessel VARCHAR(128) NULL
 ,voyage VARCHAR(128) NULL DEFAULT ('000')
 ,carrier_scac VARCHAR(128) NULL
 ,port_of_discharge VARCHAR(128) NULL
 ,port_of_loading VARCHAR(128) NULL
 ,entry_port VARCHAR(128) NULL
 ,dep DATE NULL
 ,arr DATE NULL
 ,loading_unloco VARCHAR(128) NULL
 ,discharage_unloco VARCHAR(128) NULL
 ,hmf VARCHAR(10) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,etd DATE NULL
 ,eta DATE NULL
 ,dest_state VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,export_date DATE NULL
 ,description VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL DEFAULT ('FOB')
 ,firms_code VARCHAR(128) NULL
 ,entry_number VARCHAR(128) NULL
 ,purchased VARCHAR(10) NULL DEFAULT ('Y')
 ,check_local_client VARCHAR(10) NULL DEFAULT ('OK')
 ,CONSTRAINT PK_vessel_import_declaration_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Invoice_Headers (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,invoice_number VARCHAR(128) NULL DEFAULT ('1')
 ,supplier VARCHAR(128) NULL
 ,supplier_address VARCHAR(128) NULL
 ,inco VARCHAR(10) NULL DEFAULT ('FOB')
 ,invoice_total_currency VARCHAR(10) NULL DEFAULT ('USD')
 ,line_total NUMERIC(18, 2) NULL
 ,country_of_origin VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,consignee VARCHAR(128) NULL
 ,consignee_address VARCHAR(128) NULL
 ,export_date DATE NULL
 ,transaction_related VARCHAR(3) NULL
 ,seller VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,ship_to_party VARCHAR(128) NULL
 ,broker_pga_contact_name VARCHAR(128) NULL DEFAULT ('Alessandra Mediago')
 ,broker_pga_contact_phone VARCHAR(128) NULL DEFAULT ('212-363-9300')
 ,importer VARCHAR(128) NULL
 ,manufacturer VARCHAR(128) NULL
 ,manufacturer_id VARCHAR(128) NULL
 ,broker_pga_contact_email VARCHAR(128) NULL DEFAULT ('Amediago@CharterBrokerage.net')
 ,invoice_total NUMERIC(18, 6) NULL
 ,CONSTRAINT PK_vessel_import_invoice_header_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Invoice_Lines (
  id INT IDENTITY
 ,invoice_header_id INT NOT NULL
 ,entry_no VARCHAR(128) NULL
 ,product VARCHAR(128) NULL
 ,classification VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,customs_qty NUMERIC(19, 5) NULL
 ,customs_qty_unit VARCHAR(128) NULL
 ,goods_description VARCHAR(512) NULL
 ,spi VARCHAR(128) NULL DEFAULT ('N/A')
 ,country_of_origin VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,destination_state VARCHAR(10) NULL
 ,manufacturer VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,seller VARCHAR(128) NULL
 ,seller_address VARCHAR(128) NULL
 ,gross_weight NUMERIC(18, 6) NULL DEFAULT (1)
 ,price NUMERIC(18, 2) NULL
 ,prod_id VARCHAR(128) NULL
 ,attribute1 VARCHAR(128) NULL
 ,attribute2 VARCHAR(128) NULL
 ,attribute3 VARCHAR(128) NULL
 ,transaction_related VARCHAR(10) NULL
 ,invoice_qty NUMERIC(19, 5) NULL
 ,code VARCHAR(128) NULL DEFAULT ('OFT')
 ,amount INT NULL DEFAULT (2)
 ,epa_tsca_toxic_substance_control_act_indicator VARCHAR(128) NULL
 ,tsca_indicator VARCHAR(128) NULL
 ,certifying_individual VARCHAR(128) NULL
 ,mid VARCHAR(128) NULL
 ,hmf VARCHAR(128) NULL
 ,unit_price NUMERIC(18, 6) NULL
 ,invoice_quantity_unit VARCHAR(128) NULL
 ,gross_weight_unit VARCHAR(128) NULL DEFAULT ('KG')
 ,CONSTRAINT PK_vessel_import_invoice_lines_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Miscs (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,branch VARCHAR(10) NULL
 ,broker VARCHAR(10) NULL
 ,merge_by VARCHAR(10) NULL DEFAULT ('NON')
 ,preparer_dist_port VARCHAR(128) NULL
 ,recon_issue VARCHAR(10) NULL DEFAULT ('N/A')
 ,payment_type VARCHAR(10) NULL
 ,fta_recon VARCHAR(10) NULL
 ,broker_to_pay VARCHAR(10) NULL
 ,inbond_type VARCHAR(10) NULL DEFAULT ('8')
 ,CONSTRAINT PK_vessel_import_miscs_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Packings (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,manifest_qty INT NULL DEFAULT (1)
 ,uq VARCHAR(128) NULL DEFAULT ('VL')
 ,bill_type VARCHAR(10) NULL DEFAULT ('MB')
 ,bill_issuer_scac VARCHAR(128) NULL
 ,bill_num VARCHAR(128) NULL
 ,CONSTRAINT PK_vessel_import_packing_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Documents (
  id INT IDENTITY
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NOT NULL
 ,document_type VARCHAR(128) NULL
 ,content VARBINARY(MAX) NULL
 ,description VARCHAR(1000) NULL
 ,extension VARCHAR(128) NOT NULL
 ,file_name VARCHAR(255) NOT NULL
 ,filing_header_id INT NULL
 ,Status VARCHAR(50) NULL
 ,inbound_record_id INT NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Documents] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Rule_Importer (
  id INT IDENTITY
 ,ior VARCHAR(128) NOT NULL
 ,cw_ior VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,CONSTRAINT [PK_dbo.Vessel_Rule_Importer] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Rule_Port (
  id INT IDENTITY
 ,discharge_name VARCHAR(128) NULL
 ,entry_port VARCHAR(4) NULL
 ,hmf VARCHAR(1) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,discharge_port VARCHAR(4) NULL
 ,firms_code_id INT NOT NULL DEFAULT (0)
 ,destination_code VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Vessel_Rule_Port] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Rule_Product (
  id INT IDENTITY
 ,tariff VARCHAR(10) NOT NULL
 ,goods_description VARCHAR(128) NULL
 ,customs_attribute1 VARCHAR(128) NULL
 ,customs_attribute2 VARCHAR(128) NULL
 ,invoice_uq VARCHAR(128) NULL
 ,tsca_requirement VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,CONSTRAINT [PK_dbo.Vessel_Rule_Product] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Sections (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,procedure_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Sections] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Def_Values (
  id INT IDENTITY
 ,section_id INT NOT NULL
 ,editable BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NOT NULL
 ,default_value VARCHAR(512) NULL
 ,display_on_ui TINYINT NOT NULL
 ,manual TINYINT NOT NULL
 ,single_filing_order TINYINT NULL
 ,description VARCHAR(128) NULL
 ,label VARCHAR(128) NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Def_Values] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_Import_Def_Values_Manual (
  id INT IDENTITY
 ,editable BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,filing_header_id INT NOT NULL
 ,section_title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,modification_date DATETIME NOT NULL
 ,label VARCHAR(128) NOT NULL
 ,value VARCHAR(512) NULL
 ,display_on_ui TINYINT NOT NULL
 ,manual TINYINT NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,parent_record_id INT NOT NULL
 ,section_name VARCHAR(128) NOT NULL
 ,record_id INT NOT NULL
 ,description VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Vessel_Import_Def_Values_Manual] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_ProductDescriptions (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,tariff_id INT NOT NULL
 ,CONSTRAINT [PK_dbo.Vessel_ProductDescriptions] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Vessel_DischargeTerminals (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,state_id INT NOT NULL
 ,CONSTRAINT [PK_dbo.Vessel_DischargeTerminals] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy data')
GO
SET IDENTITY_INSERT dbo.Vessel_Imports ON;
INSERT INTO dbo.Vessel_Imports (
    id
   ,eta
   ,container
   ,entry_type
   ,importer_id
   ,state_id
   ,classification_id
   ,[user_id]
   ,supplier_id
   ,vessel_id
   ,product_description_id
   ,customs_qty
   ,unit_price
   ,country_of_origin_id
   ,owner_ref
   ,firms_code_id
   ,[deleted]
   ,created_date
   ,created_user)
  SELECT
    inbnd.id
   ,inbnd.eta
   ,inbnd.container
   ,inbnd.entry_type
   ,inbnd.importer_id
   ,inbnd.state_id
   ,inbnd.classification_id
   ,inbnd.[user_id]
   ,inbnd.supplier_id
   ,inbnd.vessel_id
   ,inbnd.product_description_id
   ,inbnd.customs_qty
   ,inbnd.unit_price
   ,inbnd.country_of_origin_id
   ,inbnd.owner_ref
   ,inbnd.firms_code_id
   ,inbnd.[deleted]
   ,inbnd.created_date
   ,inbnd.created_user
  FROM dbo.imp_vessel_inbound inbnd;
SET IDENTITY_INSERT dbo.Vessel_Imports OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Filing_Headers ON;
INSERT INTO dbo.Vessel_Import_Filing_Headers (
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
  FROM dbo.imp_vessel_filing_header fh;
SET IDENTITY_INSERT dbo.Vessel_Import_Filing_Headers OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.Vessel_Import_Filing_Details (
    VI_FK
   ,Filing_Headers_FK)
  SELECT
    fd.inbound_id
   ,fd.filing_header_id
  FROM dbo.imp_vessel_filing_detail fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Declarations ON;
INSERT INTO dbo.Vessel_Import_Declarations (
    id
   ,filing_header_id
   ,main_supplier
   ,importer
   ,shpt_type
   ,transport
   ,container
   ,ent_type
   ,rlf
   ,[message]
   ,enable_ent_sum
   ,enable_cargo_rel
   ,[type]
   ,certify_cargo_release
   ,[service]
   ,issuer
   ,ocean_bill
   ,vessel
   ,voyage
   ,carrier_scac
   ,port_of_discharge
   ,port_of_loading
   ,entry_port
   ,dep
   ,arr
   ,loading_unloco
   ,discharage_unloco
   ,hmf
   ,origin
   ,destination
   ,etd
   ,eta
   ,dest_state
   ,country_of_export
   ,export_date
   ,[description]
   ,owner_ref
   ,inco
   ,firms_code
   ,entry_number
   ,purchased
   ,check_local_client)
  SELECT
    declaration.id
   ,declaration.filing_header_id
   ,declaration.main_supplier
   ,declaration.importer
   ,declaration.shpt_type
   ,declaration.transport
   ,declaration.container
   ,declaration.ent_type
   ,declaration.rlf
   ,[declaration].[message]
   ,declaration.enable_ent_sum
   ,declaration.enable_cargo_rel
   ,[declaration].[type]
   ,declaration.certify_cargo_release
   ,[declaration].[service]
   ,declaration.issuer
   ,declaration.ocean_bill
   ,declaration.vessel
   ,declaration.voyage
   ,declaration.carrier_scac
   ,declaration.port_of_discharge
   ,declaration.port_of_loading
   ,declaration.entry_port
   ,declaration.dep
   ,declaration.arr
   ,declaration.loading_unloco
   ,declaration.discharage_unloco
   ,declaration.hmf
   ,declaration.origin
   ,declaration.destination
   ,declaration.etd
   ,declaration.eta
   ,declaration.dest_state
   ,declaration.country_of_export
   ,declaration.export_date
   ,[declaration].[description]
   ,declaration.owner_ref
   ,declaration.inco
   ,declaration.firms_code
   ,declaration.entry_number
   ,declaration.purchased
   ,declaration.check_local_client
  FROM dbo.imp_vessel_declaration declaration;
SET IDENTITY_INSERT dbo.Vessel_Import_Declarations OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Invoice_Headers ON;
INSERT INTO dbo.Vessel_Import_Invoice_Headers (
    id
   ,filing_header_id
   ,invoice_number
   ,supplier
   ,supplier_address
   ,inco
   ,invoice_total_currency
   ,line_total
   ,country_of_origin
   ,country_of_export
   ,consignee
   ,consignee_address
   ,export_date
   ,transaction_related
   ,seller
   ,sold_to_party
   ,ship_to_party
   ,broker_pga_contact_name
   ,broker_pga_contact_phone
   ,broker_pga_contact_email
   ,importer
   ,manufacturer
   ,manufacturer_id
   ,invoice_total)
  SELECT
    invoice.id
   ,invoice.filing_header_id
   ,invoice.invoice_number
   ,invoice.supplier
   ,invoice.supplier_address
   ,invoice.inco
   ,invoice.invoice_total_currency
   ,invoice.line_total
   ,invoice.country_of_origin
   ,invoice.country_of_export
   ,invoice.consignee
   ,invoice.consignee_address
   ,invoice.export_date
   ,invoice.transaction_related
   ,invoice.seller
   ,invoice.sold_to_party
   ,invoice.ship_to_party
   ,invoice.broker_pga_contact_name
   ,invoice.broker_pga_contact_phone
   ,invoice.broker_pga_contact_email
   ,invoice.importer
   ,invoice.manufacturer
   ,invoice.manufacturer_id
   ,invoice.invoice_total
  FROM dbo.imp_vessel_invoice_header invoice;
SET IDENTITY_INSERT dbo.Vessel_Import_Invoice_Headers OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Invoice_Lines ON;
INSERT INTO dbo.Vessel_Import_Invoice_Lines (
    id
   ,invoice_header_id
   ,entry_no
   ,product
   ,classification
   ,tariff
   ,customs_qty
   ,customs_qty_unit
   ,goods_description
   ,spi
   ,country_of_origin
   ,country_of_export
   ,destination_state
   ,manufacturer
   ,consignee
   ,sold_to_party
   ,seller
   ,seller_address
   ,gross_weight
   ,price
   ,prod_id
   ,attribute1
   ,attribute2
   ,attribute3
   ,transaction_related
   ,invoice_qty
   ,code
   ,amount
   ,epa_tsca_toxic_substance_control_act_indicator
   ,tsca_indicator
   ,certifying_individual
   ,mid
   ,hmf
   ,unit_price
   ,invoice_quantity_unit
   ,gross_weight_unit)
  SELECT
    line.id
   ,line.parent_record_id
   ,line.entry_no
   ,line.product
   ,line.classification
   ,line.tariff
   ,line.customs_qty
   ,line.customs_qty_unit
   ,line.goods_description
   ,line.spi
   ,line.country_of_origin
   ,line.country_of_export
   ,line.destination_state
   ,line.manufacturer
   ,line.consignee
   ,line.sold_to_party
   ,line.seller
   ,line.seller_address
   ,line.gross_weight
   ,line.price
   ,line.prod_id
   ,line.attribute1
   ,line.attribute2
   ,line.attribute3
   ,line.transaction_related
   ,line.invoice_qty
   ,line.code
   ,line.amount
   ,line.epa_tsca_toxic_substance_control_act_indicator
   ,line.tsca_indicator
   ,line.certifying_individual
   ,line.mid
   ,line.hmf
   ,line.unit_price
   ,line.invoice_quantity_unit
   ,line.gross_weight_unit
  FROM dbo.imp_vessel_invoice_line line  
SET IDENTITY_INSERT dbo.Vessel_Import_Invoice_Lines OFF;
GO

PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Miscs ON;
INSERT INTO dbo.Vessel_Import_Miscs (
    id
   ,filing_header_id
   ,branch
   ,[broker]
   ,merge_by
   ,preparer_dist_port
   ,recon_issue
   ,payment_type
   ,fta_recon
   ,broker_to_pay
   ,inbond_type)
  SELECT
    misc.id
   ,misc.filing_header_id
   ,misc.branch
   ,[misc].[broker]
   ,misc.merge_by
   ,misc.preparer_dist_port
   ,misc.recon_issue
   ,misc.payment_type
   ,misc.fta_recon
   ,misc.broker_to_pay
   ,misc.inbond_type
  FROM dbo.imp_vessel_misc misc
SET IDENTITY_INSERT dbo.Vessel_Import_Miscs OFF;
GO

PRINT ('copy packing data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Packings ON;
INSERT INTO dbo.Vessel_Import_Packings (
    id
   ,filing_header_id
   ,manifest_qty
   ,uq
   ,bill_type
   ,bill_issuer_scac
   ,bill_num)
  SELECT
    pack.id
   ,pack.filing_header_id
   ,pack.manifest_qty
   ,pack.uq
   ,pack.bill_type
   ,pack.bill_issuer_scac
   ,pack.bill_num
  FROM dbo.imp_vessel_packing pack
SET IDENTITY_INSERT dbo.Vessel_Import_Packings OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Documents ON;
INSERT INTO dbo.Vessel_Import_Documents (
    id
   ,filing_header_id
   ,inbound_record_id
   ,[file_name]
   ,[extension]
   ,[description]
   ,[content]
   ,document_type
   ,[Status]
   ,created_date
   ,created_user)
  SELECT
    doc.id
   ,doc.filing_header_id
   ,doc.inbound_record_id
   ,doc.[file_name]
   ,doc.file_extension
   ,doc.file_description
   ,doc.file_content
   ,doc.document_type
   ,doc.[status]
   ,doc.created_date
   ,doc.created_user
  FROM dbo.imp_vessel_document doc;
SET IDENTITY_INSERT dbo.Vessel_Import_Documents OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.Vessel_Rule_Importer ON;
INSERT INTO dbo.Vessel_Rule_Importer (
    id
   ,ior
   ,cw_ior
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.ior
   ,r.cw_ior
   ,r.created_date
   ,r.created_user
  FROM dbo.imp_vessel_rule_importer r;
SET IDENTITY_INSERT dbo.Vessel_Rule_Importer OFF;
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.Vessel_Rule_Port ON;
INSERT INTO dbo.Vessel_Rule_Port (
    id
   ,discharge_name
   ,entry_port
   ,hmf
   ,discharge_port
   ,firms_code_id
   ,destination_code
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.discharge_name
   ,r.entry_port
   ,r.hmf
   ,r.discharge_port
   ,r.firms_code_id
   ,r.destination_code
   ,r.created_date
   ,r.created_user
  FROM dbo.imp_vessel_rule_port r;
SET IDENTITY_INSERT dbo.Vessel_Rule_Port OFF;
GO

PRINT ('copy product rule data')
GO
SET IDENTITY_INSERT dbo.Vessel_Rule_Product ON;
INSERT INTO dbo.Vessel_Rule_Product (
    id
   ,tariff
   ,goods_description
   ,customs_attribute1
   ,customs_attribute2
   ,invoice_uq
   ,tsca_requirement
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.tariff
   ,r.goods_description
   ,r.customs_attribute1
   ,r.customs_attribute2
   ,r.invoice_uq
   ,r.tsca_requirement
   ,r.created_date
   ,r.created_user
  FROM dbo.imp_vessel_rule_product r;
SET IDENTITY_INSERT dbo.Vessel_Rule_Product OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.vessel_import_sections ON;
INSERT INTO dbo.vessel_import_sections (
    id
   ,[name]
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,[procedure_name])
  SELECT
    section.id
   ,section.[name]
   ,section.title
   ,section.table_name
   ,section.is_array
   ,section.parent_id
   ,section.[procedure_name]
  FROM dbo.imp_vessel_form_section_configuration section;
SET IDENTITY_INSERT dbo.vessel_import_sections OFF;
GO

UPDATE dbo.vessel_import_sections
SET table_name = 'Vessel_Import_Declarations'
   ,procedure_name = 'vessel_import_add_declaration_record'
WHERE table_name = 'imp_vessel_declaration';
UPDATE dbo.vessel_import_sections
SET table_name = 'Vessel_Import_Invoice_Headers'
   ,procedure_name = 'vessel_import_add_invoice_header_record'
WHERE table_name = 'imp_vessel_invoice_header';
UPDATE dbo.vessel_import_sections
SET table_name = 'Vessel_Import_Invoice_Lines'
   ,procedure_name = 'vessel_import_add_invoice_line_record'
WHERE table_name = 'imp_vessel_invoice_line';
UPDATE dbo.vessel_import_sections
SET table_name = 'Vessel_Import_Packings'
   ,procedure_name = 'vessel_import_add_packing_record'
WHERE table_name = 'imp_vessel_packing';
UPDATE dbo.vessel_import_sections
SET table_name = 'Vessel_Import_Miscs'
   ,procedure_name = 'vessel_import_add_misc_record'
WHERE table_name = 'imp_vessel_misc';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.Vessel_Import_Def_Values ON;
INSERT INTO dbo.Vessel_Import_Def_Values (
    id
   ,section_id
   ,column_name
   ,[label]
   ,[description]
   ,[default_value]
   ,has_default_value
   ,editable
   ,mandatory
   ,display_on_ui
   ,[manual]
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
   ,dv.[label]
   ,dv.[description]
   ,dv.[value]
   ,dv.has_default_value
   ,dv.editable
   ,dv.mandatory
   ,dv.display_on_ui
   ,dv.[manual]
   ,dv.single_filing_order
   ,dv.paired_field_table
   ,dv.paired_field_column
   ,dv.handbook_name
   ,dv.created_date
   ,dv.created_user
  FROM dbo.imp_vessel_form_configuration dv;
SET IDENTITY_INSERT dbo.Vessel_Import_Def_Values OFF;
GO

PRINT ('copy tariff product description handbook data')
GO
SET IDENTITY_INSERT dbo.Vessel_ProductDescriptions ON;
INSERT INTO dbo.Vessel_ProductDescriptions (
    id
   ,tariff_id
   ,[name])
  SELECT
    vpd.id
   ,vpd.tariff_id
   ,vpd.[name]
  FROM dbo.handbook_tariff_product_description vpd;
SET IDENTITY_INSERT dbo.Vessel_ProductDescriptions OFF;
GO


PRINT ('add constraints')
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.app_users_user_id] FOREIGN KEY (user_id) REFERENCES dbo.app_users (UserAccount)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Clients_importer_id] FOREIGN KEY (importer_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Clients_supplier_id] FOREIGN KEY (supplier_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Countries_country_of_origin_id] FOREIGN KEY (country_of_origin_id) REFERENCES dbo.Countries (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.cw_firms_codes_firms_code_id] FOREIGN KEY (firms_code_id) REFERENCES dbo.cw_firms_codes (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Tariff_classification_id] FOREIGN KEY (classification_id) REFERENCES dbo.Tariff (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.US_States_state_id] FOREIGN KEY (state_id) REFERENCES dbo.US_States (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Vessel_ProductDescriptions_product_description_id] FOREIGN KEY (product_description_id) REFERENCES dbo.Vessel_ProductDescriptions (id)
GO

ALTER TABLE dbo.Vessel_Imports
ADD CONSTRAINT [FK_dbo.Vessel_Imports_dbo.Vessels_vessel_id] FOREIGN KEY (vessel_id) REFERENCES dbo.Vessels (id)
GO

ALTER TABLE dbo.Vessel_Import_Filing_Details
ADD CONSTRAINT [FK_dbo.Vessel_Import_Filing_Details_dbo.Vessel_Import_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Vessel_Import_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Filing_Details
ADD CONSTRAINT [FK_dbo.Vessel_Import_Filing_Details_dbo.Vessel_Imports_VI_FK] FOREIGN KEY (VI_FK) REFERENCES dbo.Vessel_Imports (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Declarations
ADD CONSTRAINT FK__vessel_import_declarations__vessel_import_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Import_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Invoice_Headers
ADD CONSTRAINT FK__vessel_import_invoice_headers__vessel_import_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Import_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Invoice_Lines
ADD CONSTRAINT FK__vessel_import_invoice_lines__vessel_import_invoice_headers__filing_header_id FOREIGN KEY (invoice_header_id) REFERENCES dbo.Vessel_Import_Invoice_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Miscs
ADD CONSTRAINT FK__vessel_import_miscs__vessel_import_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Import_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Packings
ADD CONSTRAINT FK__vessel_import_packings__vessel_import_filing_headers__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Import_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Vessel_Import_Documents
ADD CONSTRAINT [FK_dbo.Vessel_Import_Documents_dbo.Vessel_Import_Filing_Headers_filing_header_id] FOREIGN KEY (filing_header_id) REFERENCES dbo.Vessel_Import_Filing_Headers (id)
GO

ALTER TABLE dbo.Vessel_Import_Documents
ADD CONSTRAINT [FK_dbo.Vessel_Import_Documents_dbo.Vessel_Imports_inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.Vessel_Imports (id)
GO

ALTER TABLE dbo.Vessel_Rule_Port
ADD CONSTRAINT [FK_dbo.Vessel_Rule_Port_dbo.cw_firms_codes_firms_code_id] FOREIGN KEY (firms_code_id) REFERENCES dbo.cw_firms_codes (id)
GO

ALTER TABLE dbo.Vessel_Import_Sections
ADD CONSTRAINT [FK_dbo.Vessel_Import_Sections_dbo.Vessel_Import_Sections_parent_id] FOREIGN KEY (parent_id) REFERENCES dbo.Vessel_Import_Sections (id)
GO

ALTER TABLE dbo.Vessel_Import_Def_Values
ADD CONSTRAINT [FK_dbo.Vessel_Import_Def_Values_dbo.Vessel_Import_Sections_section_id] FOREIGN KEY (section_id) REFERENCES dbo.Vessel_Import_Sections (id)
GO

ALTER TABLE dbo.Vessel_ProductDescriptions
ADD CONSTRAINT [FK_dbo.Vessel_ProductDescriptions_dbo.Tariff_tariff_id] FOREIGN KEY (tariff_id) REFERENCES dbo.Tariff (id)
GO

ALTER TABLE dbo.Vessel_DischargeTerminals
ADD CONSTRAINT [FK_dbo.Vessel_DischargeTerminals_dbo.US_States_state_id] FOREIGN KEY (state_id) REFERENCES dbo.US_States (id)
GO

PRINT ('add indexes')
GO

CREATE INDEX IX_classification_id
ON dbo.Vessel_Imports (classification_id)
ON [PRIMARY]
GO

CREATE INDEX IX_country_of_origin_id
ON dbo.Vessel_Imports (country_of_origin_id)
ON [PRIMARY]
GO

CREATE INDEX IX_firms_code_id
ON dbo.Vessel_Imports (firms_code_id)
ON [PRIMARY]
GO

CREATE INDEX IX_importer_id
ON dbo.Vessel_Imports (importer_id)
ON [PRIMARY]
GO

CREATE INDEX IX_product_description_id
ON dbo.Vessel_Imports (product_description_id)
ON [PRIMARY]
GO

CREATE INDEX IX_state_id
ON dbo.Vessel_Imports (state_id)
ON [PRIMARY]
GO

CREATE INDEX IX_supplier_id
ON dbo.Vessel_Imports (supplier_id)
ON [PRIMARY]
GO

CREATE INDEX IX_user_id
ON dbo.Vessel_Imports (user_id)
ON [PRIMARY]
GO

CREATE INDEX IX_vessel_id
ON dbo.Vessel_Imports (vessel_id)
ON [PRIMARY]
GO

CREATE INDEX IX_Filing_Headers_FK
ON dbo.Vessel_Import_Filing_Details (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX IX_VI_FK
ON dbo.Vessel_Import_Filing_Details (VI_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportDeclarations_filingHeadersId
ON dbo.Vessel_Import_Declarations (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportInvoiceHeaders_filingHeadersId
ON dbo.Vessel_Import_Invoice_Headers (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportInvoiceLines_invoiceHeadersId
ON dbo.Vessel_Import_Invoice_Lines (invoice_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportMISC_filingHeadersId
ON dbo.Vessel_Import_Miscs (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportPackings_filingHeadersId
ON dbo.Vessel_Import_Packings (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX IX_filing_header_id
ON dbo.Vessel_Import_Documents (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX IX_inbound_record_id
ON dbo.Vessel_Import_Documents (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_IOR
ON dbo.Vessel_Rule_Importer (ior)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_firms_code_id
ON dbo.Vessel_Rule_Port (firms_code_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_Tariff
ON dbo.Vessel_Rule_Product (tariff)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.Vessel_Import_Sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.Vessel_Import_Sections (parent_id)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.Vessel_Import_Def_Values (section_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeaderId
ON dbo.Vessel_Import_Def_Values_Manual (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_VesselImportDefValuesManual_recordId_tableName_columnName
ON dbo.Vessel_Import_Def_Values_Manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

CREATE INDEX IX_tariff_id
ON dbo.Vessel_ProductDescriptions (tariff_id)
ON [PRIMARY]
GO

CREATE INDEX IX_state_id
ON dbo.Vessel_DischargeTerminals (state_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
--- Add Vessel Import Invoice Lines table ---
CREATE FUNCTION dbo.vessel_import_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      viil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY viil.id)
    FROM dbo.Vessel_Import_Invoice_Lines viil
    WHERE viil.invoice_header_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.Vessel_Import_Invoice_Lines
ADD line_no AS ([dbo].[vessel_import_invoice_line_number]([invoice_header_id], [id]));
GO

--- add triggers ---
CREATE TRIGGER dbo.vessel_import_declarations_befor_delete
ON dbo.Vessel_Import_Declarations
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE table_name = 'Vessel_Import_Declarations'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_import_invoice_headers_befor_delete
ON dbo.Vessel_Import_Invoice_Headers
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE table_name = 'Vessel_Import_Invoice_Headers'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_import_invoice_lines_befor_delete
ON dbo.Vessel_Import_Invoice_Lines
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE table_name = 'Vessel_Import_Invoice_Lines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_import_miscs_befor_delete
ON dbo.Vessel_Import_Miscs
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE table_name = 'Vessel_Import_Miscs'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.vessel_import_packings_befor_delete
ON dbo.Vessel_Import_Packings
FOR DELETE
AS
  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE table_name = 'Vessel_Import_Packings'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

PRINT ('create views')
GO
CREATE VIEW dbo.Vessel_Import_Grid 
AS SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,imports.eta AS eta
 ,user_data.[Broker] AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.code AS country_of_origin
 ,imports.created_date AS created_date
 ,'' AS filing_number
 ,'' AS job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,imports.deleted AS is_deleted
 ,CASE
    WHEN rules_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
 ,CASE
    WHEN rules_product.id IS NULL THEN 0
    ELSE 1
  END AS has_product_rule
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
    AND headers.mapping_status <> 0
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
LEFT OUTER JOIN dbo.cw_firms_codes firms_codes
  ON imports.firms_code_id = firms_codes.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
LEFT JOIN Vessel_Rule_Port rules_port
  ON imports.firms_code_id = rules_port.firms_code_id
LEFT JOIN Vessel_Rule_Product rules_product
  ON tariffs.USC_Tariff = rules_product.tariff

WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Vessel_Import_Filing_Headers filing_headers
  INNER JOIN dbo.Vessel_Import_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.mapping_status > 0
  AND imports.id = filing_details.VI_FK)

UNION ALL

SELECT DISTINCT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,imports.eta AS eta
 ,user_data.[Broker] AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.owner_ref AS owner_ref
 ,imports.unit_price AS unit_price
 ,imports.customs_qty AS customs_qty
 ,country.code AS country_of_origin
 ,imports.created_date AS created_date
 ,headers.filing_number
 ,headers.job_link
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,imports.deleted AS is_deleted
 ,1 AS has_port_rule
 ,1 AS has_product_rule
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
LEFT OUTER JOIN dbo.cw_firms_codes firms_codes
  ON imports.firms_code_id = firms_codes.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
LEFT OUTER JOIN dbo.Countries country
  ON imports.country_of_origin_id = country.id
LEFT OUTER JOIN dbo.app_users_data user_data
  ON imports.user_id = user_data.UserAccount
WHERE headers.mapping_status > 0
GO

CREATE VIEW dbo.Vessel_Import_Report 
AS SELECT
  headers.id AS Filing_Header_Id
 ,detailes.VI_FK AS VI_ID

 ,declaration.main_supplier AS Declarations_main_supplier
 ,declaration.importer AS Declarations_importer
 ,declaration.shpt_type AS Declarations_shpt_type
 ,declaration.transport AS Declarations_transport
 ,declaration.container AS Declarations_container
 ,declaration.ent_type AS Declarations_ent_type
 ,declaration.rlf AS Declarations_rlf
 ,declaration.message AS Declarations_message
 ,declaration.enable_ent_sum AS Declarations_enable_ent_sum
 ,declaration.enable_cargo_rel AS Declarations_enable_cargo_rel
 ,declaration.type AS Declarations_type
 ,declaration.certify_cargo_release AS Declarations_certify_cargo_release
 ,declaration.service AS Declarations_service
 ,declaration.issuer AS Declarations_issuer
 ,declaration.ocean_bill AS Declarations_ocean_bill
 ,declaration.vessel AS Declarations_vessel
 ,declaration.voyage AS Declarations_voyage
 ,declaration.carrier_scac AS Declarations_carrier_scac
 ,declaration.port_of_discharge AS Declarations_port_of_discharge
 ,declaration.port_of_loading AS Declarations_port_of_loading
 ,declaration.entry_port AS Declarations_entry_port
 ,declaration.dep AS Declarations_dep
 ,declaration.arr AS Declarations_arr
 ,declaration.loading_unloco AS Declarations_loading_unloco
 ,declaration.discharage_unloco AS Declarations_discharage_unloco
 ,declaration.hmf AS Declarations_hmf
 ,declaration.origin AS Declarations_origin
 ,declaration.destination AS Declarations_destination
 ,declaration.etd AS Declarations_etd
 ,declaration.eta AS Declarations_eta
 ,declaration.dest_state AS Declarations_dest_state
 ,declaration.country_of_export AS Declarations_country_of_export
 ,declaration.export_date AS Declarations_export_date
 ,declaration.description AS Declarations_description
 ,declaration.owner_ref AS Declarations_owner_ref
 ,declaration.inco AS Declarations_inco
 ,declaration.firms_code AS Declarations_firms_code
 ,declaration.entry_number AS Declarations_entry_number
 ,declaration.purchased AS Declarations_purchased
 ,declaration.check_local_client AS Declarations_check_local_client

 ,packings.manifest_qty AS Packings_manifest_qty
 ,packings.uq AS Packings_uq
 ,packings.bill_type AS Packings_bill_type
 ,packings.bill_issuer_scac AS Packings_bill_issuer_scac
 ,packings.bill_num AS Packings_bill_num

 ,misc.branch AS Miscs_branch
 ,misc.broker AS Miscs_broker
 ,misc.merge_by AS Miscs_merge_by
 ,misc.preparer_dist_port AS Miscs_preparer_dist_port
 ,misc.recon_issue AS Miscs_recon_issue
 ,misc.payment_type AS Miscs_payment_type
 ,misc.fta_recon AS Miscs_fta_recon
 ,misc.broker_to_pay AS Miscs_broker_to_pay
 ,misc.inbond_type AS Miscs_inbond_type

 ,invheaders.invoice_number AS Invoice_Headers_invoice_number
 ,invheaders.supplier AS Invoice_Headers_supplier
 ,invheaders.supplier_address AS Invoice_Headers_supplier_address
 ,invheaders.inco AS Invoice_Headers_inco
 ,invheaders.invoice_total AS Invoice_Headers_invoice_total
 ,invheaders.invoice_total_currency AS Invoice_Headers_invoice_total_currency
 ,invheaders.line_total AS Invoice_Headers_line_total
 ,invheaders.country_of_origin AS Invoice_Headers_country_of_origin
 ,invheaders.country_of_export AS Invoice_Headers_country_of_export
 ,invheaders.consignee AS Invoice_Headers_consignee
 ,invheaders.consignee_address AS Invoice_Headers_consignee_address
 ,invheaders.export_date AS Invoice_Headers_export_date
 ,invheaders.transaction_related AS Invoice_Headers_transaction_related
 ,invheaders.seller AS Invoice_Headers_seller
 ,invheaders.sold_to_party AS Invoice_Headers_sold_to_party
 ,invheaders.ship_to_party AS Invoice_Headers_ship_to_party
 ,invheaders.broker_pga_contact_name AS Invoice_Headers_broker_pga_contact_name
 ,invheaders.broker_pga_contact_phone AS Invoice_Headers_broker_pga_contact_phone
 ,invheaders.importer AS Invoice_Headers_importer
 ,invheaders.manufacturer AS Invoice_Headers_manufacturer
 ,invheaders.manufacturer_id AS Invoice_Headers_manufacturer_id
 ,invheaders.broker_pga_contact_email AS Invoice_Headers_broker_pga_contact_email

 ,invlines.line_no AS Invoice_Lines_line_no
 ,invlines.entry_no AS Invoice_Lines_entry_no
 ,invlines.product AS Invoice_Lines_product
 ,invlines.classification AS Invoice_Lines_classification
 ,invlines.tariff AS Invoice_Lines_tariff
 ,invlines.customs_qty AS Invoice_Lines_customs_qty
 ,invlines.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,invlines.goods_description AS Invoice_Lines_goods_description
 ,invlines.spi AS Invoice_Lines_spi
 ,invlines.country_of_origin AS Invoice_Lines_country_of_origin
 ,invlines.country_of_export AS Invoice_Lines_country_of_export
 ,invlines.destination_state AS Invoice_Lines_destination_state
 ,invlines.manufacturer AS Invoice_Lines_manufacturer
 ,invlines.consignee AS Invoice_Lines_consignee
 ,invlines.sold_to_party AS Invoice_Lines_sold_to_party
 ,invlines.seller AS Invoice_Lines_seller
 ,invlines.seller_address AS Invoice_Lines_seller_address
 ,invlines.gross_weight AS Invoice_Lines_gross_weight
 ,invlines.gross_weight_unit AS Invoice_Lines_gross_weight_unit
 ,invlines.price AS Invoice_Lines_price
 ,invlines.prod_id AS Invoice_Lines_prod_id
 ,invlines.attribute1 AS Invoice_Lines_attribute1
 ,invlines.attribute2 AS Invoice_Lines_attribute2
 ,invlines.attribute3 AS Invoice_Lines_attribute3
 ,invlines.transaction_related AS Invoice_Lines_transaction_related
 ,invlines.invoice_qty AS Invoice_Lines_invoice_qty
 ,invlines.code AS Invoice_Lines_code
 ,invlines.amount AS Invoice_Lines_amount
 ,invlines.epa_tsca_toxic_substance_control_act_indicator AS Invoice_Lines_epa_tsca_toxic_substance_control_act_indicator
 ,invlines.tsca_indicator AS Invoice_Lines_tsca_indicator
 ,invlines.certifying_individual AS Invoice_Lines_certifying_individual
 ,invlines.mid AS Invoice_Lines_mid
 ,invlines.hmf AS Invoice_Lines_hmf
 ,invlines.unit_price AS Invoice_Lines_unit_price

FROM dbo.Vessel_Import_Filing_Headers headers
INNER JOIN dbo.Vessel_Import_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT OUTER JOIN dbo.Vessel_Import_Declarations declaration
  ON declaration.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Packings AS packings
  ON packings.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Miscs AS misc
  ON misc.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Headers invheaders
  ON invheaders.filing_header_id = headers.id
LEFT OUTER JOIN dbo.Vessel_Import_Invoice_Lines invlines
  ON invlines.invoice_header_id = invheaders.id
WHERE headers.mapping_status = 2
GO

CREATE VIEW dbo.v_Vessel_Import_Def_Values 
AS SELECT
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
FROM dbo.vessel_import_def_values vdv
INNER JOIN dbo.vessel_import_sections vs
  ON vdv.section_id = vs.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(vdv.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(vs.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_Vessel_Import_Def_Values_Manual 
AS SELECT
  vdvm.id
 ,vdvm.filing_header_id
 ,vdvm.parent_record_id
 ,vdvm.record_id
 ,vdvm.section_name
 ,vdvm.section_title
 ,vdvm.table_name
 ,vdvm.column_name
 ,vdvm.label
 ,vdvm.[value]
 ,vdvm.description
 ,vdvm.editable
 ,vdvm.has_default_value
 ,vdvm.mandatory
 ,vdvm.paired_field_table
 ,vdvm.paired_field_column
 ,vdvm.handbook_name
 ,vdvm.display_on_ui
 ,vdvm.manual
 ,vdvm.modification_date
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.vessel_import_def_values_manual vdvm
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(vdvm.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(vdvm.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_Vessel_Import_Tables 
AS SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM INFORMATION_SCHEMA.COLUMNS i
INNER JOIN vessel_import_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
GO

PRINT ('create stored procedures')
GO
-- Add Vessel import def value manual values records --
CREATE PROCEDURE dbo.vessel_import_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.Vessel_Import_Def_Values v
  INNER JOIN dbo.Vessel_Import_Sections s
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

  INSERT INTO dbo.Vessel_Import_Def_Values_Manual (
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
    FROM dbo.Vessel_Import_Def_Values dv
    INNER JOIN dbo.Vessel_Import_Sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO
--- Vessel Import apply filing values for specified resulting table procedure ---
CREATE PROCEDURE dbo.vessel_import_apply_def_values (@tableName VARCHAR(128)
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
  FROM dbo.Vessel_Import_Def_Values_Manual v
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
--- add vessel import declarations record ---
CREATE PROCEDURE dbo.vessel_import_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Declarations vid
      WHERE vid.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,ent_type
       ,eta
       ,arr
       ,vessel
       ,port_of_discharge
       ,entry_port
       ,dest_state
       ,firms_code
       ,description
       ,hmf
       ,owner_ref
       ,destination)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.container
       ,vi.entry_type
       ,vi.eta
       ,vi.eta
       ,vessel.name
       ,vrp.entry_port
       ,vrp.entry_port
       ,states.StateCode
       ,firms.firms_code
       ,vpd.name
       ,vrp.hmf
       ,vi.owner_ref
       ,vrp.destination_code
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vi.firms_code_id = vrp.firms_code_id
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.app_users_data aud
        ON vi.user_id = aud.UserAccount
      LEFT JOIN cw_firms_codes firms
        ON firms.id = vi.firms_code_id
      LEFT JOIN US_States states
        ON firms.state_id = states.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
--- add vessel import invoice line record ---
CREATE PROCEDURE dbo.vessel_import_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Invoice_Lines viil
      WHERE viil.invoice_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Lines (
        invoice_header_id
       ,classification
       ,tariff
       ,goods_description
       ,destination_state
       ,consignee
       ,seller
       ,attribute1
       ,attribute2
       ,attribute3
       ,epa_tsca_toxic_substance_control_act_indicator
       ,tsca_indicator
       ,certifying_individual
       ,hmf
       ,product
       ,customs_qty_unit
       ,manufacturer
       ,sold_to_party
       ,customs_qty
       ,invoice_qty
       ,unit_price
       ,country_of_origin
       ,price
       ,invoice_quantity_unit)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,COALESCE(NULLIF(rule_product.goods_description, ''), vpd.name)
       ,state.StateCode
       ,importer.ClientCode
       ,supplier.ClientCode
       ,COALESCE(NULLIF(rule_product.customs_attribute1, ''), vpd.name)
       ,rule_product.customs_attribute2
       ,vi.owner_ref
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,vpd.name
       ,tariff.Unit
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.customs_qty
       ,vi.customs_qty
       ,vi.unit_price
       ,country.code
       ,vi.unit_price * vi.customs_qty
       ,COALESCE(NULLIF(rule_product.invoice_uq, ''), tariff.Unit)
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.Tariff tariff
        ON vi.classification_id = tariff.id
      LEFT JOIN dbo.Vessel_Rule_Product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.Vessel_Rule_Port rule_port
        ON vi.firms_code_id = rule_port.firms_code_id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      LEFT JOIN cw_firms_codes firms
        ON vi.firms_code_id = firms.id
      LEFT JOIN US_States state
        ON firms.state_id = state.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
--- add vessel import invoice header record ---
CREATE PROCEDURE dbo.vessel_import_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (
        filing_header_id
       ,supplier
       ,seller
       ,manufacturer
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,country_of_origin
       ,consignee
      --    , transaction_related
      )
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,country.code
       ,importer.ClientCode
      --       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')

      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON vi.country_of_origin_id = country.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_import_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    UPDATE dbo.Vessel_Import_Invoice_Headers
    SET line_total = line.total
       ,invoice_total = line.total
    FROM Vessel_Import_Invoice_Headers viih
    LEFT JOIN (SELECT
        viil.invoice_header_id
       ,SUM(viil.price) AS total
      FROM Vessel_Import_Invoice_Lines viil
      GROUP BY viil.invoice_header_id) AS line
      ON viih.id = line.invoice_header_id
    WHERE viih.id = @recordId


    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
--- Vessel import add misc record ---
CREATE PROCEDURE dbo.vessel_import_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Miscs'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Miscs vim
      WHERE vim.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Miscs (
        filing_header_id
       ,branch
       ,broker
       ,preparer_dist_port)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = vi.user_id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
--- add vessel import packing record ---
CREATE PROCEDURE dbo.vessel_import_add_packing_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Packings'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Vessel_Import_Packings vip
      WHERE vip.filing_header_id = @parentId)
  BEGIN
    INSERT INTO Vessel_Import_Packings (
        filing_header_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add Vessel import filing procedure
CREATE PROCEDURE dbo.vessel_import_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.vessel_import_add_declaration_record @filingHeaderId
                                               ,@filingHeaderId

  -- add invoice header
  EXEC dbo.vessel_import_add_invoice_header_record @filingHeaderId
                                                  ,@filingHeaderId

  -- add paking
  EXEC dbo.vessel_import_add_packing_record @filingHeaderId
                                           ,@filingHeaderId

  -- add misc
  EXEC dbo.vessel_import_add_misc_record @filingHeaderId
                                        ,@filingHeaderId

END;
GO
-- Vessel import filing records delete procedure ---
CREATE PROCEDURE dbo.vessel_import_filing_del (@filingHeaderId INT)
AS
BEGIN

  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.Vessel_Import_Filing_Details
  WHERE Filing_Headers_FK = @filingHeaderId

  DELETE FROM dbo.Vessel_Import_Filing_Headers
  WHERE id = @filingHeaderId

END
GO
--- Vessel import inbound record soft delete proceure ---
CREATE PROCEDURE dbo.vessel_import_del (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mapping_status = grid.mapping_status
  FROM Vessel_Import_Grid grid
  WHERE grid.id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Vessel_Imports
    SET deleted = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Vessel_Imports
      SET deleted = @deleted
      WHERE id IN (SELECT
          details.VI_FK
        FROM Vessel_Import_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO
--- Vessel import delete record from specified resulting table procedure ---
CREATE PROCEDURE dbo.vessel_import_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.Vessel_Import_Sections vis
      WHERE vis.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE
    THROW 51000, 'Invalid table name', 1
END
GO
-- Vessel import apply filing params for all resulting tables procedure ---
CREATE PROCEDURE dbo.vessel_import_filing_param (@Filing_Headers_id INT)
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
  LEFT JOIN dbo.Vessel_Import_Def_Values_Manual v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO
--- Vessel Import post save action ---
CREATE PROCEDURE dbo.vessel_import_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  )

  INSERT INTO @tbl (
      record_id
     ,filing_header_id
     ,quantity
     ,unit_price)
    SELECT
      a.record_id
     ,a.filing_header_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS Quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS UnitPrice
    FROM Vessel_Import_Def_Values_Manual a
    JOIN Vessel_Import_Def_Values_Manual b
      ON a.record_id = b.record_id
    WHERE a.filing_header_id = @filingHeaderId
    AND b.filing_header_id = @filingHeaderId
    AND a.table_name = 'Vessel_Import_Invoice_Lines'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price'

  DECLARE @total DECIMAL(18, 6)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  -- update invoice line price
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.######')
  FROM Vessel_Import_Def_Values_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Vessel_Import_Invoice_Lines'
  AND defValues.column_name = 'price'
  -- update invoice header total and line total
  UPDATE Vessel_Import_Def_Values_Manual
  SET value = FORMAT(@total, '0.######')
  WHERE filing_header_id = @filingHeaderId
  AND table_name = 'Vessel_Import_Invoice_Headers'
  AND (column_name = 'invoice_total'
  OR column_name = 'line_total')
  -- update declaration orogin
  UPDATE dest
  SET value = port.unloco
  FROM dbo.Vessel_Import_Def_Values_Manual dest
  JOIN dbo.Vessel_Import_Def_Values_Manual source
    ON dest.record_id = source.record_id
    AND source.column_name = 'port_of_loading'
  LEFT JOIN dbo.CW_Foreign_Ports port
    ON source.value = port.port_code
  WHERE dest.filing_header_id = @filingHeaderId
  AND dest.table_name = 'Vessel_Import_Declarations'
  AND dest.column_name IN ('origin','loading_unloco')
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
  ON vessel_export_doc.filing_header_id = vessel_export_header.id;
GO

PRINT ('drop old objects')
PRINT ('drop functions and computed columns')
GO

ALTER TABLE dbo.imp_vessel_invoice_line
DROP COLUMN line_number;
DROP FUNCTION dbo.fn_imp_vessel_invoice_line_number;
GO

PRINT ('drop tables')
GO

DROP TABLE dbo.imp_vessel_declaration
DROP TABLE dbo.imp_vessel_packing
DROP TABLE dbo.imp_vessel_misc
DROP TABLE dbo.imp_vessel_document
DROP TABLE dbo.imp_vessel_invoice_line
DROP TABLE dbo.imp_vessel_invoice_header
DROP TABLE dbo.imp_vessel_filing_detail
DROP TABLE dbo.imp_vessel_filing_header
DROP TABLE dbo.imp_vessel_inbound
DROP TABLE dbo.imp_vessel_rule_importer
DROP TABLE dbo.imp_vessel_rule_port
DROP TABLE dbo.imp_vessel_rule_product
DROP TABLE dbo.imp_vessel_form_configuration
DROP TABLE dbo.imp_vessel_form_section_configuration
DROP TABLE dbo.handbook_tariff_product_description
DROP TABLE dbo.handbook_vessel

PRINT ('drop views')
GO

DROP VIEW dbo.v_imp_vessel_form_configuration
DROP VIEW dbo.v_imp_vessel_field_configuration
DROP VIEW dbo.v_imp_vessel_inbound_grid
DROP VIEW dbo.v_imp_vessel_report

PRINT ('drop procedures')
GO

DROP PROCEDURE dbo.sp_imp_vessel_create_entry_records
DROP PROCEDURE dbo.sp_imp_vessel_add_declaration
DROP PROCEDURE dbo.sp_imp_vessel_add_packing
DROP PROCEDURE dbo.sp_imp_vessel_add_misc
DROP PROCEDURE dbo.sp_imp_vessel_add_invoice_header
DROP PROCEDURE dbo.sp_imp_vessel_add_invoice_line
DROP PROCEDURE dbo.sp_imp_vessel_review_entry
DROP PROCEDURE dbo.sp_imp_vessel_delete_entry_records
DROP PROCEDURE dbo.sp_imp_vessel_update_entry
DROP PROCEDURE dbo.sp_imp_vessel_recalculate
DROP PROCEDURE dbo.sp_imp_vessel_delete_inbound
