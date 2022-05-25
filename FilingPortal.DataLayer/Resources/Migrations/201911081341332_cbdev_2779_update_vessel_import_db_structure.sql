PRINT ('create tables')
GO

CREATE TABLE dbo.imp_vessel_inbound (
  id INT IDENTITY
 ,eta DATETIME NOT NULL
 ,container VARCHAR(128) NOT NULL DEFAULT ('BLK')
 ,entry_type VARCHAR(128) NOT NULL DEFAULT ('01')
 ,importer_id UNIQUEIDENTIFIER NOT NULL
 ,state_id INT NULL
 ,classification_id INT NOT NULL
 ,user_id VARCHAR(128) NOT NULL
 ,supplier_id UNIQUEIDENTIFIER NULL
 ,vessel_id INT NOT NULL
 ,product_description_id INT NOT NULL
 ,customs_qty DECIMAL(18, 6) NOT NULL
 ,unit_price DECIMAL(18, 6) NOT NULL
 ,country_of_origin_id INT NULL
 ,owner_ref VARCHAR(128) NOT NULL
 ,firms_code_id INT NOT NULL
 ,deleted BIT NOT NULL DEFAULT (0)
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_filing_header (
  id INT IDENTITY
 ,filing_number VARCHAR(255) NULL
 ,mapping_status INT NULL
 ,filing_status INT NULL
 ,error_description VARCHAR(MAX) NULL
 ,job_link VARCHAR(8000) NULL
 ,request_xml VARCHAR(MAX) NULL
 ,response_xml VARCHAR(MAX) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_filing_detail (
  inbound_id INT NOT NULL
 ,filing_header_id INT NOT NULL
 ,PRIMARY KEY CLUSTERED (inbound_id, filing_header_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_declaration (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shpt_type VARCHAR(10) NULL
 ,transport VARCHAR(10) NULL
 ,container VARCHAR(10) NULL
 ,ent_type VARCHAR(10) NULL
 ,rlf VARCHAR(128) NULL
 ,message VARCHAR(128) NULL
 ,enable_ent_sum VARCHAR(10) NULL
 ,enable_cargo_rel VARCHAR(10) NULL
 ,type VARCHAR(128) NULL
 ,certify_cargo_release VARCHAR(128) NULL
 ,service VARCHAR(128) NULL
 ,issuer VARCHAR(128) NULL
 ,ocean_bill VARCHAR(128) NULL
 ,vessel VARCHAR(128) NULL
 ,voyage VARCHAR(128) NULL
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
 ,[description] VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,entry_number VARCHAR(128) NULL
 ,purchased VARCHAR(10) NULL
 ,check_local_client VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_invoice_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,invoice_number VARCHAR(128) NULL
 ,supplier VARCHAR(128) NULL
 ,supplier_address VARCHAR(128) NULL
 ,inco VARCHAR(10) NULL
 ,invoice_total_currency VARCHAR(10) NULL
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
 ,broker_pga_contact_name VARCHAR(128) NULL
 ,broker_pga_contact_phone VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,manufacturer VARCHAR(128) NULL
 ,manufacturer_id VARCHAR(128) NULL
 ,broker_pga_contact_email VARCHAR(128) NULL
 ,invoice_total NUMERIC(18, 6) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_vessel_invoice_line (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,entry_no VARCHAR(128) NULL
 ,product VARCHAR(128) NULL
 ,classification VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,customs_qty NUMERIC(19, 5) NULL
 ,customs_qty_unit VARCHAR(128) NULL
 ,goods_description VARCHAR(512) NULL
 ,spi VARCHAR(128) NULL
 ,country_of_origin VARCHAR(10) NULL
 ,country_of_export VARCHAR(10) NULL
 ,destination_state VARCHAR(10) NULL
 ,manufacturer VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,seller VARCHAR(128) NULL
 ,seller_address VARCHAR(128) NULL
 ,gross_weight NUMERIC(18, 6) NULL
 ,price NUMERIC(18, 2) NULL
 ,prod_id VARCHAR(128) NULL
 ,attribute1 VARCHAR(128) NULL
 ,attribute2 VARCHAR(128) NULL
 ,attribute3 VARCHAR(128) NULL
 ,transaction_related VARCHAR(10) NULL
 ,invoice_qty NUMERIC(19, 5) NULL
 ,code VARCHAR(128) NULL
 ,amount INT NULL
 ,epa_tsca_toxic_substance_control_act_indicator VARCHAR(128) NULL
 ,tsca_indicator VARCHAR(128) NULL
 ,certifying_individual VARCHAR(128) NULL
 ,mid VARCHAR(128) NULL
 ,hmf VARCHAR(128) NULL
 ,unit_price NUMERIC(18, 6) NULL
 ,invoice_quantity_unit VARCHAR(128) NULL
 ,gross_weight_unit VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_vessel_misc (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,branch VARCHAR(10) NULL
 ,broker VARCHAR(10) NULL
 ,merge_by VARCHAR(10) NULL
 ,preparer_dist_port VARCHAR(128) NULL
 ,recon_issue VARCHAR(10) NULL
 ,payment_type VARCHAR(10) NULL
 ,fta_recon VARCHAR(10) NULL
 ,broker_to_pay VARCHAR(10) NULL
 ,inbond_type VARCHAR(10) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_vessel_packing (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,manifest_qty INT NULL
 ,uq VARCHAR(128) NULL
 ,bill_type VARCHAR(10) NULL
 ,bill_issuer_scac VARCHAR(128) NULL
 ,bill_num VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_document (
  id INT IDENTITY
 ,filing_header_id INT NULL
 ,inbound_record_id INT NULL
 ,[file_name] VARCHAR(255) NULL
 ,file_extension VARCHAR(128) NULL
 ,file_description VARCHAR(1000) NULL
 ,file_content VARBINARY(MAX) NULL
 ,document_type VARCHAR(120) NULL
 ,[status] VARCHAR(50) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_rule_importer (
  id INT IDENTITY
 ,ior VARCHAR(128) NOT NULL
 ,cw_ior VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_rule_port (
  id INT IDENTITY
 ,discharge_name VARCHAR(128) NULL
 ,entry_port VARCHAR(4) NULL
 ,hmf VARCHAR(1) NULL
 ,discharge_port VARCHAR(4) NULL
 ,firms_code_id INT NOT NULL DEFAULT (0)
 ,destination_code VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_rule_product (
  id INT IDENTITY
 ,tariff VARCHAR(10) NOT NULL
 ,goods_description VARCHAR(128) NULL
 ,customs_attribute1 VARCHAR(128) NULL
 ,customs_attribute2 VARCHAR(128) NULL
 ,invoice_uq VARCHAR(128) NULL
 ,tsca_requirement VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_form_section_configuration (
  id INT IDENTITY
 ,[name] VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,[procedure_name] VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_vessel_form_configuration (
  id INT IDENTITY
 ,section_id INT NOT NULL
 ,column_name VARCHAR(128) NULL
 ,[label] VARCHAR(128) NULL
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

CREATE TABLE dbo.handbook_vessel (
  id INT IDENTITY
 ,[name] VARCHAR(128) NOT NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.handbook_tariff_product_description (
  id INT IDENTITY
 ,tariff_id INT NOT NULL
 ,[name] VARCHAR(128) NOT NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_inbound ON;
INSERT INTO dbo.imp_vessel_inbound (
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
   ,COALESCE(inbnd.created_user, SUSER_NAME())
  FROM dbo.Vessel_Imports inbnd;
SET IDENTITY_INSERT dbo.imp_vessel_inbound OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_filing_header ON;
INSERT INTO dbo.imp_vessel_filing_header (
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
  FROM dbo.Vessel_Import_Filing_Headers fh;
SET IDENTITY_INSERT dbo.imp_vessel_filing_header OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.imp_vessel_filing_detail (
    inbound_id
   ,filing_header_id)
  SELECT
    fd.VI_FK
   ,fd.Filing_Headers_FK
  FROM dbo.Vessel_Import_Filing_Details fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_declaration ON;
INSERT INTO dbo.imp_vessel_declaration (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,check_local_client
   ,created_date
   ,created_user)
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
   ,declaration.ent_type
   ,declaration.rlf
   ,declaration.[message]
   ,declaration.enable_ent_sum
   ,declaration.enable_cargo_rel
   ,declaration.[type]
   ,declaration.certify_cargo_release
   ,declaration.[service]
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
   ,declaration.[description]
   ,declaration.owner_ref
   ,declaration.inco
   ,declaration.firms_code
   ,declaration.entry_number
   ,declaration.purchased
   ,declaration.check_local_client
   ,GETDATE()
   ,SUSER_NAME()
  FROM dbo.Vessel_Import_Declarations declaration;
SET IDENTITY_INSERT dbo.imp_vessel_declaration OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_invoice_header ON;
INSERT INTO dbo.imp_vessel_invoice_header (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,invoice_total
   ,created_date
   ,created_user)
  SELECT
    invoice.id
   ,invoice.filing_header_id
   ,invoice.filing_header_id
   ,NULL
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
   ,GETDATE()
   ,SUSER_NAME()
  FROM dbo.Vessel_Import_Invoice_Headers invoice;
SET IDENTITY_INSERT dbo.imp_vessel_invoice_header OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_invoice_line ON;
INSERT INTO dbo.imp_vessel_invoice_line (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
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
   ,gross_weight_unit
   ,created_date
   ,created_user)
  SELECT
    line.id
   ,invoice.filing_header_id
   ,line.invoice_header_id
   ,NULL
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
   ,GETDATE()
   ,SUSER_NAME()
  FROM dbo.Vessel_Import_Invoice_Lines line
  JOIN dbo.Vessel_Import_Invoice_Headers invoice
    ON line.invoice_header_id = invoice.id;
SET IDENTITY_INSERT dbo.imp_vessel_invoice_line OFF;
GO

PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_misc ON;
INSERT INTO dbo.imp_vessel_misc (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,branch
   ,[broker]
   ,merge_by
   ,preparer_dist_port
   ,recon_issue
   ,payment_type
   ,fta_recon
   ,broker_to_pay
   ,inbond_type
   ,created_date
   ,created_user)
  SELECT
    misc.id
   ,misc.filing_header_id
   ,misc.filing_header_id
   ,NULL
   ,misc.branch
   ,misc.[broker]
   ,misc.merge_by
   ,misc.preparer_dist_port
   ,misc.recon_issue
   ,misc.payment_type
   ,misc.fta_recon
   ,misc.broker_to_pay
   ,misc.inbond_type
   ,GETDATE()
   ,SUSER_NAME()
  FROM dbo.Vessel_Import_Miscs misc
SET IDENTITY_INSERT dbo.imp_vessel_misc OFF;
GO

PRINT ('copy packing data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_packing ON;
INSERT INTO dbo.imp_vessel_packing (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,manifest_qty
   ,uq
   ,bill_type
   ,bill_issuer_scac
   ,bill_num
   ,created_date
   ,created_user)
  SELECT
    pack.id
   ,pack.filing_header_id
   ,pack.filing_header_id
   ,NULL
   ,pack.manifest_qty
   ,pack.uq
   ,pack.bill_type
   ,pack.bill_issuer_scac
   ,pack.bill_num
   ,GETDATE()
   ,SUSER_NAME()
  FROM dbo.Vessel_Import_Packings pack
SET IDENTITY_INSERT dbo.imp_vessel_packing OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_document ON;
INSERT INTO dbo.imp_vessel_document (
    id
   ,filing_header_id
   ,inbound_record_id
   ,[file_name]
   ,file_extension
   ,file_description
   ,file_content
   ,document_type
   ,[status]
   ,created_date
   ,created_user)
  SELECT
    doc.id
   ,doc.filing_header_id
   ,doc.inbound_record_id
   ,doc.[file_name]
   ,doc.[extension]
   ,doc.[description]
   ,doc.[content]
   ,doc.document_type
   ,doc.[Status]
   ,doc.created_date
   ,doc.created_user
  FROM dbo.Vessel_Import_Documents doc;
SET IDENTITY_INSERT dbo.imp_vessel_document OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_rule_importer ON;
INSERT INTO dbo.imp_vessel_rule_importer (
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
  FROM dbo.Vessel_Rule_Importer r;
SET IDENTITY_INSERT dbo.imp_vessel_rule_importer OFF;
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_rule_port ON;
INSERT INTO dbo.imp_vessel_rule_port (
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
  FROM dbo.Vessel_Rule_Port r;
SET IDENTITY_INSERT dbo.imp_vessel_rule_port OFF;
GO

PRINT ('copy product rule data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_rule_product ON;
INSERT INTO dbo.imp_vessel_rule_product (
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
  FROM dbo.Vessel_Rule_Product r;
SET IDENTITY_INSERT dbo.imp_vessel_rule_product OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_form_section_configuration ON;
INSERT INTO dbo.imp_vessel_form_section_configuration (
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
  FROM dbo.vessel_import_sections section;
SET IDENTITY_INSERT dbo.imp_vessel_form_section_configuration OFF;
GO

UPDATE dbo.imp_vessel_form_section_configuration
SET table_name = 'imp_vessel_declaration'
   ,procedure_name = 'sp_imp_vessel_add_declaration'
WHERE table_name = 'Vessel_Import_Declarations';
UPDATE dbo.imp_vessel_form_section_configuration
SET table_name = 'imp_vessel_invoice_header'
   ,procedure_name = 'sp_imp_vessel_add_invoice_header'
WHERE table_name = 'Vessel_Import_Invoice_Headers';
UPDATE dbo.imp_vessel_form_section_configuration
SET table_name = 'imp_vessel_invoice_line'
   ,procedure_name = 'sp_imp_vessel_add_invoice_line'
WHERE table_name = 'Vessel_Import_Invoice_Lines';
UPDATE dbo.imp_vessel_form_section_configuration
SET table_name = 'imp_vessel_packing'
   ,procedure_name = 'sp_imp_vessel_add_paking'
WHERE table_name = 'Vessel_Import_Packings';
UPDATE dbo.imp_vessel_form_section_configuration
SET table_name = 'imp_vessel_misc'
   ,procedure_name = 'sp_imp_vessel_add_misc'
WHERE table_name = 'Vessel_Import_Miscs';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.imp_vessel_form_configuration ON;
INSERT INTO dbo.imp_vessel_form_configuration (
    id
   ,section_id
   ,column_name
   ,[label]
   ,[description]
   ,[value]
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
   ,dv.default_value
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
   ,COALESCE(dv.created_user, SUSER_NAME())
  FROM dbo.Vessel_Import_Def_Values dv;
SET IDENTITY_INSERT dbo.imp_vessel_form_configuration OFF;
GO

PRINT ('copy vessel handbook data')
GO
SET IDENTITY_INSERT dbo.handbook_vessel ON;
INSERT INTO dbo.handbook_vessel (
    id
   ,[name])
  SELECT
    v.id
   ,v.[name]
  FROM dbo.Vessels v;
SET IDENTITY_INSERT dbo.handbook_vessel OFF;
GO

PRINT ('copy tariff product description handbook data')
GO
SET IDENTITY_INSERT dbo.handbook_tariff_product_description ON;
INSERT INTO dbo.handbook_tariff_product_description (
    id
   ,tariff_id
   ,[name])
  SELECT
    vpd.id
   ,vpd.tariff_id
   ,vpd.[name]
  FROM dbo.Vessel_ProductDescriptions vpd;
SET IDENTITY_INSERT dbo.handbook_tariff_product_description OFF;
GO

PRINT ('add constraints')
GO
ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound_app_users__user_id] FOREIGN KEY ([user_id]) REFERENCES dbo.app_users (UserAccount)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__Clients__importer_id] FOREIGN KEY (importer_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__Clients__supplier_id] FOREIGN KEY (supplier_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__Countries__country_of_origin_id] FOREIGN KEY (country_of_origin_id) REFERENCES dbo.Countries (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__cw_firms_codes__firms_code_id] FOREIGN KEY (firms_code_id) REFERENCES dbo.cw_firms_codes (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__Tariff__classification_id] FOREIGN KEY (classification_id) REFERENCES dbo.Tariff (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__US_States__state_id] FOREIGN KEY (state_id) REFERENCES dbo.US_States (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__handbook_tariff_product_description__product_description_id] FOREIGN KEY (product_description_id) REFERENCES dbo.handbook_tariff_product_description (id)
GO

ALTER TABLE dbo.imp_vessel_inbound
ADD CONSTRAINT [FK__imp_vessel_inbound__handbook_vessel__vessel_id] FOREIGN KEY (vessel_id) REFERENCES dbo.handbook_vessel (id)
GO

ALTER TABLE dbo.imp_vessel_filing_header
ADD CONSTRAINT [FK__imp_vessel_filing_header__FilingStatus__filing_status] FOREIGN KEY (filing_status) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.imp_vessel_filing_header
ADD CONSTRAINT [FK__imp_vessel_filing_header__MappingStatus__mapping_status] FOREIGN KEY (mapping_status) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.imp_vessel_filing_detail
ADD CONSTRAINT FK__imp_vessel_filing_detail__imp_vessel_inbound__inbound_id FOREIGN KEY (inbound_id) REFERENCES dbo.imp_vessel_inbound (id)
GO

ALTER TABLE dbo.imp_vessel_filing_detail
ADD CONSTRAINT FK__imp_vessel_filing_detail__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id)
GO

ALTER TABLE dbo.imp_vessel_declaration
ADD CONSTRAINT FK__imp_vessel_declaration__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_packing
ADD CONSTRAINT FK__imp_vessel_packing__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_invoice_header
ADD CONSTRAINT FK__imp_vessel_invoice_header__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_invoice_line
ADD CONSTRAINT FK__imp_vessel_invoice_line__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id)
GO

ALTER TABLE dbo.imp_vessel_invoice_line
ADD CONSTRAINT FK__imp_vessel_invoice_line__imp_vessel_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.imp_vessel_invoice_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_misc
ADD CONSTRAINT FK__imp_vessel_misc__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_rule_port
ADD CONSTRAINT [FK__imp_vessel_rule_port__cw_firms_codes__firms_code_id] FOREIGN KEY (firms_code_id) REFERENCES dbo.cw_firms_codes (id)
GO

ALTER TABLE dbo.imp_vessel_document
ADD CONSTRAINT [FK__imp_vessel_document__imp_vessel_inbound__inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.imp_vessel_inbound (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_document
ADD CONSTRAINT FK__imp_vessel_document__imp_vessel_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_vessel_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_vessel_form_section_configuration
ADD CONSTRAINT FK__imp_vessel_form_section_configuration__imp_vessel_form_section_configuration__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.imp_vessel_form_section_configuration (id)
GO

ALTER TABLE dbo.imp_vessel_form_configuration
ADD CONSTRAINT FK__imp_vessel_form_configuration__imp_vessel_form_section_configuration__section_id FOREIGN KEY (section_id) REFERENCES dbo.imp_vessel_form_section_configuration (id)
GO

ALTER TABLE dbo.handbook_tariff_product_description
ADD CONSTRAINT FK__handbook_tariff_product_description__Tariff__tariff_id FOREIGN KEY (tariff_id) REFERENCES dbo.Tariff (id)
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
FROM imp_vessel_form_configuration field
JOIN imp_vessel_form_section_configuration section
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
AND field.[value] IS NOT NULL
ORDER BY section.table_name;

EXEC (@command)

PRINT ('add indexes')
GO
CREATE INDEX Idx__classification_id
ON dbo.imp_vessel_inbound (classification_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__country_of_origin_id
ON dbo.imp_vessel_inbound (country_of_origin_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__firms_code_id
ON dbo.imp_vessel_inbound (firms_code_id)
ON [PRIMARY]
GO


CREATE INDEX Idx__importer_id
ON dbo.imp_vessel_inbound (importer_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__product_description_id
ON dbo.imp_vessel_inbound (product_description_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__state_id
ON dbo.imp_vessel_inbound (state_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__supplier_id
ON dbo.imp_vessel_inbound (supplier_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__user_id
ON dbo.imp_vessel_inbound ([user_id])
ON [PRIMARY]
GO

CREATE INDEX Idx__vessel_id
ON dbo.imp_vessel_inbound (vessel_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_status
ON dbo.imp_vessel_filing_header (filing_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__mapping_status
ON dbo.imp_vessel_filing_header (mapping_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_filing_detail (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_id
ON dbo.imp_vessel_filing_detail (inbound_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON dbo.imp_vessel_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_misc (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_packing (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_vessel_document (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_record_id
ON dbo.imp_vessel_document (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__ior
ON dbo.imp_vessel_rule_importer (ior)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__firms_code_id
ON dbo.imp_vessel_rule_port (firms_code_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__tariff
ON dbo.imp_vessel_rule_product (tariff)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__name
ON dbo.imp_vessel_form_section_configuration (name)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_id
ON dbo.imp_vessel_form_section_configuration (parent_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__section_id
ON dbo.imp_vessel_form_configuration (section_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__tariff_id
ON dbo.handbook_tariff_product_description (tariff_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
--- gets vessel import invoice line number ---
CREATE FUNCTION dbo.fn_imp_vessel_invoice_line_number (@invoiceHeaderId INT
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
    FROM dbo.imp_vessel_invoice_line line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.imp_vessel_invoice_line
ADD line_number AS (dbo.fn_imp_vessel_invoice_line_number(parent_record_id, id));
GO

PRINT ('create views')
GO
CREATE VIEW dbo.v_imp_vessel_form_configuration
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
FROM dbo.imp_vessel_form_configuration form
INNER JOIN dbo.imp_vessel_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL;
GO

CREATE VIEW dbo.v_imp_vessel_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN imp_vessel_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id');
GO

CREATE VIEW dbo.v_imp_vessel_inbound_grid
AS
SELECT
  inbnd.id AS id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,vessel.[name] AS vessel
 ,states.StateCode AS state
 ,firms_codes.firms_code + ' - ' + firms_codes.name AS firms_code
 ,tariffs.USC_Tariff AS classification
 ,descriptions.[name] AS product_description
 ,inbnd.eta
 ,user_data.[Broker] AS filer_id
 ,inbnd.container
 ,inbnd.entry_type
 ,inbnd.owner_ref
 ,inbnd.unit_price
 ,inbnd.customs_qty
 ,country.code AS country_of_origin
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
    WHEN rules_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
 ,CASE
    WHEN rules_product.id IS NULL THEN 0
    ELSE 1
  END AS has_product_rule
FROM dbo.imp_vessel_inbound inbnd
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN dbo.Clients AS supplier
  ON inbnd.supplier_id = supplier.id
LEFT JOIN dbo.US_States states
  ON inbnd.state_id = states.id
LEFT JOIN dbo.Tariff tariffs
  ON inbnd.classification_id = tariffs.id
LEFT JOIN dbo.handbook_vessel vessel
  ON inbnd.vessel_id = vessel.id
LEFT JOIN dbo.cw_firms_codes firms_codes
  ON inbnd.firms_code_id = firms_codes.id
LEFT JOIN dbo.handbook_tariff_product_description descriptions
  ON inbnd.product_description_id = descriptions.id
LEFT JOIN dbo.Countries country
  ON inbnd.country_of_origin_id = country.id
LEFT JOIN dbo.app_users_data user_data
  ON inbnd.user_id = user_data.UserAccount
LEFT JOIN dbo.imp_vessel_rule_port rules_port
  ON inbnd.firms_code_id = rules_port.firms_code_id
LEFT JOIN dbo.imp_vessel_rule_product rules_product
  ON tariffs.USC_Tariff = rules_product.tariff

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_vessel_filing_header etfh
  JOIN dbo.imp_vessel_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
WHERE inbnd.deleted = 0;
GO

CREATE VIEW dbo.v_imp_vessel_report
AS
SELECT
  header.id AS Filing_Header_Id
 ,detaile.inbound_id AS VI_ID

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

 ,packing.manifest_qty AS Packings_manifest_qty
 ,packing.uq AS Packings_uq
 ,packing.bill_type AS Packings_bill_type
 ,packing.bill_issuer_scac AS Packings_bill_issuer_scac
 ,packing.bill_num AS Packings_bill_num

 ,misc.branch AS Miscs_branch
 ,misc.broker AS Miscs_broker
 ,misc.merge_by AS Miscs_merge_by
 ,misc.preparer_dist_port AS Miscs_preparer_dist_port
 ,misc.recon_issue AS Miscs_recon_issue
 ,misc.payment_type AS Miscs_payment_type
 ,misc.fta_recon AS Miscs_fta_recon
 ,misc.broker_to_pay AS Miscs_broker_to_pay
 ,misc.inbond_type AS Miscs_inbond_type

 ,invoice.invoice_number AS Invoice_Headers_invoice_number
 ,invoice.supplier AS Invoice_Headers_supplier
 ,invoice.supplier_address AS Invoice_Headers_supplier_address
 ,invoice.inco AS Invoice_Headers_inco
 ,invoice.invoice_total AS Invoice_Headers_invoice_total
 ,invoice.invoice_total_currency AS Invoice_Headers_invoice_total_currency
 ,invoice.line_total AS Invoice_Headers_line_total
 ,invoice.country_of_origin AS Invoice_Headers_country_of_origin
 ,invoice.country_of_export AS Invoice_Headers_country_of_export
 ,invoice.consignee AS Invoice_Headers_consignee
 ,invoice.consignee_address AS Invoice_Headers_consignee_address
 ,invoice.export_date AS Invoice_Headers_export_date
 ,invoice.transaction_related AS Invoice_Headers_transaction_related
 ,invoice.seller AS Invoice_Headers_seller
 ,invoice.sold_to_party AS Invoice_Headers_sold_to_party
 ,invoice.ship_to_party AS Invoice_Headers_ship_to_party
 ,invoice.broker_pga_contact_name AS Invoice_Headers_broker_pga_contact_name
 ,invoice.broker_pga_contact_phone AS Invoice_Headers_broker_pga_contact_phone
 ,invoice.importer AS Invoice_Headers_importer
 ,invoice.manufacturer AS Invoice_Headers_manufacturer
 ,invoice.manufacturer_id AS Invoice_Headers_manufacturer_id
 ,invoice.broker_pga_contact_email AS Invoice_Headers_broker_pga_contact_email

 ,line.line_number AS Invoice_Lines_line_no
 ,line.entry_no AS Invoice_Lines_entry_no
 ,line.product AS Invoice_Lines_product
 ,line.classification AS Invoice_Lines_classification
 ,line.tariff AS Invoice_Lines_tariff
 ,line.customs_qty AS Invoice_Lines_customs_qty
 ,line.customs_qty_unit AS Invoice_Lines_customs_qty_unit
 ,line.goods_description AS Invoice_Lines_goods_description
 ,line.spi AS Invoice_Lines_spi
 ,line.country_of_origin AS Invoice_Lines_country_of_origin
 ,line.country_of_export AS Invoice_Lines_country_of_export
 ,line.destination_state AS Invoice_Lines_destination_state
 ,line.manufacturer AS Invoice_Lines_manufacturer
 ,line.consignee AS Invoice_Lines_consignee
 ,line.sold_to_party AS Invoice_Lines_sold_to_party
 ,line.seller AS Invoice_Lines_seller
 ,line.seller_address AS Invoice_Lines_seller_address
 ,line.gross_weight AS Invoice_Lines_gross_weight
 ,line.gross_weight_unit AS Invoice_Lines_gross_weight_unit
 ,line.price AS Invoice_Lines_price
 ,line.prod_id AS Invoice_Lines_prod_id
 ,line.attribute1 AS Invoice_Lines_attribute1
 ,line.attribute2 AS Invoice_Lines_attribute2
 ,line.attribute3 AS Invoice_Lines_attribute3
 ,line.transaction_related AS Invoice_Lines_transaction_related
 ,line.invoice_qty AS Invoice_Lines_invoice_qty
 ,line.code AS Invoice_Lines_code
 ,line.amount AS Invoice_Lines_amount
 ,line.epa_tsca_toxic_substance_control_act_indicator AS Invoice_Lines_epa_tsca_toxic_substance_control_act_indicator
 ,line.tsca_indicator AS Invoice_Lines_tsca_indicator
 ,line.certifying_individual AS Invoice_Lines_certifying_individual
 ,line.mid AS Invoice_Lines_mid
 ,line.hmf AS Invoice_Lines_hmf
 ,line.unit_price AS Invoice_Lines_unit_price

FROM dbo.imp_vessel_filing_header header
JOIN dbo.imp_vessel_filing_detail detaile
  ON header.id = detaile.filing_header_id
LEFT JOIN dbo.imp_vessel_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.imp_vessel_packing AS packing
  ON packing.filing_header_id = header.id
LEFT JOIN dbo.imp_vessel_misc AS misc
  ON misc.filing_header_id = header.id
LEFT JOIN dbo.imp_vessel_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT OUTER JOIN dbo.imp_vessel_invoice_line line
  ON line.parent_record_id = invoice.id
WHERE header.mapping_status = 2;
GO

PRINT ('create stored procedures')
GO
-- review mapped data
CREATE PROCEDURE dbo.sp_imp_vessel_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM dbo.imp_vessel_form_section_configuration rs
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
    FROM dbo.imp_vessel_form_configuration defValue
    INNER JOIN dbo.imp_vessel_form_section_configuration section
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

-- add declaration record --
CREATE PROCEDURE dbo.sp_imp_vessel_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_vessel_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_vessel_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_vessel_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
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
       ,destination
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,inbnd.container
       ,inbnd.entry_type
       ,inbnd.eta
       ,inbnd.eta
       ,vessel.name
       ,rule_port.entry_port
       ,rule_port.entry_port
       ,states.StateCode
       ,firms_code.firms_code
       ,product_description.name
       ,rule_port.hmf
       ,inbnd.owner_ref
       ,rule_port.destination_code
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_vessel_filing_detail AS detail
      JOIN dbo.imp_vessel_inbound AS inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients AS supplier
        ON inbnd.supplier_id = supplier.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.handbook_vessel AS vessel
        ON vessel.id = inbnd.vessel_id
      LEFT JOIN dbo.imp_vessel_rule_port AS rule_port
        ON inbnd.firms_code_id = rule_port.firms_code_id
      LEFT JOIN dbo.handbook_tariff_product_description AS product_description
        ON inbnd.product_description_id = product_description.id
      LEFT JOIN dbo.app_users_data AS user_data
        ON inbnd.user_id = user_data.UserAccount
      LEFT JOIN cw_firms_codes AS firms_code
        ON firms_code.id = inbnd.firms_code_id
      LEFT JOIN US_States AS states
        ON firms_code.state_id = states.id
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add invoice line record --
CREATE PROCEDURE dbo.sp_imp_vessel_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_vessel_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_vessel_invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_vessel_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
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
       ,invoice_quantity_unit
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,COALESCE(NULLIF(rule_product.goods_description, ''), product_description.name)
       ,state.StateCode
       ,importer.ClientCode
       ,supplier.ClientCode
       ,COALESCE(NULLIF(rule_product.customs_attribute1, ''), product_description.name)
       ,rule_product.customs_attribute2
       ,inbnd.owner_ref
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,product_description.name
       ,tariff.Unit
       ,supplier.ClientCode
       ,importer.ClientCode
       ,inbnd.customs_qty
       ,inbnd.customs_qty
       ,inbnd.unit_price
       ,country.code
       ,inbnd.unit_price * inbnd.customs_qty
       ,COALESCE(NULLIF(rule_product.invoice_uq, ''), tariff.Unit)
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_vessel_filing_detail detail
      JOIN dbo.imp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.handbook_tariff_product_description product_description
        ON inbnd.product_description_id = product_description.id
      LEFT JOIN dbo.Tariff tariff
        ON inbnd.classification_id = tariff.id
      LEFT JOIN dbo.imp_vessel_rule_product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.imp_vessel_rule_port rule_port
        ON inbnd.firms_code_id = rule_port.firms_code_id
      LEFT JOIN dbo.Clients importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON inbnd.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON inbnd.country_of_origin_id = country.id
      LEFT JOIN dbo.cw_firms_codes firms
        ON inbnd.firms_code_id = firms.id
      LEFT JOIN dbo.US_States state
        ON firms.state_id = state.id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add truck import invoice header record --
CREATE PROCEDURE dbo.sp_imp_vessel_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_vessel_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_vessel_invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_vessel_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,supplier
       ,seller
       ,manufacturer
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,country_of_origin
       ,consignee
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,country.code
       ,importer.ClientCode
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_vessel_filing_detail detail
      JOIN dbo.imp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.Clients importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON inbnd.supplier_id = supplier.id
      LEFT JOIN dbo.Countries country
        ON inbnd.country_of_origin_id = country.id
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
    EXEC dbo.sp_imp_vessel_add_invoice_line @filingHeaderId
                                           ,@recordId
                                           ,@filingUser
                                           ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END;
END;
GO
-- add truck import misc record --
CREATE PROCEDURE dbo.sp_imp_vessel_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_vessel_misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_vessel_form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_vessel_misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_vessel_misc (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,branch
       ,[broker]
       ,preparer_dist_port
       ,created_date
       ,created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.[Broker]
       ,user_data.[Location]
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_vessel_filing_detail detail
      JOIN dbo.imp_vessel_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add packing record --
CREATE PROCEDURE dbo.sp_imp_vessel_add_packing (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_vessel_packing';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_vessel_form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add сontainersTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_vessel_packing packing
      WHERE packing.filing_header_id = @parentId)
  BEGIN
    INSERT INTO dbo.imp_vessel_packing (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user)
    VALUES (
      @filingHeaderId
     ,@parentId
     ,@operationId
     ,GETDATE()
     ,@filingUser)
  END;
END;
GO

-- add filing records --
CREATE PROCEDURE dbo.sp_imp_vessel_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_imp_vessel_add_declaration @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add invoice header
  EXEC dbo.sp_imp_vessel_add_invoice_header @filingHeaderId
                                           ,@filingHeaderId
                                           ,@filingUser
                                           ,@operationId
  -- add misc
  EXEC dbo.sp_imp_vessel_add_misc @filingHeaderId
                                 ,@filingHeaderId
                                 ,@filingUser
                                 ,@operationId
  -- add paking
  EXEC dbo.sp_imp_vessel_add_packing @filingHeaderId
                                    ,@filingHeaderId
                                    ,@filingUser
                                    ,@operationId
END;
GO
-- delete filing entry
CREATE PROCEDURE dbo.sp_imp_vessel_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.imp_vessel_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.imp_vessel_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.imp_vessel_form_section_configuration ps
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
CREATE PROCEDURE dbo.sp_imp_vessel_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_imp_vessel_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE imp_vessel_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE imp_vessel_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM imp_vessel_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- update rail filing entry
CREATE PROCEDURE dbo.sp_imp_vessel_update_entry (@json VARCHAR(MAX))
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
    INNER JOIN dbo.imp_vessel_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.imp_vessel_form_section_configuration section
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
CREATE PROCEDURE dbo.sp_imp_vessel_recalculate (@filingHeaderId INT
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
    LEFT JOIN dbo.imp_vessel_form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN dbo.imp_vessel_form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values

  -- quantity, unit_price, api
  DECLARE @tbl AS TABLE (
    record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(18, 6)
  );

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,quantity
     ,unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
     ,CONVERT(DECIMAL(18, 6), b.value) AS unit_price
    FROM @config a
    JOIN @config b
      ON a.record_id = b.record_id
    WHERE a.table_name = 'imp_vessel_invoice_line'
    AND a.column_name = 'invoice_qty'
    AND b.column_name = 'unit_price';

  -- calculate
  DECLARE @tblUpdatedFields field_update_list;
  -- invoice line price
  INSERT INTO @tblUpdatedFields (
      id
     ,record_id
     ,parent_record_id
     ,value)
    SELECT
      id
     ,fields.record_id
     ,fields.parent_record_id
     ,FORMAT(quantity * unit_price, '0.##############')
    FROM @config AS fields
    JOIN @tbl AS tbl
      ON fields.record_id = tbl.record_id
    WHERE table_name = 'imp_vessel_invoice_line'
    AND column_name = 'price';
  -- invoice header invoice total
  DECLARE @total DECIMAL(18, 6);
  SELECT
    @total = SUM(quantity * unit_price)
  FROM @tbl;
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
    WHERE table_name = 'imp_vessel_invoice_header'
    AND column_name IN ('invoice_total', 'line_total');

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
     ,foreign_port.unloco
    FROM @config AS field
    JOIN @config AS field2
      ON field.record_id = field2.record_id
        AND field2.column_name = 'port_of_loading'
    LEFT JOIN dbo.CW_Foreign_Ports foreign_port
      ON field2.value = foreign_port.port_code
    WHERE field.table_name = 'imp_vessel_declaration'
    AND field.column_name IN ('origin', 'loading_unloco');


  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
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
  ON vessel_export_doc.filing_header_id = vessel_export_header.id;
GO

PRINT ('drop old objects')
PRINT ('drop functions and computed columns')
GO
ALTER TABLE dbo.Vessel_Import_Invoice_Lines
DROP COLUMN line_no;
DROP FUNCTION dbo.vessel_import_invoice_line_number;
GO

PRINT ('drop tables')
GO
DROP TABLE dbo.Vessel_Import_Documents
DROP TABLE dbo.Vessel_Import_Declarations
DROP TABLE dbo.Vessel_Import_Packings
DROP TABLE dbo.Vessel_Import_Miscs
DROP TABLE dbo.Vessel_Import_Invoice_Lines
DROP TABLE dbo.Vessel_Import_Invoice_Headers
DROP TABLE dbo.Vessel_Import_Filing_Details
DROP TABLE dbo.Vessel_Import_Filing_Headers
DROP TABLE dbo.Vessel_Imports
DROP TABLE dbo.Vessel_Rule_Importer
DROP TABLE dbo.Vessel_Rule_Port
DROP TABLE dbo.Vessel_Rule_Product
DROP TABLE dbo.Vessel_Import_Def_Values
DROP TABLE dbo.Vessel_Import_Sections
DROP TABLE dbo.Vessel_Import_Def_Values_Manual
DROP TABLE dbo.Vessel_ProductDescriptions
DROP TABLE dbo.Vessel_DischargeTerminals

PRINT ('drop views')
GO
DROP VIEW dbo.v_Vessel_Import_Tables
DROP VIEW dbo.v_Vessel_Import_Def_Values_Manual
DROP VIEW dbo.v_Vessel_Import_Def_Values
DROP VIEW dbo.Vessel_Import_Grid
DROP VIEW dbo.Vessel_Import_Report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.vessel_import_filing
DROP PROCEDURE dbo.vessel_import_add_declaration_record
DROP PROCEDURE dbo.vessel_import_add_packing_record
DROP PROCEDURE dbo.vessel_import_add_misc_record
DROP PROCEDURE dbo.vessel_import_add_invoice_header_record
DROP PROCEDURE dbo.vessel_import_add_invoice_line_record
DROP PROCEDURE dbo.vessel_import_add_def_values_manual
DROP PROCEDURE dbo.vessel_import_apply_def_values
DROP PROCEDURE dbo.vessel_import_filing_del
DROP PROCEDURE dbo.vessel_import_del
DROP PROCEDURE dbo.vessel_import_delete_record
DROP PROCEDURE dbo.vessel_import_filing_param
DROP PROCEDURE dbo.vessel_import_filing_post_save