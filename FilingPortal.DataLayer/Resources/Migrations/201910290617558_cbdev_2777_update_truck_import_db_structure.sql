PRINT ('create tables')
GO

CREATE TABLE dbo.imp_truck_inbound (
  id INT IDENTITY
 ,importer NVARCHAR(200) NOT NULL
 ,paps VARCHAR(20) NOT NULL
 ,deleted BIT NOT NULL DEFAULT (0)
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_truck_filing_detail (
  inbound_id INT NOT NULL
 ,filing_header_id INT NOT NULL
 ,PRIMARY KEY CLUSTERED (inbound_id, filing_header_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_truck_declaration (
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
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_truck_container (
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

CREATE TABLE dbo.imp_truck_invoice_header (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,invoice_no VARCHAR(128) NULL
 ,supplier VARCHAR(128) NULL
 ,supplier_address VARCHAR(128) NULL
 ,inco VARCHAR(128) NULL
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
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_truck_invoice_line (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,invoice_no VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,customs_qty NUMERIC(18, 6) NULL
 ,line_price NUMERIC(18, 6) NULL
 ,goods_description VARCHAR(128) NULL
 ,org VARCHAR(128) NULL
 ,spi VARCHAR(128) NULL
 ,gr_weight NUMERIC(18, 6) NULL
 ,gr_weight_unit VARCHAR(2) NULL
 ,uq VARCHAR(128) NULL
 ,price_unit NUMERIC(18, 6) NULL
 ,prod_id1 VARCHAR(128) NULL
 ,attribute1 VARCHAR(128) NULL
 ,attribute2 VARCHAR(128) NULL
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
 ,amount INT NULL
 ,[description] VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_truck_misc (
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

CREATE TABLE dbo.imp_truck_document (
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

CREATE TABLE dbo.imp_truck_rule_importer (
  id INT IDENTITY
 ,ior NVARCHAR(200) NOT NULL
 ,cw_ior VARCHAR(128) NULL
 ,cw_supplier VARCHAR(128) NULL
 ,arrival_port VARCHAR(128) NULL
 ,entry_port VARCHAR(128) NULL
 ,ce VARCHAR(128) NULL
 ,charges DECIMAL(18, 6) NULL
 ,co VARCHAR(128) NULL
 ,custom_attrib1 VARCHAR(128) NULL
 ,custom_attrib2 VARCHAR(128) NULL
 ,custom_quantity DECIMAL(18, 6) NULL
 ,custom_uq VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,goods_description VARCHAR(128) NULL
 ,gross_weight DECIMAL(18, 6) NULL
 ,gross_weight_uq VARCHAR(128) NULL
 ,invoice_qty DECIMAL(18, 6) NULL
 ,invoice_uq VARCHAR(128) NULL
 ,line_price DECIMAL(18, 6) NULL
 ,manufacturer_mid VARCHAR(128) NULL
 ,nafta_recon VARCHAR(128) NULL
 ,product_id VARCHAR(128) NULL
 ,recon_issue VARCHAR(128) NULL
 ,spi VARCHAR(128) NULL
 ,supplier_mid VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,transactions_related VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_truck_rule_port (
  id INT IDENTITY
 ,entry_port VARCHAR(128) NOT NULL
 ,arrival_port VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_truck_form_section_configuration (
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

CREATE TABLE dbo.imp_truck_form_configuration (
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
SET IDENTITY_INSERT dbo.imp_truck_inbound ON;
INSERT INTO dbo.imp_truck_inbound (
    id
   ,importer
   ,paps
   ,deleted
   ,created_date
   ,created_user)
  SELECT
    ti.Id
   ,ti.Importer
   ,ti.PAPs
   ,ti.FDeleted
   ,ti.CreatedDate
   ,ti.CreatedUser
  FROM dbo.Truck_Inbound ti;
SET IDENTITY_INSERT dbo.imp_truck_inbound OFF;
GO

PRINT ('copy filing header data')
GO
DECLARE @truck_headers_drop_constraints_sql NVARCHAR(MAX) = N'';

SELECT
  @truck_headers_drop_constraints_sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
  + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
  ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.objects
WHERE OBJECT_NAME(parent_object_id) = 'Truck_Filing_Headers' AND (type = 'D' OR type = 'F' )

--PRINT @truck_headers_drop_constraints_sql;
EXEC sp_executesql @truck_headers_drop_constraints_sql;

DECLARE @drop_index_sql NVARCHAR(MAX) = N'';
SELECT
  @drop_index_sql += N'
DROP INDEX ' + QUOTENAME(OBJECT_SCHEMA_NAME(indx.object_id))
  + '.' + QUOTENAME(OBJECT_NAME(indx.object_id))
  + '.' + QUOTENAME(indx.name) + ';'
FROM sys.indexes AS indx
WHERE indx.object_id = OBJECT_ID('Truck_Filing_Headers')
AND is_primary_key <> 1;
EXEC (@drop_index_sql);
GO

EXEC sp_rename N'dbo.Truck_Filing_Headers.CreatedUser'
              ,N'created_user'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.CreatedDate'
              ,N'created_date'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.ErrorDescription'
              ,N'error_description'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.FilingNumber'
              ,N'filing_number'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.FilingStatus'
              ,N'filing_status'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.MappingStatus'
              ,N'mapping_status'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.RequestXML'
              ,N'request_xml'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Truck_Filing_Headers.ResponseXML'
              ,N'response_xml'
              ,'COLUMN'
GO

EXEC sp_rename N'dbo.Truck_Filing_Headers'
              ,N'imp_truck_filing_header'
              ,'OBJECT'
GO

ALTER TABLE dbo.imp_truck_filing_header
  ALTER
    COLUMN mapping_status int
GO
ALTER TABLE dbo.imp_truck_filing_header
  ALTER
    COLUMN filing_status int
GO
ALTER TABLE dbo.imp_truck_filing_header
  ALTER
    COLUMN job_link varchar(8000)
GO

ALTER TABLE dbo.imp_truck_filing_header
  ADD DEFAULT GETDATE() FOR created_date
GO

ALTER TABLE dbo.imp_truck_filing_header
  ADD DEFAULT SUSER_NAME() FOR created_user
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.imp_truck_filing_detail (
    inbound_id
   ,filing_header_id)
  SELECT
    fd.BDP_FK
   ,fd.Filing_Headers_FK
  FROM dbo.Truck_Filing_Details fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.imp_truck_declaration ON;
INSERT INTO dbo.imp_truck_declaration (
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
   ,declaration.CreatedDate
   ,declaration.CreatedUser
  FROM dbo.Truck_DeclarationTab declaration;
SET IDENTITY_INSERT dbo.imp_truck_declaration OFF;
GO

PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.imp_truck_container ON;
INSERT INTO dbo.imp_truck_container (
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
  FROM dbo.Truck_ContainersTab container;
SET IDENTITY_INSERT dbo.imp_truck_container OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.imp_truck_invoice_header ON;
INSERT INTO dbo.imp_truck_invoice_header (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,invoice_no
   ,supplier
   ,supplier_address
   ,inco
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
   ,created_date
   ,created_user)
  SELECT
    invoice.id
   ,invoice.Filing_Headers_FK
   ,invoice.Filing_Headers_FK
   ,NULL
   ,invoice.Invoice_No
   ,invoice.Supplier
   ,invoice.Supplier_Address
   ,invoice.INCO
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
   ,invoice.CreatedDate
   ,invoice.CreatedUser
  FROM dbo.Truck_InvoiceHeaders invoice;
SET IDENTITY_INSERT dbo.imp_truck_invoice_header OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.imp_truck_invoice_line ON;
INSERT INTO dbo.imp_truck_invoice_line (
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
  FROM dbo.Truck_InvoiceLines line
SET IDENTITY_INSERT dbo.imp_truck_invoice_line OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.imp_truck_misc ON;
INSERT INTO dbo.imp_truck_misc (
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
  FROM dbo.Truck_MISC misc
SET IDENTITY_INSERT dbo.imp_truck_misc OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.imp_truck_document ON;
INSERT INTO dbo.imp_truck_document (
    id
   ,filing_header_id
   ,inbound_record_id
   ,file_name
   ,file_extension
   ,file_description
   ,file_content
   ,document_type
   ,[status]
   ,created_date
   ,created_user)
  SELECT
    doc.id
   ,doc.Filing_Headers_FK
   ,doc.inbound_record_id
   ,doc.[filename]
   ,doc.file_extension
   ,doc.file_desc
   ,doc.file_content
   ,doc.document_type
   ,doc.[Status]
   ,doc.CreatedDate
   ,doc.CreatedUser
  FROM dbo.Truck_Documents doc;
SET IDENTITY_INSERT dbo.imp_truck_document OFF;
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.imp_truck_rule_port ON;
INSERT INTO dbo.imp_truck_rule_port (
    id
   ,entry_port
   ,arrival_port
   ,firms_code
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.entry_port
   ,r.arrival_port
   ,r.firms_code
   ,r.created_date
   ,r.created_user
  FROM dbo.Truck_Rule_Ports r;
SET IDENTITY_INSERT dbo.imp_truck_rule_port OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.imp_truck_rule_importer ON;
INSERT INTO dbo.imp_truck_rule_importer (
    id
   ,arrival_port
   ,ce
   ,charges
   ,co
   ,custom_attrib1
   ,custom_attrib2
   ,custom_quantity
   ,custom_uq
   ,cw_ior
   ,cw_supplier
   ,destination_state
   ,entry_port
   ,goods_description
   ,gross_weight
   ,gross_weight_uq
   ,invoice_qty
   ,invoice_uq
   ,ior
   ,line_price
   ,manufacturer_mid
   ,nafta_recon
   ,product_id
   ,recon_issue
   ,spi
   ,supplier_mid
   ,tariff
   ,transactions_related
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.arrival_port
   ,r.ce
   ,r.charges
   ,r.co
   ,r.custom_attrib1
   ,r.custom_attrib2
   ,r.custom_quantity
   ,r.custom_uq
   ,r.cw_ior
   ,r.cw_supplier
   ,r.destination_state
   ,r.entry_port
   ,r.goods_description
   ,r.gross_weight
   ,r.gross_weight_uq
   ,r.invoice_qty
   ,r.invoice_uq
   ,r.ior
   ,r.line_price
   ,r.manufacturer_mid
   ,r.nafta_recon
   ,r.product_id
   ,r.recon_issue
   ,r.spi
   ,r.supplier_mid
   ,r.tariff
   ,r.transactions_related
   ,r.created_date
   ,r.created_user
  FROM dbo.Truck_Rule_Importers r;
SET IDENTITY_INSERT dbo.imp_truck_rule_importer OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.imp_truck_form_section_configuration ON;
INSERT INTO dbo.imp_truck_form_section_configuration (
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
  FROM dbo.truck_sections section;
SET IDENTITY_INSERT dbo.imp_truck_form_section_configuration OFF;
GO

UPDATE imp_truck_form_section_configuration
SET table_name = 'imp_truck_declaration'
   ,procedure_name = 'sp_imp_truck_add_declaration'
WHERE table_name = 'Truck_DeclarationTab';
UPDATE imp_truck_form_section_configuration
SET table_name = 'imp_truck_invoice_header'
   ,procedure_name = 'sp_imp_truck_add_invoice_header'
WHERE table_name = 'Truck_InvoiceHeaders';
UPDATE imp_truck_form_section_configuration
SET table_name = 'imp_truck_invoice_line'
   ,procedure_name = 'sp_imp_truck_add_invoice_line'
WHERE table_name = 'Truck_InvoiceLines';
UPDATE imp_truck_form_section_configuration
SET table_name = 'imp_truck_container'
   ,procedure_name = 'sp_imp_truck_add_container'
WHERE table_name = 'Truck_ContainersTab';
UPDATE imp_truck_form_section_configuration
SET table_name = 'imp_truck_misc'
   ,procedure_name = 'sp_imp_truck_add_misc'
WHERE table_name = 'Truck_MISC';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.imp_truck_form_configuration ON;
INSERT INTO dbo.imp_truck_form_configuration (
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
   ,dv.created_user
  FROM dbo.truck_def_values dv;
SET IDENTITY_INSERT dbo.imp_truck_form_configuration OFF;
GO

UPDATE dbo.imp_truck_form_configuration
SET column_name = 'arr2'
WHERE column_name = 'Arr_2'
AND section_id = 2;
UPDATE dbo.imp_truck_form_configuration
SET column_name = 'attribute1'
WHERE column_name = 'Attribute_1'
AND section_id = 5;
UPDATE dbo.imp_truck_form_configuration
SET column_name = 'attribute2'
WHERE column_name = 'Attribute_2'
AND section_id = 5;
UPDATE dbo.imp_truck_form_configuration
SET column_name = 'prod_id1'
WHERE column_name = 'Prod_ID_1'
AND section_id = 5;
UPDATE dbo.imp_truck_form_configuration
SET column_name = 'price_unit'
WHERE column_name = 'PriceUnit'
AND section_id = 5;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.imp_truck_filing_header
ADD CONSTRAINT [FK__imp_truck_filing_header__FilingStatus__filing_status] FOREIGN KEY (filing_status) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.imp_truck_filing_header
ADD CONSTRAINT [FK__imp_truck_filing_header__MappingStatus__mapping_status] FOREIGN KEY (mapping_status) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.imp_truck_filing_detail
ADD CONSTRAINT FK__imp_truck_filing_detail__imp_truck_inbound__inbound_id FOREIGN KEY (inbound_id) REFERENCES dbo.imp_truck_inbound (id)
GO

ALTER TABLE dbo.imp_truck_filing_detail
ADD CONSTRAINT FK__imp_truck_filing_detail__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id)
GO

ALTER TABLE dbo.imp_truck_declaration
ADD CONSTRAINT FK__imp_truck_declaration__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_container
ADD CONSTRAINT FK__imp_truck_container__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_invoice_header
ADD CONSTRAINT FK__imp_truck_invoice_header__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_invoice_line
ADD CONSTRAINT FK__imp_truck_invoice_line__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id)
GO

ALTER TABLE dbo.imp_truck_invoice_line
ADD CONSTRAINT FK__imp_truck_invoice_line__imp_truck_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.imp_truck_invoice_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_misc
ADD CONSTRAINT FK__imp_truck_misc__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_document
ADD CONSTRAINT [FK__imp_truck_document__imp_truck_inbound__inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.imp_truck_inbound (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_document
ADD CONSTRAINT FK__imp_truck_document__imp_truck_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_truck_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_truck_form_section_configuration
ADD CONSTRAINT FK__imp_truck_form_section_configuration__imp_truck_form_section_configuration__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.imp_truck_form_section_configuration (id)
GO

ALTER TABLE dbo.imp_truck_form_configuration
ADD CONSTRAINT FK__imp_truck_form_configuration__imp_truck_form_section_configuration__section_id FOREIGN KEY (section_id) REFERENCES dbo.imp_truck_form_section_configuration (id)
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
FROM imp_truck_form_configuration field
JOIN imp_truck_form_section_configuration section
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
ON dbo.imp_truck_inbound (importer)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_status
ON dbo.imp_truck_filing_header (filing_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__mapping_status
ON dbo.imp_truck_filing_header (mapping_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_filing_detail (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_id
ON dbo.imp_truck_filing_detail (inbound_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_container (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_invoice_line (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON dbo.imp_truck_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_misc (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_truck_document (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__inbound_record_id
ON dbo.imp_truck_document (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__entry_port
ON dbo.imp_truck_rule_port (entry_port)
ON [PRIMARY]
GO

CREATE INDEX Idx__arrival_port__entry_port
ON dbo.imp_truck_rule_port (arrival_port, entry_port)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__ior
ON dbo.imp_truck_rule_importer (ior)
ON [PRIMARY]
GO

CREATE INDEX Idx__arrival_port__entry_port
ON dbo.imp_truck_rule_importer (arrival_port, entry_port)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__name
ON dbo.imp_truck_form_section_configuration (name)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_id
ON dbo.imp_truck_form_section_configuration (parent_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__section_id
ON dbo.imp_truck_form_configuration (section_id)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets truck export invoice header number
CREATE FUNCTION dbo.fn_imp_truck_invoice_line_number (@invoiceHeaderId INT
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
    FROM dbo.imp_truck_invoice_line line
    WHERE line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.imp_truck_invoice_line
ADD invoice_line_number AS (dbo.fn_imp_truck_invoice_line_number(parent_record_id, id));
ALTER TABLE dbo.imp_truck_invoice_line
ADD gr_weight_tons AS (dbo.fn_app_weight_to_ton(gr_weight, gr_weight_unit))
GO
-- gets truck import invoice total
CREATE FUNCTION dbo.fn_imp_truck_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(line.invoice_qty * line.price_unit)
  FROM dbo.imp_truck_invoice_line line
  WHERE line.parent_record_id = @invoiceHeaderId
  GROUP BY line.parent_record_id

  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.imp_truck_invoice_header
ADD invoice_total AS (dbo.fn_imp_truck_invoice_total(id));
GO

PRINT ('create views')
GO

CREATE VIEW dbo.v_imp_truck_form_configuration
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
FROM dbo.imp_truck_form_configuration form
INNER JOIN dbo.imp_truck_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL;
GO

CREATE VIEW dbo.v_imp_truck_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN imp_truck_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id');
GO

CREATE VIEW dbo.v_imp_truck_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.Importer AS base_importer
 ,inbnd.paps
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,rule_importer.cw_ior AS importer
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,IIF(rule_importer.ior IS NULL, 0, 1) AS has_importer_rule
 ,IIF(rule_port.entry_port IS NULL, 0, 1) AS has_port_rule
FROM dbo.imp_truck_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_truck_filing_header etfh
  JOIN dbo.imp_truck_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN dbo.imp_truck_rule_importer rule_importer
  ON inbnd.importer = rule_importer.ior
LEFT JOIN dbo.imp_truck_rule_port rule_port
  ON rule_port.entry_port = rule_importer.entry_port
    AND rule_port.arrival_port = rule_importer.arrival_port
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

WHERE inbnd.deleted = 0;
GO

CREATE VIEW dbo.v_imp_truck_report
AS
SELECT
  header.id

 ,declaration.main_supplier
 ,declaration.importer
 ,declaration.shipment_type
 ,declaration.transport
 ,declaration.entry_type
 ,declaration.rlf
 ,declaration.enable_entry_sum
 ,declaration.[type]
 ,declaration.certify_cargo_release
 ,declaration.[service]
 ,declaration.issuer
 ,declaration.master_bill
 ,declaration.carrier_scac
 ,declaration.discharge
 ,declaration.entry_port
 ,declaration.dep
 ,declaration.arr
 ,declaration.arr2 AS Arr_2
 ,declaration.hmf
 ,declaration.origin
 ,declaration.destination
 ,declaration.destination_state
 ,declaration.country_of_export
 ,declaration.eta
 ,declaration.export_date
 ,declaration.[description]
 ,declaration.owner_ref
 ,declaration.inco
 ,declaration.total_weight
 ,declaration.total_volume
 ,declaration.no_packages
 ,declaration.firms_code
 ,declaration.centralized_exam_site
 ,declaration.purchased
 ,declaration.manual_entry
 ,declaration.importer_of_record
 ,declaration.split_shipment_release
 ,declaration.check_local_client
 ,declaration.container AS Truck_DeclarationTab_Container

 ,container.bill_type
 ,container.bill_num
 ,container.bill_number
 ,container.uq AS Containers_UQ
 ,container.manifest_qty
 ,container.packing_uq
 ,container.bill_issuer_scac

 ,invoice.invoice_no
 ,invoice.consignee_address
 ,invoice.invoice_total
 ,invoice.curr
 ,invoice.payment_date
 ,invoice.consignee
 ,invoice.inv_date
 ,invoice.agreed_place
 ,invoice.inv_gross_weight
 ,invoice.net_weight
 ,invoice.manufacturer
 ,invoice.seller
 ,invoice.sold_to_party
 ,invoice.ship_to_party
 ,invoice.broker_pga_contact_name
 ,invoice.broker_pga_contact_phone
 ,invoice.broker_pga_contact_email
 ,invoice.transaction_related AS Truck_InvoiceHeaders_Transaction_Related
 ,invoice.epa_tsca_cert_date AS Truck_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invoice.supplier AS Truck_InvoiceHeaders_Supplier
 ,invoice.origin AS Truck_InvoiceHeaders_Origin

 ,line.invoice_line_number AS LNO
 ,line.tariff
 ,line.customs_qty
 ,line.line_price
 ,line.goods_description
 ,line.org
 ,line.spi
 ,line.gr_weight
 ,line.gr_weight_unit
 ,line.gr_weight_tons
 ,line.uq
 ,line.price_unit AS PriceUnit
 ,line.prod_id1 AS Prod_ID_1
 ,line.attribute1 AS Attribute_1
 ,line.attribute2 AS Attribute_2
 ,line.export
 ,line.invoice_qty
 ,line.invoice_qty_unit
 ,line.code
 ,line.amount
 ,line.cif_component
 ,line.epa_tsca_toxic_substance_control_act_indicator
 ,line.tsca_indicator
 ,line.certifying_individual
 ,line.manufacturer AS Truck_InvoiceLines_Manufacturer
 ,line.transaction_related AS Truck_InvoiceLines_Transaction_Related

 ,misc.branch
 ,misc.[broker]
 ,misc.merge_by
 ,misc.tax_deferrable_ind
 ,misc.preparer_dist_port
 ,misc.recon_issue
 ,misc.fta_recon
 ,misc.bond_type
 ,misc.payment_type
 ,misc.broker_to_pay
 ,misc.prelim_statement_date
 ,misc.submitter
FROM dbo.imp_truck_filing_header header
LEFT JOIN dbo.imp_truck_container container
  ON container.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_declaration declaration
  ON declaration.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_invoice_header invoice
  ON invoice.filing_header_id = header.id
LEFT JOIN dbo.imp_truck_invoice_line line
  ON line.parent_record_id = invoice.id
LEFT JOIN dbo.imp_truck_misc misc
  ON misc.filing_header_id = header.id
WHERE header.mapping_status = 2;
GO

PRINT ('create stored procedures')
GO
-- review mapped data
CREATE PROCEDURE dbo.sp_imp_truck_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM dbo.imp_truck_form_section_configuration rs
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
    FROM dbo.imp_truck_form_configuration defValue
    INNER JOIN dbo.imp_truck_form_section_configuration section
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
CREATE PROCEDURE dbo.sp_imp_truck_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_declaration'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_declaration declaration
      WHERE declaration.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO dbo.imp_truck_declaration (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,main_supplier
       ,importer
       ,issuer
       ,master_bill
       ,carrier_scac
       ,discharge
       ,entry_port
       ,destination_state
       ,[description]
       ,firms_code)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs))
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,rule_importer.arrival_port
       ,rule_importer.Entry_Port
       ,rule_importer.Destination_State
       ,rule_importer.Goods_Description
       ,rule_port.FIRMs_Code
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbound
        ON detail.inbound_id = inbound.id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbound.importer = rule_importer.ior
      LEFT JOIN dbo.imp_truck_rule_port rule_port
        ON rule_importer.arrival_port = rule_port.arrival_port
          AND rule_importer.entry_port = rule_port.entry_port
      WHERE detail.filing_header_id = @filingHeaderId
  END
END;
GO
-- add truck import container record --
CREATE PROCEDURE dbo.sp_imp_truck_add_container (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_container';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName;

  -- add сontainersTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_container сontainer
      WHERE сontainer.filing_header_id = @parentId)
  BEGIN
    INSERT INTO dbo.imp_truck_container (
        filing_header_id
       ,parent_record_id
       ,operation_id)
      SELECT @filingHeaderId, @parentId, @operationId;
  END;
END;
GO
-- add truck import invoice line record --
CREATE PROCEDURE dbo.sp_imp_truck_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_line invoice_line
      WHERE invoice_line.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_truck_invoice_line (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,transaction_related
       ,tariff
       ,customs_qty
       ,goods_description
       ,spi
       ,org
       ,export
       ,gr_weight
       ,gr_weight_unit
       ,uq
       ,price_unit
       ,prod_id1
       ,attribute1
       ,attribute2
       ,invoice_qty
       ,invoice_qty_unit
       ,amount
       ,line_price)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.PAPs
       ,rule_importer.transactions_related
       ,rule_importer.Tariff
       ,rule_importer.custom_quantity
       ,rule_importer.Goods_Description
       ,rule_importer.SPI
       ,rule_importer.co
       ,rule_importer.ce
       ,rule_importer.gross_weight
       ,rule_importer.gross_weight_uq
       ,rule_importer.custom_uq
       ,rule_importer.Line_Price
       ,rule_importer.product_id
       ,rule_importer.custom_attrib1
       ,rule_importer.custom_attrib2
       ,rule_importer.Invoice_Qty
       ,rule_importer.invoice_uq
       ,rule_importer.charges
       ,rule_importer.invoice_qty * rule_importer.line_price
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.importer = rule_importer.ior
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add truck import invoice header record --
CREATE PROCEDURE dbo.sp_imp_truck_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM imp_truck_form_section_configuration section
  WHERE section.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_invoice_header invoice_header
      WHERE invoice_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_truck_invoice_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,invoice_no
       ,supplier
       ,consignee
       ,transaction_related
       ,manufacturer
       ,seller
       ,importer
       ,sold_to_party
       ,ship_to_party
       ,export
       ,origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,inbnd.PAPs
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,rule_importer.transactions_related
       ,rule_importer.cw_supplier
       ,rule_importer.cw_supplier
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.cw_ior
       ,rule_importer.ce
       ,rule_importer.co
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.importer = rule_importer.ior
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
    EXEC dbo.sp_imp_truck_add_invoice_line @filingHeaderId
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
CREATE PROCEDURE dbo.sp_imp_truck_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_truck_misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_truck_form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_truck_misc misc
      WHERE misc.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_truck_misc (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,branch
       ,[broker]
       ,preparer_dist_port
       ,recon_issue
       ,fta_recon)
      SELECT DISTINCT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,rule_importer.Recon_Issue
       ,rule_importer.nafta_recon
      FROM dbo.imp_truck_filing_detail detail
      INNER JOIN dbo.imp_truck_inbound inbnd
        ON inbnd.id = detail.inbound_id
      LEFT JOIN dbo.imp_truck_rule_importer rule_importer
        ON inbnd.Importer = rule_importer.ior
      LEFT JOIN app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.filing_header_id = @filingHeaderId
  END;
END;
GO
-- add filing records --
CREATE PROCEDURE dbo.sp_imp_truck_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_imp_truck_add_declaration @filingHeaderId
                                       ,@filingHeaderId
                                       ,@filingUser
                                       ,@operationId
  -- add container
  EXEC dbo.sp_imp_truck_add_container @filingHeaderId
                                     ,@filingHeaderId
                                     ,@filingUser
                                     ,@operationId
  -- add invoice header
  EXEC dbo.sp_imp_truck_add_invoice_header @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
                                          ,@operationId
  -- add misc
  EXEC dbo.sp_imp_truck_add_misc @filingHeaderId
                                ,@filingHeaderId
                                ,@filingUser
                                ,@operationId
END;
GO
-- delete filing entry
CREATE PROCEDURE dbo.sp_imp_truck_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.imp_truck_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.imp_truck_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.imp_truck_form_section_configuration ps
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
CREATE PROCEDURE dbo.sp_imp_truck_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM v_imp_truck_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE imp_truck_inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE imp_truck_inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM imp_truck_filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- update rail filing entry
CREATE PROCEDURE dbo.sp_imp_truck_update_entry (@json VARCHAR(MAX))
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
    INNER JOIN dbo.imp_truck_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.imp_truck_form_section_configuration section
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
CREATE PROCEDURE dbo.sp_imp_truck_recalculate
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
ALTER TABLE dbo.Truck_InvoiceHeaders
DROP COLUMN Invoice_Total
DROP FUNCTION dbo.[truck_invoice_total]
ALTER TABLE dbo.Truck_InvoiceLines
DROP COLUMN invoice_line_number
DROP FUNCTION dbo.[truck_invoice_line_number]
ALTER TABLE dbo.Truck_InvoiceLines
DROP COLUMN Gr_Weight_Tons
GO

PRINT ('drop tables')
GO
DROP TABLE dbo.Truck_Documents
DROP TABLE dbo.Truck_DeclarationTab
DROP TABLE dbo.Truck_ContainersTab
DROP TABLE dbo.Truck_MISC
DROP TABLE dbo.Truck_InvoiceLines
DROP TABLE dbo.Truck_InvoiceHeaders
DROP TABLE dbo.Truck_Filing_Details
DROP TABLE dbo.Truck_Inbound
DROP TABLE dbo.Truck_Rule_Importers
DROP TABLE dbo.Truck_Rule_Ports
DROP TABLE dbo.truck_def_values
DROP TABLE dbo.truck_sections
DROP TABLE dbo.truck_def_values_manual

PRINT ('drop views')
GO
DROP VIEW dbo.v_Truck_Filing_Data
DROP VIEW dbo.v_truck_tables
DROP VIEW dbo.v_truck_def_values_manual
DROP VIEW dbo.v_truck_def_values
DROP VIEW dbo.Truck_Inbound_Grid
DROP VIEW dbo.Truck_Report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.truck_filing
DROP PROCEDURE dbo.truck_add_declaration_record
DROP PROCEDURE dbo.truck_add_container_record
DROP PROCEDURE dbo.truck_add_misc_record
DROP PROCEDURE dbo.truck_add_invoice_header_record
DROP PROCEDURE dbo.truck_add_invoice_line_record
DROP PROCEDURE dbo.truck_add_def_values_manual
DROP PROCEDURE dbo.truck_apply_def_values
DROP PROCEDURE dbo.truck_filing_del
DROP PROCEDURE dbo.truck_inbound_del
DROP PROCEDURE dbo.truck_delete_record
DROP PROCEDURE dbo.truck_filing_param