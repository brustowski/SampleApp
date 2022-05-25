PRINT ('create tables')
GO

CREATE TABLE dbo.imp_pipeline_inbound (
  id INT IDENTITY
 ,importer NVARCHAR(200) NOT NULL
 ,batch VARCHAR(20) NOT NULL
 ,ticket_number VARCHAR(20) NOT NULL
 ,quantity NUMERIC(18, 6) NOT NULL
 ,api NUMERIC(18, 4) NOT NULL
 ,export_date DATETIME NOT NULL
 ,import_date DATETIME NOT NULL
 ,site_name VARCHAR(128) NULL
 ,facility VARCHAR(128) NULL
 ,entry_number VARCHAR(11) NOT NULL DEFAULT ('')
 ,deleted BIT NOT NULL DEFAULT (0)
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

DECLARE @drop_constraints_sql NVARCHAR(MAX) = N'';
SELECT
  @drop_constraints_sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
  + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
  ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.objects
WHERE OBJECT_NAME(parent_object_id) = 'Pipeline_Filing_Headers'
AND (type = 'D'
OR type = 'F');
EXEC (@drop_constraints_sql);
GO

DECLARE @drop_index_sql NVARCHAR(MAX) = N'';
SELECT
  @drop_index_sql += N'
DROP INDEX ' + QUOTENAME(OBJECT_SCHEMA_NAME(indx.object_id))
  + '.' + QUOTENAME(OBJECT_NAME(indx.object_id))
  + '.' + QUOTENAME(indx.name) + ';'
FROM sys.indexes AS indx
WHERE indx.object_id = OBJECT_ID('Pipeline_Filing_Headers')
AND is_primary_key <> 1;
EXEC (@drop_index_sql);
GO

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.ResponseXML'
              ,N'response_xml'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.RequestXML'
              ,N'request_xml'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.MappingStatus'
              ,N'mapping_status'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.FilingStatus'
              ,N'filing_status'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.FilingNumber'
              ,N'filing_number'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.ErrorDescription'
              ,N'error_description'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.CreatedUser'
              ,N'created_user'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers.CreatedDate'
              ,N'created_date'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Filing_Headers'
              ,N'imp_pipeline_filing_header'
              ,'OBJECT'
GO

ALTER TABLE dbo.imp_pipeline_filing_header
ADD DEFAULT (GETDATE()) FOR created_date
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ADD DEFAULT (SUSER_NAME()) FOR created_user
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ALTER COLUMN job_link VARCHAR(8000) NULL
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ALTER COLUMN request_xml VARCHAR(MAX) NULL
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ALTER COLUMN response_xml VARCHAR(MAX) NULL
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ALTER COLUMN created_date DATETIME NOT NULL
GO
ALTER TABLE dbo.imp_pipeline_filing_header
ALTER COLUMN created_user VARCHAR(128) NOT NULL
GO

CREATE TABLE dbo.imp_pipeline_filing_detail (
  inbound_id INT NOT NULL
 ,filing_header_id INT NOT NULL
 ,PRIMARY KEY CLUSTERED (inbound_id, filing_header_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_declaration (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,shipment_type VARCHAR(128) NULL
 ,transport VARCHAR(128) NULL
 ,container VARCHAR(128) NULL
 ,entry_type VARCHAR(128) NULL
 ,rlf VARCHAR(128) NULL
 ,enable_entry_sum VARCHAR(128) NULL
 ,[type] VARCHAR(128) NULL
 ,certify_cargo_release VARCHAR(128) NULL
 ,[service] VARCHAR(128) NULL
 ,issuer VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,batch_ticket VARCHAR(20) NULL
 ,pipeline VARCHAR(128) NULL
 ,trip_id VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,discharge VARCHAR(128) NULL
 ,entry_port VARCHAR(128) NULL
 ,dep DATE NULL
 ,arr DATE NULL
 ,arr2 DATE NULL
 ,hmf VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,country_of_export VARCHAR(128) NULL
 ,eta DATE NULL
 ,export_date DATE NULL
 ,[description] VARCHAR(128) NULL
 ,owner_ref VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL
 ,total_weight VARCHAR(128) NULL
 ,total_volume VARCHAR(128) NULL
 ,no_packages VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,centralized_exam_site VARCHAR(128) NULL
 ,purchased VARCHAR(128) NULL
 ,manual_entry VARCHAR(128) NULL
 ,importer_of_record VARCHAR(128) NULL
 ,split_shipment_release VARCHAR(128) NULL
 ,check_local_client VARCHAR(128) NULL
 ,entry_number VARCHAR(11) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_container (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,bill_type VARCHAR(128) NULL
 ,manifest_qty VARCHAR(128) NULL
 ,uq VARCHAR(128) NULL
 ,bill_issuer_scac VARCHAR(128) NULL
 ,it_number VARCHAR(128) NULL
 ,is_split VARCHAR(128) NULL
 ,bill_number VARCHAR(128) NULL
 ,container_number VARCHAR(128) NULL
 ,pack_qty VARCHAR(128) NULL
 ,pack_type VARCHAR(128) NULL
 ,marks_and_numbers VARCHAR(128) NULL
 ,shipping_symbol VARCHAR(128) NULL
 ,seal_number VARCHAR(128) NULL
 ,[type] VARCHAR(128) NULL
 ,mode VARCHAR(128) NULL
 ,goods_weight VARCHAR(128) NULL
 ,bill_num VARCHAR(128) NULL
 ,packing_uq VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_invoice_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,supplier VARCHAR(128) NULL
 ,supplier_address VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL
 ,invoice_total DECIMAL(28, 15) NULL
 ,curr VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,payment_date VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,consignee_address VARCHAR(128) NULL
 ,inv_date VARCHAR(128) NULL
 ,agreed_place VARCHAR(128) NULL
 ,inv_gross_weight VARCHAR(128) NULL
 ,net_weight VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,export_date DATE NULL
 ,first_sale VARCHAR(128) NULL
 ,transaction_related VARCHAR(128) NULL
 ,packages VARCHAR(128) NULL
 ,manufacturer VARCHAR(128) NULL
 ,seller VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,ship_to_party VARCHAR(128) NULL
 ,broker_pga_contact_name VARCHAR(128) NULL
 ,broker_pga_contact_phone VARCHAR(128) NULL
 ,broker_pga_contact_email VARCHAR(128) NULL
 ,epa_pst_cert_date VARCHAR(128) NULL
 ,epa_tsca_cert_date VARCHAR(128) NULL
 ,epa_vne_cert_date VARCHAR(128) NULL
 ,fsis_cert_date VARCHAR(128) NULL
 ,fws_cert_date VARCHAR(128) NULL
 ,lacey_act_cert_date VARCHAR(128) NULL
 ,nhtsa_cert_date VARCHAR(128) NULL
 ,landed_costing_ex_rate VARCHAR(128) NULL
 ,invoice_no VARCHAR(128) NULL
 ,manufacturer_address VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_pipeline_invoice_line (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,invoice_no VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,customs_qty NUMERIC(18, 6) NULL
 ,line_price DECIMAL(28, 15) NULL
 ,goods_description VARCHAR(128) NULL
 ,org VARCHAR(128) NULL
 ,spi VARCHAR(128) NULL
 ,gr_weight NUMERIC(18, 6) NULL
 ,gr_weight_unit VARCHAR(2) NULL
 ,uq VARCHAR(128) NULL
 ,price_unit DECIMAL(28, 15) NULL
 ,prod_id1 VARCHAR(128) NULL
 ,attribute1 VARCHAR(128) NULL
 ,attribute2 DECIMAL(18, 6) NULL
 ,attribute3 VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,dest_state VARCHAR(128) NULL
 ,transaction_related VARCHAR(128) NULL
 ,invoice_qty NUMERIC(18, 6) NULL
 ,invoice_qty_unit VARCHAR(128) NULL
 ,manufacturer VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,code VARCHAR(128) NULL
 ,curr VARCHAR(128) NULL
 ,cif_component VARCHAR(128) NULL
 ,epa_tsca_toxic_substance_control_act_indicator VARCHAR(128) NULL
 ,tsca_indicator VARCHAR(128) NULL
 ,certifying_individual VARCHAR(128) NULL
 ,pga_contact_name VARCHAR(128) NULL
 ,pga_contact_phone VARCHAR(128) NULL
 ,pga_contact_email VARCHAR(128) NULL
 ,amount DECIMAL(18, 2) NULL
 ,[description] VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_pipeline_misc (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,branch VARCHAR(128) NULL
 ,[broker] VARCHAR(128) NULL
 ,merge_by VARCHAR(128) NULL
 ,tax_deferrable_ind VARCHAR(128) NULL
 ,preparer_dist_port VARCHAR(128) NULL
 ,recon_issue VARCHAR(128) NULL
 ,fta_recon VARCHAR(128) NULL
 ,bond_type VARCHAR(128) NULL
 ,payment_type VARCHAR(128) NULL
 ,broker_to_pay VARCHAR(128) NULL
 ,prelim_statement_date VARCHAR(128) NULL
 ,submitter VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

DECLARE @drop_constraints_sql NVARCHAR(MAX) = N'';
SELECT
  @drop_constraints_sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
  + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
  ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.objects
WHERE OBJECT_NAME(parent_object_id) = 'Pipeline_Documents'
AND (type = 'D'
OR type = 'F');
EXEC (@drop_constraints_sql);
GO

DECLARE @drop_index_sql NVARCHAR(MAX) = N'';
SELECT
  @drop_index_sql += N'
DROP INDEX ' + QUOTENAME(OBJECT_SCHEMA_NAME(indx.object_id))
  + '.' + QUOTENAME(OBJECT_NAME(indx.object_id))
  + '.' + QUOTENAME(indx.name) + ';'
FROM sys.indexes AS indx
WHERE indx.object_id = OBJECT_ID('Pipeline_Documents')
AND is_primary_key <> 1;
EXEC (@drop_index_sql);
GO

EXEC sp_rename N'dbo.Pipeline_Documents.Status'
              ,N'status'
              ,'COLUMN'
EXEC sp_rename N'dbo.Pipeline_Documents.Filing_Headers_FK'
              ,N'filing_header_id'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents.filename'
              ,N'file_name'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents.file_desc'
              ,N'file_description'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents.DocumentType'
              ,N'document_type'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents.CreatedUser'
              ,N'created_user'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents.CreatedDate'
              ,N'created_date'
              ,'COLUMN'

EXEC sp_rename N'dbo.Pipeline_Documents'
              ,N'imp_pipeline_document'
              ,'OBJECT'
GO

ALTER TABLE dbo.imp_pipeline_document
ADD DEFAULT (GETDATE()) FOR created_date
GO
ALTER TABLE dbo.imp_pipeline_document
ADD DEFAULT (SUSER_NAME()) FOR created_user
GO
ALTER TABLE dbo.imp_pipeline_document
ALTER COLUMN status VARCHAR(50)
GO
ALTER TABLE dbo.imp_pipeline_document
ALTER COLUMN created_date DATETIME NOT NULL
GO
ALTER TABLE dbo.imp_pipeline_document
ALTER COLUMN created_user VARCHAR(128) NOT NULL
GO

CREATE TABLE dbo.imp_pipeline_rule_api (
  id INT IDENTITY
 ,api NVARCHAR(128) NOT NULL
 ,tariff NVARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_rule_batch_code (
  id INT IDENTITY
 ,batch_code VARCHAR(128) NOT NULL
 ,product VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_rule_consignee_importer (
  id INT IDENTITY
 ,ticket_importer VARCHAR(128) NOT NULL
 ,importer_code VARCHAR(128) NOT NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_rule_facility (
  id INT IDENTITY
 ,facility VARCHAR(128) NOT NULL
 ,[port] VARCHAR(128) NOT NULL
 ,destination VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,issuer VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,pipeline VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_rule_importer (
  id INT IDENTITY
 ,consignee VARCHAR(128) NULL
 ,country_of_export VARCHAR(128) NULL
 ,fta_recon VARCHAR(128) NULL
 ,importer VARCHAR(128) NOT NULL
 ,manufacturer VARCHAR(128) NULL
 ,manufacturer_address VARCHAR(128) NULL
 ,mid VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,recon_issue VARCHAR(128) NULL
 ,seller VARCHAR(128) NULL
 ,spi VARCHAR(128) NULL
 ,supplier VARCHAR(128) NULL
 ,transaction_related VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_rule_price (
  id INT IDENTITY
 ,importer_id UNIQUEIDENTIFIER NOT NULL
 ,crude_type_id INT NULL
 ,pricing DECIMAL(28, 15) NOT NULL
 ,freight DECIMAL(18, 6) NOT NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_pipeline_form_section_configuration (
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

CREATE TABLE dbo.imp_pipeline_form_configuration (
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

PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_inbound ON;
INSERT INTO dbo.imp_pipeline_inbound (
    id
   ,importer
   ,batch
   ,ticket_number
   ,quantity
   ,api
   ,export_date
   ,import_date
   ,site_name
   ,facility
   ,entry_number
   ,deleted
   ,created_date
   ,created_user)
  SELECT
    ti.Id
   ,ti.Importer
   ,ti.Batch
   ,ti.TicketNumber
   ,ti.Quantity
   ,ti.API
   ,ti.ExportDate
   ,ti.ImportDate
   ,ti.SiteName
   ,ti.Facility
   ,ti.entry_number
   ,ti.FDeleted
   ,ti.CreatedDate
   ,ti.CreatedUser
  FROM dbo.Pipeline_Inbound ti;
SET IDENTITY_INSERT dbo.imp_pipeline_inbound OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.imp_pipeline_filing_detail (
    inbound_id
   ,filing_header_id)
  SELECT
    fd.Pipeline_Inbounds_FK
   ,fd.Filing_Headers_FK
  FROM dbo.Pipeline_Filing_Details fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_declaration ON;
INSERT INTO dbo.imp_pipeline_declaration (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,main_supplier
   ,importer
   ,shipment_type
   ,transport
   ,container
   ,entry_type
   ,rlf
   ,enable_entry_sum
   ,type
   ,certify_cargo_release
   ,service
   ,issuer
   ,master_bill
   ,batch_ticket
   ,pipeline
   ,trip_id
   ,carrier_scac
   ,discharge
   ,entry_port
   ,dep
   ,arr
   ,arr2
   ,hmf
   ,origin
   ,destination
   ,destination_state
   ,country_of_export
   ,eta
   ,export_date
   ,description
   ,owner_ref
   ,inco
   ,total_weight
   ,total_volume
   ,no_packages
   ,firms_code
   ,centralized_exam_site
   ,purchased
   ,manual_entry
   ,importer_of_record
   ,split_shipment_release
   ,check_local_client
   ,entry_number
   ,created_date
   ,created_user)
  SELECT
    declaration.id
   ,declaration.Filing_Headers_FK
   ,declaration.Filing_Headers_FK
   ,NULL
   ,declaration.Main_Supplier
   ,declaration.Importer
   ,declaration.Shipment_Type
   ,declaration.Transport
   ,declaration.Container
   ,declaration.Entry_Type
   ,declaration.RLF
   ,declaration.Enable_Entry_Sum
   ,declaration.Type
   ,declaration.Certify_Cargo_Release
   ,declaration.Service
   ,declaration.Issuer
   ,declaration.Master_Bill
   ,declaration.Batch_Ticket
   ,declaration.Pipeline
   ,declaration.TripID
   ,declaration.Carrier_SCAC
   ,declaration.Discharge
   ,declaration.Entry_Port
   ,declaration.Dep
   ,declaration.Arr
   ,declaration.Arr_2
   ,declaration.HMF
   ,declaration.Origin
   ,declaration.Destination
   ,declaration.Destination_State
   ,declaration.Country_of_Export
   ,declaration.ETA
   ,declaration.Export_Date
   ,declaration.Description
   ,declaration.Owner_Ref
   ,declaration.INCO
   ,declaration.Total_Weight
   ,declaration.Total_Volume
   ,declaration.No_Packages
   ,declaration.FIRMs_Code
   ,declaration.Centralized_Exam_Site
   ,declaration.Purchased
   ,declaration.Manual_Entry
   ,declaration.Importer_of_record
   ,declaration.Split_Shipment_Release
   ,declaration.Check_Local_Client
   ,declaration.EntryNumber
   ,declaration.CreatedDate
   ,declaration.CreatedUser
  FROM dbo.Pipeline_DeclarationTab declaration;
SET IDENTITY_INSERT dbo.imp_pipeline_declaration OFF;
GO

PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_container ON;
INSERT INTO dbo.imp_pipeline_container (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,bill_type
   ,manifest_qty
   ,uq
   ,bill_issuer_scac
   ,it_number
   ,is_split
   ,bill_number
   ,container_number
   ,pack_qty
   ,pack_type
   ,marks_and_numbers
   ,shipping_symbol
   ,seal_number
   ,type
   ,mode
   ,goods_weight
   ,bill_num
   ,packing_uq
   ,created_date
   ,created_user)
  SELECT
    container.id
   ,container.Filing_Headers_FK
   ,container.Filing_Headers_FK
   ,NULL
   ,container.Bill_Type
   ,container.Manifest_QTY
   ,container.UQ
   ,container.Bill_Issuer_SCAC
   ,container.IT_Number
   ,container.Is_Split
   ,container.Bill_Number
   ,container.Container_Number
   ,container.Pack_QTY
   ,container.Pack_Type
   ,container.Marks_and_Numbers
   ,container.Shipping_Symbol
   ,container.Seal_Number
   ,container.Type
   ,container.Mode
   ,container.Goods_Weight
   ,container.Bill_Num
   ,container.Packing_UQ
   ,container.CreatedDate
   ,container.CreatedUser
  FROM dbo.Pipeline_ContainersTab container;
SET IDENTITY_INSERT dbo.imp_pipeline_container OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_invoice_header ON;
INSERT INTO dbo.imp_pipeline_invoice_header (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,supplier
   ,supplier_address
   ,inco
   ,invoice_total
   ,curr
   ,origin
   ,payment_date
   ,consignee
   ,consignee_address
   ,inv_date
   ,agreed_place
   ,inv_gross_weight
   ,net_weight
   ,export
   ,export_date
   ,first_sale
   ,transaction_related
   ,packages
   ,manufacturer
   ,seller
   ,importer
   ,sold_to_party
   ,ship_to_party
   ,broker_pga_contact_name
   ,broker_pga_contact_phone
   ,broker_pga_contact_email
   ,epa_pst_cert_date
   ,epa_tsca_cert_date
   ,epa_vne_cert_date
   ,fsis_cert_date
   ,fws_cert_date
   ,lacey_act_cert_date
   ,nhtsa_cert_date
   ,landed_costing_ex_rate
   ,invoice_no
   ,manufacturer_address
   ,created_date
   ,created_user)
  SELECT
    invoice.id
   ,invoice.Filing_Headers_FK
   ,invoice.Filing_Headers_FK
   ,NULL
   ,invoice.Supplier
   ,invoice.Supplier_Address
   ,invoice.INCO
   ,invoice.Invoice_Total
   ,invoice.Curr
   ,invoice.Origin
   ,invoice.Payment_Date
   ,invoice.Consignee
   ,invoice.Consignee_Address
   ,invoice.Inv_Date
   ,invoice.Agreed_Place
   ,invoice.Inv_Gross_Weight
   ,invoice.Net_Weight
   ,invoice.Export
   ,invoice.Export_Date
   ,invoice.First_Sale
   ,invoice.Transaction_Related
   ,invoice.Packages
   ,invoice.Manufacturer
   ,invoice.Seller
   ,invoice.Importer
   ,invoice.Sold_to_party
   ,invoice.Ship_to_party
   ,invoice.Broker_PGA_Contact_Name
   ,invoice.Broker_PGA_Contact_Phone
   ,invoice.Broker_PGA_Contact_Email
   ,invoice.EPA_PST_Cert_Date
   ,invoice.EPA_TSCA_Cert_Date
   ,invoice.EPA_VNE_Cert_Date
   ,invoice.FSIS_Cert_Date
   ,invoice.FWS_Cert_Date
   ,invoice.LACEY_ACT_Cert_Date
   ,invoice.NHTSA_Cert_Date
   ,invoice.Landed_Costing_Ex_Rate
   ,invoice.Invoice_No
   ,invoice.Manufacturer_Address
   ,invoice.CreatedDate
   ,invoice.CreatedUser
  FROM dbo.Pipeline_InvoiceHeaders invoice;
SET IDENTITY_INSERT dbo.imp_pipeline_invoice_header OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_invoice_line ON;
INSERT INTO dbo.imp_pipeline_invoice_line (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,invoice_no
   ,tariff
   ,customs_qty
   ,line_price
   ,goods_description
   ,org
   ,spi
   ,gr_weight
   ,gr_weight_unit
   ,uq
   ,price_unit
   ,prod_id1
   ,attribute1
   ,attribute2
   ,attribute3
   ,export
   ,origin
   ,dest_state
   ,transaction_related
   ,invoice_qty
   ,invoice_qty_unit
   ,manufacturer
   ,consignee
   ,sold_to_party
   ,code
   ,curr
   ,cif_component
   ,epa_tsca_toxic_substance_control_act_indicator
   ,tsca_indicator
   ,certifying_individual
   ,pga_contact_name
   ,pga_contact_phone
   ,pga_contact_email
   ,amount
   ,description
   ,created_date
   ,created_user)
  SELECT
    line.id
   ,line.Filing_Headers_FK
   ,line.InvoiceHeaders_FK
   ,NULL
   ,line.Invoice_No
   ,line.Tariff
   ,line.Customs_QTY
   ,line.Line_Price
   ,line.Goods_Description
   ,line.ORG
   ,line.SPI
   ,line.Gr_Weight
   ,line.Gr_Weight_Unit
   ,line.UQ
   ,line.PriceUnit
   ,line.Prod_ID_1
   ,line.Attribute_1
   ,line.Attribute_2
   ,line.Attribute_3
   ,line.Export
   ,line.Origin
   ,line.Dest_State
   ,line.Transaction_Related
   ,line.Invoice_Qty
   ,line.Invoice_Qty_Unit
   ,line.Manufacturer
   ,line.Consignee
   ,line.Sold_to_party
   ,line.Code
   ,line.Curr
   ,line.CIF_Component
   ,line.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
   ,line.TSCA_Indicator
   ,line.Certifying_Individual
   ,line.PGA_Contact_Name
   ,line.PGA_Contact_Phone
   ,line.PGA_Contact_Email
   ,line.Amount
   ,line.Description
   ,line.CreatedDate
   ,line.CreatedUser
  FROM dbo.Pipeline_InvoiceLines line
SET IDENTITY_INSERT dbo.imp_pipeline_invoice_line OFF;
GO

PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_misc ON;
INSERT INTO dbo.imp_pipeline_misc (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,branch
   ,broker
   ,merge_by
   ,tax_deferrable_ind
   ,preparer_dist_port
   ,recon_issue
   ,fta_recon
   ,bond_type
   ,payment_type
   ,broker_to_pay
   ,prelim_statement_date
   ,submitter
   ,created_date
   ,created_user)
  SELECT
    misc.id
   ,misc.Filing_Headers_FK
   ,misc.Filing_Headers_FK
   ,NULL
   ,misc.Branch
   ,misc.Broker
   ,misc.Merge_By
   ,misc.Tax_Deferrable_Ind
   ,misc.Preparer_Dist_Port
   ,misc.Recon_Issue
   ,misc.FTA_Recon
   ,misc.Bond_Type
   ,misc.Payment_Type
   ,misc.Broker_to_Pay
   ,misc.Prelim_Statement_Date
   ,misc.Submitter
   ,misc.CreatedDate
   ,misc.CreatedUser
  FROM dbo.Pipeline_MISC misc
SET IDENTITY_INSERT dbo.imp_pipeline_misc OFF;
GO


PRINT ('copy api rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_api ON;
INSERT INTO dbo.imp_pipeline_rule_api (
    id
   ,api
   ,tariff
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.API
   ,r.Tariff
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_API r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_api OFF;
GO

PRINT ('copy batch code rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_batch_code ON;
INSERT INTO dbo.imp_pipeline_rule_batch_code (
    id
   ,batch_code
   ,product
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.batch_code
   ,r.product
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_BatchCode r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_batch_code OFF;
GO

PRINT ('copy consignee-importer rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_consignee_importer ON;
INSERT INTO dbo.imp_pipeline_rule_consignee_importer (
    id
   ,ticket_importer
   ,importer_code
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.ticket_importer
   ,r.importer_code
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_Consignee_Importer r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_consignee_importer OFF;
GO

PRINT ('copy facility rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_facility ON;
INSERT INTO dbo.imp_pipeline_rule_facility (
    id
   ,facility
   ,port
   ,destination
   ,destination_state
   ,firms_code
   ,issuer
   ,origin
   ,pipeline
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.facility
   ,r.port
   ,r.destination
   ,r.destination_state
   ,r.firms_code
   ,r.issuer
   ,r.origin
   ,r.pipeline
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_Facility r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_facility OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_importer ON;
INSERT INTO dbo.imp_pipeline_rule_importer (
    id
   ,consignee
   ,country_of_export
   ,fta_recon
   ,importer
   ,manufacturer
   ,manufacturer_address
   ,mid
   ,origin
   ,recon_issue
   ,seller
   ,spi
   ,supplier
   ,transaction_related
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.consignee
   ,r.country_of_export
   ,r.fta_recon
   ,r.importer
   ,r.manufacturer
   ,r.manufacturer_address
   ,r.mid
   ,r.origin
   ,r.recon_issue
   ,r.seller
   ,r.spi
   ,r.supplier
   ,r.transaction_related
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_Importer r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_importer OFF;
GO

PRINT ('copy price rule data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_rule_price ON;
INSERT INTO dbo.imp_pipeline_rule_price (
    id
   ,importer_id
   ,crude_type_id
   ,pricing
   ,freight
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.importer_id
   ,r.crude_type_id
   ,r.pricing
   ,r.freight
   ,r.created_date
   ,r.created_user
  FROM dbo.Pipeline_Rule_Price r;
SET IDENTITY_INSERT dbo.imp_pipeline_rule_price OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_form_section_configuration ON;
INSERT INTO dbo.imp_pipeline_form_section_configuration (
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
  FROM dbo.Pipeline_sections section;
SET IDENTITY_INSERT dbo.imp_pipeline_form_section_configuration OFF;
GO

UPDATE imp_pipeline_form_section_configuration
SET table_name = 'imp_pipeline_declaration'
   ,procedure_name = 'sp_imp_pipeline_add_declaration'
WHERE table_name = 'Pipeline_DeclarationTab';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'imp_pipeline_invoice_header'
   ,procedure_name = 'sp_imp_pipeline_add_invoice_header'
WHERE table_name = 'Pipeline_InvoiceHeaders';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'imp_pipeline_invoice_line'
   ,procedure_name = 'sp_imp_pipeline_add_invoice_line'
WHERE table_name = 'Pipeline_InvoiceLines';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'imp_pipeline_container'
   ,procedure_name = 'sp_imp_pipeline_add_container'
WHERE table_name = 'Pipeline_ContainersTab';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'imp_pipeline_misc'
   ,procedure_name = 'sp_imp_pipeline_add_misc'
WHERE table_name = 'Pipeline_MISC';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.imp_pipeline_form_configuration ON;
INSERT INTO dbo.imp_pipeline_form_configuration (
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
   ,dv.ColName
   ,dv.[ValueLabel]
   ,dv.[ValueDesc]
   ,dv.DefValue
   ,dv.FHasDefaultVal
   ,dv.FEditable
   ,dv.FMandatory
   ,dv.Display_on_UI
   ,dv.[FManual]
   ,dv.SingleFilingOrder
   ,dv.paired_field_table
   ,dv.paired_field_column
   ,dv.handbook_name
   ,dv.CreatedDate
   ,COALESCE(dv.CreatedUser, SUSER_NAME())
  FROM dbo.Pipeline_DEFValues dv;
SET IDENTITY_INSERT dbo.imp_pipeline_form_configuration OFF;
GO

UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'arr2'
WHERE column_name = 'Arr_2'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'trip_id'
WHERE column_name = 'TripID'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'entry_number'
WHERE column_name = 'EntryNumber'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'attribute1'
WHERE column_name = 'Attribute_1'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'attribute2'
WHERE column_name = 'Attribute_2'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'attribute3'
WHERE column_name = 'Attribute_3'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'prod_id1'
WHERE column_name = 'Prod_ID_1'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'price_unit'
WHERE column_name = 'PriceUnit'
AND section_id = 5;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.imp_pipeline_filing_header
ADD CONSTRAINT [FK__imp_pipeline_filing_header__FilingStatus__filing_status] FOREIGN KEY (filing_status) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.imp_pipeline_filing_header
ADD CONSTRAINT [FK__imp_pipeline_filing_header__MappingStatus__mapping_status] FOREIGN KEY (mapping_status) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.imp_pipeline_filing_detail
ADD CONSTRAINT FK__imp_pipeline_filing_detail__imp_pipeline_inbound__inbound_id FOREIGN KEY (inbound_id) REFERENCES dbo.imp_pipeline_inbound (id)
GO

ALTER TABLE dbo.imp_pipeline_filing_detail
ADD CONSTRAINT FK__imp_pipeline_filing_detail__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id)
GO

ALTER TABLE dbo.imp_pipeline_declaration
ADD CONSTRAINT FK__imp_pipeline_declaration__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_container
ADD CONSTRAINT FK__imp_pipeline_container__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_invoice_header
ADD CONSTRAINT FK__imp_pipeline_invoice_header__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_invoice_line
ADD CONSTRAINT FK__imp_pipeline_invoice_line__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id)
GO

ALTER TABLE dbo.imp_pipeline_invoice_line
ADD CONSTRAINT FK__imp_pipeline_invoice_line__imp_pipeline_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.imp_pipeline_invoice_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_misc
ADD CONSTRAINT FK__imp_pipeline_misc__imp_Pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_rule_price
ADD CONSTRAINT FK__imp_pipeline_rule_price__Clients__importer_id FOREIGN KEY (importer_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.imp_pipeline_rule_price
ADD CONSTRAINT FK__imp_pipeline_rule_price__imp_pipeline_rule_batch_code__crude_type_id FOREIGN KEY (crude_type_id) REFERENCES dbo.imp_pipeline_rule_batch_code (id)
GO

ALTER TABLE dbo.imp_pipeline_document
ADD CONSTRAINT [FK__imp_pipeline_document__imp_pipeline_inbound__inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.imp_pipeline_inbound (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_document
ADD CONSTRAINT FK__imp_pipeline_document__imp_pipeline_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_pipeline_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_pipeline_form_section_configuration
ADD CONSTRAINT FK__imp_pipeline_form_section_configuration__imp_pipeline_form_section_configuration__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.imp_pipeline_form_section_configuration (id)
GO

ALTER TABLE dbo.imp_pipeline_form_configuration
ADD CONSTRAINT FK__imp_pipeline_form_configuration__imp_pipeline_form_section_configuration__section_id FOREIGN KEY (section_id) REFERENCES dbo.imp_pipeline_form_section_configuration (id)
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
FROM imp_pipeline_form_configuration field
JOIN imp_pipeline_form_section_configuration section
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
CREATE INDEX Idx__importer
ON dbo.imp_pipeline_inbound (importer)
ON [PRIMARY]
GO

CREATE INDEX Idx__batch
ON dbo.imp_pipeline_inbound (batch)
ON [PRIMARY]
GO

CREATE INDEX Idx__facility
ON dbo.imp_pipeline_inbound (facility)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_status
ON dbo.imp_pipeline_filing_header (filing_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__mapping_status
ON dbo.imp_pipeline_filing_header (mapping_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_filing_detail (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_id
ON dbo.imp_pipeline_filing_detail (inbound_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_container (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON dbo.imp_pipeline_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_misc (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_pipeline_document (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_record_id
ON dbo.imp_pipeline_document (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__batch_code
ON dbo.imp_pipeline_rule_batch_code (batch_code)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__ticket_importer
ON dbo.imp_pipeline_rule_consignee_importer (ticket_importer)
ON [PRIMARY]
GO

CREATE INDEX Idx__facility
ON dbo.imp_pipeline_rule_facility (facility)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__importer
ON dbo.imp_pipeline_rule_importer (importer)
ON [PRIMARY]
GO

CREATE INDEX Idx__crude_type_id
ON dbo.imp_pipeline_rule_price (crude_type_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__importer_id
ON dbo.imp_pipeline_rule_price (importer_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__name
ON dbo.imp_pipeline_form_section_configuration (name)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_id
ON dbo.imp_pipeline_form_section_configuration (parent_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__section_id
ON dbo.imp_pipeline_form_configuration (section_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets truck export invoice header number
CREATE FUNCTION dbo.fn_imp_pipeline_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      line.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY line.id)
    FROM dbo.imp_pipeline_invoice_line line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.imp_pipeline_invoice_line
ADD invoice_line_number AS (dbo.fn_imp_pipeline_invoice_line_number(parent_record_id, id));
ALTER TABLE dbo.imp_pipeline_invoice_line
ADD gr_weight_tons AS (dbo.fn_app_weight_to_ton(gr_weight, gr_weight_unit))
GO
-- gets batch code from inbound batch
CREATE FUNCTION dbo.fn_imp_pipeline_batch_code (@batch VARCHAR(20))
RETURNS VARCHAR(3)
AS
BEGIN
  DECLARE @result VARCHAR(3)
  SELECT
    @result = SUBSTRING(@batch, 0, CHARINDEX('-', @batch))
  RETURN @result;
END
GO
-- gets importer code by filing header id
CREATE FUNCTION dbo.fn_imp_pipeline_importer_code (@filingHeaderId INT)
RETURNS VARCHAR(128)
AS
BEGIN
  DECLARE @ImporterCode VARCHAR(128);

  SET @ImporterCode = (SELECT
      CASE
        WHEN importer_lookup.importer_code IS NULL THEN inbnd.Importer
        ELSE importer_lookup.importer_code
      END
    FROM dbo.imp_pipeline_filing_detail detail
    JOIN dbo.imp_pipeline_inbound inbnd
      ON detail.inbound_id = inbnd.id
    LEFT JOIN dbo.imp_pipeline_rule_consignee_importer importer_lookup
      ON inbnd.importer = importer_lookup.ticket_importer
    WHERE detail.filing_header_id = @filingHeaderId);

  RETURN @ImporterCode;
END;
GO

CREATE FUNCTION dbo.fn_imp_pipeline_weight (@bbl NUMERIC(18, 6) = 0,
@api NUMERIC(18, 6) = 0)
RETURNS NUMERIC(18, 6)
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = 0
  SELECT
    @result = (@bbl * 42 * (((141.5 / (131.5 + @api)) * 8.338426855) - 0.0101578)) * 0.453592 / 1000
  RETURN @result
END
GO

PRINT ('create views')
GO
CREATE VIEW dbo.v_imp_pipeline_form_configuration
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
FROM dbo.imp_pipeline_form_configuration form
INNER JOIN dbo.imp_pipeline_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL;
GO

CREATE VIEW dbo.v_imp_pipeline_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN imp_pipeline_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id');
GO

CREATE VIEW dbo.v_imp_pipeline_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.importer
 ,inbnd.batch
 ,inbnd.ticket_number
 ,inbnd.facility
 ,inbnd.site_name
 ,inbnd.quantity
 ,inbnd.api
 ,inbnd.entry_number
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.created_date
 ,inbnd.[deleted]
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
        FROM dbo.imp_pipeline_rule_importer rule_importer
        WHERE inbnd.importer = rule_importer.importer) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_batch_code rule_batch
        WHERE dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code) THEN 1
    ELSE 0
  END AS has_batch_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_facility rule_facility
        WHERE inbnd.facility = rule_facility.facility) THEN 1
    ELSE 0
  END AS has_facility_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM dbo.imp_pipeline_rule_price rule_price
        INNER JOIN dbo.Clients clients
          ON rule_price.importer_id = clients.id
        WHERE inbnd.importer = clients.ClientCode
        AND clients.id = rule_price.importer_id) THEN 1
    ELSE 0
  END AS has_price_rule
FROM dbo.imp_pipeline_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_pipeline_filing_header etfh
  JOIN dbo.imp_pipeline_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
WHERE inbnd.deleted = 0;
GO

CREATE VIEW dbo.v_imp_pipeline_report
AS
SELECT
  header.id AS Pipeline_Filing_Headers_id
 ,detail.inbound_id AS PI_ID

 ,declaration.arr AS Pipeline_DeclarationTab_Arr
 ,declaration.arr2 AS Pipeline_DeclarationTab_Arr_2
 ,declaration.carrier_scac AS Pipeline_DeclarationTab_Carrier_SCAC
 ,declaration.centralized_exam_site AS Pipeline_DeclarationTab_Centralized_Exam_Site
 ,declaration.certify_cargo_release AS Pipeline_DeclarationTab_Certify_Cargo_Release
 ,declaration.check_local_client AS Pipeline_DeclarationTab_Check_Local_Client
 ,declaration.container AS Pipeline_DeclarationTab_Container
 ,declaration.country_of_export AS Pipeline_DeclarationTab_Country_of_Export
 ,declaration.dep AS Pipeline_DeclarationTab_Dep
 ,declaration.[description] AS Pipeline_DeclarationTab_Description
 ,declaration.destination AS Pipeline_DeclarationTab_Destination
 ,declaration.destination_state AS Pipeline_DeclarationTab_Destination_State
 ,declaration.discharge AS Pipeline_DeclarationTab_Discharge
 ,declaration.enable_entry_sum AS Pipeline_DeclarationTab_Enable_Entry_Sum
 ,declaration.entry_port AS Pipeline_DeclarationTab_Entry_Port
 ,declaration.entry_type AS Pipeline_DeclarationTab_Entry_Type
 ,declaration.eta AS Pipeline_DeclarationTab_ETA
 ,declaration.export_date AS Pipeline_DeclarationTab_Export_Date
 ,declaration.firms_code AS Pipeline_DeclarationTab_FIRMs_Code
 ,declaration.hmf AS Pipeline_DeclarationTab_HMF
 ,declaration.importer AS Pipeline_DeclarationTab_Importer
 ,declaration.importer_of_record AS Pipeline_DeclarationTab_Importer_of_record
 ,declaration.inco AS Pipeline_DeclarationTab_INCO
 ,declaration.issuer AS Pipeline_DeclarationTab_Issuer
 ,declaration.main_supplier AS Pipeline_DeclarationTab_Main_Supplier
 ,declaration.manual_entry AS Pipeline_DeclarationTab_Manual_Entry
 ,declaration.master_bill AS Pipeline_DeclarationTab_Master_Bill
 ,declaration.no_packages AS Pipeline_DeclarationTab_No_Packages
 ,declaration.origin AS Pipeline_DeclarationTab_Origin
 ,declaration.owner_ref AS Pipeline_DeclarationTab_Owner_Ref
 ,declaration.purchased AS Pipeline_DeclarationTab_Purchased
 ,declaration.rlf AS Pipeline_DeclarationTab_RLF
 ,declaration.[service] AS Pipeline_DeclarationTab_Service
 ,declaration.shipment_type AS Pipeline_DeclarationTab_Shipment_Type
 ,declaration.split_shipment_release AS Pipeline_DeclarationTab_Split_Shipment_Release
 ,declaration.total_volume AS Pipeline_DeclarationTab_Total_Volume
 ,declaration.total_weight AS Pipeline_DeclarationTab_Total_Weight
 ,declaration.transport AS Pipeline_DeclarationTab_Transport
 ,declaration.[type] AS Pipeline_DeclarationTab_Type
 ,declaration.pipeline AS Pipeline_DeclarationTab_Pipeline
 ,declaration.entry_number AS Pipeline_DeclarationTab_EntryNumber

 ,container.bill_issuer_scac AS Pipeline_Packing_Bill_Issuer_SCAC
 ,container.bill_number AS Pipeline_Packing_Bill_Number
 ,container.bill_type AS Pipeline_Packing_Bill_Type
 ,container.container_number AS Pipeline_Packing_Container_Number
 ,container.is_split AS Pipeline_Packing_Is_Split
 ,container.it_number AS Pipeline_Packing_IT_Number
 ,container.manifest_qty AS Pipeline_Packing_Manifest_QTY
 ,container.marks_and_numbers AS Pipeline_Packing_Marks_and_Numbers
 ,container.pack_qty AS Pipeline_Packing_Pack_QTY
 ,container.pack_type AS Pipeline_Packing_Pack_Type
 ,container.shipping_symbol AS Pipeline_Packing_Shipping_Symbol
 ,container.uq AS Pipeline_Packing_UQ
 ,container.packing_uq AS Pipeline_Packing_Container_Packing_UQ
 ,container.seal_number AS Pipeline_Packing_Seal_Number
 ,container.[type] AS Pipeline_Packing_Type
 ,container.mode AS Pipeline_Packing_Mode
 ,container.goods_weight AS Pipeline_Packing_Goods_Weight
 ,container.bill_num AS Pipeline_Packing_Bill_Num

 ,invoice.agreed_place AS Pipeline_InvoiceHeaders_Agreed_Place
 ,invoice.broker_pga_contact_email AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invoice.broker_pga_contact_name AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invoice.broker_pga_contact_phone AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invoice.consignee AS Pipeline_InvoiceHeaders_Consignee
 ,invoice.consignee_address AS Pipeline_InvoiceHeaders_Consignee_Address
 ,invoice.curr AS Pipeline_InvoiceHeaders_Curr
 ,invoice.epa_pst_cert_date AS Pipeline_InvoiceHeaders_EPA_PST_Cert_Date
 ,invoice.epa_tsca_cert_date AS Pipeline_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invoice.epa_vne_cert_date AS Pipeline_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invoice.export AS Pipeline_InvoiceHeaders_Export
 ,invoice.export_date AS Pipeline_InvoiceHeaders_Export_Date
 ,invoice.first_sale AS Pipeline_InvoiceHeaders_First_Sale
 ,invoice.fsis_cert_date AS Pipeline_InvoiceHeaders_FSIS_Cert_Date
 ,invoice.fws_cert_date AS Pipeline_InvoiceHeaders_FWS_Cert_Date
 ,invoice.importer AS Pipeline_InvoiceHeaders_Importer
 ,invoice.inco AS Pipeline_InvoiceHeaders_INCO
 ,invoice.inv_date AS Pipeline_InvoiceHeaders_Inv_Date
 ,invoice.inv_gross_weight AS Pipeline_InvoiceHeaders_Inv_Gross_Weight
 ,invoice.invoice_no AS Pipeline_InvoiceHeaders_Invoice_No
 ,invoice.invoice_total AS Pipeline_InvoiceHeaders_Invoice_Total
 ,invoice.lacey_act_cert_date AS Pipeline_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invoice.landed_costing_ex_rate AS Pipeline_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invoice.manufacturer AS Pipeline_InvoiceHeaders_Manufacturer
 ,invoice.manufacturer_address AS Pipeline_InvoiceHeaders_Manufacturer_Address
 ,invoice.net_weight AS Pipeline_InvoiceHeaders_Net_Weight
 ,invoice.nhtsa_cert_date AS Pipeline_InvoiceHeaders_NHTSA_Cert_Date
 ,invoice.origin AS Pipeline_InvoiceHeaders_Origin
 ,invoice.packages AS Pipeline_InvoiceHeaders_Packages
 ,invoice.payment_date AS Pipeline_InvoiceHeaders_Payment_Date
 ,invoice.seller AS Pipeline_InvoiceHeaders_Seller
 ,invoice.ship_to_party AS Pipeline_InvoiceHeaders_Ship_to_party
 ,invoice.sold_to_party AS Pipeline_InvoiceHeaders_Sold_to_party
 ,invoice.supplier AS Pipeline_InvoiceHeaders_Supplier
 ,invoice.supplier_address AS Pipeline_InvoiceHeaders_Supplier_Address
 ,invoice.transaction_related AS Pipeline_InvoiceHeaders_Transaction_Related

 ,line.attribute1 AS Pipeline_InvoiceLines_Attribute_1
 ,'API @ 60° F = ' + FORMAT(line.attribute2, '0.######') AS Pipeline_InvoiceLines_Attribute_2
 ,line.attribute3 AS Pipeline_InvoiceLines_Attribute_3
 ,line.certifying_individual AS Pipeline_InvoiceLines_Certifying_Individual
 ,line.cif_component AS Pipeline_InvoiceLines_CIF_Component
 ,line.code AS Pipeline_InvoiceLines_Code
 ,line.consignee AS Pipeline_InvoiceLines_Consignee
 ,line.curr AS Pipeline_InvoiceLines_Curr
 ,line.customs_qty AS Pipeline_InvoiceLines_Customs_QTY
 ,line.dest_state AS Pipeline_InvoiceLines_Dest_State
 ,line.epa_tsca_toxic_substance_control_act_indicator AS Pipeline_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,line.export AS Pipeline_InvoiceLines_Export
 ,line.goods_description AS Pipeline_InvoiceLines_Goods_Description
 ,line.gr_weight AS Pipeline_InvoiceLines_Gr_Weight
 ,line.invoice_no AS Pipeline_InvoiceLines_Invoice_No
 ,line.invoice_qty AS Pipeline_InvoiceLines_Invoice_Qty
 ,line.invoice_qty_unit AS Pipeline_InvoiceLines_Invoice_Qty_Unit
 ,line.line_price AS Pipeline_InvoiceLines_Line_Price
 ,line.invoice_line_number AS Pipeline_InvoiceLines_LNO
 ,line.manufacturer AS Pipeline_InvoiceLines_Manufacturer
 ,line.org AS Pipeline_InvoiceLines_ORG
 ,line.origin AS Pipeline_InvoiceLines_Origin
 ,line.pga_contact_email AS Pipeline_InvoiceLines_PGA_Contact_Email
 ,line.pga_contact_name AS Pipeline_InvoiceLines_PGA_Contact_Name
 ,line.pga_contact_phone AS Pipeline_InvoiceLines_PGA_Contact_Phone
 ,line.price_unit AS Pipeline_InvoiceLines_PriceUnit
 ,line.prod_id1 AS Pipeline_InvoiceLines_Prod_ID_1
 ,line.sold_to_party AS Pipeline_InvoiceLines_Sold_To_Party
 ,line.spi AS Pipeline_InvoiceLines_SPI
 ,line.tariff AS Pipeline_InvoiceLines_Tariff
 ,line.transaction_related AS Pipeline_InvoiceLines_Transaction_Related
 ,line.tsca_indicator AS Pipeline_InvoiceLines_TSCA_Indicator
 ,line.uq AS Pipeline_InvoiceLines_UQ
 ,line.amount AS Pipeline_InvoiceLines_Amount

 ,misc.bond_type AS Pipeline_MISC_Bond_Type
 ,misc.branch AS Pipeline_MISC_Branch
 ,misc.[broker] AS Pipeline_MISC_Broker
 ,misc.broker_to_pay AS Pipeline_MISC_Broker_to_Pay
 ,misc.fta_recon AS Pipeline_MISC_FTA_Recon
 ,misc.merge_by AS Pipeline_MISC_Merge_By
 ,misc.payment_type AS Pipeline_MISC_Payment_Type
 ,misc.prelim_statement_date AS Pipeline_MISC_Prelim_Statement_Date
 ,misc.preparer_dist_port AS Pipeline_MISC_Preparer_Dist_Port
 ,misc.recon_issue AS Pipeline_MISC_Recon_Issue
 ,misc.submitter AS Pipeline_MISC_Submitter
 ,misc.tax_deferrable_ind AS Pipeline_MISC_Tax_Deferrable_Ind

FROM dbo.imp_pipeline_filing_header AS header
JOIN dbo.imp_pipeline_filing_detail AS detail
  ON header.id = detail.filing_header_id
LEFT JOIN dbo.imp_pipeline_declaration AS declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.imp_pipeline_container AS container
  ON container.filing_header_id = header.id
LEFT JOIN dbo.imp_pipeline_invoice_header AS invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.imp_pipeline_invoice_line AS line
  ON line.parent_record_id = invoice.id
LEFT JOIN dbo.imp_pipeline_misc AS misc
  ON misc.filing_header_id = header.id
WHERE (header.mapping_status = 2);
GO

CREATE VIEW dbo.v_imp_pipeline_review_grid
AS
SELECT
  inbnd.id AS id
 ,header.id AS filing_header_id
 ,declaration.importer
 ,inbnd.batch
 ,inbnd.ticket_number
 ,inbnd.facility
 ,inbnd.site_name
 ,inbnd.quantity
 ,inbnd.api
 ,inbnd.export_date
 ,inbnd.import_date
FROM dbo.imp_pipeline_filing_header header
LEFT JOIN dbo.imp_pipeline_declaration declaration
  ON declaration.filing_header_id = header.id
JOIN dbo.imp_pipeline_filing_detail detail
  ON detail.filing_header_id = header.id
JOIN dbo.imp_pipeline_inbound inbnd
  ON inbnd.id = detail.inbound_id;
GO

PRINT ('create stored procedures')
GO
-- review mapped data
CREATE PROCEDURE dbo.sp_imp_pipeline_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM dbo.imp_pipeline_form_section_configuration rs
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
    FROM dbo.imp_pipeline_form_configuration defValue
    INNER JOIN dbo.imp_pipeline_form_section_configuration section
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

-- add truck import declaration record --
CREATE PROCEDURE dbo.sp_imp_pipeline_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_declaration'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_pipeline_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,main_supplier
       ,importer
       ,issuer
       ,batch_ticket
       ,pipeline
       ,carrier_scac
       ,discharge
       ,entry_port
       ,dep
       ,arr
       ,arr2
       ,origin
       ,destination
       ,destination_state
       ,eta
       ,export_date
       ,[description]
       ,owner_ref
       ,firms_code
       ,master_bill
       ,importer_of_record
       ,entry_number
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.supplier
       ,@importerCode
       ,rule_facility.issuer
       ,REPLACE(inbnd.ticket_number, '-', '')
       ,rule_facility.pipeline
       ,rule_facility.issuer
       ,rule_facility.[port]
       ,rule_facility.[port]
       ,inbnd.export_date
       ,inbnd.import_date
       ,inbnd.import_date
       ,rule_facility.origin
       ,rule_facility.destination
       ,rule_facility.destination_state
       ,inbnd.import_date
       ,inbnd.import_date
       ,CONCAT(rule_facility.pipeline, ' P/L', ': ', inbnd.batch)
       ,inbnd.ticket_number
       ,rule_facility.firms_code
       ,REPLACE(inbnd.ticket_number, '-', '')
       ,@ImporterCode
       ,inbnd.entry_number
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add truck import container record --
CREATE PROCEDURE dbo.sp_imp_pipeline_add_container (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_container';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add сontainersTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_container сontainer
      WHERE сontainer.filing_header_id = @parentId)
  BEGIN
    INSERT INTO dbo.imp_pipeline_container (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,manifest_qty
       ,bill_num
       ,bill_issuer_scac
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,ROUND(inbnd.quantity, 0)
       ,REPLACE(inbnd.ticket_number, '-', '')
       ,rule_facility.issuer
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON detail.inbound_id = inbnd.id
      LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add truck import invoice line record --
CREATE PROCEDURE dbo.sp_imp_pipeline_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_invoice_line';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get current tariff ---------------
  DECLARE @tariffs TABLE (
    inbound_id INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (
      inbound_id
     ,tariff)
    SELECT
      detail.inbound_id
     ,CASE
        WHEN inbnd.api < 25 THEN 1
        WHEN inbnd.api >= 25 THEN 2
      END
    FROM dbo.imp_pipeline_filing_detail detail
    JOIN dbo.imp_pipeline_inbound inbnd
      ON detail.inbound_id = inbnd.id
    WHERE detail.filing_header_id = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_pipeline_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,transaction_related
       ,tariff
       ,customs_qty
       ,goods_description
       ,spi
       ,gr_weight
       ,price_unit
       ,attribute1
       ,attribute2
       ,attribute3
       ,invoice_qty
       ,org
       ,line_price
       ,amount
       ,manufacturer
       ,consignee
       ,sold_to_party
       ,origin
       ,export
       ,dest_state
       ,created_date
       ,created_user)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.batch -- ?? is it ok?
       ,rule_importer.transaction_related
       ,rule_api.tariff
       ,inbnd.quantity
       ,CONCAT(rule_batch.product, ' - ', rule_batch.batch_code)
       ,rule_importer.spi
       ,CONVERT(DECIMAL(18, 3), dbo.fn_imp_pipeline_weight(inbnd.quantity, inbnd.api))
       ,COALESCE(rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.batch
       ,inbnd.api
       ,CONCAT(rule_facility.pipeline, ' P/L')
       ,inbnd.quantity
       ,rule_importer.origin
       ,inbnd.quantity * COALESCE(rule_price_exact.pricing, rule_price.pricing)
       ,inbnd.quantity * COALESCE(rule_price_exact.freight, rule_price.freight)
       ,rule_importer.manufacturer
       ,rule_importer.consignee
       ,rule_importer.consignee
       ,rule_importer.origin
       ,rule_importer.country_of_export
       ,rule_facility.destination_state
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
        ON inbnd.facility = rule_facility.facility
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price
        ON client.id = rule_price.importer_id
          AND rule_price.crude_type_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price_exact
        ON client.id = rule_price_exact.importer_id
          AND rule_batch.id = rule_price_exact.crude_type_id
      LEFT JOIN @tariffs tariff
        ON inbnd.id = tariff.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_api rule_api
        ON tariff.tariff = rule_api.id
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add truck import invoice header record --
CREATE PROCEDURE dbo.sp_imp_pipeline_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_pipeline_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_pipeline_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,supplier
       ,invoice_total
       ,origin
       ,consignee
       ,transaction_related
       ,manufacturer
       ,seller
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,supplier_address
       ,export
       ,manufacturer_address
       ,created_date
       ,created_user)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.batch
       ,rule_importer.supplier
       ,inbnd.quantity * COALESCE(rule_price_exact.pricing, rule_price.pricing)
       ,rule_importer.origin
       ,rule_importer.consignee
       ,rule_importer.transaction_related
       ,rule_importer.manufacturer
       ,rule_importer.supplier
       ,@importerCode
       ,rule_importer.consignee
       ,rule_importer.consignee
       ,rule_importer.seller
       ,rule_importer.country_of_export
       ,rule_importer.mid
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.Clients client
        ON client.ClientCode = @importerCode
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price
        ON client.id = rule_price.importer_id
          AND rule_price.crude_type_id IS NULL
      LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
        ON dbo.fn_imp_pipeline_batch_code(inbnd.Batch) = rule_batch.batch_code
      LEFT JOIN dbo.imp_pipeline_rule_price rule_price_exact
        ON client.id = rule_price_exact.importer_id
          AND rule_batch.id = rule_price_exact.crude_type_id
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
    EXEC dbo.sp_imp_pipeline_add_invoice_line @filingHeaderId
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
CREATE PROCEDURE dbo.sp_imp_pipeline_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_pipeline_misc';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @importerCode VARCHAR(128);

  SET @operationId = COALESCE(@operationId, NEWID());

  --Get importer code for the inbound importer
  SELECT
    @importerCode = dbo.fn_imp_pipeline_importer_code(@filingHeaderId);

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_pipeline_form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_pipeline_misc (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,branch
       ,[broker]
       ,preparer_dist_port
       ,recon_issue
       ,fta_recon
       ,created_date
       ,created_user)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,rule_importer.recon_issue
       ,rule_importer.fta_recon
       ,GETDATE()
       ,@filingUser
      FROM dbo.imp_pipeline_filing_detail detail
      JOIN dbo.imp_pipeline_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_pipeline_rule_importer rule_importer
        ON @importerCode = rule_importer.importer
      LEFT JOIN dbo.app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add filing records --
CREATE PROCEDURE dbo.sp_imp_pipeline_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_imp_pipeline_add_declaration @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
                                          ,@operationId
  -- add container
  EXEC dbo.sp_imp_pipeline_add_container @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
                                        ,@operationId
  -- add invoice header
  EXEC dbo.sp_imp_pipeline_add_invoice_header @filingHeaderId
                                             ,@filingHeaderId
                                             ,@filingUser
                                             ,@operationId
  -- add misc
  EXEC dbo.sp_imp_pipeline_add_misc @filingHeaderId
                                   ,@filingHeaderId
                                   ,@filingUser
                                   ,@operationId
END;
GO
-- delete filing entry
CREATE PROCEDURE dbo.sp_imp_pipeline_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.imp_pipeline_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.imp_pipeline_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.imp_pipeline_form_section_configuration ps
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
CREATE PROCEDURE dbo.sp_imp_pipeline_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_imp_pipeline_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE imp_pipeline_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE imp_pipeline_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM imp_pipeline_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- update rail filing entry
CREATE PROCEDURE dbo.sp_imp_pipeline_update_entry (@json VARCHAR(MAX))
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
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN dbo.imp_pipeline_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.imp_pipeline_form_section_configuration section
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
CREATE PROCEDURE dbo.sp_imp_pipeline_recalculate
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
FROM imp_truck_document truck_export_doc
INNER JOIN imp_truck_filing_header truck_export_header
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
ALTER TABLE dbo.Pipeline_InvoiceLines
DROP COLUMN invoice_line_number;
DROP FUNCTION dbo.pipeline_invoice_line_number;
ALTER TABLE dbo.Pipeline_InvoiceLines
DROP COLUMN Gr_Weight_Tons;
DROP FUNCTION dbo.weightToTon;
DROP FUNCTION dbo.extractBatchCode;
DROP FUNCTION dbo.fn_pipeline_GetImporterCode;
DROP FUNCTION dbo.fn_pipeline_weight;
GO

PRINT ('drop tables')
GO
DROP TABLE dbo.Pipeline_DeclarationTab
DROP TABLE dbo.Pipeline_ContainersTab
DROP TABLE dbo.Pipeline_MISC
DROP TABLE dbo.Pipeline_InvoiceLines
DROP TABLE dbo.Pipeline_InvoiceHeaders
DROP TABLE dbo.Pipeline_Filing_Details
DROP TABLE dbo.Pipeline_Inbound
DROP TABLE dbo.Pipeline_Rule_Importer
DROP TABLE dbo.Pipeline_Rule_API
DROP TABLE dbo.Pipeline_Rule_Consignee_Importer
DROP TABLE dbo.Pipeline_Rule_Facility
DROP TABLE dbo.Pipeline_Rule_Price
DROP TABLE dbo.Pipeline_Rule_BatchCode
DROP TABLE dbo.Pipeline_DEFValues
DROP TABLE dbo.pipeline_sections
DROP TABLE dbo.Pipeline_DEFValues_Manual
DROP TABLE dbo.App_WeightsConversion

PRINT ('drop views')
GO
DROP VIEW dbo.v_Pipeline_Filing_Data
DROP VIEW dbo.v_Pipeline_Tables
DROP VIEW dbo.v_Pipeline_DEFValues_Manual
DROP VIEW dbo.v_Pipeline_DEFValues
DROP VIEW dbo.Pipeline_Inbound_Grid
DROP VIEW dbo.Pipeline_Report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.pipeline_filing
DROP PROCEDURE dbo.pipeline_add_declaration_record
DROP PROCEDURE dbo.pipeline_add_container_record
DROP PROCEDURE dbo.pipeline_add_misc_record
DROP PROCEDURE dbo.pipeline_add_invoice_header_record
DROP PROCEDURE dbo.pipeline_add_invoice_line_record
DROP PROCEDURE dbo.pipeline_add_def_values_manual
DROP PROCEDURE dbo.pipeline_apply_def_values
DROP PROCEDURE dbo.pipeline_filing_del
DROP PROCEDURE dbo.pipeline_inbound_del
DROP PROCEDURE dbo.pipeline_delete_record
DROP PROCEDURE dbo.pipeline_filing_param
DROP PROCEDURE dbo.pipeline_filing_post_save