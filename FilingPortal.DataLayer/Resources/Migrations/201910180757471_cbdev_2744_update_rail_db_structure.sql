PRINT ('create tables')
GO
CREATE TABLE dbo.app_weight_conversion_rate (
  weight_unit VARCHAR(2) NOT NULL
 ,rate NUMERIC(18, 9) NOT NULL
 ,PRIMARY KEY CLUSTERED (weight_unit)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_broker_download (
  id INT IDENTITY
 ,em_message_text VARCHAR(MAX) NOT NULL
 ,cw_last_modified_date DATETIME NOT NULL
 ,em_pk_cbrnyc UNIQUEIDENTIFIER NULL
 ,em_system_create_time_utc SMALLDATETIME NULL
 ,em_status TINYINT NULL
 ,last_modified_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_inbound (
  id INT IDENTITY
 ,broker_download_id INT NOT NULL
 ,importer NVARCHAR(200) NULL
 ,supplier NVARCHAR(200) NULL
 ,equipment_initial VARCHAR(4) NULL
 ,equipment_number NVARCHAR(6) NULL
 ,issuer_code VARCHAR(5) NULL
 ,bill_of_lading NVARCHAR(20) NULL
 ,port_of_unlading VARCHAR(4) NULL
 ,description1 NVARCHAR(500) NULL
 ,manifest_units VARCHAR(3) NULL
 ,weight NVARCHAR(10) NULL
 ,weight_unit VARCHAR(2) NULL
 ,reference_number1 NVARCHAR(50) NULL
 ,deleted BIT NOT NULL
 ,duplicate_of INT NULL
 ,cw_created_date_utc DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_filing_header (
  id INT IDENTITY
 ,filing_number VARCHAR(255) NULL
 ,mapping_status INT NULL
 ,filing_status INT NULL
 ,error_description VARCHAR(MAX) NULL
 ,job_hyperlink VARCHAR(8000) NULL
 ,request_xml VARCHAR(MAX) NULL
 ,response_xml VARCHAR(MAX) NULL
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_filing_detail (
  inbound_id INT NOT NULL
 ,filing_header_id INT NOT NULL
 ,PRIMARY KEY CLUSTERED (inbound_id, filing_header_id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_declaration (
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
 ,type VARCHAR(128) NULL
 ,certify_cargo_release VARCHAR(128) NULL
 ,service VARCHAR(128) NULL
 ,issuer VARCHAR(128) NULL
 ,master_bill VARCHAR(128) NULL
 ,carrier_scac VARCHAR(128) NULL
 ,discharge VARCHAR(128) NULL
 ,entry_port VARCHAR(128) NULL
 ,dep DATE NULL
 ,arr DATE NULL
 ,arr_2 DATE NULL
 ,hmf VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,country_of_export VARCHAR(128) NULL
 ,eta DATE NULL
 ,export_date DATE NULL
 ,description NVARCHAR(500) NULL
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
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_container (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,source_record_id INT NULL
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
 ,type VARCHAR(128) NULL
 ,mode VARCHAR(128) NULL
 ,goods_weight VARCHAR(128) NULL
 ,bill_num VARCHAR(128) NULL
 ,packing_uq VARCHAR(128) NULL
 ,gross_weight NUMERIC(18, 6) NULL
 ,gross_weight_unit VARCHAR(2) NULL
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_invoice_header (
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
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE.dbo.imp_rail_invoice_line (
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
 ,uq VARCHAR(128) NULL
 ,price_unit NUMERIC(18, 6) NULL
 ,prod_id_1 VARCHAR(128) NULL
 ,attribute_1 VARCHAR(128) NULL
 ,attribute_2 VARCHAR(128) NULL
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
 ,description VARCHAR(128) NULL
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_misc (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,operation_id UNIQUEIDENTIFIER NULL
 ,branch VARCHAR(128) NULL
 ,broker VARCHAR(128) NULL
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
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_rule_port (
  id INT IDENTITY
 ,port VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,export VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_rule_importer_supplier (
  id INT IDENTITY
 ,importer_name NVARCHAR(200) NULL
 ,supplier_name NVARCHAR(200) NULL
 ,main_supplier VARCHAR(128) NULL
 ,importer VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,consignee VARCHAR(128) NULL
 ,manufacturer VARCHAR(128) NULL
 ,seller VARCHAR(128) NULL
 ,sold_to_party VARCHAR(128) NULL
 ,ship_to_party VARCHAR(128) NULL
 ,country_of_origin VARCHAR(128) NULL
 ,relationship VARCHAR(128) NULL
 ,dft VARCHAR(128) NULL
 ,value_recon VARCHAR(128) NULL
 ,fta_recon VARCHAR(128) NULL
 ,payment_type INT NULL
 ,broker_to_pay VARCHAR(128) NULL
 ,value NUMERIC(18, 6) NULL
 ,freight INT NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_rule_product (
  id INT IDENTITY
 ,description1 NVARCHAR(500) NOT NULL
 ,prod_id_1 VARCHAR(128) NULL
 ,attribute_1 VARCHAR(128) NULL
 ,attribute_2 VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,description VARCHAR(128) NULL
 ,goods_description VARCHAR(128) NULL
 ,invoice_uom VARCHAR(128) NULL
 ,template_hts_quantity DECIMAL(18, 6) NULL
 ,template_invoice_quantity DECIMAL(18, 6) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.imp_rail_form_section_configuration (
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

CREATE TABLE dbo.imp_rail_form_configuration (
  id INT IDENTITY
 ,section_id INT NOT NULL
 ,column_name VARCHAR(128) NULL
 ,label VARCHAR(128) NULL
 ,[description] VARCHAR(128) NULL
 ,[value] VARCHAR(500) NULL
 ,has_default_value TINYINT NOT NULL DEFAULT (0)
 ,editable TINYINT NOT NULL DEFAULT (1)
 ,mandatory TINYINT NOT NULL DEFAULT (0)
 ,display_on_ui TINYINT NOT NULL DEFAULT (0)
 ,[manual] TINYINT NOT NULL DEFAULT (0)
 ,single_filing_order TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,created_date DATETIME NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy weight conversion rate data')
GO
INSERT INTO dbo.app_weight_conversion_rate (
    weight_unit
   ,rate)
  SELECT
    awc.WeightUnit
   ,awc.RateInTn
  FROM dbo.App_WeightsConversion awc;
GO

PRINT ('copy broker download data')
GO
SET IDENTITY_INSERT dbo.imp_rail_broker_download ON;
INSERT INTO dbo.imp_rail_broker_download (
    id
   ,em_message_text
   ,cw_last_modified_date
   ,em_pk_cbrnyc
   ,em_system_create_time_utc
   ,em_status
   ,last_modified_date
   ,created_date
   ,created_user)
  SELECT
    RE.EM_PK
   ,re.EM_MessageText
   ,re.CW_LastModifiedDate
   ,re.EM_PK_CBRNYC
   ,re.EM_SystemCreateTimeUtc
   ,re.EM_Status
   ,re.LastModifiedDate
   ,re.CreatedDate
   ,re.CreatedUser
  FROM DBO.Rail_EDIMessage re;
SET IDENTITY_INSERT dbo.imp_rail_broker_download OFF;
GO

PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.imp_rail_inbound ON;
INSERT INTO dbo.imp_rail_inbound (
    id
   ,broker_download_id
   ,importer
   ,supplier
   ,equipment_initial
   ,equipment_number
   ,issuer_code
   ,bill_of_lading
   ,port_of_unlading
   ,description1
   ,manifest_units
   ,weight
   ,weight_unit
   ,reference_number1
   ,deleted
   ,duplicate_of
   ,cw_created_date_utc
   ,created_date
   ,created_user)
  SELECT
    rbp.BDP_PK
   ,rbp.BDP_EM
   ,rbp.Importer
   ,rbp.Supplier
   ,rbp.EquipmentInitial
   ,rbp.EquipmentNumber
   ,rbp.IssuerCode
   ,rbp.BillofLading
   ,rbp.PortofUnlading
   ,rbp.Description1
   ,rbp.ManifestUnits
   ,rbp.Weight
   ,rbp.WeightUnit
   ,rbp.ReferenceNumber1
   ,rbp.FDeleted
   ,rbp.DuplicateOf
   ,rbp.CW_CreatedDateUTC
   ,rbp.CreatedDate
   ,rbp.CreatedUser
  FROM dbo.Rail_BD_Parsed rbp;
SET IDENTITY_INSERT dbo.imp_rail_inbound OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.imp_rail_filing_header ON;
INSERT INTO dbo.imp_rail_filing_header (
    id
   ,filing_number
   ,mapping_status
   ,filing_status
   ,error_description
   ,job_hyperlink
   ,request_xml
   ,response_xml
   ,created_date
   ,created_user)
  SELECT
    rfh.id
   ,rfh.FilingNumber
   ,rfh.MappingStatus
   ,rfh.FilingStatus
   ,rfh.ErrorDescription
   ,rfh.JobPKHyperlink
   ,rfh.RequestXML
   ,rfh.ResponseXML
   ,rfh.CreatedDate
   ,rfh.CreatedUser
  FROM dbo.Rail_Filing_Headers rfh;
SET IDENTITY_INSERT dbo.imp_rail_filing_header OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.imp_rail_filing_detail (
    inbound_id
   ,filing_header_id)
  SELECT
    rfd.BDP_FK
   ,rfd.Filing_Headers_FK
  FROM dbo.Rail_Filing_Details rfd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.imp_rail_declaration ON;
INSERT INTO dbo.imp_rail_declaration (
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
   ,arr_2
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
    rdt.id
   ,rdt.Filing_Headers_FK
   ,rdt.Filing_Headers_FK
   ,NULL
   ,rdt.Main_Supplier
   ,rdt.Importer
   ,rdt.Shipment_Type
   ,rdt.Transport
   ,rdt.Container
   ,rdt.Entry_Type
   ,rdt.RLF
   ,rdt.Enable_Entry_Sum
   ,rdt.Type
   ,rdt.Certify_Cargo_Release
   ,rdt.Service
   ,rdt.Issuer
   ,rdt.Master_Bill
   ,rdt.Carrier_SCAC
   ,rdt.Discharge
   ,rdt.Entry_Port
   ,rdt.Dep
   ,rdt.Arr
   ,rdt.Arr_2
   ,rdt.HMF
   ,rdt.Origin
   ,rdt.Destination
   ,rdt.Destination_State
   ,rdt.Country_of_Export
   ,rdt.ETA
   ,rdt.Export_Date
   ,rdt.Description
   ,rdt.Owner_Ref
   ,rdt.INCO
   ,rdt.Total_Weight
   ,rdt.Total_Volume
   ,rdt.No_Packages
   ,rdt.FIRMs_Code
   ,rdt.Centralized_Exam_Site
   ,rdt.Purchased
   ,rdt.Manual_Entry
   ,rdt.Importer_of_record
   ,rdt.Split_Shipment_Release
   ,rdt.Check_Local_Client
   ,rdt.CreatedDate
   ,rdt.CreatedUser
  FROM dbo.Rail_DeclarationTab rdt;
SET IDENTITY_INSERT dbo.imp_rail_declaration OFF;
GO

PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.imp_rail_container ON;
INSERT INTO dbo.imp_rail_container (
    id
   ,filing_header_id
   ,parent_record_id
   ,operation_id
   ,source_record_id
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
   ,gross_weight
   ,gross_weight_unit
   ,created_date
   ,created_user)
  SELECT
    rct.id
   ,rct.Filing_Headers_FK
   ,rct.Filing_Headers_FK
   ,NULL
   ,rct.BDP_FK
   ,rct.Bill_Type
   ,rct.Manifest_QTY
   ,rct.UQ
   ,rct.Bill_Issuer_SCAC
   ,rct.IT_Number
   ,rct.Is_Split
   ,rct.Bill_Number
   ,rct.Container_Number
   ,rct.Pack_QTY
   ,rct.Pack_Type
   ,rct.Marks_and_Numbers
   ,rct.Shipping_Symbol
   ,rct.Seal_Number
   ,rct.Type
   ,rct.Mode
   ,rct.Goods_Weight
   ,rct.Bill_Num
   ,rct.Packing_UQ
   ,rct.Gross_Weight
   ,rct.Gross_Weight_Unit
   ,rct.CreatedDate
   ,rct.CreatedUser
  FROM dbo.Rail_ContainersTab rct;
SET IDENTITY_INSERT dbo.imp_rail_container OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.imp_rail_invoice_header ON;
INSERT INTO dbo.imp_rail_invoice_header (
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
    rih.id
   ,rih.Filing_Headers_FK
   ,rih.Filing_Headers_FK
   ,NULL
   ,rih.Invoice_No
   ,rih.Supplier
   ,rih.Supplier_Address
   ,rih.INCO
   ,rih.Curr
   ,rih.Origin
   ,rih.Payment_Date
   ,rih.Consignee
   ,rih.Consignee_Address
   ,rih.Inv_Date
   ,rih.Agreed_Place
   ,rih.Inv_Gross_Weight
   ,rih.Net_Weight
   ,rih.Export
   ,rih.Export_Date
   ,rih.First_Sale
   ,rih.Transaction_Related
   ,rih.Packages
   ,rih.Manufacturer
   ,rih.Seller
   ,rih.Importer
   ,rih.Sold_to_party
   ,rih.Ship_to_party
   ,rih.Broker_PGA_Contact_Name
   ,rih.Broker_PGA_Contact_Phone
   ,rih.Broker_PGA_Contact_Email
   ,rih.EPA_PST_Cert_Date
   ,rih.EPA_TSCA_Cert_Date
   ,rih.EPA_VNE_Cert_Date
   ,rih.FSIS_Cert_Date
   ,rih.FWS_Cert_Date
   ,rih.LACEY_ACT_Cert_Date
   ,rih.NHTSA_Cert_Date
   ,rih.Landed_Costing_Ex_Rate
   ,rih.CreatedDate
   ,rih.CreatedUser
  FROM dbo.Rail_InvoiceHeaders rih;
SET IDENTITY_INSERT dbo.imp_rail_invoice_header OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.imp_rail_invoice_line ON;
INSERT INTO dbo.imp_rail_invoice_line (
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
   ,uq
   ,price_unit
   ,prod_id_1
   ,attribute_1
   ,attribute_2
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
    ril.id
   ,ril.Filing_Headers_FK
   ,ril.InvoiceHeaders_FK
   ,NULL
   ,ril.Invoice_No
   ,ril.Tariff
   ,ril.Customs_QTY
   ,ril.Line_Price
   ,ril.Goods_Description
   ,ril.ORG
   ,ril.SPI
   ,ril.UQ
   ,ril.PriceUnit
   ,ril.Prod_ID_1
   ,ril.Attribute_1
   ,ril.Attribute_2
   ,ril.Export
   ,ril.Origin
   ,ril.Dest_State
   ,ril.Transaction_Related
   ,ril.Invoice_Qty
   ,ril.Invoice_Qty_Unit
   ,ril.Manufacturer
   ,ril.Consignee
   ,ril.Sold_To_Party
   ,ril.Code
   ,ril.Curr
   ,ril.CIF_Component
   ,ril.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
   ,ril.TSCA_Indicator
   ,ril.Certifying_Individual
   ,ril.PGA_Contact_Name
   ,ril.PGA_Contact_Phone
   ,ril.PGA_Contact_Email
   ,ril.Amount
   ,ril.Description
   ,ril.CreatedDate
   ,ril.CreatedUser
  FROM dbo.Rail_InvoiceLines ril;
SET IDENTITY_INSERT dbo.imp_rail_invoice_line OFF;
GO

PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.imp_rail_misc ON;
INSERT INTO dbo.imp_rail_misc (
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
    rm.id
   ,rm.Filing_Headers_FK
   ,rm.Filing_Headers_FK
   ,NULL
   ,rm.Branch
   ,rm.Broker
   ,rm.Merge_By
   ,rm.Tax_Deferrable_Ind
   ,rm.Preparer_Dist_Port
   ,rm.Recon_Issue
   ,rm.FTA_Recon
   ,rm.Bond_Type
   ,rm.Payment_Type
   ,rm.Broker_to_Pay
   ,rm.Prelim_Statement_Date
   ,rm.Submitter
   ,rm.CreatedDate
   ,rm.CreatedUser
  FROM dbo.Rail_MISC rm;
SET IDENTITY_INSERT dbo.imp_rail_misc OFF;
GO

PRINT ('copy documents data')
GO
DECLARE @rail_docs_drop_constraints_sql NVARCHAR(MAX) = N'';

SELECT
  @rail_docs_drop_constraints_sql += N'
ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
  + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
  ' DROP CONSTRAINT ' + QUOTENAME(name) + ';'
FROM sys.objects
WHERE OBJECT_NAME(parent_object_id) = 'Rail_Documents' AND (type = 'D' OR type = 'F' )

PRINT @rail_docs_drop_constraints_sql;
EXEC sp_executesql @rail_docs_drop_constraints_sql;

DROP INDEX IF EXISTS Idx_filing_header_id ON dbo.Rail_Documents

EXEC sp_rename N'dbo.Rail_Documents.CreatedUser'
              ,N'created_user'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents.CreatedDate'
              ,N'created_date'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents.Status'
              ,N'status'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents.IsManifest'
              ,N'is_manifest'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents.DocumentType'
              ,N'document_type'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents.Filing_Headers_FK'
              ,N'filing_header_id'
              ,'COLUMN'
GO
EXEC sp_rename N'dbo.Rail_Documents'
              ,N'imp_rail_document'
              ,'OBJECT'
GO

ALTER TABLE dbo.imp_rail_document
  ADD DEFAULT 0 FOR is_manifest
GO

ALTER TABLE dbo.imp_rail_document
  ADD DEFAULT GETDATE() FOR created_date
GO

ALTER TABLE dbo.imp_rail_document
  ADD DEFAULT SUSER_NAME() FOR created_user
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.imp_rail_rule_port ON;
INSERT INTO dbo.imp_rail_rule_port (
    id
   ,port
   ,origin
   ,destination
   ,firms_code
   ,export
   ,created_date
   ,created_user)
  SELECT
    rrp.id
   ,rrp.Port
   ,rrp.Origin
   ,rrp.Destination
   ,rrp.FIRMs_Code
   ,rrp.Export
   ,rrp.CreatedDate
   ,rrp.CreatedUser
  FROM dbo.Rail_Rule_Port rrp;
SET IDENTITY_INSERT dbo.imp_rail_rule_port OFF;
GO

PRINT ('copy importer-supplier rule data')
GO
SET IDENTITY_INSERT dbo.imp_rail_rule_importer_supplier ON;
INSERT INTO dbo.imp_rail_rule_importer_supplier (
    id
   ,importer_name
   ,supplier_name
   ,main_supplier
   ,importer
   ,destination_state
   ,consignee
   ,manufacturer
   ,seller
   ,sold_to_party
   ,ship_to_party
   ,country_of_origin
   ,relationship
   ,dft
   ,value_recon
   ,fta_recon
   ,payment_type
   ,broker_to_pay
   ,value
   ,freight
   ,created_date
   ,created_user)
  SELECT
    rris.id
   ,rris.Importer_Name
   ,rris.Supplier_Name
   ,rris.Main_Supplier
   ,rris.Importer
   ,rris.Destination_State
   ,rris.Consignee
   ,rris.Manufacturer
   ,rris.Seller
   ,rris.Sold_to_party
   ,rris.Ship_to_party
   ,rris.CountryofOrigin
   ,rris.Relationship
   ,rris.DFT
   ,rris.Value_Recon
   ,rris.FTA_Recon
   ,rris.Payment_Type
   ,rris.Broker_to_pay
   ,rris.Value
   ,rris.Freight
   ,rris.CreatedDate
   ,rris.CreatedUser
  FROM dbo.Rail_Rule_ImporterSupplier rris;
SET IDENTITY_INSERT dbo.imp_rail_rule_importer_supplier OFF;
GO

PRINT ('copy product rule data')
GO
SET IDENTITY_INSERT dbo.imp_rail_rule_product ON;
INSERT INTO dbo.imp_rail_rule_product (
    id
   ,description1
   ,prod_id_1
   ,attribute_1
   ,attribute_2
   ,tariff
   ,description
   ,goods_description
   ,invoice_uom
   ,template_hts_quantity
   ,template_invoice_quantity
   ,created_date
   ,created_user)
  SELECT
    rrdd.id
   ,rrdd.Description1
   ,rrdd.Prod_ID_1
   ,rrdd.Attribute_1
   ,rrdd.Attribute_2
   ,rrdd.Tariff
   ,rrdd.Description
   ,rrdd.Goods_Description
   ,rrdd.Invoice_UOM
   ,rrdd.Template_HTS_Quantity
   ,rrdd.Template_Invoice_Quantity
   ,rrdd.CreatedDate
   ,rrdd.CreatedUser
  FROM dbo.Rail_Rule_Desc1_Desc2 rrdd;
SET IDENTITY_INSERT dbo.imp_rail_rule_product OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.imp_rail_form_section_configuration ON;
INSERT INTO dbo.imp_rail_form_section_configuration (
    id
   ,name
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,procedure_name)
  SELECT
    rs.id
   ,rs.name
   ,rs.title
   ,rs.table_name
   ,rs.is_array
   ,rs.parent_id
   ,rs.procedure_name
  FROM dbo.rail_sections rs;
SET IDENTITY_INSERT dbo.imp_rail_form_section_configuration OFF;
GO

UPDATE imp_rail_form_section_configuration
SET table_name = 'imp_rail_container'
   ,procedure_name = 'sp_imp_rail_add_container'
WHERE table_name = 'Rail_ContainersTab';
UPDATE imp_rail_form_section_configuration
SET table_name = 'imp_rail_declaration'
   ,procedure_name = 'sp_imp_rail_add_declaration'
WHERE table_name = 'Rail_DeclarationTab';
UPDATE imp_rail_form_section_configuration
SET table_name = 'imp_rail_invoice_header'
   ,procedure_name = 'sp_imp_rail_add_invoice_header'
WHERE table_name = 'Rail_InvoiceHeaders';
UPDATE imp_rail_form_section_configuration
SET table_name = 'imp_rail_invoice_line'
   ,procedure_name = 'sp_imp_rail_add_invoice_line'
WHERE table_name = 'Rail_InvoiceLines';
UPDATE imp_rail_form_section_configuration
SET table_name = 'imp_rail_misc'
   ,procedure_name = 'sp_imp_rail_add_misc'
WHERE table_name = 'Rail_MISC';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.imp_rail_form_configuration ON;
INSERT INTO dbo.imp_rail_form_configuration (
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
    rd.id
   ,rd.section_id
   ,rd.ColName
   ,rd.ValueLabel
   ,rd.ValueDesc
   ,rd.DefValue
   ,rd.FHasDefaultVal
   ,rd.FEditable
   ,rd.FMandatory
   ,rd.Display_on_UI
   ,rd.FManual
   ,rd.SingleFilingOrder
   ,rd.paired_field_table
   ,rd.paired_field_column
   ,rd.handbook_name
   ,rd.CreatedDate
   ,rd.CreatedUser
  FROM dbo.Rail_DEFValues rd;
SET IDENTITY_INSERT dbo.imp_rail_form_configuration OFF;
GO
UPDATE dbo.imp_rail_form_configuration
SET column_name = 'price_unit'
WHERE column_name = 'PriceUnit';
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.imp_rail_inbound
ADD CONSTRAINT FK__imp_rail_inbound__imp_rail_edi_message__broker_download_id FOREIGN KEY (broker_download_id) REFERENCES dbo.imp_rail_broker_download (id)
GO

ALTER TABLE dbo.imp_rail_filing_header
ADD CONSTRAINT [FK__imp_rail_filing_header__FilingStatus_filing_status] FOREIGN KEY (filing_status) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.imp_rail_filing_header
ADD CONSTRAINT [FK__imp_rail_filing_header__MappingStatus_mapping_status] FOREIGN KEY (mapping_status) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.imp_rail_filing_detail
ADD CONSTRAINT FK__imp_rail_filing_detail__imp_rail_inbound__inbound_id FOREIGN KEY (inbound_id) REFERENCES dbo.imp_rail_inbound (id)
GO

ALTER TABLE dbo.imp_rail_filing_detail
ADD CONSTRAINT FK__imp_rail_filing_detail__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id)
GO

ALTER TABLE dbo.imp_rail_declaration
ADD CONSTRAINT FK__imp_rail_declaration__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_container
ADD CONSTRAINT FK__imp_rail_container__imp_rail_inbound__source_record_id FOREIGN KEY (source_record_id) REFERENCES dbo.imp_rail_inbound (id)
GO

ALTER TABLE dbo.imp_rail_container
ADD CONSTRAINT FK__imp_rail_container__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_invoice_header
ADD CONSTRAINT FK__imp_rail_invoice_header__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_invoice_line
ADD CONSTRAINT FK__imp_rail_invoice_line__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id)
GO

ALTER TABLE dbo.imp_rail_invoice_line
ADD CONSTRAINT FK__imp_rail_invoice_line__imp_rail_invoice_header__parent_record_id FOREIGN KEY (parent_record_id) REFERENCES dbo.imp_rail_invoice_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_misc
ADD CONSTRAINT FK__imp_rail_misc__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_document
ADD CONSTRAINT [FK__imp_rail_document__imp_rail_inbound__inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.imp_rail_inbound (id)
GO

ALTER TABLE dbo.imp_rail_document
ADD CONSTRAINT FK__imp_rail_document__imp_rail_filing_header__filing_header_id FOREIGN KEY (filing_header_id) REFERENCES dbo.imp_rail_filing_header (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.imp_rail_form_section_configuration
ADD CONSTRAINT FK_imp_rail_form_section_configuration__imp_rail_form_section_configuration__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.imp_rail_form_section_configuration (id)
GO

ALTER TABLE dbo.imp_rail_form_configuration
ADD CONSTRAINT FK__imp_rail_form_configuration__imp_rail_form_section_configuration__section_id FOREIGN KEY (section_id) REFERENCES dbo.imp_rail_form_section_configuration (id)
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
FROM imp_rail_form_configuration field
JOIN imp_rail_form_section_configuration section
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
CREATE INDEX Idx__description1
ON dbo.imp_rail_inbound (description1)
ON [PRIMARY]
GO

CREATE INDEX Idx__duplicate_of
ON dbo.imp_rail_inbound (duplicate_of)
ON [PRIMARY]
GO

CREATE INDEX Idx__importer__supplier
ON dbo.imp_rail_inbound (importer, supplier)
ON [PRIMARY]
GO

CREATE INDEX Idx__port_of_unlading
ON dbo.imp_rail_inbound (port_of_unlading)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_status
ON dbo.imp_rail_filing_header (filing_status)
ON [PRIMARY]
GO

CREATE INDEX Idx_mapping_status
ON dbo.imp_rail_filing_header (mapping_status)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_rail_declaration (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_rail_container (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_rail_invoice_header (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_rail_invoice_line (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_record_id
ON dbo.imp_rail_invoice_line (parent_record_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx__filing_header_id
ON dbo.imp_rail_misc (filing_header_id)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_filing_header_id
ON dbo.imp_rail_document (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_inbound_record_id
ON dbo.imp_rail_document (inbound_record_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__port
ON dbo.imp_rail_rule_port (port)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__importer_name__supplier_name
ON dbo.imp_rail_rule_importer_supplier (importer_name, supplier_name)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__description1
ON dbo.imp_rail_rule_product (description1)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__name
ON dbo.imp_rail_form_section_configuration (name)
ON [PRIMARY]
GO

CREATE INDEX Idx__parent_id
ON dbo.imp_rail_form_section_configuration (parent_id)
ON [PRIMARY]
GO

CREATE INDEX Idx__section_id
ON dbo.imp_rail_form_configuration (section_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx__single_filing_order
ON dbo.imp_rail_form_configuration (single_filing_order)
WHERE ([single_filing_order] IS NOT NULL)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- weight conversion function
CREATE FUNCTION dbo.fn_app_weight_to_ton (@quantity DECIMAL(18, 9) = 0,
@unit VARCHAR(2))
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @rate DECIMAL(18, 9) = NULL;
  SELECT
    @rate = wcr.rate
  FROM dbo.app_weight_conversion_rate wcr
  WHERE wcr.weight_unit = @unit
  RETURN @rate * @quantity
END
GO

-- gets rail gross weight
CREATE FUNCTION dbo.fn_imp_rail_gross_weight (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(1)
  FROM dbo.imp_rail_container container
  WHERE container.filing_header_id = @filingHeaderId;

  SELECT
    @result = SUM(dbo.fn_app_weight_to_ton(container.gross_weight, container.gross_weight_unit))
  FROM dbo.imp_rail_container container
  WHERE container.filing_header_id = @filingHeaderId;

  RETURN CASE
    WHEN @count > 1 THEN @result
    ELSE @result * 1000
  END;
END;
GO

-- gets rail gross weight unit
CREATE FUNCTION dbo.fn_imp_rail_gross_weight_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(1)
  FROM dbo.imp_rail_container container
  WHERE container.filing_header_id = @filingHeaderId;

  SELECT
    @result =
    CASE
      WHEN @count > 1 THEN 'T'
      ELSE 'KG'
    END;

  RETURN @result;
END;
GO

-- gets rail invoice line number
CREATE FUNCTION dbo.fn_imp_rail_invoice_line_number (@invoiceHeaderId INT
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
    FROM dbo.imp_rail_invoice_line invoice_line
    WHERE invoice_line.parent_record_id = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END;
GO

-- alter rail invoice line table
ALTER TABLE dbo.imp_rail_invoice_line
ADD invoice_line_number AS (dbo.fn_imp_rail_invoice_line_number(parent_record_id, id));
ALTER TABLE dbo.imp_rail_invoice_line
ADD gross_weight AS (dbo.fn_imp_rail_gross_weight(filing_header_id));
ALTER TABLE dbo.imp_rail_invoice_line
ADD gross_weight_unit AS (dbo.fn_imp_rail_gross_weight_unit(filing_header_id));
GO

-- gets invoice total
CREATE FUNCTION dbo.fn_imp_rail_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(invoice_line.customs_qty * invoice_line.price_unit)
  FROM dbo.imp_rail_invoice_line invoice_line
  WHERE invoice_line.parent_record_id = @invoiceHeaderId
  GROUP BY invoice_line.parent_record_id

  RETURN @result
END;
GO

-- alter rail invoice header table
ALTER TABLE dbo.imp_rail_invoice_header
ADD invoice_total AS (dbo.fn_imp_rail_invoice_total(id));
GO

-- gets rail summary gross weight
CREATE FUNCTION dbo.fn_imp_rail_gross_weight_summary (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;

  SELECT
    @result = SUM(invoice_line.gross_weight)
  FROM dbo.imp_rail_invoice_line invoice_line
  WHERE invoice_line.filing_header_id = @filingHeaderId;

  RETURN @result;
END;
GO

-- gets rail summary gross weight unit
CREATE FUNCTION dbo.fn_imp_rail_gross_weight_summary_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;

  SELECT TOP (1)
    @result = invoice_line.gross_weight_unit
  FROM dbo.imp_rail_invoice_line invoice_line
  WHERE invoice_line.filing_header_id = @filingHeaderId;

  RETURN @result;
END;
GO

-- alter rail invoice header table
ALTER TABLE dbo.imp_rail_filing_header
ADD gross_weight_summary_unit AS (dbo.fn_imp_rail_gross_weight_summary_unit(id));
ALTER TABLE dbo.imp_rail_filing_header
ADD gross_weight_summary AS (dbo.fn_imp_rail_gross_weight_summary(id));
GO

-- gets rail car count
CREATE FUNCTION dbo.fn_imp_rail_car_count (@filingHeaderId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = COUNT(detail.inbound_id)
  FROM dbo.imp_rail_filing_detail detail
  WHERE detail.filing_header_id = @filingHeaderId
  GROUP BY detail.filing_header_id
  RETURN @result
END;
GO

PRINT ('create views')
GO

CREATE VIEW dbo.v_imp_rail_form_configuration
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
FROM dbo.imp_rail_form_configuration form
INNER JOIN dbo.imp_rail_form_section_configuration sections
  ON form.section_id = sections.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_name = sections.table_name
WHERE (clmn.column_name
NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id', 'broker_download_id')
AND clmn.table_schema = 'dbo')
OR clmn.column_name IS NULL
GO

CREATE VIEW dbo.v_imp_rail_review_grid
AS
SELECT
  f.inbound_id AS id
 ,h.id AS filing_header_id
 ,p.broker_download_id AS manifest_id
 ,d.importer AS importer
 ,d.entry_port AS port_code
 ,c.bill_num AS bill_of_lading
 ,c.container_number AS container_number
 ,p.reference_number1 AS train_number
 ,ISNULL(c.gross_weight, 0) AS gross_weight
 ,ISNULL(c.gross_weight_unit, 0) AS gross_weight_unit
FROM dbo.imp_rail_filing_header h
INNER JOIN dbo.imp_rail_filing_detail f
  ON h.id = f.filing_header_id
INNER JOIN dbo.imp_rail_inbound p
  ON f.inbound_id = p.id
LEFT OUTER JOIN dbo.imp_rail_declaration d
  ON d.filing_header_id = h.id
LEFT OUTER JOIN dbo.imp_rail_container c
  ON c.filing_header_id = h.id
    AND f.inbound_id = c.source_record_id
GO

CREATE VIEW dbo.v_imp_rail_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN imp_rail_form_section_configuration s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id', 'broker_download_id')
GO

CREATE VIEW dbo.v_imp_rail_inbound_grid
AS
SELECT
  inbound.id AS BD_Parsed_Id
 ,inbound.broker_download_id AS BD_Parsed_EDIMessage_Id
 ,filing_headers.id AS Filing_Headers_id
 ,inbound.importer AS BD_Parsed_Importer
 ,inbound.supplier AS BD_Parsed_Supplier
 ,inbound.port_of_unlading AS BD_Parsed_PortOfUnlading
 ,inbound.description1 AS BD_Parsed_Description1
 ,inbound.bill_of_lading AS BD_Parsed_BillofLading
 ,inbound.issuer_code AS BD_Parsed_Issuer_Code
 ,CONCAT(inbound.equipment_initial, inbound.equipment_number) AS BD_Parsed_Container_Number
 ,inbound.reference_number1 AS BD_Parsed_ReferenceNumber1
 ,inbound.created_date AS BD_Parsed_CreatedDate
 ,inbound.deleted AS BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(inbound.duplicate_of, 0)) AS BD_Parsed_Is_Duplicated
 ,importer_supplier.importer AS Rule_ImporterSupplier_Importer
 ,importer_supplier.main_supplier AS Rule_ImporterSupplier_Main_Supplier
 ,rail_description.tariff AS Rule_Desc1_Desc2_Tariff
 ,rail_port.[port] AS Rule_Port_Port
 ,filing_headers.filing_number AS Filing_Headers_FilingNumber
 ,filing_headers.job_hyperlink AS Filing_Headers_JobLink
 ,mapping_status.id AS Filing_Headers_MappingStatus
 ,mapping_status.[name] AS Filing_Headers_MappingStatus_Title
 ,filing_status.id AS Filing_Headers_FilingStatus
 ,filing_status.[name] AS Filing_Headers_FilingStatus_Title
 ,rail_description.[description]

FROM dbo.imp_rail_inbound inbound
LEFT JOIN dbo.imp_rail_rule_port rail_port
  ON inbound.port_of_unlading = rail_port.Port
LEFT JOIN dbo.imp_rail_rule_importer_supplier importer_supplier
  ON inbound.importer = importer_supplier.importer_name
    AND (inbound.supplier = importer_supplier.supplier_name
      OR (inbound.supplier IS NULL
        AND importer_supplier.supplier_name IS NULL))
LEFT JOIN dbo.imp_rail_rule_product rail_description
  ON rail_description.description1 = inbound.description1

OUTER APPLY (SELECT
    irfh.id
   ,irfh.filing_number
   ,irfh.job_hyperlink
   ,irfh.mapping_status
   ,irfh.filing_status
  FROM dbo.imp_rail_filing_header irfh
  JOIN dbo.imp_rail_filing_detail irfd
    ON irfh.id = irfd.filing_header_id
  WHERE irfd.inbound_id = inbound.id
  AND irfh.mapping_status > 0) AS filing_headers

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.filing_status, 0) = filing_status.id
GO

CREATE VIEW dbo.v_imp_rail_report
AS
SELECT
  headers.id AS Rail_Filing_Headers_id
 ,detailes.inbound_id AS BDP_PK
 ,declaration.arr AS Rail_DeclarationTab_Arr
 ,declaration.arr_2 AS Rail_DeclarationTab_Arr_2
 ,declaration.carrier_scac AS Rail_DeclarationTab_Carrier_SCAC
 ,declaration.centralized_exam_site AS Rail_DeclarationTab_Centralized_Exam_Site
 ,declaration.certify_cargo_release AS Rail_DeclarationTab_Certify_Cargo_Release
 ,declaration.check_local_client AS Rail_DeclarationTab_Check_Local_Client
 ,declaration.container AS Rail_DeclarationTab_Container
 ,declaration.country_of_export AS Rail_DeclarationTab_Country_of_Export
 ,declaration.dep AS Rail_DeclarationTab_Dep
 ,declaration.description AS Rail_DeclarationTab_Description
 ,declaration.destination AS Rail_DeclarationTab_Destination
 ,declaration.destination_state AS Rail_DeclarationTab_Destination_State
 ,declaration.discharge AS Rail_DeclarationTab_Discharge
 ,declaration.enable_entry_sum AS Rail_DeclarationTab_Enable_Entry_Sum
 ,declaration.entry_port AS Rail_DeclarationTab_Entry_Port
 ,declaration.entry_type AS Rail_DeclarationTab_Entry_Type
 ,declaration.eta AS Rail_DeclarationTab_ETA
 ,declaration.export_date AS Rail_DeclarationTab_Export_Date
 ,declaration.firms_code AS Rail_DeclarationTab_FIRMs_Code
 ,declaration.hmf AS Rail_DeclarationTab_HMF
 ,declaration.importer AS Rail_DeclarationTab_Importer
 ,declaration.importer_of_record AS Rail_DeclarationTab_Importer_of_record
 ,declaration.inco AS Rail_DeclarationTab_INCO
 ,declaration.issuer AS Rail_DeclarationTab_Issuer
 ,declaration.main_supplier AS Rail_DeclarationTab_Main_Supplier
 ,declaration.manual_entry AS Rail_DeclarationTab_Manual_Entry
 ,declaration.master_bill AS Rail_DeclarationTab_Master_Bill
 ,declaration.no_packages AS Rail_DeclarationTab_No_Packages
 ,declaration.origin AS Rail_DeclarationTab_Origin
 ,declaration.owner_ref AS Rail_DeclarationTab_Owner_Ref
 ,declaration.purchased AS Rail_DeclarationTab_Purchased
 ,declaration.rlf AS Rail_DeclarationTab_RLF
 ,declaration.service AS Rail_DeclarationTab_Service
 ,declaration.shipment_type AS Rail_DeclarationTab_Shipment_Type
 ,declaration.split_shipment_release AS Rail_DeclarationTab_Split_Shipment_Release
 ,declaration.total_volume AS Rail_DeclarationTab_Total_Volume
 ,declaration.total_weight AS Rail_DeclarationTab_Total_Weight
 ,declaration.transport AS Rail_DeclarationTab_Transport
 ,declaration.type AS Rail_DeclarationTab_Type

 ,containers.bill_issuer_scac AS Rail_Packing_Bill_Issuer_SCAC
 ,containers.bill_number AS Rail_Packing_Bill_Number
 ,containers.bill_type AS Rail_Packing_Bill_Type
 ,containers.container_number AS Rail_Packing_Container_Number
 ,containers.is_split AS Rail_Packing_Is_Split
 ,containers.it_number AS Rail_Packing_IT_Number
 ,containers.manifest_qty AS Rail_Packing_Manifest_QTY
 ,containers.marks_and_numbers AS Rail_Packing_Marks_and_Numbers
 ,containers.pack_qty AS Rail_Packing_Pack_QTY
 ,containers.pack_type AS Rail_Packing_Pack_Type
 ,containers.shipping_symbol AS Rail_Packing_Shipping_Symbol
 ,containers.uq AS Rail_Packing_UQ
 ,containers.packing_uq AS Rail_Packing_Container_Packing_UQ
 ,containers.seal_number AS Rail_Packing_Seal_Number
 ,containers.type AS Rail_Packing_Type
 ,containers.mode AS Rail_Packing_Mode
 ,containers.goods_weight AS Rail_Packing_Goods_Weight
 ,containers.bill_num AS Rail_Packing_Bill_Num
 ,containers.gross_weight AS Rail_Packing_Gross_Weight
 ,containers.gross_weight_unit AS Rail_Packing_Gross_Weight_Unit

 ,invheaders.agreed_place AS Rail_InvoiceHeaders_Agreed_Place
 ,invheaders.broker_pga_contact_email AS Rail_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.broker_pga_contact_name AS Rail_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.broker_pga_contact_phone AS Rail_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.consignee AS Rail_InvoiceHeaders_Consignee
 ,invheaders.consignee_address AS Rail_InvoiceHeaders_Consignee_Address
 ,invheaders.curr AS Rail_InvoiceHeaders_Curr
 ,invheaders.epa_pst_cert_date AS Rail_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.epa_tsca_cert_date AS Rail_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.epa_vne_cert_date AS Rail_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.export AS Rail_InvoiceHeaders_Export
 ,invheaders.export_date AS Rail_InvoiceHeaders_Export_Date
 ,invheaders.first_sale AS Rail_InvoiceHeaders_First_Sale
 ,invheaders.fsis_cert_date AS Rail_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.fws_cert_date AS Rail_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.importer AS Rail_InvoiceHeaders_Importer
 ,invheaders.inco AS Rail_InvoiceHeaders_INCO
 ,invheaders.inv_date AS Rail_InvoiceHeaders_Inv_Date
 ,invheaders.inv_gross_weight AS Rail_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.invoice_no AS Rail_InvoiceHeaders_Invoice_No
 ,invheaders.invoice_total AS Rail_InvoiceHeaders_Invoice_Total
 ,invheaders.lacey_act_cert_date AS Rail_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.landed_costing_ex_rate AS Rail_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.manufacturer AS Rail_InvoiceHeaders_Manufacturer
 ,invheaders.net_weight AS Rail_InvoiceHeaders_Net_Weight
 ,invheaders.nhtsa_cert_date AS Rail_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.origin AS Rail_InvoiceHeaders_Origin
 ,invheaders.packages AS Rail_InvoiceHeaders_Packages
 ,invheaders.payment_date AS Rail_InvoiceHeaders_Payment_Date
 ,invheaders.seller AS Rail_InvoiceHeaders_Seller
 ,invheaders.ship_to_party AS Rail_InvoiceHeaders_Ship_to_party
 ,invheaders.sold_to_party AS Rail_InvoiceHeaders_Sold_to_party
 ,invheaders.supplier AS Rail_InvoiceHeaders_Supplier
 ,invheaders.supplier_address AS Rail_InvoiceHeaders_Supplier_Address
 ,invheaders.transaction_related AS Rail_InvoiceHeaders_Transaction_Related

 ,invlines.attribute_1 AS Rail_InvoiceLines_Attribute_1
 ,invlines.attribute_2 AS Rail_InvoiceLines_Attribute_2
 ,invlines.certifying_individual AS Rail_InvoiceLines_Certifying_Individual
 ,invlines.cif_component AS Rail_InvoiceLines_CIF_Component
 ,invlines.code AS Rail_InvoiceLines_Code
 ,invlines.consignee AS Rail_InvoiceLines_Consignee
 ,invlines.curr AS Rail_InvoiceLines_Curr
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.customs_qty) AS Rail_InvoiceLines_Customs_QTY
 ,invlines.dest_state AS Rail_InvoiceLines_Dest_State
 ,invlines.epa_tsca_toxic_substance_control_act_indicator AS Rail_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.export AS Rail_InvoiceLines_Export
 ,invlines.goods_description AS Rail_InvoiceLines_Goods_Description
 ,invlines.invoice_no AS Rail_InvoiceLines_Invoice_No
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.invoice_qty) AS Rail_InvoiceLines_Invoice_Qty
 ,invlines.invoice_qty_unit AS Rail_InvoiceLines_Invoice_Qty_Unit
 ,invlines.line_price AS Rail_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Rail_InvoiceLines_LNO
 ,invlines.manufacturer AS Rail_InvoiceLines_Manufacturer
 ,invlines.org AS Rail_InvoiceLines_ORG
 ,invlines.origin AS Rail_InvoiceLines_Origin
 ,invlines.pga_contact_email AS Rail_InvoiceLines_PGA_Contact_Email
 ,invlines.pga_contact_name AS Rail_InvoiceLines_PGA_Contact_Name
 ,invlines.pga_contact_phone AS Rail_InvoiceLines_PGA_Contact_Phone
 ,invlines.price_unit AS Rail_InvoiceLines_PriceUnit
 ,invlines.prod_id_1 AS Rail_InvoiceLines_Prod_ID_1
 ,invlines.sold_to_party AS Rail_InvoiceLines_Sold_To_Party
 ,invlines.spi AS Rail_InvoiceLines_SPI
 ,invlines.tariff AS Rail_InvoiceLines_Tariff
 ,invlines.transaction_related AS Rail_InvoiceLines_Transaction_Related
 ,invlines.tsca_indicator AS Rail_InvoiceLines_TSCA_Indicator
 ,invlines.uq AS Rail_InvoiceLines_UQ
 ,(dbo.fn_imp_rail_car_count(invlines.filing_header_id) * invlines.amount) AS Rail_InvoiceLines_Amount
 ,invlines.gross_weight AS Rail_GrossWeightSummary
 ,invlines.gross_weight_unit AS Rail_GrossWeightSummaryUnit

 ,misc.bond_type AS Rail_MISC_Bond_Type
 ,misc.branch AS Rail_MISC_Branch
 ,misc.broker AS Rail_MISC_Broker
 ,misc.broker_to_pay AS Rail_MISC_Broker_to_Pay
 ,misc.fta_recon AS Rail_MISC_FTA_Recon
 ,misc.merge_by AS Rail_MISC_Merge_By
 ,misc.payment_type AS Rail_MISC_Payment_Type
 ,misc.prelim_statement_date AS Rail_MISC_Prelim_Statement_Date
 ,misc.preparer_dist_port AS Rail_MISC_Preparer_Dist_Port
 ,misc.recon_issue AS Rail_MISC_Recon_Issue
 ,misc.submitter AS Rail_MISC_Submitter
 ,misc.tax_deferrable_ind AS Rail_MISC_Tax_Deferrable_Ind

FROM dbo.imp_rail_filing_header headers
INNER JOIN dbo.imp_rail_filing_detail detailes
  ON headers.id = detailes.filing_header_id
LEFT JOIN dbo.imp_rail_declaration declaration
  ON declaration.filing_header_id = headers.id
LEFT JOIN dbo.imp_rail_container containers
  ON containers.filing_header_id = headers.id
    AND detailes.inbound_id = containers.id
LEFT JOIN dbo.imp_rail_invoice_line invlines
  ON invlines.filing_header_id = headers.id
LEFT JOIN dbo.imp_rail_invoice_header invheaders
  ON invheaders.filing_header_id = headers.id
    AND invheaders.id = invlines.parent_record_id
LEFT JOIN dbo.imp_rail_misc misc
  ON misc.filing_header_id = headers.id
WHERE headers.mapping_status = 2
GO

PRINT ('create stored procedures')
GO
-- transpose table row
CREATE PROCEDURE dbo.sp_app_transpose (@tableName VARCHAR(128)
, @filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL)
AS
BEGIN
  DECLARE @selectColumnsList VARCHAR(MAX);
  DECLARE @columnsList VARCHAR(MAX);
  DECLARE @selectStatment VARCHAR(MAX);
  DECLARE @mergeStatment VARCHAR(MAX);

  -- get table column names with type converion for select statment
  SELECT
    @selectColumnsList = COALESCE(@selectColumnsList + ',', '') + 'isnull(cast(' +
    CASE
      WHEN t.name LIKE N'date%' THEN 'format(' + QUOTENAME(c.name) + ', ''MM/dd/yyyy'')'
      WHEN t.name LIKE N'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
      WHEN t.name LIKE N'decimal' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
      ELSE QUOTENAME(c.name)
    END
    + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.name)
  FROM sys.columns c
  INNER JOIN sys.types t
    ON c.user_type_id = t.user_type_id
  WHERE c.object_id = OBJECT_ID(@tableName)

  -- get table column names for UNPIVOT statment
  SELECT
    @columnsList = COALESCE(@columnsList + ',', '') + QUOTENAME(c.name)
  FROM sys.columns c
  WHERE objecT_id = OBJECT_ID(@tableName)
  AND (c.name) NOT IN (N'id', N'filing_header_id', N'parent_record_id', N'operation_id', N'source_record_id', N'created_date', N'created_user')

  -- set sselect statment
  SET @selectStatment =
  ' SELECT unpvt.id, unpvt.filing_header_id ,unpvt.parent_record_id, ''' + @tableName + ''' AS tableName, unpvt.column_name, unpvt.value
  FROM (SELECT ' + @selectColumnsList + ' FROM  ' + @tableName + ' where filing_header_id IN (' + @filingHeaderIds + ') ' +
  IIF(@operationId IS NOT NULL, ' AND operation_id=''' + CAST(@operationId AS VARCHAR(38)) + '''', '') + ') p
  UNPIVOT (value FOR column_name IN (' + @columnsList + ')) as unpvt'

  EXEC (@selectStatment)
END
GO

-- review mapped data
CREATE PROCEDURE dbo.sp_imp_rail_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM dbo.imp_rail_form_section_configuration rs
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
    FROM dbo.imp_rail_form_configuration defValue
    INNER JOIN dbo.imp_rail_form_section_configuration section
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

-- add rail declaration tab record --
CREATE PROCEDURE dbo.sp_imp_rail_add_declaration (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_declaration'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @masterBill VARCHAR(128)

  SET @operationId = COALESCE(@operationId, NEWID());

  SELECT TOP 1
    @masterBill = p.bill_of_lading
  FROM dbo.imp_rail_filing_detail d
  INNER JOIN dbo.imp_rail_inbound p
    ON p.id = d.inbound_id
  WHERE d.filing_header_id = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_declaration pdt
      WHERE pdt.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_declaration (
        parent_record_id
       ,filing_header_id
       ,carrier_scac
       ,country_of_export
       ,description
       ,destination
       ,destination_state
       ,discharge
       ,entry_port
       ,firms_code
       ,importer
       ,issuer
       ,main_supplier
       ,origin
       ,master_bill
       ,owner_ref
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,p.issuer_code AS Carrier_SCAC
       ,rp.export AS Country_of_Export
       ,p.description1 AS Description
       ,rp.destination
       ,rn.destination_state
       ,p.port_of_unlading AS Discharge
       ,p.port_of_unlading AS Entry_Port
       ,rp.firms_code
       ,rn.importer
       ,p.issuer_code AS Issuer
       ,rn.main_supplier
       ,rp.origin
       ,@masterBill
       ,p.reference_number1
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO
-- add rail containers tab record --
CREATE PROCEDURE dbo.sp_imp_rail_add_container (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_container';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName;

  -- add container data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_container pct
      WHERE pct.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_container (
        parent_record_id
       ,filing_header_id
       ,bill_issuer_scac
       ,bill_num
       ,bill_number
       ,container_number
       ,gross_weight
       ,gross_weight_unit
       ,operation_id
       ,source_record_id)
      SELECT
        @parentId
       ,@filingHeaderId
       ,p.issuer_code
       ,p.bill_of_lading
       ,CONCAT('MB:', p.bill_of_lading)
       ,CONCAT(p.equipment_initial, p.equipment_number)
       ,p.weight
       ,p.weight_unit
       ,@operationId
       ,p.id
      FROM dbo.imp_rail_filing_detail details
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = details.inbound_id
      WHERE details.filing_header_id = @filingHeaderId
  END
END;
GO
-- add rail misc record --
CREATE PROCEDURE dbo.sp_imp_rail_add_misc (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_misc';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_misc pm
      WHERE pm.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_misc (
        parent_record_id
       ,filing_header_id
       ,recon_issue
       ,fta_recon
       ,payment_type
       ,broker_to_pay
       ,submitter
       ,branch
       ,broker
       ,preparer_dist_port
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,ISNULL(rn.value_recon, 'N/A') AS recon_issue
       ,rn.fta_recon
       ,rn.payment_type
       ,rn.broker_to_pay
       ,rn.importer AS submitter
       ,user_data.Branch
       ,user_data.Broker
       ,user_data.Location
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN app_users_data user_data
        ON user_data.UserAccount = @filingUser
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO
-- add rail invoice line record --
CREATE PROCEDURE dbo.sp_imp_rail_add_invoice_line (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_invoice_line';
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_line pil
      WHERE pil.parent_record_id = @parentId)
  BEGIN
    INSERT INTO imp_rail_invoice_line (
        parent_record_id
       ,filing_header_id
       ,attribute_1
       ,attribute_2
       ,consignee
       ,dest_state
       ,export
       ,goods_description
       ,manufacturer
       ,org
       ,origin
       ,prod_id_1
       ,tariff
       ,transaction_related
       ,customs_qty
       ,spi
       ,uq
       ,price_unit
       ,invoice_qty
       ,invoice_qty_unit
       ,amount
       ,line_price
       ,description
       ,operation_id)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,rd.attribute_1
       ,rd.attribute_2
       ,rn.consignee
       ,rn.destination_state
       ,rp.export
       ,rd.goods_description
       ,rn.manufacturer
       ,rn.country_of_origin AS org
       ,rn.country_of_origin AS origin
       ,rd.prod_id_1
       ,rd.tariff
       ,rn.relationship AS transaction_related
       ,rd.template_hts_quantity AS customs_qty
       ,rn.dft AS spi
       ,rd.invoice_uom AS uq
       ,rn.[value] AS price_unit
       ,rd.template_invoice_quantity AS invoice_qty
       ,rd.invoice_uom AS invoice_qty_unit
       ,rn.freight AS amount
       ,rn.[value] * rd.template_invoice_quantity AS line_price
       ,rd.description AS description
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON p.importer = rn.importer_name
          AND (p.supplier = rn.supplier_name
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      LEFT JOIN dbo.imp_rail_rule_product rd
        ON RTRIM(LTRIM(rd.description1)) = RTRIM(LTRIM(p.description1))
      WHERE d.filing_header_id = @filingHeaderId
  END
END;
GO
-- add rail invoice header record --
CREATE PROCEDURE dbo.sp_imp_rail_add_invoice_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'imp_rail_invoice_header';
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM imp_rail_form_section_configuration ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.imp_rail_invoice_header pih
      WHERE pih.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO imp_rail_invoice_header (
        parent_record_id
       ,filing_header_id
       ,consignee
       ,export
       ,importer
       ,manufacturer
       ,origin
       ,seller
       ,ship_to_party
       ,sold_to_party
       ,supplier
       ,transaction_related
       ,operation_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,@filingHeaderId
       ,rn.consignee
       ,rp.export
       ,rn.importer
       ,rn.manufacturer
       ,rn.country_of_origin AS origin
       ,rn.seller
       ,rn.ship_to_party
       ,rn.sold_to_party
       ,rn.main_supplier AS supplier
       ,rn.relationship AS transaction_related
       ,@operationId
      FROM dbo.imp_rail_filing_detail d
      INNER JOIN dbo.imp_rail_inbound p
        ON p.id = d.inbound_id
      LEFT JOIN dbo.imp_rail_rule_port rp
        ON p.port_of_unlading = rp.port
      LEFT JOIN dbo.imp_rail_rule_importer_supplier rn
        ON RTRIM(LTRIM(p.importer)) = RTRIM(LTRIM(rn.importer_name))
          AND (RTRIM(LTRIM(p.supplier)) = RTRIM(LTRIM(rn.supplier_name))
            OR (p.supplier IS NULL
              AND rn.supplier_name IS NULL))
      WHERE d.filing_header_id = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.sp_imp_rail_add_invoice_line @filingHeaderId
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
CREATE PROCEDURE dbo.sp_imp_rail_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  
  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add declaration
  EXEC dbo.sp_imp_rail_add_declaration @filingHeaderId
                                      ,@filingHeaderId
                                      ,@filingUser
                                      ,@operationId
  -- add container
  EXEC dbo.sp_imp_rail_add_container @filingHeaderId
                                    ,@filingHeaderId
                                    ,@filingUser
                                    ,@operationId
  -- add invoice header
  EXEC dbo.sp_imp_rail_add_invoice_header @filingHeaderId
                                         ,@filingHeaderId
                                         ,@filingUser
                                         ,@operationId
  -- add misc
  EXEC dbo.sp_imp_rail_add_misc @filingHeaderId
                               ,@filingHeaderId
                               ,@filingUser
                               ,@operationId
END;
GO
-- delete rail filing entry
CREATE PROCEDURE dbo.sp_imp_rail_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM dbo.imp_rail_filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM dbo.imp_rail_filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM dbo.imp_rail_form_section_configuration ps
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
-- soft delete rail inbound record
CREATE PROCEDURE dbo.sp_imp_rail_delete_inbound (@id INT,
@FDeleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = rig.Filing_Headers_id
   ,@mapping_status = rig.Filing_Headers_MappingStatus
  FROM v_imp_rail_inbound_grid rig
  WHERE rig.BD_Parsed_Id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE imp_rail_inbound
    SET deleted = @FDeleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE imp_rail_inbound
      SET deleted = @FDeleted
      WHERE id IN (SELECT
          rfd.inbound_id
        FROM imp_rail_filing_detail rfd
        WHERE rfd.filing_header_id = @filingHeaderId)
    END
  END
END
GO
-- update rail filing entry
CREATE PROCEDURE dbo.sp_imp_rail_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,value VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (
      id
     ,record_id
     ,value
     ,table_name
     ,column_name)
    SELECT
      field.id
     ,field.record_id
     ,field.value
     ,section.table_name
     ,config.column_name
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN dbo.imp_rail_form_configuration config
      ON config.id = field.id
    INNER JOIN dbo.imp_rail_form_section_configuration section
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
  AND field.table_name = @tableName;

  SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@recordId AS VARCHAR(10)) + ';' + CHAR(10);

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  END;

  CLOSE cur;
  DEALLOCATE cur

  EXEC (@command);
END
GO
-- recalculate rail fileds
CREATE PROCEDURE dbo.sp_imp_rail_recalculate
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
DROP FUNCTION dbo.fn_GetRailCarCount
ALTER TABLE dbo.Rail_Filing_Headers
DROP COLUMN GrossWeightSummaryUnit
ALTER TABLE dbo.Rail_Filing_Headers
DROP COLUMN GrossWeightSummary
DROP FUNCTION dbo.rail_gross_weight_summary
DROP FUNCTION dbo.rail_gross_weight_summary_unit
ALTER TABLE dbo.Rail_InvoiceHeaders
DROP COLUMN Invoice_Total
DROP FUNCTION dbo.rail_invoice_total
ALTER TABLE dbo.Rail_InvoiceLines
DROP COLUMN invoice_line_number
ALTER TABLE dbo.Rail_InvoiceLines
DROP COLUMN Gr_Weight
ALTER TABLE dbo.Rail_InvoiceLines
DROP COLUMN Gr_Weight_Unit
DROP FUNCTION dbo.rail_gross_weight
DROP FUNCTION dbo.rail_gross_weight_unit
DROP FUNCTION dbo.rail_invoice_line_number

PRINT ('drop tables')
GO
DROP TABLE dbo.Rail_DeclarationTab
DROP TABLE dbo.Rail_ContainersTab
DROP TABLE dbo.Rail_MISC
DROP TABLE dbo.Rail_InvoiceLines
DROP TABLE dbo.Rail_InvoiceHeaders
DROP TABLE dbo.Rail_Filing_Details
DROP TABLE dbo.Rail_Filing_Headers
DROP TABLE dbo.Rail_BD_Parsed
DROP TABLE dbo.Rail_EDIMessage
DROP TABLE dbo.Rail_Rule_Port
DROP TABLE dbo.Rail_Rule_ImporterSupplier
DROP TABLE dbo.Rail_Rule_Desc1_Desc2
DROP TABLE dbo.Rail_DEFValues_Manual
DROP TABLE dbo.Rail_DEFValues
DROP TABLE dbo.rail_sections

PRINT ('drop views')
GO
DROP VIEW dbo.v_Rail_DEFValues
DROP VIEW dbo.v_Rail_DEFValues_Manual
DROP VIEW dbo.v_Rail_Tables
DROP VIEW dbo.Rail_Inbound_Grid
DROP VIEW dbo.v_Rail_Filing_Data
DROP VIEW dbo.Rail_Report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.rail_filing_param
DROP PROCEDURE dbo.rail_filing
DROP PROCEDURE dbo.rail_filing_del
DROP PROCEDURE dbo.rail_add_declaration_record
DROP PROCEDURE dbo.rail_add_container_record
DROP PROCEDURE dbo.rail_add_invoice_line_record
DROP PROCEDURE dbo.rail_add_invoice_header_record
DROP PROCEDURE dbo.rail_add_misc_record
DROP PROCEDURE dbo.rail_add_def_values_manual
DROP PROCEDURE dbo.rail_delete_record
DROP PROCEDURE dbo.rail_BD_Parsed_del
DROP PROCEDURE dbo.rail_apply_def_values