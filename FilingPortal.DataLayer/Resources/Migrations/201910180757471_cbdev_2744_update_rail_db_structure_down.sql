PRINT ('create tables')
GO
CREATE TABLE dbo.Rail_EDIMessage (
  EM_PK INT IDENTITY
 ,EM_MessageText VARCHAR(MAX) NOT NULL
 ,CW_LastModifiedDate DATETIME NOT NULL
 ,EM_PK_CBRNYC UNIQUEIDENTIFIER NULL
 ,EM_SystemCreateTimeUtc SMALLDATETIME NULL
 ,EM_Status TINYINT NULL
 ,LastModifiedDate DATETIME NOT NULL DEFAULT (GETDATE())
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (EM_PK)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_BD_Parsed (
  BDP_PK INT IDENTITY
 ,BDP_EM INT NOT NULL
 ,Importer NVARCHAR(200) NULL
 ,Supplier NVARCHAR(200) NULL
 ,EquipmentInitial VARCHAR(4) NULL
 ,EquipmentNumber NVARCHAR(6) NULL
 ,IssuerCode VARCHAR(5) NULL
 ,BillofLading NVARCHAR(20) NULL
 ,PortofUnlading VARCHAR(4) NULL
 ,Description1 NVARCHAR(500) NULL
 ,ManifestUnits VARCHAR(3) NULL
 ,Weight NVARCHAR(10) NULL
 ,WeightUnit VARCHAR(2) NULL
 ,ReferenceNumber1 NVARCHAR(50) NULL
 ,FDeleted BIT NOT NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__BD_Parsed__Creat__3864608B DEFAULT (SUSER_NAME())
 ,DuplicateOf INT NULL
 ,CW_CreatedDateUTC DATETIME NOT NULL DEFAULT (GETDATE())
 ,CONSTRAINT PK_BD_Parse PRIMARY KEY CLUSTERED (BDP_PK)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Filing_Headers (
  id INT IDENTITY
 ,FilingNumber VARCHAR(255) NULL
 ,MappingStatus INT NULL
 ,FilingStatus INT NULL
 ,ErrorDescription VARCHAR(MAX) NULL
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Filling_H__Creat__55009F39 DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Filling_H__Creat__55F4C372 DEFAULT (SUSER_NAME())
 ,JobPKHyperlink VARCHAR(8000) NULL
 ,RequestXML VARCHAR(MAX) NULL
 ,ResponseXML VARCHAR(MAX) NULL
 ,CONSTRAINT PK__Rail_Fil__3213E83F8FA5100E PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Filing_Details (
  BDP_FK INT NOT NULL
 ,Filing_Headers_FK INT NOT NULL
 ,CONSTRAINT PK_RAIL_FILING_DETAILS PRIMARY KEY CLUSTERED (BDP_FK, Filing_Headers_FK)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_DeclarationTab (
  id INT IDENTITY
 ,Main_Supplier VARCHAR(128) NULL
 ,Importer VARCHAR(128) NULL
 ,Shipment_Type VARCHAR(128) NULL
 ,Transport VARCHAR(128) NULL
 ,Container VARCHAR(128) NULL
 ,Entry_Type VARCHAR(128) NULL
 ,RLF VARCHAR(128) NULL
 ,Enable_Entry_Sum VARCHAR(128) NULL
 ,Type VARCHAR(128) NULL
 ,Certify_Cargo_Release VARCHAR(128) NULL
 ,Service VARCHAR(128) NULL
 ,Issuer VARCHAR(128) NULL
 ,Master_Bill VARCHAR(128) NULL
 ,Carrier_SCAC VARCHAR(128) NULL
 ,Discharge VARCHAR(128) NULL
 ,Entry_Port VARCHAR(128) NULL
 ,Dep DATE NULL
 ,Arr DATE NULL
 ,Arr_2 DATE NULL
 ,HMF VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Destination VARCHAR(128) NULL
 ,Destination_State VARCHAR(128) NULL
 ,Country_of_Export VARCHAR(128) NULL
 ,ETA DATE NULL
 ,Export_Date DATE NULL
 ,Description NVARCHAR(500) NULL
 ,Owner_Ref VARCHAR(128) NULL
 ,INCO VARCHAR(128) NULL
 ,Total_Weight VARCHAR(128) NULL
 ,Total_Volume VARCHAR(128) NULL
 ,No_Packages VARCHAR(128) NULL
 ,FIRMs_Code VARCHAR(128) NULL
 ,Centralized_Exam_Site VARCHAR(128) NULL
 ,Purchased VARCHAR(128) NULL
 ,Manual_Entry VARCHAR(128) NULL
 ,Importer_of_record VARCHAR(128) NULL
 ,Split_Shipment_Release VARCHAR(128) NULL
 ,Check_Local_Client VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Rail_Decl__Creat__300424B4 DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Rail_Decl__Creat__30F848ED DEFAULT (SUSER_NAME())
 ,Filing_Headers_FK INT NOT NULL
 ,CONSTRAINT PK__Rail_Dec__3213E83F9090258E PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_ContainersTab (
  id INT IDENTITY
 ,Bill_Type VARCHAR(128) NULL
 ,Manifest_QTY VARCHAR(128) NULL
 ,UQ VARCHAR(128) NULL
 ,Bill_Issuer_SCAC VARCHAR(128) NULL
 ,IT_Number VARCHAR(128) NULL
 ,Is_Split VARCHAR(128) NULL
 ,Bill_Number VARCHAR(128) NULL
 ,Container_Number VARCHAR(128) NULL
 ,Pack_QTY VARCHAR(128) NULL
 ,Pack_Type VARCHAR(128) NULL
 ,Marks_and_Numbers VARCHAR(128) NULL
 ,Shipping_Symbol VARCHAR(128) NULL
 ,Seal_Number VARCHAR(128) NULL
 ,Type VARCHAR(128) NULL
 ,Mode VARCHAR(128) NULL
 ,Goods_Weight VARCHAR(128) NULL
 ,Bill_Num VARCHAR(128) NULL
 ,Packing_UQ VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Gross_Weight NUMERIC(18, 6) NULL
 ,Gross_Weight_Unit VARCHAR(2) NULL
 ,BDP_FK INT NULL
 ,Filing_Headers_FK INT NOT NULL
 ,CONSTRAINT PK__Rail_Pac__3213E83FF1371689 PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_InvoiceHeaders (
  id INT IDENTITY
 ,Invoice_No VARCHAR(128) NULL
 ,Supplier VARCHAR(128) NULL
 ,Supplier_Address VARCHAR(128) NULL
 ,INCO VARCHAR(128) NULL
 ,Curr VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Payment_Date VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Consignee_Address VARCHAR(128) NULL
 ,Inv_Date VARCHAR(128) NULL
 ,Agreed_Place VARCHAR(128) NULL
 ,Inv_Gross_Weight VARCHAR(128) NULL
 ,Net_Weight VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,Export_Date DATE NULL
 ,First_Sale VARCHAR(128) NULL
 ,Transaction_Related VARCHAR(128) NULL
 ,Packages VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Seller VARCHAR(128) NULL
 ,Importer VARCHAR(128) NULL
 ,Sold_to_party VARCHAR(128) NULL
 ,Ship_to_party VARCHAR(128) NULL
 ,Broker_PGA_Contact_Name VARCHAR(128) NULL
 ,Broker_PGA_Contact_Phone VARCHAR(128) NULL
 ,Broker_PGA_Contact_Email VARCHAR(128) NULL
 ,EPA_PST_Cert_Date VARCHAR(128) NULL
 ,EPA_TSCA_Cert_Date VARCHAR(128) NULL
 ,EPA_VNE_Cert_Date VARCHAR(128) NULL
 ,FSIS_Cert_Date VARCHAR(128) NULL
 ,FWS_Cert_Date VARCHAR(128) NULL
 ,LACEY_ACT_Cert_Date VARCHAR(128) NULL
 ,NHTSA_Cert_Date VARCHAR(128) NULL
 ,Landed_Costing_Ex_Rate VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Filing_Headers_FK INT NOT NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_InvoiceLines (
  id INT IDENTITY
 ,Invoice_No VARCHAR(128) NULL
 ,Tariff VARCHAR(128) NULL
 ,Customs_QTY NUMERIC(18, 6) NULL
 ,Line_Price NUMERIC(18, 6) NULL
 ,Goods_Description VARCHAR(128) NULL
 ,ORG VARCHAR(128) NULL
 ,SPI VARCHAR(128) NULL
 ,UQ VARCHAR(128) NULL
 ,PriceUnit NUMERIC(18, 6) NULL
 ,Prod_ID_1 VARCHAR(128) NULL
 ,Attribute_1 VARCHAR(128) NULL
 ,Attribute_2 VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Dest_State VARCHAR(128) NULL
 ,Transaction_Related VARCHAR(128) NULL
 ,Invoice_Qty NUMERIC(18, 6) NULL
 ,Invoice_Qty_Unit VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Sold_To_Party VARCHAR(128) NULL
 ,Code VARCHAR(128) NULL
 ,Curr VARCHAR(128) NULL
 ,CIF_Component VARCHAR(128) NULL
 ,EPA_TSCA_Toxic_Substance_Control_Act_Indicator VARCHAR(128) NULL
 ,TSCA_Indicator VARCHAR(128) NULL
 ,Certifying_Individual VARCHAR(128) NULL
 ,PGA_Contact_Name VARCHAR(128) NULL
 ,PGA_Contact_Phone VARCHAR(128) NULL
 ,PGA_Contact_Email VARCHAR(128) NULL
 ,Amount INT NULL
 ,Description VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Rail_Invo__Creat__49C3F6B7 DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Rail_Invo__Creat__4AB81AF0 DEFAULT (SUSER_NAME())
 ,Filing_Headers_FK INT NOT NULL
 ,InvoiceHeaders_FK INT NOT NULL
 ,BDP_FK INT NULL
 ,CONSTRAINT PK__Rail_Inv__3213E83F7CAE2E0D PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_MISC (
  id INT IDENTITY
 ,Branch VARCHAR(128) NULL
 ,Broker VARCHAR(128) NULL
 ,Merge_By VARCHAR(128) NULL
 ,Tax_Deferrable_Ind VARCHAR(128) NULL
 ,Preparer_Dist_Port VARCHAR(128) NULL
 ,Recon_Issue VARCHAR(128) NULL
 ,FTA_Recon VARCHAR(128) NULL
 ,Bond_Type VARCHAR(128) NULL
 ,Payment_Type VARCHAR(128) NULL
 ,Broker_to_Pay VARCHAR(128) NULL
 ,Prelim_Statement_Date VARCHAR(128) NULL
 ,Submitter VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Rail_MISC__Creat__4E88ABD4 DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Rail_MISC__Creat__4F7CD00D DEFAULT (SUSER_NAME())
 ,BDP_FK INT NULL
 ,Filing_Headers_FK INT NOT NULL
 ,CONSTRAINT PK__Rail_MIS__3213E83F8ACD87E4 PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Documents (
  id INT IDENTITY
 ,Filing_Headers_FK INT NULL
 ,filename VARCHAR(255) NULL
 ,file_extension VARCHAR(128) NULL
 ,file_desc VARCHAR(1000) NULL
 ,file_content VARBINARY(MAX) NULL
 ,DocumentType VARCHAR(120) NULL
 ,IsManifest TINYINT NOT NULL DEFAULT (0)
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Files__CreatedDa__503BEA1C DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Files__CreatedUs__51300E55 DEFAULT (SUSER_NAME())
 ,Status VARCHAR(50) NULL
 ,inbound_record_id INT NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Rule_Port (
  id INT IDENTITY
 ,Port VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Destination VARCHAR(128) NULL
 ,FIRMs_Code VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,CreatedDate DATETIME NOT NULL
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,CONSTRAINT PK_Rail_Rule_Port PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Rule_ImporterSupplier (
  id INT IDENTITY
 ,Importer_Name VARCHAR(128) NULL
 ,Supplier_Name VARCHAR(128) NULL
 ,Main_Supplier VARCHAR(128) NULL
 ,Importer VARCHAR(128) NULL
 ,Destination_State VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Seller VARCHAR(128) NULL
 ,Sold_to_party VARCHAR(128) NULL
 ,Ship_to_party VARCHAR(128) NULL
 ,CountryofOrigin VARCHAR(128) NULL
 ,Relationship VARCHAR(128) NULL
 ,DFT VARCHAR(128) NULL
 ,Value_Recon VARCHAR(128) NULL
 ,FTA_Recon VARCHAR(128) NULL
 ,Payment_Type INT NULL
 ,Broker_to_pay VARCHAR(128) NULL
 ,Value NUMERIC(18, 6) NULL
 ,Freight INT NULL
 ,CreatedDate DATETIME NOT NULL
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,CONSTRAINT PK_Rail_Rule_ImporterSupplier PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_Rule_Desc1_Desc2 (
  id INT IDENTITY
 ,Description1 VARCHAR(500) NOT NULL
 ,Prod_ID_1 VARCHAR(128) NULL
 ,Attribute_1 VARCHAR(128) NULL
 ,Tariff VARCHAR(128) NULL
 ,Description VARCHAR(128) NULL
 ,Goods_Description VARCHAR(128) NULL
 ,Invoice_UOM VARCHAR(128) NULL
 ,Template_HTS_Quantity DECIMAL(18, 6) NULL
 ,Template_Invoice_Quantity DECIMAL(18, 6) NULL
 ,CreatedDate DATETIME NOT NULL
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Attribute_2 VARCHAR(128) NULL
 ,CONSTRAINT PK_Rail_Rule_Desc1_Desc2 PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.rail_sections (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,procedure_name VARCHAR(128) NULL
 ,CONSTRAINT PK_rail_sections_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_DEFValues (
  id INT IDENTITY
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Rail_DEFValues_Display_on_UI] DEFAULT (0)
 ,ValueLabel VARCHAR(128) NULL
 ,ValueDesc VARCHAR(128) NULL
 ,DefValue VARCHAR(500) NULL
 ,ColName VARCHAR(128) NULL
 ,FManual TINYINT NOT NULL DEFAULT (0)
 ,FHasDefaultVal TINYINT NOT NULL DEFAULT (0)
 ,FEditable TINYINT NOT NULL DEFAULT (1)
 ,FMandatory TINYINT NOT NULL DEFAULT (0)
 ,CreatedDate DATETIME NULL CONSTRAINT DF__Rail_DEFV__Creat__5441852A DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL CONSTRAINT DF__Rail_DEFV__Creat__5535A963 DEFAULT (SUSER_NAME())
 ,SingleFilingOrder TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,section_id INT NOT NULL
 ,CONSTRAINT PK__Rail_DEF__3213E83F149D71C0 PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Rail_DEFValues_Manual (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Rail_DEFValues_Manual_Display_on_UI] DEFAULT (0)
 ,ValueLabel VARCHAR(128) NULL
 ,FManual TINYINT NOT NULL DEFAULT (0)
 ,FHasDefaultVal TINYINT NOT NULL DEFAULT (0)
 ,FEditable TINYINT NOT NULL DEFAULT (1)
 ,FMandatory TINYINT NOT NULL DEFAULT (0)
 ,ModifiedDate DATETIME NOT NULL DEFAULT (GETDATE())
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,parent_record_id INT NOT NULL
 ,section_name VARCHAR(128) NOT NULL
 ,section_title VARCHAR(128) NOT NULL
 ,record_id INT NOT NULL
 ,description VARCHAR(128) NULL
 ,table_name VARCHAR(128) NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,value VARCHAR(512) NULL
 ,CONSTRAINT PK__Rail_ PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy broker download data')
GO
SET IDENTITY_INSERT dbo.Rail_EDIMessage ON;
INSERT INTO dbo.Rail_EDIMessage (
    EM_PK
   ,EM_MessageText
   ,CW_LastModifiedDate
   ,EM_PK_CBRNYC
   ,EM_SystemCreateTimeUtc
   ,EM_Status
   ,LastModifiedDate
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,em_message_text
   ,cw_last_modified_date
   ,em_pk_cbrnyc
   ,em_system_create_time_utc
   ,em_status
   ,last_modified_date
   ,created_date
   ,created_user
  FROM dbo.imp_rail_broker_download;
SET IDENTITY_INSERT dbo.Rail_EDIMessage OFF;
GO

PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.Rail_BD_Parsed ON;
INSERT INTO dbo.Rail_BD_Parsed (
    BDP_PK
   ,BDP_EM
   ,Importer
   ,Supplier
   ,EquipmentInitial
   ,EquipmentNumber
   ,IssuerCode
   ,BillofLading
   ,PortofUnlading
   ,Description1
   ,ManifestUnits
   ,Weight
   ,WeightUnit
   ,ReferenceNumber1
   ,FDeleted
   ,DuplicateOf
   ,CW_CreatedDateUTC
   ,CreatedDate
   ,CreatedUser)
  SELECT
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
   ,created_user
  FROM dbo.imp_rail_inbound;
SET IDENTITY_INSERT dbo.Rail_BD_Parsed OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.Rail_Filing_Headers ON;
INSERT INTO dbo.Rail_Filing_Headers (
    id
   ,FilingNumber
   ,MappingStatus
   ,FilingStatus
   ,ErrorDescription
   ,JobPKHyperlink
   ,RequestXML
   ,ResponseXML
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_number
   ,mapping_status
   ,filing_status
   ,error_description
   ,job_hyperlink
   ,request_xml
   ,response_xml
   ,created_date
   ,created_user
  FROM dbo.imp_rail_filing_header;
SET IDENTITY_INSERT dbo.Rail_Filing_Headers OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.Rail_Filing_Details (
    BDP_FK
   ,Filing_Headers_FK)
  SELECT
    inbound_id
   ,filing_header_id
  FROM dbo.imp_rail_filing_detail;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.Rail_DeclarationTab ON;
INSERT INTO dbo.Rail_DeclarationTab (
    id
   ,Filing_Headers_FK
   ,Main_Supplier
   ,Importer
   ,Shipment_Type
   ,Transport
   ,Container
   ,Entry_Type
   ,RLF
   ,Enable_Entry_Sum
   ,Type
   ,Certify_Cargo_Release
   ,Service
   ,Issuer
   ,Master_Bill
   ,Carrier_SCAC
   ,Discharge
   ,Entry_Port
   ,Dep
   ,Arr
   ,Arr_2
   ,HMF
   ,Origin
   ,Destination
   ,Destination_State
   ,Country_of_Export
   ,ETA
   ,Export_Date
   ,Description
   ,Owner_Ref
   ,INCO
   ,Total_Weight
   ,Total_Volume
   ,No_Packages
   ,FIRMs_Code
   ,Centralized_Exam_Site
   ,Purchased
   ,Manual_Entry
   ,Importer_of_record
   ,Split_Shipment_Release
   ,Check_Local_Client
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
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
   ,created_user
  FROM dbo.imp_rail_declaration rdt;
SET IDENTITY_INSERT dbo.Rail_DeclarationTab OFF;
GO

PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.Rail_ContainersTab ON;
INSERT INTO dbo.Rail_ContainersTab (
    id
   ,Filing_Headers_FK
   ,BDP_FK
   ,Bill_Type
   ,Manifest_QTY
   ,UQ
   ,Bill_Issuer_SCAC
   ,IT_Number
   ,Is_Split
   ,Bill_Number
   ,Container_Number
   ,Pack_QTY
   ,Pack_Type
   ,Marks_and_Numbers
   ,Shipping_Symbol
   ,Seal_Number
   ,Type
   ,Mode
   ,Goods_Weight
   ,Bill_Num
   ,Packing_UQ
   ,Gross_Weight
   ,Gross_Weight_Unit
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
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
   ,created_user
  FROM dbo.imp_rail_container rct;
SET IDENTITY_INSERT dbo.Rail_ContainersTab OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.Rail_InvoiceHeaders ON;
INSERT INTO dbo.Rail_InvoiceHeaders (
    id
   ,Filing_Headers_FK
   ,Invoice_No
   ,Supplier
   ,Supplier_Address
   ,INCO
   ,Curr
   ,Origin
   ,Payment_Date
   ,Consignee
   ,Consignee_Address
   ,Inv_Date
   ,Agreed_Place
   ,Inv_Gross_Weight
   ,Net_Weight
   ,Export
   ,Export_Date
   ,First_Sale
   ,Transaction_Related
   ,Packages
   ,Manufacturer
   ,Seller
   ,Importer
   ,Sold_to_party
   ,Ship_to_party
   ,Broker_PGA_Contact_Name
   ,Broker_PGA_Contact_Phone
   ,Broker_PGA_Contact_Email
   ,EPA_PST_Cert_Date
   ,EPA_TSCA_Cert_Date
   ,EPA_VNE_Cert_Date
   ,FSIS_Cert_Date
   ,FWS_Cert_Date
   ,LACEY_ACT_Cert_Date
   ,NHTSA_Cert_Date
   ,Landed_Costing_Ex_Rate
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
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
   ,created_user
  FROM dbo.imp_rail_invoice_header rih;
SET IDENTITY_INSERT dbo.Rail_InvoiceHeaders OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.Rail_InvoiceLines ON;
INSERT INTO dbo.Rail_InvoiceLines (
    id
   ,Filing_Headers_FK
   ,InvoiceHeaders_FK
   ,Invoice_No
   ,Tariff
   ,Customs_QTY
   ,Line_Price
   ,Goods_Description
   ,ORG
   ,SPI
   ,UQ
   ,PriceUnit
   ,Prod_ID_1
   ,Attribute_1
   ,Attribute_2
   ,Export
   ,Origin
   ,Dest_State
   ,Transaction_Related
   ,Invoice_Qty
   ,Invoice_Qty_Unit
   ,Manufacturer
   ,Consignee
   ,Sold_To_Party
   ,Code
   ,Curr
   ,CIF_Component
   ,EPA_TSCA_Toxic_Substance_Control_Act_Indicator
   ,TSCA_Indicator
   ,Certifying_Individual
   ,PGA_Contact_Name
   ,PGA_Contact_Phone
   ,PGA_Contact_Email
   ,Amount
   ,Description
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
   ,parent_record_id
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
   ,created_user
  FROM dbo.imp_rail_invoice_line ril;
SET IDENTITY_INSERT dbo.Rail_InvoiceLines OFF;
GO


PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.Rail_MISC ON;
INSERT INTO dbo.Rail_MISC (
    id
   ,Filing_Headers_FK
   ,Branch
   ,Broker
   ,Merge_By
   ,Tax_Deferrable_Ind
   ,Preparer_Dist_Port
   ,Recon_Issue
   ,FTA_Recon
   ,Bond_Type
   ,Payment_Type
   ,Broker_to_Pay
   ,Prelim_Statement_Date
   ,Submitter
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
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
   ,created_user
  FROM dbo.imp_rail_misc rm;
SET IDENTITY_INSERT dbo.Rail_MISC OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.Rail_Documents ON;
INSERT INTO dbo.Rail_Documents (
    id
   ,Filing_Headers_FK
   ,inbound_record_id
   ,filename
   ,file_extension
   ,file_desc
   ,file_content
   ,DocumentType
   ,IsManifest
   ,Status
   ,CreatedDate
   ,CreatedUser)
  SELECT
    id
   ,filing_header_id
   ,inbound_record_id
   ,filename
   ,file_extension
   ,file_desc
   ,file_content
   ,document_type
   ,is_manifest
   ,status
   ,created_date
   ,created_user
  FROM dbo.imp_rail_document rd;
SET IDENTITY_INSERT dbo.Rail_Documents OFF;
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.Rail_Rule_Port ON;
INSERT INTO dbo.Rail_Rule_Port (
    id
   ,Port
   ,Origin
   ,Destination
   ,FIRMs_Code
   ,Export
   ,CreatedDate
   ,CreatedUser)
  SELECT
    irrp.id
   ,irrp.port
   ,irrp.origin
   ,irrp.destination
   ,irrp.firms_code
   ,irrp.export
   ,irrp.created_date
   ,irrp.created_user
  FROM dbo.imp_rail_rule_port irrp;
SET IDENTITY_INSERT dbo.Rail_Rule_Port OFF;
GO

PRINT ('copy importer-supplier rule data')
GO
SET IDENTITY_INSERT dbo.Rail_Rule_ImporterSupplier ON;
INSERT INTO dbo.Rail_Rule_ImporterSupplier (
    id
   ,Importer_Name
   ,Supplier_Name
   ,Main_Supplier
   ,Importer
   ,Destination_State
   ,Consignee
   ,Manufacturer
   ,Seller
   ,Sold_to_party
   ,Ship_to_party
   ,CountryofOrigin
   ,Relationship
   ,DFT
   ,Value_Recon
   ,FTA_Recon
   ,Payment_Type
   ,Broker_to_pay
   ,Value
   ,Freight
   ,CreatedDate
   ,CreatedUser)
  SELECT
    rris.id
   ,rris.importer_name
   ,rris.supplier_name
   ,rris.main_supplier
   ,rris.importer
   ,rris.destination_state
   ,rris.consignee
   ,rris.manufacturer
   ,rris.seller
   ,rris.sold_to_party
   ,rris.ship_to_party
   ,rris.country_of_origin
   ,rris.relationship
   ,rris.dft
   ,rris.value_recon
   ,rris.fta_recon
   ,rris.payment_type
   ,rris.broker_to_pay
   ,rris.value
   ,rris.freight
   ,rris.created_date
   ,rris.created_user
  FROM dbo.imp_rail_rule_importer_supplier rris;
SET IDENTITY_INSERT dbo.Rail_Rule_ImporterSupplier OFF;
GO

PRINT ('copy product rule data')
GO
SET IDENTITY_INSERT dbo.Rail_Rule_Desc1_Desc2 ON;
INSERT INTO dbo.Rail_Rule_Desc1_Desc2 (
    id
   ,Description1
   ,Prod_ID_1
   ,Attribute_1
   ,Attribute_2
   ,Tariff
   ,Description
   ,Goods_Description
   ,Invoice_UOM
   ,Template_HTS_Quantity
   ,Template_Invoice_Quantity
   ,CreatedDate
   ,CreatedUser)
  SELECT
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
   ,created_user
  FROM dbo.imp_rail_rule_product rrdd;
SET IDENTITY_INSERT dbo.Rail_Rule_Desc1_Desc2 OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.rail_sections ON;
INSERT INTO dbo.rail_sections (
    id
   ,name
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,procedure_name)
  SELECT
    id
   ,name
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,procedure_name
  FROM dbo.imp_rail_form_section_configuration rs;
SET IDENTITY_INSERT dbo.rail_sections OFF;
GO

UPDATE rail_sections
SET table_name = 'Rail_ContainersTab'
   ,procedure_name = 'rail_add_container_record'
WHERE table_name = 'imp_rail_container';
UPDATE rail_sections
SET table_name = 'Rail_DeclarationTab'
   ,procedure_name = 'rail_add_declaration_record'
WHERE table_name = 'imp_rail_declaration';
UPDATE rail_sections
SET table_name = 'Rail_InvoiceHeaders'
   ,procedure_name = 'rail_add_invoice_header_record'
WHERE table_name = 'imp_rail_invoice_header';
UPDATE rail_sections
SET table_name = 'Rail_InvoiceLines'
   ,procedure_name = 'rail_add_invoice_line_record'
WHERE table_name = 'imp_rail_invoice_line';
UPDATE rail_sections
SET table_name = 'Rail_MISC'
   ,procedure_name = 'rail_add_misc_record'
WHERE table_name = 'imp_rail_misc';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.Rail_DEFValues ON;
INSERT INTO dbo.Rail_DEFValues (
    id
   ,section_id
   ,ColName
   ,ValueLabel
   ,ValueDesc
   ,DefValue
   ,FHasDefaultVal
   ,FEditable
   ,FMandatory
   ,Display_on_UI
   ,FManual
   ,SingleFilingOrder
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,CreatedDate
   ,CreatedUser)
  SELECT
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
   ,created_user
  FROM dbo.imp_rail_form_configuration rd;
SET IDENTITY_INSERT dbo.Rail_DEFValues OFF;
GO
UPDATE dbo.Rail_DEFValues
SET ColName = 'PriceUnit'
WHERE ColName = 'price_unit';
GO

PRINT ('add constraints')
GO
ALTER TABLE dbo.Rail_BD_Parsed
ADD CONSTRAINT FK_BD_Parse_EDIMessage FOREIGN KEY (BDP_EM) REFERENCES dbo.Rail_EDIMessage (EM_PK)
GO

ALTER TABLE dbo.Rail_Filing_Headers
ADD CONSTRAINT [FK_dbo.Rail_Filing_Headers_dbo.FilingStatus_FilingStatus] FOREIGN KEY (FilingStatus) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.Rail_Filing_Headers
ADD CONSTRAINT [FK_dbo.Rail_Filing_Headers_dbo.MappingStatus_MappingStatus] FOREIGN KEY (MappingStatus) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.Rail_Filing_Details
ADD CONSTRAINT FK_RAIL_FIL_REFERENCE_RAIL_BD_ FOREIGN KEY (BDP_FK) REFERENCES dbo.Rail_BD_Parsed (BDP_PK)
GO

ALTER TABLE dbo.Rail_Filing_Details
ADD CONSTRAINT FK_RAIL_FIL_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

ALTER TABLE dbo.Rail_DeclarationTab
ADD CONSTRAINT FK_RAIL_DEC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Rail_ContainersTab
ADD CONSTRAINT FK_RAIL_CON_REFERENCE_RAIL_BD_ FOREIGN KEY (BDP_FK) REFERENCES dbo.Rail_BD_Parsed (BDP_PK)
GO

ALTER TABLE dbo.Rail_ContainersTab
ADD CONSTRAINT FK_Rail_Packing_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
ADD CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Rail_InvoiceLines
ADD CONSTRAINT FK_Rail_InvoiceLines_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

ALTER TABLE dbo.Rail_InvoiceLines
ADD CONSTRAINT FK_Rail_InvoiceLines_Rail_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Rail_InvoiceHeaders (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Rail_MISC
ADD CONSTRAINT FK_Rail_Rail_MISC_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Rail_Documents
ADD CONSTRAINT [FK_dbo.Rail_Documents_dbo.Rail_BD_Parsed_inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.Rail_BD_Parsed (BDP_PK)
GO

ALTER TABLE dbo.Rail_Documents
ADD CONSTRAINT FK_RAIL_DOC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.rail_sections
ADD CONSTRAINT FK_rail_sections_parent_id FOREIGN KEY (parent_id) REFERENCES dbo.rail_sections (id)
GO

ALTER TABLE dbo.Rail_DEFValues
ADD CONSTRAINT FK_Rail_DEFValues__rail_sections__section_id FOREIGN KEY (section_id) REFERENCES dbo.rail_sections (id)
GO

PRINT ('add indexes')
GO
CREATE INDEX Idx_Description1
ON dbo.Rail_BD_Parsed (Description1)
ON [PRIMARY]
GO

CREATE INDEX Idx_DuplicateOf
ON dbo.Rail_BD_Parsed (DuplicateOf)
ON [PRIMARY]
GO

CREATE INDEX Idx_Importer_Supplier
ON dbo.Rail_BD_Parsed (Importer, Supplier)
ON [PRIMARY]
GO

CREATE INDEX Idx_PortOfUnlading
ON dbo.Rail_BD_Parsed (PortofUnlading)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingStatus
ON dbo.Rail_Filing_Headers (FilingStatus)
ON [PRIMARY]
GO

CREATE INDEX Idx_MappingStatus
ON dbo.Rail_Filing_Headers (MappingStatus)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailDeclarationTab_filingHeadersFK
ON dbo.Rail_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailContainersTab_filingHeadersFK
ON dbo.Rail_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailInvoiceHeaders_filingHeadersFK
ON dbo.Rail_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailInvoiceLines_filingHeadersFK
ON dbo.Rail_InvoiceLines (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailInvoiceLines_invoiceHeadersFK
ON dbo.Rail_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailMISC_filingHeadersFK
ON dbo.Rail_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX IX_Filing_Headers_FK
ON dbo.Rail_Documents (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX IX_inbound_record_id
ON dbo.Rail_Documents (inbound_record_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_Port
ON dbo.Rail_Rule_Port (Port)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_ImportName_SupplierName
ON dbo.Rail_Rule_ImporterSupplier (Importer_Name, Supplier_Name)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_Description1
ON dbo.Rail_Rule_Desc1_Desc2 (Description1)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.rail_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.rail_sections (parent_id)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.Rail_DEFValues (section_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX UK_Rail_DEFValues_SingleFilingOrder
ON dbo.Rail_DEFValues (SingleFilingOrder)
WHERE ([SingleFilingOrder] IS NOT NULL)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeadersFK
ON dbo.Rail_DEFValues_Manual (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_RailDEFValuesManual_recordId_tableName_columnName
ON dbo.Rail_DEFValues_Manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets rail gross weight
CREATE FUNCTION dbo.rail_gross_weight (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(rct.BDP_FK)
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  SELECT
    @result = SUM(dbo.weightToTon(rct.Gross_Weight, rct.Gross_Weight_unit))
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  RETURN CASE
    WHEN @count > 1 THEN @result
    ELSE @result * 1000
  END
END;
GO

-- gets rail gross weight unit
CREATE FUNCTION dbo.rail_gross_weight_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;
  DECLARE @count INT = 0;

  SELECT
    @count = COUNT(rct.BDP_FK)
  FROM dbo.Rail_ContainersTab rct
  WHERE rct.Filing_Headers_FK = @filingHeaderId

  SELECT
    @result =
    CASE
      WHEN @count > 1 THEN 'T'
      ELSE 'KG'
    END

  RETURN @result
END;
GO

-- gets rail invoice line number
CREATE FUNCTION dbo.rail_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      ril.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY ril.id)
    FROM dbo.Rail_InvoiceLines ril
    WHERE ril.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END;
GO

-- alter rail invoice line table
ALTER TABLE dbo.Rail_InvoiceLines
ADD invoice_line_number AS (dbo.rail_invoice_line_number([InvoiceHeaders_FK], id));
ALTER TABLE dbo.Rail_InvoiceLines
ADD Gr_Weight AS (dbo.rail_gross_weight([Filing_Headers_FK]));
ALTER TABLE dbo.Rail_InvoiceLines
ADD Gr_Weight_Unit AS (dbo.rail_gross_weight_unit([Filing_Headers_FK]));
GO

-- gets invoice total
CREATE FUNCTION dbo.rail_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(ril.Customs_QTY * ril.PriceUnit)
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.InvoiceHeaders_FK = @invoiceHeaderId
  GROUP BY ril.InvoiceHeaders_FK

  RETURN @result
END
GO

-- alter rail invoice header table
ALTER TABLE dbo.Rail_InvoiceHeaders
ADD Invoice_Total AS (dbo.rail_invoice_total(id));
GO

-- gets rail summary gross weight
CREATE FUNCTION dbo.rail_gross_weight_summary (@filingHeaderId INT)
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result DECIMAL(18, 9) = NULL;

  SELECT
    @result = SUM(ril.Gr_Weight)
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId

  RETURN @result
END
GO

-- gets rail summary gross weight unit
CREATE FUNCTION dbo.rail_gross_weight_summary_unit (@filingHeaderId INT)
RETURNS VARCHAR(2) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result VARCHAR(2) = NULL;

  SELECT TOP (1)
    @result = ril.Gr_Weight_Unit
  FROM dbo.Rail_InvoiceLines ril
  WHERE ril.Filing_Headers_FK = @filingHeaderId

  RETURN @result
END;
GO

-- alter rail invoice header table
ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummaryUnit AS (dbo.rail_gross_weight_summary_unit(id));
ALTER TABLE dbo.Rail_Filing_Headers
ADD GrossWeightSummary AS (dbo.rail_gross_weight_summary(id));
GO

-- gets rail car count
CREATE FUNCTION dbo.fn_GetRailCarCount (@filingHeaderId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = COUNT(d.BDP_FK)
  FROM dbo.Rail_Filing_Details d
  WHERE d.Filing_Headers_FK = @filingHeaderId
  GROUP BY d.Filing_Headers_FK
  RETURN @result
END;
GO

PRINT ('create triggers')
GO
CREATE TRIGGER dbo.rail_declaration_tab_befor_delete
ON dbo.Rail_DeclarationTab
FOR DELETE
AS
  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE table_name = 'Rail_DeclarationTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.rail_containers_tab_befor_delete
ON dbo.Rail_ContainersTab
FOR DELETE
AS
  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE table_name = 'Rail_ContainersTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.rail_invoice_headers_befor_delete
ON dbo.Rail_InvoiceHeaders
FOR DELETE
AS
  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE table_name = 'Rail_InvoiceHeaders'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.rail_invoice_lines_befor_delete
ON dbo.Rail_InvoiceLines
FOR DELETE
AS
  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE table_name = 'Rail_InvoiceLines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

CREATE TRIGGER dbo.rail_misc_befor_delete
ON dbo.Rail_MISC
FOR DELETE
AS
  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE table_name = 'Rail_MISC'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

PRINT ('create views')
GO
CREATE VIEW dbo.Rail_Inbound_Grid
AS
SELECT DISTINCT
  bd_parsed.BDP_PK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.[Port] Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.[name] Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.[name] Filing_Headers_FilingStatus_Title
 ,rail_description.[Description]

FROM dbo.Rail_BD_Parsed bd_parsed
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
LEFT JOIN dbo.Rail_Filing_Details filing_details
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers filing_headers
  ON filing_headers.id = filing_details.Filing_Headers_FK
    AND filing_headers.MappingStatus <> 0
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.MappingStatus, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.FilingStatus, 0) = filing_status.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Rail_Filing_Headers filing_headers
  INNER JOIN dbo.Rail_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.MappingStatus > 0
  AND bd_parsed.BDP_PK = filing_details.BDP_FK)

UNION ALL

SELECT
  filing_details.BDP_FK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.[Port] Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.[name] Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.[name] Filing_Headers_FilingStatus_Title
 ,rail_description.[Description]

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.[Port]
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
WHERE filing_headers.MappingStatus > 0
GO

CREATE VIEW dbo.Rail_Report
AS
SELECT
  headers.id AS Rail_Filing_Headers_id
 ,detailes.BDP_FK AS BDP_PK
 ,declaration.Arr AS Rail_DeclarationTab_Arr
 ,declaration.Arr_2 AS Rail_DeclarationTab_Arr_2
 ,declaration.Carrier_SCAC AS Rail_DeclarationTab_Carrier_SCAC
 ,declaration.Centralized_Exam_Site AS Rail_DeclarationTab_Centralized_Exam_Site
 ,declaration.Certify_Cargo_Release AS Rail_DeclarationTab_Certify_Cargo_Release
 ,declaration.Check_Local_Client AS Rail_DeclarationTab_Check_Local_Client
 ,declaration.Container AS Rail_DeclarationTab_Container
 ,declaration.Country_of_Export AS Rail_DeclarationTab_Country_of_Export
 ,declaration.Dep AS Rail_DeclarationTab_Dep
 ,declaration.Description AS Rail_DeclarationTab_Description
 ,declaration.Destination AS Rail_DeclarationTab_Destination
 ,declaration.Destination_State AS Rail_DeclarationTab_Destination_State
 ,declaration.Discharge AS Rail_DeclarationTab_Discharge
 ,declaration.Enable_Entry_Sum AS Rail_DeclarationTab_Enable_Entry_Sum
 ,declaration.Entry_Port AS Rail_DeclarationTab_Entry_Port
 ,declaration.Entry_Type AS Rail_DeclarationTab_Entry_Type
 ,declaration.ETA AS Rail_DeclarationTab_ETA
 ,declaration.Export_Date AS Rail_DeclarationTab_Export_Date
 ,declaration.FIRMs_Code AS Rail_DeclarationTab_FIRMs_Code
 ,declaration.HMF AS Rail_DeclarationTab_HMF
 ,declaration.Importer AS Rail_DeclarationTab_Importer
 ,declaration.Importer_of_record AS Rail_DeclarationTab_Importer_of_record
 ,declaration.INCO AS Rail_DeclarationTab_INCO
 ,declaration.Issuer AS Rail_DeclarationTab_Issuer
 ,declaration.Main_Supplier AS Rail_DeclarationTab_Main_Supplier
 ,declaration.Manual_Entry AS Rail_DeclarationTab_Manual_Entry
 ,declaration.Master_Bill AS Rail_DeclarationTab_Master_Bill
 ,declaration.No_Packages AS Rail_DeclarationTab_No_Packages
 ,declaration.Origin AS Rail_DeclarationTab_Origin
 ,declaration.Owner_Ref AS Rail_DeclarationTab_Owner_Ref
 ,declaration.Purchased AS Rail_DeclarationTab_Purchased
 ,declaration.RLF AS Rail_DeclarationTab_RLF
 ,declaration.Service AS Rail_DeclarationTab_Service
 ,declaration.Shipment_Type AS Rail_DeclarationTab_Shipment_Type
 ,declaration.Split_Shipment_Release AS Rail_DeclarationTab_Split_Shipment_Release
 ,declaration.Total_Volume AS Rail_DeclarationTab_Total_Volume
 ,declaration.Total_Weight AS Rail_DeclarationTab_Total_Weight
 ,declaration.Transport AS Rail_DeclarationTab_Transport
 ,declaration.Type AS Rail_DeclarationTab_Type

 ,containers.Bill_Issuer_SCAC AS Rail_Packing_Bill_Issuer_SCAC
 ,containers.Bill_Number AS Rail_Packing_Bill_Number
 ,containers.Bill_Type AS Rail_Packing_Bill_Type
 ,containers.Container_Number AS Rail_Packing_Container_Number
 ,containers.Is_Split AS Rail_Packing_Is_Split
 ,containers.IT_Number AS Rail_Packing_IT_Number
 ,containers.Manifest_QTY AS Rail_Packing_Manifest_QTY
 ,containers.Marks_and_Numbers AS Rail_Packing_Marks_and_Numbers
 ,containers.Pack_QTY AS Rail_Packing_Pack_QTY
 ,containers.Pack_Type AS Rail_Packing_Pack_Type
 ,containers.Shipping_Symbol AS Rail_Packing_Shipping_Symbol
 ,containers.UQ AS Rail_Packing_UQ
 ,containers.Packing_UQ AS Rail_Packing_Container_Packing_UQ
 ,containers.Seal_Number AS Rail_Packing_Seal_Number
 ,containers.Type AS Rail_Packing_Type
 ,containers.Mode AS Rail_Packing_Mode
 ,containers.Goods_Weight AS Rail_Packing_Goods_Weight
 ,containers.Bill_Num AS Rail_Packing_Bill_Num
 ,containers.Gross_Weight AS Rail_Packing_Gross_Weight
 ,containers.Gross_Weight_Unit AS Rail_Packing_Gross_Weight_Unit

 ,invheaders.Agreed_Place AS Rail_InvoiceHeaders_Agreed_Place
 ,invheaders.Broker_PGA_Contact_Email AS Rail_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.Broker_PGA_Contact_Name AS Rail_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone AS Rail_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.Consignee AS Rail_InvoiceHeaders_Consignee
 ,invheaders.Consignee_Address AS Rail_InvoiceHeaders_Consignee_Address
 ,invheaders.Curr AS Rail_InvoiceHeaders_Curr
 ,invheaders.EPA_PST_Cert_Date AS Rail_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.EPA_TSCA_Cert_Date AS Rail_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.EPA_VNE_Cert_Date AS Rail_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.Export AS Rail_InvoiceHeaders_Export
 ,invheaders.Export_Date AS Rail_InvoiceHeaders_Export_Date
 ,invheaders.First_Sale AS Rail_InvoiceHeaders_First_Sale
 ,invheaders.FSIS_Cert_Date AS Rail_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.FWS_Cert_Date AS Rail_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.Importer AS Rail_InvoiceHeaders_Importer
 ,invheaders.INCO AS Rail_InvoiceHeaders_INCO
 ,invheaders.Inv_Date AS Rail_InvoiceHeaders_Inv_Date
 ,invheaders.Inv_Gross_Weight AS Rail_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.Invoice_No AS Rail_InvoiceHeaders_Invoice_No
 ,invheaders.Invoice_Total AS Rail_InvoiceHeaders_Invoice_Total
 ,invheaders.LACEY_ACT_Cert_Date AS Rail_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.Landed_Costing_Ex_Rate AS Rail_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.Manufacturer AS Rail_InvoiceHeaders_Manufacturer
 ,invheaders.Net_Weight AS Rail_InvoiceHeaders_Net_Weight
 ,invheaders.NHTSA_Cert_Date AS Rail_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.Origin AS Rail_InvoiceHeaders_Origin
 ,invheaders.Packages AS Rail_InvoiceHeaders_Packages
 ,invheaders.Payment_Date AS Rail_InvoiceHeaders_Payment_Date
 ,invheaders.Seller AS Rail_InvoiceHeaders_Seller
 ,invheaders.Ship_to_party AS Rail_InvoiceHeaders_Ship_to_party
 ,invheaders.Sold_to_party AS Rail_InvoiceHeaders_Sold_to_party
 ,invheaders.Supplier AS Rail_InvoiceHeaders_Supplier
 ,invheaders.Supplier_Address AS Rail_InvoiceHeaders_Supplier_Address
 ,invheaders.Transaction_Related AS Rail_InvoiceHeaders_Transaction_Related

 ,invlines.Attribute_1 AS Rail_InvoiceLines_Attribute_1
 ,invlines.Attribute_2 AS Rail_InvoiceLines_Attribute_2
 ,invlines.Certifying_Individual AS Rail_InvoiceLines_Certifying_Individual
 ,invlines.CIF_Component AS Rail_InvoiceLines_CIF_Component
 ,invlines.Code AS Rail_InvoiceLines_Code
 ,invlines.Consignee AS Rail_InvoiceLines_Consignee
 ,invlines.Curr AS Rail_InvoiceLines_Curr
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Customs_QTY) AS Rail_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Rail_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Rail_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Rail_InvoiceLines_Export
 ,invlines.Goods_Description AS Rail_InvoiceLines_Goods_Description

 ,invlines.Invoice_No AS Rail_InvoiceLines_Invoice_No
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Invoice_Qty) AS Rail_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Rail_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Rail_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Rail_InvoiceLines_LNO
 ,invlines.Manufacturer AS Rail_InvoiceLines_Manufacturer
 ,invlines.ORG AS Rail_InvoiceLines_ORG
 ,invlines.Origin AS Rail_InvoiceLines_Origin
 ,invlines.PGA_Contact_Email AS Rail_InvoiceLines_PGA_Contact_Email
 ,invlines.PGA_Contact_Name AS Rail_InvoiceLines_PGA_Contact_Name
 ,invlines.PGA_Contact_Phone AS Rail_InvoiceLines_PGA_Contact_Phone
 ,invlines.PriceUnit AS Rail_InvoiceLines_PriceUnit
 ,invlines.Prod_ID_1 AS Rail_InvoiceLines_Prod_ID_1
 ,invlines.Sold_To_Party AS Rail_InvoiceLines_Sold_To_Party
 ,invlines.SPI AS Rail_InvoiceLines_SPI
 ,invlines.Tariff AS Rail_InvoiceLines_Tariff
 ,invlines.Transaction_Related AS Rail_InvoiceLines_Transaction_Related
 ,invlines.TSCA_Indicator AS Rail_InvoiceLines_TSCA_Indicator
 ,invlines.UQ AS Rail_InvoiceLines_UQ
 ,(dbo.fn_GetRailCarCount(invlines.Filing_Headers_FK) * invlines.Amount) AS Rail_InvoiceLines_Amount
 ,invlines.Gr_Weight AS Rail_GrossWeightSummary
 ,invlines.Gr_Weight_Unit AS Rail_GrossWeightSummaryUnit

 ,misc.Bond_Type AS Rail_MISC_Bond_Type
 ,misc.Branch AS Rail_MISC_Branch
 ,misc.Broker AS Rail_MISC_Broker
 ,misc.Broker_to_Pay AS Rail_MISC_Broker_to_Pay
 ,misc.FTA_Recon AS Rail_MISC_FTA_Recon
 ,misc.Merge_By AS Rail_MISC_Merge_By
 ,misc.Payment_Type AS Rail_MISC_Payment_Type
 ,misc.Prelim_Statement_Date AS Rail_MISC_Prelim_Statement_Date
 ,misc.Preparer_Dist_Port AS Rail_MISC_Preparer_Dist_Port
 ,misc.Recon_Issue AS Rail_MISC_Recon_Issue
 ,misc.Submitter AS Rail_MISC_Submitter
 ,misc.Tax_Deferrable_Ind AS Rail_MISC_Tax_Deferrable_Ind

FROM dbo.Rail_Filing_Headers headers
INNER JOIN dbo.Rail_Filing_Details detailes
  ON headers.id = detailes.Filing_Headers_FK
LEFT JOIN dbo.Rail_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Rail_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
    AND detailes.BDP_FK = containers.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT JOIN dbo.Rail_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT JOIN dbo.Rail_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO

CREATE VIEW dbo.v_Rail_DEFValues
AS
SELECT
  v.id
 ,v.ValueLabel AS label
 ,v.DefValue AS default_value
 ,v.ValueDesc AS description
 ,sections.table_name
 ,v.ColName AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,v.Display_on_UI AS display_on_ui
 ,v.FManual AS manual
 ,v.SingleFilingOrder AS single_filing_order
 ,CAST(v.FHasDefaultVal AS BIT) AS has_default_value
 ,CAST(v.FEditable AS BIT) AS editable
 ,CAST(v.FMandatory AS BIT) AS mandatory
 ,v.CreatedDate AS created_date
 ,v.CreatedUser AS created_user
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.Rail_DEFValues v
INNER JOIN dbo.rail_sections sections
  ON v.section_id = sections.id
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(sections.table_name)
WHERE (UPPER(i.column_name)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Rail_Inbounds_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

CREATE VIEW dbo.v_Rail_DEFValues_Manual
AS
SELECT
  v.id
 ,v.Filing_Headers_FK AS filing_header_id
 ,v.CreatedDate AS created_date
 ,v.CreatedUser AS created_user
 ,v.Display_on_UI AS display_on_ui
 ,CAST(v.FEditable AS BIT) AS editable
 ,CAST(v.FHasDefaultVal AS BIT) AS has_default_value
 ,CAST(v.FMandatory AS BIT) AS mandatory
 ,v.FManual AS [manual]
 ,v.ModifiedDate AS modification_date
 ,v.ValueLabel AS label
 ,v.parent_record_id
 ,v.section_name
 ,v.section_title
 ,v.record_id
 ,v.[description]
 ,v.table_name
 ,v.column_name
 ,v.[value]
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.Rail_DEFValues_Manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Rail_Inbounds_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL;
GO

CREATE VIEW dbo.v_Rail_Filing_Data
AS
SELECT
  f.BDP_FK AS id
 ,h.id AS Filing_header_id
 ,p.BDP_EM AS Manifest_id
 ,d.Importer AS Importer
 ,d.Entry_Port AS Port_code
 ,c.Bill_Num AS Bill_of_lading
 ,c.Container_Number AS Container_number
 ,p.ReferenceNumber1 AS Train_number
 ,ISNULL(c.Gross_Weight, 0) AS Gross_weight
 ,ISNULL(c.Gross_Weight_Unit, 0) AS Gross_weight_unit

FROM dbo.Rail_Filing_Headers h
INNER JOIN dbo.Rail_Filing_Details f
  ON h.id = f.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed p
  ON f.BDP_FK = p.BDP_PK
LEFT OUTER JOIN dbo.Rail_DeclarationTab d
  ON d.Filing_Headers_FK = h.id
LEFT OUTER JOIN dbo.Rail_ContainersTab c
  ON c.Filing_Headers_FK = h.id
    AND f.BDP_FK = c.BDP_FK
GO

CREATE VIEW dbo.v_Rail_Tables
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN rail_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

PRINT ('create stored procedures')
GO
-- Add Rail Import def values manual records --
CREATE PROCEDURE dbo.rail_add_def_values_manual (@tableName VARCHAR(128)
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
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Rail_DEFValues v
  INNER JOIN dbo.rail_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0
  OR v.SingleFilingOrder > 0)
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

  INSERT INTO dbo.Rail_DEFValues_Manual (
      Filing_Headers_FK
     ,parent_record_id
     ,section_name
     ,section_title
     ,record_id
     ,column_name
     ,table_name
     ,ModifiedDate
     ,value
     ,FEditable
     ,Display_on_UI
     ,FHasDefaultVal
     ,FMandatory
     ,FManual
     ,description
     ,ValueLabel
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
    SELECT
      @filingHeaderId
     ,@parentId
     ,s.name
     ,s.title
     ,@recordId
     ,dv.ColName
     ,s.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.FEditable
     ,dv.Display_on_UI
     ,dv.FHasDefaultVal
     ,dv.FMandatory
     ,dv.FManual
     ,dv.ValueDesc
     ,dv.ValueLabel
     ,dv.handbook_name
     ,dv.paired_field_table
     ,dv.paired_field_column
    FROM dbo.Rail_DEFValues dv
    INNER JOIN dbo.rail_sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO
-- apply def values for specified table --
CREATE PROCEDURE dbo.rail_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + dfm.table_name + ' set ' + dfm.column_name + ' = ' +
    CASE
      WHEN dfm.[value] IS NULL THEN 'NULL '
      ELSE 'try_cast(''' + ISNULL(dfm.[value], '') + ''' as ' +
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
        + ') '
    END
    + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.Rail_DEFValues_Manual dfm
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON i.COLUMN_NAME = dfm.column_name
      AND i.TABLE_NAME = dfm.table_name
  WHERE dfm.table_name = @tableName
  AND i.TABLE_SCHEMA = 'dbo'
  AND dfm.FHasDefaultVal = 1
  AND dfm.record_id = @recordId

  EXEC (@str);
END
GO
-- add rail declaration tab record --
CREATE PROCEDURE dbo.rail_add_declaration_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );
  DECLARE @masterBill VARCHAR(128)

  SELECT TOP 1
    @masterBill = p.BillofLading
  FROM dbo.Rail_Filing_Details d
  INNER JOIN dbo.Rail_BD_Parsed p
    ON p.BDP_PK = d.BDP_FK
  WHERE d.Filing_Headers_FK = @filingHeaderId

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_DeclarationTab (
        Carrier_SCAC
       ,Country_of_Export
       ,Description
       ,Destination
       ,Destination_State
       ,Discharge
       ,Entry_Port
       ,FIRMs_Code
       ,Importer
       ,Issuer
       ,Main_Supplier
       ,Origin
       ,Master_Bill
       ,Owner_Ref
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        p.IssuerCode AS Carrier_SCAC
       ,rp.Export AS Country_of_Export
       ,Description1 AS Description
       ,rp.Destination AS Destination
       ,rn.Destination_State AS Destination_State
       ,p.PortOfUnlading AS Discharge
       ,p.PortOfUnlading AS Entry_Port
       ,rp.FIRMs_Code AS FIRMs_Code
       ,rn.Importer AS Importer
       ,p.IssuerCode AS Issuer
       ,rn.Main_Supplier AS Main_Supplier
       ,rp.Origin AS Origin
       ,@masterBill
       ,p.ReferenceNumber1
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail containers tab record --
CREATE PROCEDURE dbo.rail_add_container_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_ContainersTab (
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,Gross_Weight
       ,Gross_Weight_Unit
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        details.BDP_FK
       ,p.IssuerCode
       ,p.BillofLading
       ,CONCAT('MB:', p.BillofLading)
       ,CONCAT(EquipmentInitial, EquipmentNumber)
       ,p.Weight
       ,p.WeightUnit
       ,details.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details details
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = details.BDP_FK
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail invoice line record --
CREATE PROCEDURE dbo.rail_add_invoice_line_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_InvoiceLines (
        InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Consignee
       ,Dest_State
       ,Export
       ,Goods_Description
       ,Manufacturer
       ,ORG
       ,Origin
       ,Prod_ID_1
       ,Tariff
       ,Transaction_Related
       ,Customs_QTY
       ,SPI
       ,UQ
       ,PriceUnit
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
       ,Description
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId AS InvoiceHeaders_FK
       ,rd.Attribute_1 AS Attribute_1
       ,rd.Attribute_2 AS Attribute_2
       ,rn.Consignee AS Consignee
       ,rn.Destination_State AS Dest_State
       ,rp.Export AS Export
       ,rd.Goods_Description AS Goods_Description
       ,rn.Manufacturer AS Manufacturer
       ,rn.CountryofOrigin AS ORG
       ,rn.CountryofOrigin AS Origin
       ,rd.Prod_ID_1 AS Prod_ID_1
       ,rd.Tariff AS Tariff
       ,rn.Relationship AS Transaction_Related
       ,rd.Template_HTS_Quantity AS Customs_QTY
       ,rn.DFT AS SPI
       ,rd.Invoice_UOM AS UQ
       ,rn.Value AS PriceUnit
       ,rd.Template_Invoice_Quantity AS Invoice_Qty
       ,rd.Invoice_UOM AS Invoice_Qty_Unit
       ,rn.Freight AS Amount
       ,rn.Value * rd.Template_Invoice_Quantity AS Line_Price
       ,rd.Description AS Description
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON p.Importer = rn.Importer_Name
          AND (p.Supplier = rn.Supplier_Name
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rd
        ON RTRIM(LTRIM(rd.Description1)) = RTRIM(LTRIM(p.Description1))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add rail invoice header record --
CREATE PROCEDURE dbo.rail_add_invoice_header_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_InvoiceHeaders (
        Consignee
       ,Export
       ,Importer
       ,Manufacturer
       ,Origin
       ,Seller
       ,Ship_to_party
       ,Sold_to_party
       ,Supplier
       ,Transaction_Related
       ,FILING_HEADERS_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        rn.Consignee AS Consignee
       ,rp.Export AS Export
       ,rn.Importer AS Importer
       ,rn.Manufacturer AS Manufacturer
       ,rn.CountryofOrigin AS Origin
       ,rn.Seller AS Seller
       ,rn.Ship_to_party AS Ship_to_party
       ,rn.Sold_to_party AS Sold_to_party
       ,rn.Main_Supplier AS Supplier
       ,rn.Relationship AS Transaction_Related
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_Port rp
        ON p.PortOfUnlading = RP.Port
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      WHERE d.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.rail_add_invoice_line_record @filingHeaderId
                                         ,@recordId
                                         ,@filingUser

    -- fill the def value manual table
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF;
END;
GO
-- add rail misc record --
CREATE PROCEDURE dbo.rail_add_misc_record (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );


  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Rail_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Rail_MISC (
        Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,Filing_Headers_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
       ,rn.FTA_Recon AS FTA_Recon
       ,rn.Payment_Type AS Payment_Type
       ,rn.Broker_to_Pay AS Broker_to_Pay
       ,rn.Importer AS Submitter
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,d.FILING_HEADERS_FK
      FROM dbo.Rail_Filing_Details d
      INNER JOIN dbo.Rail_BD_Parsed p
        ON p.BDP_PK = d.BDP_FK
      LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
        ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
          AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
            OR (p.Supplier IS NULL
              AND rn.Supplier_Name IS NULL))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @filingUser
      WHERE d.Filing_Headers_FK = @filingHeaderId

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
    EXEC dbo.rail_add_def_values_manual @tableName
                                       ,@filingHeaderId
                                       ,@parentId
                                       ,@recordId

    -- apply default values
    EXEC dbo.rail_apply_def_values @tableName
                                  ,@recordId

    -- update def value manual table date with values from result table
    EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
  SET NOCOUNT OFF
END;
GO
-- add filing records --
CREATE PROCEDURE dbo.rail_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.rail_add_declaration_record @filingHeaderId
                                      ,@filingHeaderId
                                      ,@filingUser
  -- add container
  EXEC dbo.rail_add_container_record @filingHeaderId
                                    ,@filingHeaderId
                                    ,@filingUser
  -- add invoice header
  EXEC dbo.rail_add_invoice_header_record @filingHeaderId
                                         ,@filingHeaderId
                                         ,@filingUser
  -- add misc
  EXEC dbo.rail_add_misc_record @filingHeaderId
                               ,@filingHeaderId
                               ,@filingUser
END;
GO
/****** Object:  StoredProcedure [dbo].[rail_BD_Parsed_del]    Script Date: 24.12.2018 ******/
CREATE PROCEDURE dbo.rail_BD_Parsed_del (@BDP_PK INT,
@FDeleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = rig.Filing_Headers_id
   ,@mapping_status = rig.Filing_Headers_MappingStatus
  FROM Rail_Inbound_Grid rig
  WHERE rig.BD_Parsed_Id = @BDP_PK

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Rail_BD_Parsed
    SET FDeleted = @FDeleted
    WHERE BDP_PK = @BDP_PK
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Rail_BD_Parsed
      SET FDeleted = @FDeleted
      WHERE BDP_PK IN (SELECT
          rfd.BDP_FK
        FROM Rail_Filing_Details rfd
        WHERE rfd.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO
-- delete record from resulting table
CREATE PROCEDURE dbo.rail_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.rail_sections ps
      WHERE ps.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE
    THROW 51000, 'Invalid table name', 1
END
GO
-- delete filing record
CREATE PROCEDURE dbo.rail_filing_del (@Filing_Headers_id INT)
AS
BEGIN

  DELETE FROM dbo.Rail_DEFValues_Manual
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM dbo.Rail_Filing_Details
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM dbo.Rail_Filing_Headers
  WHERE id = @Filing_Headers_id

END;
GO
-- apply def values --
CREATE PROCEDURE dbo.rail_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.value IS NULL THEN +'NULL '
      ELSE 'try_cast(''' + ISNULL(v.value, '') + ''' as ' +
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
        + ') '
    END
    + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.Filing_Headers_FK = @Filing_Headers_id
  AND v.FEditable = 1

  EXEC (@str);
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
 ,rail_doc.DocumentType AS document_type
 ,'Rail_Imp' AS transport_shipment_type
FROM Rail_Documents rail_doc
INNER JOIN Rail_Filing_Headers rail_header
  ON rail_doc.Filing_Headers_FK = rail_header.id
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
DROP FUNCTION dbo.fn_imp_rail_car_count
ALTER TABLE dbo.imp_rail_filing_header
DROP COLUMN gross_weight_summary_unit
ALTER TABLE dbo.imp_rail_filing_header
DROP COLUMN gross_weight_summary
DROP FUNCTION dbo.fn_imp_rail_gross_weight_summary
DROP FUNCTION dbo.fn_imp_rail_gross_weight_summary_unit
ALTER TABLE dbo.imp_rail_invoice_header
DROP COLUMN invoice_total
DROP FUNCTION dbo.fn_imp_rail_invoice_total
ALTER TABLE dbo.imp_rail_invoice_line
DROP COLUMN invoice_line_number
ALTER TABLE dbo.imp_rail_invoice_line
DROP COLUMN gross_weight
ALTER TABLE dbo.imp_rail_invoice_line
DROP COLUMN gross_weight_unit
DROP FUNCTION dbo.fn_imp_rail_gross_weight
DROP FUNCTION dbo.fn_imp_rail_gross_weight_unit
DROP FUNCTION dbo.fn_imp_rail_invoice_line_number
DROP FUNCTION dbo.fn_app_weight_to_ton

PRINT ('drop tables')
GO
DROP TABLE dbo.imp_rail_declaration
DROP TABLE dbo.imp_rail_container
DROP TABLE dbo.imp_rail_misc
DROP TABLE dbo.imp_rail_document
DROP TABLE dbo.imp_rail_invoice_line
DROP TABLE dbo.imp_rail_invoice_header
DROP TABLE dbo.imp_rail_filing_detail
DROP TABLE dbo.imp_rail_filing_header
DROP TABLE dbo.imp_rail_inbound
DROP TABLE dbo.imp_rail_broker_download
DROP TABLE dbo.imp_rail_rule_port
DROP TABLE dbo.imp_rail_rule_importer_supplier
DROP TABLE dbo.imp_rail_rule_product
DROP TABLE dbo.imp_rail_form_configuration
DROP TABLE dbo.imp_rail_form_section_configuration
DROP TABLE dbo.app_weight_conversion_rate

PRINT ('drop views')
GO
DROP VIEW dbo.v_imp_rail_form_configuration
DROP VIEW dbo.v_imp_rail_field_configuration
DROP VIEW dbo.v_imp_rail_inbound_grid
DROP VIEW dbo.v_imp_rail_review_grid
DROP VIEW dbo.v_imp_rail_report

PRINT ('drop procedures')
GO
DROP PROCEDURE dbo.sp_imp_rail_create_entry_records
DROP PROCEDURE dbo.sp_imp_rail_add_declaration
DROP PROCEDURE dbo.sp_imp_rail_add_container
DROP PROCEDURE dbo.sp_imp_rail_add_invoice_header
DROP PROCEDURE dbo.sp_imp_rail_add_invoice_line
DROP PROCEDURE dbo.sp_imp_rail_add_misc
DROP PROCEDURE dbo.sp_imp_rail_review_entry
DROP PROCEDURE dbo.sp_imp_rail_delete_entry_records
DROP PROCEDURE dbo.sp_imp_rail_update_entry
DROP PROCEDURE dbo.sp_imp_rail_recalculate
DROP PROCEDURE dbo.sp_imp_rail_delete_inbound
DROP PROCEDURE dbo.sp_app_transpose
