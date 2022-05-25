PRINT ('create tables')
GO

CREATE TABLE dbo.Pipeline_Inbound (
  Id INT IDENTITY
 ,Importer NVARCHAR(200) NOT NULL
 ,Batch VARCHAR(20) NOT NULL
 ,TicketNumber VARCHAR(20) NOT NULL
 ,Quantity NUMERIC(18, 6) NOT NULL
 ,API NUMERIC(18, 4) NOT NULL
 ,ExportDate DATETIME NOT NULL
 ,ImportDate DATETIME NOT NULL
 ,CreatedDate DATETIME NOT NULL
 ,CreatedUser VARCHAR(128) NOT NULL
 ,FDeleted BIT NOT NULL DEFAULT (0)
 ,SiteName VARCHAR(128) NULL
 ,Facility VARCHAR(128) NULL
 ,entry_number VARCHAR(11) NOT NULL DEFAULT ('')
 ,CONSTRAINT [PK_dbo.Pipeline_Inbound] PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Filing_Headers (
  id INT IDENTITY
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,ErrorDescription VARCHAR(MAX) NULL
 ,FilingNumber VARCHAR(255) NULL
 ,FilingStatus INT NULL
 ,MappingStatus INT NULL
 ,RequestXML NVARCHAR(MAX) NULL
 ,ResponseXML NVARCHAR(MAX) NULL
 ,job_link VARCHAR(1024) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Filing_Details (
  Filing_Headers_FK INT NOT NULL
 ,Pipeline_Inbounds_FK INT NOT NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Filing_Details] PRIMARY KEY CLUSTERED (Filing_Headers_FK, Pipeline_Inbounds_FK)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_DeclarationTab (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,PI_FK INT NOT NULL
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
 ,Batch_Ticket VARCHAR(20) NULL
 ,Pipeline VARCHAR(128) NULL
 ,TripID VARCHAR(128) NULL
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
 ,Description VARCHAR(128) NULL
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
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,EntryNumber VARCHAR(11) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_ContainersTab (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,PI_FK INT NULL
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
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_InvoiceHeaders (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,Supplier VARCHAR(128) NULL
 ,Supplier_Address VARCHAR(128) NULL
 ,INCO VARCHAR(128) NULL
 ,Invoice_Total DECIMAL(28, 15) NULL
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
 ,Invoice_No VARCHAR(128) NULL
 ,Manufacturer_Address VARCHAR(128) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_InvoiceLines (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,InvoiceHeaders_FK INT NOT NULL
 ,PI_FK INT NULL
 ,Invoice_No VARCHAR(128) NULL
 ,Tariff VARCHAR(128) NULL
 ,Customs_QTY NUMERIC(18, 6) NULL
 ,Line_Price DECIMAL(28, 15) NULL
 ,Goods_Description VARCHAR(128) NULL
 ,ORG VARCHAR(128) NULL
 ,SPI VARCHAR(128) NULL
 ,Gr_Weight NUMERIC(18, 6) NULL
 ,UQ VARCHAR(128) NULL
 ,PriceUnit DECIMAL(28, 15) NULL
 ,Prod_ID_1 VARCHAR(128) NULL
 ,Attribute_1 VARCHAR(128) NULL
 ,Attribute_3 VARCHAR(128) NULL
 ,Export VARCHAR(128) NULL
 ,Origin VARCHAR(128) NULL
 ,Dest_State VARCHAR(128) NULL
 ,Transaction_Related VARCHAR(128) NULL
 ,Invoice_Qty NUMERIC(18, 6) NULL
 ,Invoice_Qty_Unit VARCHAR(128) NULL
 ,Manufacturer VARCHAR(128) NULL
 ,Consignee VARCHAR(128) NULL
 ,Sold_to_party VARCHAR(128) NULL
 ,Code VARCHAR(128) NULL
 ,Curr VARCHAR(128) NULL
 ,CIF_Component VARCHAR(128) NULL
 ,EPA_TSCA_Toxic_Substance_Control_Act_Indicator VARCHAR(128) NULL
 ,TSCA_Indicator VARCHAR(128) NULL
 ,Certifying_Individual VARCHAR(128) NULL
 ,PGA_Contact_Name VARCHAR(128) NULL
 ,PGA_Contact_Phone VARCHAR(128) NULL
 ,PGA_Contact_Email VARCHAR(128) NULL
 ,Amount DECIMAL(18, 2) NULL
 ,Description VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Gr_Weight_Unit VARCHAR(2) NULL
 ,Attribute_2 DECIMAL(18, 6) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_MISC (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,PI_FK INT NULL
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
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Documents (
  id INT IDENTITY
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,DocumentType VARCHAR(128) NULL
 ,file_content VARBINARY(MAX) NULL
 ,file_desc VARCHAR(1000) NULL
 ,file_extension VARCHAR(128) NULL
 ,filename VARCHAR(255) NULL
 ,Filing_Headers_FK INT NULL
 ,Status NVARCHAR(50) NULL
 ,inbound_record_id INT NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_API (
  Id INT NOT NULL
 ,API NVARCHAR(128) NOT NULL
 ,Tariff NVARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_API] PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_BatchCode (
  id INT IDENTITY
 ,batch_code VARCHAR(128) NOT NULL
 ,product VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_BatchCode] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_Consignee_Importer (
  id INT IDENTITY
 ,ticket_importer VARCHAR(128) NOT NULL
 ,importer_code VARCHAR(128) NOT NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_Consignee_Importer] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_Facility (
  id INT IDENTITY
 ,facility VARCHAR(128) NOT NULL
 ,port VARCHAR(128) NOT NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,destination VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,issuer VARCHAR(128) NULL
 ,origin VARCHAR(128) NULL
 ,pipeline VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_Facility] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_Importer (
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
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_Importer] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_Rule_Price (
  id INT IDENTITY
 ,importer_id UNIQUEIDENTIFIER NOT NULL
 ,crude_type_id INT NULL
 ,pricing DECIMAL(28, 15) NOT NULL
 ,freight DECIMAL(18, 6) NOT NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Pipeline_Rule_Price] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.pipeline_sections (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,procedure_name VARCHAR(128) NULL
 ,CONSTRAINT PK_pipeline_sections_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_DEFValues (
  id INT IDENTITY
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Pipeline_DEFValues_Display_on_UI] DEFAULT (0)
 ,ValueLabel VARCHAR(128) NULL
 ,ValueDesc VARCHAR(128) NULL
 ,DefValue VARCHAR(128) NULL
 ,ColName VARCHAR(128) NULL
 ,FManual TINYINT NOT NULL
 ,FHasDefaultVal TINYINT NOT NULL
 ,FEditable TINYINT NOT NULL
 ,FMandatory TINYINT NOT NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,SingleFilingOrder TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,section_id INT NOT NULL
 ,CONSTRAINT [PK_dbo.Pipeline_DEFValues] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Pipeline_DEFValues_Manual (
  id INT IDENTITY
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Pipeline_DEFValues_Manual_Display_on_UI] DEFAULT (0)
 ,FEditable TINYINT NOT NULL DEFAULT (1)
 ,FHasDefaultVal TINYINT NOT NULL DEFAULT (0)
 ,Filing_Headers_FK INT NOT NULL
 ,FMandatory TINYINT NOT NULL DEFAULT (0)
 ,FManual TINYINT NOT NULL DEFAULT (0)
 ,ModifiedDate DATETIME NOT NULL DEFAULT (GETDATE())
 ,ValueLabel VARCHAR(128) NULL
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
 ,CONSTRAINT [PK_dbo.Pipeline_DEFValues_Manual] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.App_WeightsConversion (
  WeightUnit NVARCHAR(2) NOT NULL
 ,RateInTn NUMERIC(18, 9) NOT NULL
 ,CONSTRAINT [PK_dbo.App_WeightsConversion] PRIMARY KEY CLUSTERED (WeightUnit)
) ON [PRIMARY]
GO

PRINT ('copy data')
GO
PRINT ('copy inbound data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Inbound ON;
INSERT INTO dbo.Pipeline_Inbound (
    Id
   ,Importer
   ,Batch
   ,TicketNumber
   ,Quantity
   ,API
   ,ExportDate
   ,ImportDate
   ,SiteName
   ,Facility
   ,entry_number
   ,FDeleted
   ,CreatedDate
   ,CreatedUser)
  SELECT
    ti.id
   ,ti.importer
   ,ti.batch
   ,ti.ticket_number
   ,ti.quantity
   ,ti.api
   ,ti.export_date
   ,ti.import_date
   ,ti.site_name
   ,ti.facility
   ,ti.entry_number
   ,ti.deleted
   ,ti.created_date
   ,ti.created_user
  FROM dbo.imp_pipeline_inbound ti;
SET IDENTITY_INSERT dbo.Pipeline_Inbound OFF;
GO

PRINT ('copy filing header data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Filing_Headers ON;
INSERT INTO dbo.Pipeline_Filing_Headers (
    id
   ,FilingNumber
   ,MappingStatus
   ,FilingStatus
   ,ErrorDescription
   ,job_link
   ,RequestXML
   ,ResponseXML
   ,CreatedDate
   ,CreatedUser)
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
  FROM dbo.imp_pipeline_filing_header fh;
SET IDENTITY_INSERT dbo.Pipeline_Filing_Headers OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.Pipeline_Filing_Details (
    Pipeline_Inbounds_FK
   ,Filing_Headers_FK)
  SELECT
    fd.inbound_id
   ,fd.filing_header_id
  FROM dbo.imp_pipeline_filing_detail fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.Pipeline_DeclarationTab ON;
INSERT INTO dbo.Pipeline_DeclarationTab (
    id
   ,Filing_Headers_FK
   ,PI_FK
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
   ,Batch_Ticket
   ,Pipeline
   ,TripID
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
   ,EntryNumber
   ,CreatedDate
   ,CreatedUser)
  SELECT
    declaration.id
   ,declaration.filing_header_id
   ,detail.inbound_id
   ,declaration.main_supplier
   ,declaration.importer
   ,declaration.shipment_type
   ,declaration.transport
   ,declaration.container
   ,declaration.entry_type
   ,declaration.rlf
   ,declaration.enable_entry_sum
   ,declaration.type
   ,declaration.certify_cargo_release
   ,declaration.service
   ,declaration.issuer
   ,declaration.master_bill
   ,declaration.batch_ticket
   ,declaration.pipeline
   ,declaration.trip_id
   ,declaration.carrier_scac
   ,declaration.discharge
   ,declaration.entry_port
   ,declaration.dep
   ,declaration.arr
   ,declaration.arr2
   ,declaration.hmf
   ,declaration.origin
   ,declaration.destination
   ,declaration.destination_state
   ,declaration.country_of_export
   ,declaration.eta
   ,declaration.export_date
   ,declaration.description
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
   ,declaration.entry_number
   ,declaration.created_date
   ,declaration.created_user
  FROM dbo.imp_pipeline_declaration declaration
  JOIN dbo.imp_pipeline_filing_header header
    ON declaration.filing_header_id = header.id
  LEFT JOIN dbo.imp_pipeline_filing_detail detail
    ON detail.filing_header_id = header.id;
SET IDENTITY_INSERT dbo.Pipeline_DeclarationTab OFF;
GO

PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.Pipeline_ContainersTab ON;
INSERT INTO dbo.Pipeline_ContainersTab (
    id
   ,Filing_Headers_FK
   ,PI_FK
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
   ,CreatedDate
   ,CreatedUser)
  SELECT
    container.id
   ,container.filing_header_id
   ,detail.inbound_id
   ,container.bill_type
   ,container.manifest_qty
   ,container.uq
   ,container.bill_issuer_scac
   ,container.it_number
   ,container.is_split
   ,container.bill_number
   ,container.container_number
   ,container.pack_qty
   ,container.pack_type
   ,container.marks_and_numbers
   ,container.shipping_symbol
   ,container.seal_number
   ,container.type
   ,container.mode
   ,container.goods_weight
   ,container.bill_num
   ,container.packing_uq
   ,container.created_date
   ,container.created_user
  FROM dbo.imp_pipeline_container container
  JOIN dbo.imp_pipeline_filing_header header
    ON container.filing_header_id = header.id
  LEFT JOIN dbo.imp_pipeline_filing_detail detail
    ON detail.filing_header_id = header.id;
SET IDENTITY_INSERT dbo.Pipeline_ContainersTab OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.Pipeline_InvoiceHeaders ON;
INSERT INTO dbo.Pipeline_InvoiceHeaders (
    id
   ,Filing_Headers_FK
   ,Supplier
   ,Supplier_Address
   ,INCO
   ,Invoice_Total
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
   ,Invoice_No
   ,Manufacturer_Address
   ,CreatedDate
   ,CreatedUser)
  SELECT
    invoice.id
   ,invoice.filing_header_id
   ,invoice.supplier
   ,invoice.supplier_address
   ,invoice.inco
   ,invoice.invoice_total
   ,invoice.curr
   ,invoice.origin
   ,invoice.payment_date
   ,invoice.consignee
   ,invoice.consignee_address
   ,invoice.inv_date
   ,invoice.agreed_place
   ,invoice.inv_gross_weight
   ,invoice.net_weight
   ,invoice.export
   ,invoice.export_date
   ,invoice.first_sale
   ,invoice.transaction_related
   ,invoice.packages
   ,invoice.manufacturer
   ,invoice.seller
   ,invoice.importer
   ,invoice.sold_to_party
   ,invoice.ship_to_party
   ,invoice.broker_pga_contact_name
   ,invoice.broker_pga_contact_phone
   ,invoice.broker_pga_contact_email
   ,invoice.epa_pst_cert_date
   ,invoice.epa_tsca_cert_date
   ,invoice.epa_vne_cert_date
   ,invoice.fsis_cert_date
   ,invoice.fws_cert_date
   ,invoice.lacey_act_cert_date
   ,invoice.nhtsa_cert_date
   ,invoice.landed_costing_ex_rate
   ,invoice.invoice_no
   ,invoice.manufacturer_address
   ,invoice.created_date
   ,invoice.created_user
  FROM dbo.imp_pipeline_invoice_header invoice;
SET IDENTITY_INSERT dbo.Pipeline_InvoiceHeaders OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.Pipeline_InvoiceLines ON;
INSERT INTO dbo.Pipeline_InvoiceLines (
    id
   ,Filing_Headers_FK
   ,InvoiceHeaders_FK
   ,PI_FK
   ,Invoice_No
   ,Tariff
   ,Customs_QTY
   ,Line_Price
   ,Goods_Description
   ,ORG
   ,SPI
   ,Gr_Weight
   ,Gr_Weight_Unit
   ,UQ
   ,PriceUnit
   ,Prod_ID_1
   ,Attribute_1
   ,Attribute_2
   ,Attribute_3
   ,Export
   ,Origin
   ,Dest_State
   ,Transaction_Related
   ,Invoice_Qty
   ,Invoice_Qty_Unit
   ,Manufacturer
   ,Consignee
   ,Sold_to_party
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
    line.id
   ,line.filing_header_id
   ,line.parent_record_id
   ,detail.inbound_id
   ,line.invoice_no
   ,line.tariff
   ,line.customs_qty
   ,line.line_price
   ,line.goods_description
   ,line.org
   ,line.spi
   ,line.gr_weight
   ,line.gr_weight_unit
   ,line.uq
   ,line.price_unit
   ,line.prod_id1
   ,line.attribute1
   ,line.attribute2
   ,line.attribute3
   ,line.export
   ,line.origin
   ,line.dest_state
   ,line.transaction_related
   ,line.invoice_qty
   ,line.invoice_qty_unit
   ,line.manufacturer
   ,line.consignee
   ,line.sold_to_party
   ,line.code
   ,line.curr
   ,line.cif_component
   ,line.epa_tsca_toxic_substance_control_act_indicator
   ,line.tsca_indicator
   ,line.certifying_individual
   ,line.pga_contact_name
   ,line.pga_contact_phone
   ,line.pga_contact_email
   ,line.amount
   ,line.description
   ,line.created_date
   ,line.created_user
  FROM dbo.imp_pipeline_invoice_line line
  JOIN dbo.imp_pipeline_filing_header header
    ON line.filing_header_id = header.id
  LEFT JOIN dbo.imp_pipeline_filing_detail detail
    ON detail.filing_header_id = header.id;
SET IDENTITY_INSERT dbo.Pipeline_InvoiceLines OFF;
GO

PRINT ('copy misc data')
GO
SET IDENTITY_INSERT dbo.Pipeline_MISC ON;
INSERT INTO dbo.Pipeline_MISC (
    id
   ,Filing_Headers_FK
   ,PI_FK
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
    misc.id
   ,misc.filing_header_id
   ,detail.inbound_id
   ,misc.branch
   ,misc.broker
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
   ,misc.created_date
   ,misc.created_user
  FROM dbo.imp_pipeline_misc misc
  JOIN dbo.imp_pipeline_filing_header header
    ON misc.filing_header_id = header.id
  LEFT JOIN dbo.imp_pipeline_filing_detail detail
    ON detail.filing_header_id = header.id;
SET IDENTITY_INSERT dbo.Pipeline_MISC OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Documents ON;
INSERT INTO dbo.Pipeline_Documents (
    id
   ,Filing_Headers_FK
   ,inbound_record_id
   ,[filename]
   ,file_extension
   ,file_desc
   ,file_content
   ,DocumentType
   ,[Status]
   ,CreatedDate
   ,CreatedUser)
  SELECT
    doc.id
   ,doc.filing_header_id
   ,doc.inbound_record_id
   ,[doc].[file_name]
   ,doc.file_extension
   ,doc.file_description
   ,doc.file_content
   ,doc.document_type
   ,[doc].[status]
   ,doc.created_date
   ,doc.created_user
  FROM dbo.imp_pipeline_document doc;
SET IDENTITY_INSERT dbo.Pipeline_Documents OFF;
GO

PRINT ('copy api rule data')
GO
INSERT INTO dbo.Pipeline_Rule_API (
    id
   ,API
   ,Tariff
   ,created_date
   ,created_user)
  SELECT
    r.id
   ,r.api
   ,r.tariff
   ,r.created_date
   ,r.created_user
  FROM dbo.imp_pipeline_rule_api r;
GO

PRINT ('copy batch code rule data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Rule_BatchCode ON;
INSERT INTO dbo.Pipeline_Rule_BatchCode (
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
  FROM dbo.imp_pipeline_rule_batch_code r;
SET IDENTITY_INSERT dbo.Pipeline_Rule_BatchCode OFF;
GO

PRINT ('copy consignee-importer rule data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Rule_Consignee_Importer ON;
INSERT INTO dbo.Pipeline_Rule_Consignee_Importer (
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
  FROM dbo.imp_pipeline_rule_consignee_importer r;
SET IDENTITY_INSERT dbo.Pipeline_Rule_Consignee_Importer OFF;
GO

PRINT ('copy facility rule data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Rule_Facility ON;
INSERT INTO dbo.Pipeline_Rule_Facility (
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
  FROM dbo.imp_pipeline_rule_facility r;
SET IDENTITY_INSERT dbo.Pipeline_Rule_Facility OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Rule_Importer ON;
INSERT INTO dbo.Pipeline_Rule_Importer (
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
  FROM dbo.imp_pipeline_rule_importer r;
SET IDENTITY_INSERT dbo.Pipeline_Rule_Importer OFF;
GO

PRINT ('copy price rule data')
GO
SET IDENTITY_INSERT dbo.Pipeline_Rule_Price ON;
INSERT INTO dbo.Pipeline_Rule_Price (
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
  FROM dbo.imp_pipeline_rule_price r;
SET IDENTITY_INSERT dbo.Pipeline_Rule_Price OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.Pipeline_sections ON;
INSERT INTO dbo.Pipeline_sections (
    id
   ,[name]
   ,title
   ,table_name
   ,is_array
   ,parent_id
   ,[procedure_name])
  SELECT
    section.id
   ,[section].[name]
   ,section.title
   ,section.table_name
   ,section.is_array
   ,section.parent_id
   ,[section].[procedure_name]
  FROM dbo.imp_pipeline_form_section_configuration section;
SET IDENTITY_INSERT dbo.Pipeline_sections OFF;
GO

UPDATE imp_pipeline_form_section_configuration
SET table_name = 'Pipeline_DeclarationTab'
   ,procedure_name = 'pipeline_add_declaration_record'
WHERE table_name = 'imp_pipeline_declaration';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'Pipeline_InvoiceHeaders'
   ,procedure_name = 'pipeline_add_invoice_header_record'
WHERE table_name = 'imp_pipeline_invoice_header';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'Pipeline_InvoiceLines'
   ,procedure_name = 'pipeline_add_invoice_line_record'
WHERE table_name = 'imp_pipeline_invoice_line';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'Pipeline_ContainersTab'
   ,procedure_name = 'pipeline_add_container_record'
WHERE table_name = 'imp_pipeline_container';
UPDATE imp_pipeline_form_section_configuration
SET table_name = 'Pipeline_MISC'
   ,procedure_name = 'pipeline_add_misc_record'
WHERE table_name = 'imp_pipeline_misc';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.Pipeline_DEFValues ON;
INSERT INTO dbo.Pipeline_DEFValues (
    id
   ,section_id
   ,ColName
   ,[ValueLabel]
   ,[ValueDesc]
   ,DefValue
   ,FHasDefaultVal
   ,FEditable
   ,FMandatory
   ,Display_on_UI
   ,[FManual]
   ,SingleFilingOrder
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,CreatedDate
   ,CreatedUser)
  SELECT
    dv.id
   ,dv.section_id
   ,dv.column_name
   ,[dv].[label]
   ,[dv].[description]
   ,[dv].[value]
   ,dv.has_default_value
   ,dv.editable
   ,dv.mandatory
   ,dv.display_on_ui
   ,[dv].[manual]
   ,dv.single_filing_order
   ,dv.paired_field_table
   ,dv.paired_field_column
   ,dv.handbook_name
   ,dv.created_date
   ,dv.created_user
  FROM dbo.imp_pipeline_form_configuration dv;
SET IDENTITY_INSERT dbo.Pipeline_DEFValues OFF;
GO

UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'Arr_2'
WHERE column_name = 'arr2'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'TripID'
WHERE column_name = 'trip_id'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'EntryNumber'
WHERE column_name = 'entry_number'
AND section_id = 2;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'Attribute_1'
WHERE column_name = 'attribute1'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'Attribute_2'
WHERE column_name = 'attribute2'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'Attribute_3'
WHERE column_name = 'attribute3'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'Prod_ID_1'
WHERE column_name = 'prod_id1'
AND section_id = 5;
UPDATE dbo.imp_pipeline_form_configuration
SET column_name = 'PriceUnit'
WHERE column_name = 'price_unit'
AND section_id = 5;
GO

PRINT ('copy weights convertion rate data')
GO
INSERT INTO dbo.App_WeightsConversion (
    WeightUnit
   ,RateInTn)
  SELECT
    rate.weight_unit
   ,rate.rate
  FROM dbo.app_weight_conversion_rate rate;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.Pipeline_Filing_Headers
ADD CONSTRAINT [FK_dbo.Pipeline_Filing_Headers_dbo.FilingStatus_FilingStatus] FOREIGN KEY (FilingStatus) REFERENCES dbo.FilingStatus (id)
GO

ALTER TABLE dbo.Pipeline_Filing_Headers
ADD CONSTRAINT [FK_dbo.Pipeline_Filing_Headers_dbo.MappingStatus_MappingStatus] FOREIGN KEY (MappingStatus) REFERENCES dbo.MappingStatus (id)
GO

ALTER TABLE dbo.Pipeline_Filing_Details
ADD CONSTRAINT [FK_dbo.Pipeline_Filing_Details_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_Filing_Details
ADD CONSTRAINT [FK_dbo.Pipeline_Filing_Details_dbo.Pipeline_Inbound_Pipeline_Inbounds_FK] FOREIGN KEY (Pipeline_Inbounds_FK) REFERENCES dbo.Pipeline_Inbound (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_DeclarationTab
ADD CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_DeclarationTab
ADD CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Inbound_PI_FK] FOREIGN KEY (PI_FK) REFERENCES dbo.Pipeline_Inbound (Id)
GO

ALTER TABLE dbo.Pipeline_ContainersTab
ADD CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_ContainersTab
ADD CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Inbound_PI_FK] FOREIGN KEY (PI_FK) REFERENCES dbo.Pipeline_Inbound (Id)
GO

ALTER TABLE dbo.Pipeline_InvoiceHeaders
ADD CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_InvoiceLines
ADD CONSTRAINT [FK_dbo.Pipeline_InvoiceLines.Pipeline_Inbound_PI_FK] FOREIGN KEY (PI_FK) REFERENCES dbo.Pipeline_Inbound (Id)
GO

ALTER TABLE dbo.Pipeline_InvoiceLines
ADD CONSTRAINT FK_Pipeline_InvoiceLines_Pipeline_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Pipeline_InvoiceHeaders (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_MISC
ADD CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_MISC
ADD CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Inbound_PI_FK] FOREIGN KEY (PI_FK) REFERENCES dbo.Pipeline_Inbound (Id)
GO

ALTER TABLE dbo.Pipeline_Documents
ADD CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pipeline_Documents
ADD CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Inbound_inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.Pipeline_Inbound (Id)
GO

ALTER TABLE dbo.Pipeline_Rule_Price
ADD CONSTRAINT [FK_dbo.Pipeline_Rule_Price_dbo.Clients_importer_id] FOREIGN KEY (importer_id) REFERENCES dbo.Clients (id)
GO

ALTER TABLE dbo.Pipeline_Rule_Price
ADD CONSTRAINT [FK_dbo.Pipeline_Rule_Price_dbo.Pipeline_Rule_BatchCode_crude_type_id] FOREIGN KEY (crude_type_id) REFERENCES dbo.Pipeline_Rule_BatchCode (id)
GO

ALTER TABLE dbo.pipeline_sections
ADD CONSTRAINT FK_pipeline_sections_parent_id FOREIGN KEY (parent_id) REFERENCES dbo.pipeline_sections (id)
GO

ALTER TABLE dbo.Pipeline_DEFValues
ADD CONSTRAINT [FK_dbo.Pipeline_DEFValues_dbo.pipeline_sections_section_id] FOREIGN KEY (section_id) REFERENCES dbo.pipeline_sections (id)
GO

PRINT ('add indexes')
GO

CREATE INDEX Idx_FilingStatus
ON dbo.Pipeline_Filing_Headers (FilingStatus)
ON [PRIMARY]
GO

CREATE INDEX Idx_MappingStatus
ON dbo.Pipeline_Filing_Headers (MappingStatus)
ON [PRIMARY]
GO

CREATE INDEX IX_Filing_Headers_FK
ON dbo.Pipeline_Filing_Details (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX IX_Pipeline_Inbounds_FK
ON dbo.Pipeline_Filing_Details (Pipeline_Inbounds_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_PipelineDeclarationTab_filingHeadersFK
ON dbo.Pipeline_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_PipelineContainersTab_filingHeadersFK
ON dbo.Pipeline_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_PipelineInvoiceHeaders_filingHeadersFK
ON dbo.Pipeline_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_PipelineInvoiceLines_invoiceHeadersFK
ON dbo.Pipeline_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_PipelineMISC_filingHeadersFK
ON dbo.Pipeline_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_BatchCode
ON dbo.Pipeline_Rule_BatchCode (batch_code)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_TicketImporter
ON dbo.Pipeline_Rule_Consignee_Importer (ticket_importer)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_Facility
ON dbo.Pipeline_Rule_Facility (facility)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX Idx_Importer
ON dbo.Pipeline_Rule_Importer (importer)
ON [PRIMARY]
GO

CREATE INDEX IX_crude_type_id
ON dbo.Pipeline_Rule_Price (crude_type_id)
ON [PRIMARY]
GO

CREATE INDEX IX_importer_id
ON dbo.Pipeline_Rule_Price (importer_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.pipeline_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.pipeline_sections (parent_id)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.Pipeline_DEFValues (section_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeadersFK
ON dbo.Pipeline_DEFValues_Manual (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_recordId_tableName_columnName
ON dbo.Pipeline_DEFValues_Manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets pipeline import invoice line number
CREATE FUNCTION dbo.pipeline_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      pil.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY pil.id)
    FROM dbo.Pipeline_InvoiceLines pil
    WHERE pil.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- converts weight to Ton
CREATE FUNCTION dbo.weightToTon (@quantity DECIMAL(18, 9) = 0,
@unit VARCHAR(2))
RETURNS DECIMAL(18, 9) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @rate DECIMAL(18, 9) = NULL;
  SELECT
    @rate = awc.RateInTn
  FROM dbo.App_WeightsConversion awc
  WHERE awc.WeightUnit = @unit
  RETURN @rate * @quantity
END
GO

-- alter pipeline import invoice line table
ALTER TABLE dbo.Pipeline_InvoiceLines
ADD invoice_line_number AS ([dbo].[pipeline_invoice_line_number]([InvoiceHeaders_FK], [id]));
ALTER TABLE dbo.Pipeline_InvoiceLines
ADD Gr_Weight_Tons AS ([dbo].[weightToTon]([Gr_Weight], [Gr_Weight_Unit]));
GO

CREATE FUNCTION dbo.extractBatchCode (@batch VARCHAR(20))
RETURNS VARCHAR(3)
AS
BEGIN
  DECLARE @result VARCHAR(3)
  SELECT
    @result = SUBSTRING(@batch, 0, CHARINDEX('-', @batch))
  RETURN @result
END
GO

CREATE FUNCTION dbo.fn_pipeline_GetImporterCode (@Filing_Headers_id INT)
RETURNS VARCHAR(128)
AS
BEGIN
  DECLARE @ImporterCode VARCHAR(128);

  SET @ImporterCode = (SELECT
      CASE
        WHEN importerlookup.importer_code IS NULL THEN inbound.Importer
        ELSE importerlookup.importer_code
      END
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound inbound
      ON details.Pipeline_Inbounds_FK = inbound.id
    LEFT JOIN Pipeline_Rule_Consignee_Importer importerlookup
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(importerlookup.ticket_importer))
    WHERE Filing_Headers_FK = @Filing_Headers_id)

  RETURN @ImporterCode
END
GO

CREATE FUNCTION dbo.fn_pipeline_weight (@bbl NUMERIC(18, 6) = 0,
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

PRINT ('create triggers')
GO
CREATE TRIGGER dbo.pipeline_declaration_tab_befor_delete
ON dbo.Pipeline_DeclarationTab
FOR DELETE
AS
  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE table_name = 'Pipeline_DeclarationTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.pipeline_containers_tab_befor_delete
ON dbo.Pipeline_ContainersTab
FOR DELETE
AS
  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE table_name = 'Pipeline_ContainersTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.pipeline_invoice_headers_befor_delete
ON dbo.Pipeline_InvoiceHeaders
FOR DELETE
AS
  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE table_name = 'Pipeline_InvoiceHeaders'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
--- add triggers ---
CREATE TRIGGER dbo.pipeline_invoice_lines_befor_delete
ON dbo.Pipeline_InvoiceLines
FOR DELETE
AS
  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE table_name = 'Pipeline_InvoiceLines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.pipeline_misc_befor_delete
ON dbo.Pipeline_MISC
FOR DELETE
AS
  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE table_name = 'Pipeline_MISC'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

PRINT ('create views')
GO
CREATE VIEW dbo.Pipeline_Inbound_Grid
AS
SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.entry_number
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Importer ruleImporter
        WHERE RTRIM(LTRIM(pi.Importer)) = RTRIM(LTRIM(ruleImporter.importer))) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_BatchCode ruleBatch
        WHERE dbo.extractBatchCode(pi.Batch) = ruleBatch.batch_code) THEN 1
    ELSE 0
  END AS has_batch_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Facility ruleFacility
        WHERE pi.Facility = ruleFacility.facility) THEN 1
    ELSE 0
  END AS has_facility_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Pipeline_Rule_Price rulePrice
        INNER JOIN Clients clients
          ON rulePrice.importer_id = clients.id
        WHERE RTRIM(LTRIM(pi.Importer)) = RTRIM(LTRIM(clients.ClientCode))
        AND clients.id = rulePrice.importer_id) THEN 1
    ELSE 0
  END AS has_price_rule

FROM dbo.Pipeline_Inbound pi
LEFT OUTER JOIN dbo.Pipeline_Filing_Details fd
  ON pi.Id = fd.Pipeline_Inbounds_FK
LEFT OUTER JOIN dbo.Pipeline_Filing_Headers fh
  ON fd.Filing_Headers_FK = fh.id
    AND fh.MappingStatus <> 0
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Pipeline_Filing_Headers fh
  INNER JOIN dbo.Pipeline_Filing_Details fd
    ON fh.id = fd.Filing_Headers_FK
  WHERE fh.MappingStatus > 0
  AND pi.Id = fd.Pipeline_Inbounds_FK)
AND pi.FDeleted = 0

UNION

SELECT
  pi.Id
 ,pi.Importer
 ,pi.Batch
 ,pi.TicketNumber
 ,pi.Facility
 ,pi.SiteName
 ,pi.Quantity
 ,pi.API
 ,pi.entry_number
 ,pi.ExportDate
 ,pi.ImportDate
 ,pi.CreatedDate
 ,fh.id AS Filing_Headers_Id
 ,ISNULL(fh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ms.name Filing_Headers_MappingStatus_Title
 ,ISNULL(fh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,fs.name Filing_Headers_FilingStatus_Title
 ,fh.FilingNumber AS Filing_Headers_FilingNumber
 ,fh.job_link
 ,pi.FDeleted
 ,1 AS has_importer_rule
 ,1 AS has_batch_rule
 ,1 AS has_facility_rule
 ,1 AS has_price_rule
FROM dbo.Pipeline_Filing_Headers fh
INNER JOIN dbo.Pipeline_Filing_Details fd
  ON fh.id = fd.Filing_Headers_FK
INNER JOIN dbo.Pipeline_Inbound pi
  ON fd.Pipeline_Inbounds_FK = pi.Id
LEFT JOIN MappingStatus ms
  ON ISNULL(fh.MappingStatus, 0) = ms.id
LEFT JOIN FilingStatus fs
  ON ISNULL(fh.FilingStatus, 0) = fs.id
WHERE fh.MappingStatus > 0
AND pi.FDeleted = 0
GO

CREATE VIEW dbo.Pipeline_Report
AS
SELECT
  header.id AS Pipeline_Filing_Headers_id
 ,details.Pipeline_Inbounds_FK AS PI_ID
 ,declaration.Arr AS Pipeline_DeclarationTab_Arr
 ,declaration.Arr_2 AS Pipeline_DeclarationTab_Arr_2
 ,declaration.Carrier_SCAC AS Pipeline_DeclarationTab_Carrier_SCAC
 ,declaration.Centralized_Exam_Site AS Pipeline_DeclarationTab_Centralized_Exam_Site
 ,declaration.Certify_Cargo_Release AS Pipeline_DeclarationTab_Certify_Cargo_Release
 ,declaration.Check_Local_Client AS Pipeline_DeclarationTab_Check_Local_Client
 ,declaration.Container AS Pipeline_DeclarationTab_Container
 ,declaration.Country_of_Export AS Pipeline_DeclarationTab_Country_of_Export
 ,declaration.Dep AS Pipeline_DeclarationTab_Dep
 ,declaration.Description AS Pipeline_DeclarationTab_Description
 ,declaration.Destination AS Pipeline_DeclarationTab_Destination
 ,declaration.Destination_State AS Pipeline_DeclarationTab_Destination_State
 ,declaration.Discharge AS Pipeline_DeclarationTab_Discharge
 ,declaration.Enable_Entry_Sum AS Pipeline_DeclarationTab_Enable_Entry_Sum
 ,declaration.Entry_Port AS Pipeline_DeclarationTab_Entry_Port
 ,declaration.Entry_Type AS Pipeline_DeclarationTab_Entry_Type
 ,declaration.ETA AS Pipeline_DeclarationTab_ETA
 ,declaration.Export_Date AS Pipeline_DeclarationTab_Export_Date
 ,declaration.FIRMs_Code AS Pipeline_DeclarationTab_FIRMs_Code
 ,declaration.HMF AS Pipeline_DeclarationTab_HMF
 ,declaration.Importer AS Pipeline_DeclarationTab_Importer
 ,declaration.Importer_of_record AS Pipeline_DeclarationTab_Importer_of_record
 ,declaration.INCO AS Pipeline_DeclarationTab_INCO
 ,declaration.Issuer AS Pipeline_DeclarationTab_Issuer
 ,declaration.Main_Supplier AS Pipeline_DeclarationTab_Main_Supplier
 ,declaration.Manual_Entry AS Pipeline_DeclarationTab_Manual_Entry
 ,declaration.Master_Bill AS Pipeline_DeclarationTab_Master_Bill
 ,declaration.No_Packages AS Pipeline_DeclarationTab_No_Packages
 ,declaration.Origin AS Pipeline_DeclarationTab_Origin
 ,declaration.Owner_Ref AS Pipeline_DeclarationTab_Owner_Ref
 ,declaration.Purchased AS Pipeline_DeclarationTab_Purchased
 ,declaration.RLF AS Pipeline_DeclarationTab_RLF
 ,declaration.Service AS Pipeline_DeclarationTab_Service
 ,declaration.Shipment_Type AS Pipeline_DeclarationTab_Shipment_Type
 ,declaration.Split_Shipment_Release AS Pipeline_DeclarationTab_Split_Shipment_Release
 ,declaration.Total_Volume AS Pipeline_DeclarationTab_Total_Volume
 ,declaration.Total_Weight AS Pipeline_DeclarationTab_Total_Weight
 ,declaration.Transport AS Pipeline_DeclarationTab_Transport
 ,declaration.Type AS Pipeline_DeclarationTab_Type
 ,containers.Bill_Issuer_SCAC AS Pipeline_Packing_Bill_Issuer_SCAC
 ,containers.Bill_Number AS Pipeline_Packing_Bill_Number
 ,containers.Bill_Type AS Pipeline_Packing_Bill_Type
 ,containers.Container_Number AS Pipeline_Packing_Container_Number
 ,containers.Is_Split AS Pipeline_Packing_Is_Split
 ,containers.IT_Number AS Pipeline_Packing_IT_Number
 ,containers.Manifest_QTY AS Pipeline_Packing_Manifest_QTY
 ,containers.Marks_and_Numbers AS Pipeline_Packing_Marks_and_Numbers
 ,containers.Pack_QTY AS Pipeline_Packing_Pack_QTY
 ,containers.Pack_Type AS Pipeline_Packing_Pack_Type
 ,containers.Shipping_Symbol AS Pipeline_Packing_Shipping_Symbol
 ,containers.UQ AS Pipeline_Packing_UQ
 ,containers.Packing_UQ AS Pipeline_Packing_Container_Packing_UQ
 ,containers.Seal_Number AS Pipeline_Packing_Seal_Number
 ,containers.Type AS Pipeline_Packing_Type
 ,containers.Mode AS Pipeline_Packing_Mode
 ,containers.Goods_Weight AS Pipeline_Packing_Goods_Weight
 ,containers.Bill_Num AS Pipeline_Packing_Bill_Num
 ,invheaders.Agreed_Place AS Pipeline_InvoiceHeaders_Agreed_Place
 ,invheaders.Broker_PGA_Contact_Email AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Email
 ,invheaders.Broker_PGA_Contact_Name AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone AS Pipeline_InvoiceHeaders_Broker_PGA_Contact_Phone
 ,invheaders.Consignee AS Pipeline_InvoiceHeaders_Consignee
 ,invheaders.Consignee_Address AS Pipeline_InvoiceHeaders_Consignee_Address
 ,invheaders.Curr AS Pipeline_InvoiceHeaders_Curr
 ,invheaders.EPA_PST_Cert_Date AS Pipeline_InvoiceHeaders_EPA_PST_Cert_Date
 ,invheaders.EPA_TSCA_Cert_Date AS Pipeline_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,invheaders.EPA_VNE_Cert_Date AS Pipeline_InvoiceHeaders_EPA_VNE_Cert_Date
 ,invheaders.Export AS Pipeline_InvoiceHeaders_Export
 ,invheaders.Export_Date AS Pipeline_InvoiceHeaders_Export_Date
 ,invheaders.First_Sale AS Pipeline_InvoiceHeaders_First_Sale
 ,invheaders.FSIS_Cert_Date AS Pipeline_InvoiceHeaders_FSIS_Cert_Date
 ,invheaders.FWS_Cert_Date AS Pipeline_InvoiceHeaders_FWS_Cert_Date
 ,invheaders.Importer AS Pipeline_InvoiceHeaders_Importer
 ,invheaders.INCO AS Pipeline_InvoiceHeaders_INCO
 ,invheaders.Inv_Date AS Pipeline_InvoiceHeaders_Inv_Date
 ,invheaders.Inv_Gross_Weight AS Pipeline_InvoiceHeaders_Inv_Gross_Weight
 ,invheaders.Invoice_No AS Pipeline_InvoiceHeaders_Invoice_No
 ,invheaders.Invoice_Total AS Pipeline_InvoiceHeaders_Invoice_Total
 ,invheaders.LACEY_ACT_Cert_Date AS Pipeline_InvoiceHeaders_LACEY_ACT_Cert_Date
 ,invheaders.Landed_Costing_Ex_Rate AS Pipeline_InvoiceHeaders_Landed_Costing_Ex_Rate
 ,invheaders.Manufacturer AS Pipeline_InvoiceHeaders_Manufacturer
 ,invheaders.Net_Weight AS Pipeline_InvoiceHeaders_Net_Weight
 ,invheaders.NHTSA_Cert_Date AS Pipeline_InvoiceHeaders_NHTSA_Cert_Date
 ,invheaders.Origin AS Pipeline_InvoiceHeaders_Origin
 ,invheaders.Packages AS Pipeline_InvoiceHeaders_Packages
 ,invheaders.Payment_Date AS Pipeline_InvoiceHeaders_Payment_Date
 ,invheaders.Seller AS Pipeline_InvoiceHeaders_Seller
 ,invheaders.Ship_to_party AS Pipeline_InvoiceHeaders_Ship_to_party
 ,invheaders.Sold_to_party AS Pipeline_InvoiceHeaders_Sold_to_party
 ,invheaders.Supplier AS Pipeline_InvoiceHeaders_Supplier
 ,invheaders.Supplier_Address AS Pipeline_InvoiceHeaders_Supplier_Address
 ,invheaders.Transaction_Related AS Pipeline_InvoiceHeaders_Transaction_Related
 ,invlines.Attribute_1 AS Pipeline_InvoiceLines_Attribute_1
 ,'API @ 60° F = ' + FORMAT(invlines.Attribute_2, '0.######') AS Pipeline_InvoiceLines_Attribute_2
 ,invlines.Certifying_Individual AS Pipeline_InvoiceLines_Certifying_Individual
 ,invlines.CIF_Component AS Pipeline_InvoiceLines_CIF_Component
 ,invlines.Code AS Pipeline_InvoiceLines_Code
 ,invlines.Consignee AS Pipeline_InvoiceLines_Consignee
 ,invlines.Curr AS Pipeline_InvoiceLines_Curr
 ,invlines.Customs_QTY AS Pipeline_InvoiceLines_Customs_QTY
 ,invlines.Dest_State AS Pipeline_InvoiceLines_Dest_State
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator AS Pipeline_InvoiceLines_EPA_TSCA_Toxic_Indicator
 ,invlines.Export AS Pipeline_InvoiceLines_Export
 ,invlines.Goods_Description AS Pipeline_InvoiceLines_Goods_Description
 ,invlines.Gr_Weight AS Pipeline_InvoiceLines_Gr_Weight
 ,invlines.Invoice_No AS Pipeline_InvoiceLines_Invoice_No
 ,invlines.Invoice_Qty AS Pipeline_InvoiceLines_Invoice_Qty
 ,invlines.Invoice_Qty_Unit AS Pipeline_InvoiceLines_Invoice_Qty_Unit
 ,invlines.Line_Price AS Pipeline_InvoiceLines_Line_Price
 ,invlines.invoice_line_number AS Pipeline_InvoiceLines_LNO
 ,invlines.Manufacturer AS Pipeline_InvoiceLines_Manufacturer
 ,invlines.ORG AS Pipeline_InvoiceLines_ORG
 ,invlines.Origin AS Pipeline_InvoiceLines_Origin
 ,invlines.PGA_Contact_Email AS Pipeline_InvoiceLines_PGA_Contact_Email
 ,invlines.PGA_Contact_Name AS Pipeline_InvoiceLines_PGA_Contact_Name
 ,invlines.PGA_Contact_Phone AS Pipeline_InvoiceLines_PGA_Contact_Phone
 ,invlines.PriceUnit AS Pipeline_InvoiceLines_PriceUnit
 ,invlines.Prod_ID_1 AS Pipeline_InvoiceLines_Prod_ID_1
 ,invlines.Sold_to_party AS Pipeline_InvoiceLines_Sold_To_Party
 ,invlines.SPI AS Pipeline_InvoiceLines_SPI
 ,invlines.Tariff AS Pipeline_InvoiceLines_Tariff
 ,invlines.Transaction_Related AS Pipeline_InvoiceLines_Transaction_Related
 ,invlines.TSCA_Indicator AS Pipeline_InvoiceLines_TSCA_Indicator
 ,invlines.UQ AS Pipeline_InvoiceLines_UQ
 ,invlines.Amount AS Pipeline_InvoiceLines_Amount
 ,misc.Bond_Type AS Pipeline_MISC_Bond_Type
 ,misc.Branch AS Pipeline_MISC_Branch
 ,misc.Broker AS Pipeline_MISC_Broker
 ,misc.Broker_to_Pay AS Pipeline_MISC_Broker_to_Pay
 ,misc.FTA_Recon AS Pipeline_MISC_FTA_Recon
 ,misc.Merge_By AS Pipeline_MISC_Merge_By
 ,misc.Payment_Type AS Pipeline_MISC_Payment_Type
 ,misc.Prelim_Statement_Date AS Pipeline_MISC_Prelim_Statement_Date
 ,misc.Preparer_Dist_Port AS Pipeline_MISC_Preparer_Dist_Port
 ,misc.Recon_Issue AS Pipeline_MISC_Recon_Issue
 ,misc.Submitter AS Pipeline_MISC_Submitter
 ,misc.Tax_Deferrable_Ind AS Pipeline_MISC_Tax_Deferrable_Ind
 ,declaration.Pipeline AS Pipeline_DeclarationTab_Pipeline
 ,invlines.Attribute_3 AS Pipeline_InvoiceLines_Attribute_3
 ,invheaders.Manufacturer_Address AS Pipeline_InvoiceHeaders_Manufacturer_Address
 ,declaration.EntryNumber AS Pipeline_DeclarationTab_EntryNumber
FROM dbo.Pipeline_Filing_Headers AS header
INNER JOIN dbo.Pipeline_Filing_Details AS details
  ON header.id = details.Filing_Headers_FK
LEFT OUTER JOIN dbo.Pipeline_DeclarationTab AS declaration
  ON declaration.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = declaration.PI_FK
LEFT OUTER JOIN dbo.Pipeline_ContainersTab AS containers
  ON containers.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = containers.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceLines AS invlines
  ON invlines.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = invlines.PI_FK
LEFT OUTER JOIN dbo.Pipeline_InvoiceHeaders AS invheaders
  ON invheaders.Filing_Headers_FK = header.id
    AND invheaders.id = invlines.InvoiceHeaders_FK
LEFT OUTER JOIN dbo.Pipeline_MISC AS misc
  ON misc.Filing_Headers_FK = header.id
    AND details.Pipeline_Inbounds_FK = misc.PI_FK
WHERE (header.MappingStatus = 2)
GO

CREATE VIEW dbo.v_Pipeline_DEFValues
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
FROM dbo.Pipeline_DEFValues v
INNER JOIN dbo.pipeline_sections sections
  ON v.section_id = sections.id
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(sections.table_name)
WHERE (UPPER(i.column_name)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

CREATE VIEW dbo.v_Pipeline_DEFValues_Manual
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
 ,v.FManual AS manual
 ,v.ModifiedDate AS modification_date
 ,v.ValueLabel AS label
 ,v.parent_record_id
 ,v.section_name
 ,v.section_title
 ,v.record_id
 ,v.description
 ,v.table_name
 ,v.column_name
 ,v.value
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.Pipeline_DEFValues_Manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_Pipeline_Filing_Data
AS
SELECT
  inbound.Id AS id
 ,headers.id AS Filing_header_id
 ,declaration.Importer AS Importer
 ,inbound.Batch AS Batch
 ,inbound.TicketNumber AS TicketNumber
 ,inbound.Facility
 ,inbound.SiteName
 ,inbound.Quantity AS Quantity
 ,inbound.API AS API
 ,inbound.ExportDate AS ExportDate
 ,inbound.ImportDate AS ImportDate
FROM Pipeline_Filing_Headers headers
LEFT OUTER JOIN dbo.Pipeline_DeclarationTab declaration
  ON declaration.FILING_HEADERS_FK = headers.id
INNER JOIN Pipeline_Inbound inbound
  ON inbound.Id = declaration.PI_FK
GO

CREATE VIEW dbo.v_Pipeline_Tables
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN pipeline_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(i.COLUMN_NAME) NOT IN ('ID', 'PI_FK', 'FILING_HEADERS_FK', 'PIPELINE_INBOUNDS_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

PRINT ('create stored procedures')
GO
-- Add Pipeline Import def values manual records --
CREATE PROCEDURE dbo.pipeline_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.Pipeline_DEFValues v
  INNER JOIN dbo.pipeline_sections s
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

  INSERT INTO dbo.Pipeline_DEFValues_Manual (
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
    FROM dbo.Pipeline_DEFValues dv
    INNER JOIN dbo.pipeline_sections s
      ON dv.section_id = s.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO
-- Apply Pipeline def values for specified table --
CREATE PROCEDURE dbo.pipeline_apply_def_values (@tableName VARCHAR(128)
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
  FROM dbo.Pipeline_DEFValues_Manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.FHasDefaultVal = 1
  AND v.record_id = @recordId

  EXEC (@str);
END
GO
-- Add Pipeline Declaration Tab record --
CREATE PROCEDURE dbo.pipeline_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_DeclarationTab (
        Filing_Headers_FK
       ,PI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Batch_Ticket
       ,Pipeline
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Dep
       ,Arr
       ,Arr_2
       ,Origin
       ,Destination
       ,Destination_State
       ,ETA
       ,Export_Date
       ,Description
       ,Owner_Ref
       ,FIRMs_Code
       ,Master_Bill
       ,Importer_of_record
       ,EntryNumber)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.Pipeline_Inbounds_FK
       ,ruleImporters.Supplier
       ,@ImporterCode
       ,ruleFacility.Issuer
       ,REPLACE(inbound.TicketNumber, '-', '')
       ,ruleFacility.Pipeline
       ,ruleFacility.Issuer
       ,ruleFacility.port
       ,ruleFacility.port
       ,inbound.ExportDate
       ,inbound.ImportDate
       ,inbound.ImportDate
       ,ruleFacility.Origin
       ,ruleFacility.Destination
       ,ruleFacility.Destination_State
       ,inbound.ImportDate
       ,inbound.ImportDate
       ,CONCAT(ruleFacility.pipeline, ' P/L', ': ', inbound.Batch)
       ,inbound.TicketNumber
       ,ruleFacility.FIRMs_Code
       ,REPLACE(inbound.TicketNumber, '-', '')
       ,@ImporterCode
       ,inbound.entry_number
      FROM Pipeline_Filing_Details details
      INNER JOIN Pipeline_Inbound inbound
        ON details.Pipeline_Inbounds_FK = inbound.id
      LEFT JOIN Pipeline_Rule_Facility ruleFacility
        ON inbound.facility = ruleFacility.facility
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
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
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO
-- Add Pipeline Container record --
CREATE PROCEDURE dbo.pipeline_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO Pipeline_ContainersTab (
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @filingHeaderId
       ,details.Pipeline_Inbounds_FK
       ,ROUND(inbound.Quantity, 0)
       ,REPLACE(inbound.TicketNumber, '-', '')
       ,rulefacility.Issuer
      FROM Pipeline_Filing_Details details
      INNER JOIN Pipeline_Inbound inbound
        ON details.Pipeline_Inbounds_FK = inbound.id
      LEFT JOIN Pipeline_Rule_Facility ruleFacility
        ON inbound.facility = ruleFacility.facility
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
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
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO
-- Add Pipeline Invoice Line record --
CREATE PROCEDURE dbo.pipeline_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  -- Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- get current tariff ---------------
  DECLARE @tariffs TABLE (
    pi INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (
      pi
     ,tariff)
    SELECT
      details.Pipeline_Inbounds_FK
     ,TariffId =
      CASE
        WHEN pi.API < 25 THEN 1
        WHEN pi.API >= 25 THEN 2
      END
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound pi
      ON details.Pipeline_Inbounds_FK = pi.id
    WHERE details.Filing_Headers_FK = @filingHeaderId

  -- add invoice line data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,tariff
       ,Customs_QTY
       ,Goods_Description
       ,spi
       ,Gr_Weight
       ,PriceUnit
       ,Attribute_1
       ,Attribute_2
       ,Attribute_3
       ,Invoice_Qty
       ,ORG
       ,Line_Price
       ,Amount
       ,Manufacturer
       ,Consignee
       ,Sold_to_party
       ,Origin
       ,Export
       ,Dest_State)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,details.Pipeline_Inbounds_FK
       ,inbound.Batch -- ?? is it ok?
       ,ruleImporters.Transaction_Related
       ,ruleAPI.Tariff
       ,inbound.Quantity
       ,CONCAT(ruleBatch.product, ' - ', ruleBatch.batch_code)
       ,ruleImporters.SPI
       ,CONVERT(DECIMAL(18, 3), dbo.fn_pipeline_weight(inbound.Quantity, inbound.API))
       ,COALESCE(rulePriceExact.pricing, rulePrice.pricing)
       ,inbound.Batch
       ,inbound.API
       ,CONCAT(ruleFacility.pipeline, ' P/L')
       ,inbound.Quantity
       ,ruleImporters.origin
       ,inbound.Quantity * COALESCE(rulePriceExact.pricing, rulePrice.pricing)
       ,inbound.Quantity * COALESCE(rulePriceExact.freight, rulePrice.freight)
       ,ruleImporters.manufacturer
       ,ruleImporters.Consignee
       ,ruleImporters.Consignee
       ,ruleImporters.origin
       ,ruleImporters.country_of_export
       ,ruleFacility.Destination_State
      FROM dbo.Pipeline_Filing_Details details
      INNER JOIN dbo.Pipeline_Inbound inbound
        ON inbound.id = details.Pipeline_Inbounds_FK
      LEFT JOIN Pipeline_Rule_Facility ruleFacility
        ON inbound.facility = ruleFacility.facility
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
      LEFT JOIN Clients clients
        ON clients.ClientCode = @ImporterCode
      LEFT JOIN Pipeline_Rule_Price rulePrice
        ON clients.id = rulePrice.importer_id
          AND rulePrice.crude_type_id IS NULL
      LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
        ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
      LEFT JOIN Pipeline_Rule_Price rulePriceExact
        ON clients.id = rulePriceExact.importer_id
          AND ruleBatch.id = rulePriceExact.crude_type_id
      LEFT JOIN @tariffs t
        ON inbound.id = t.pi
      LEFT JOIN Pipeline_Rule_API ruleAPI
        ON t.Tariff = ruleAPI.id
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
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
    RETURN @recordId
  END
END;
GO
-- Add Pipeline Invoice Header record --
CREATE PROCEDURE dbo.pipeline_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128);
  DECLARE @IDs TABLE (
    ID INT
  );

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules
  -- inbound.Quantity * ruleImporters.value - invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_InvoiceHeaders (
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Invoice_Total
       ,Origin
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Supplier_Address
       ,Export
       ,Manufacturer_Address)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,inbound.Batch
       ,ruleImporters.supplier
       ,inbound.Quantity * COALESCE(rulePriceExact.pricing, rulePrice.pricing)
       ,ruleImporters.Origin
       ,ruleImporters.Consignee
       ,ruleImporters.transaction_related
       ,ruleImporters.manufacturer
       ,ruleImporters.supplier
       ,@ImporterCode
       ,ruleImporters.Consignee
       ,ruleImporters.Consignee
       ,ruleImporters.seller
       ,ruleImporters.country_of_export
       ,ruleImporters.mid
      FROM dbo.Pipeline_Filing_Details details
      INNER JOIN dbo.Pipeline_Inbound inbound
        ON inbound.Id = details.Pipeline_Inbounds_FK
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
      LEFT JOIN Clients clients
        ON clients.ClientCode = @ImporterCode
      LEFT JOIN Pipeline_Rule_Price rulePrice
        ON clients.id = rulePrice.importer_id
          AND rulePrice.crude_type_id IS NULL
      LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
        ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
      LEFT JOIN Pipeline_Rule_Price rulePriceExact
        ON clients.id = rulePriceExact.importer_id
          AND ruleBatch.id = rulePriceExact.crude_type_id
      WHERE details.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.pipeline_add_invoice_line_record @filingHeaderId
                                             ,@recordId
                                             ,@filingUser

    -- fill the def value manual table
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO
-- Add Pipeline MISC record --
CREATE PROCEDURE dbo.pipeline_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @ImporterCode VARCHAR(128)
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Pipeline_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
  BEGIN

    INSERT INTO Pipeline_MISC (
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,recon_issue
       ,fta_recon)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.Pipeline_Inbounds_FK
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,ruleImporters.recon_issue
       ,ruleImporters.fta_recon
      FROM dbo.Pipeline_Filing_Details details
      INNER JOIN dbo.Pipeline_Inbound inbound
        ON inbound.id = details.Pipeline_Inbounds_FK
      LEFT JOIN Pipeline_Rule_Importer ruleImporters
        ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @filingUser
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
    EXEC dbo.pipeline_add_def_values_manual @tableName
                                           ,@filingHeaderId
                                           ,@parentId
                                           ,@recordId

    -- apply default values
    EXEC dbo.pipeline_apply_def_values @tableName
                                      ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO
--- update existing Pipeline filing procedure ---
CREATE PROCEDURE dbo.pipeline_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.pipeline_add_declaration_record @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
  -- add invoice header
  EXEC dbo.pipeline_add_invoice_header_record @filingHeaderId
                                             ,@filingHeaderId
                                             ,@filingUser
  -- add container
  EXEC dbo.pipeline_add_container_record @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
  -- add misc
  EXEC dbo.pipeline_add_misc_record @filingHeaderId
                                   ,@filingHeaderId
                                   ,@filingUser
END;
GO
--
CREATE PROCEDURE dbo.pipeline_filing_del (@Filing_Headers_id INT)
AS
BEGIN

  DELETE FROM dbo.Pipeline_DEFValues_Manual
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM dbo.Pipeline_Filing_Details
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM dbo.Pipeline_Filing_Headers
  WHERE id = @Filing_Headers_id

END;
GO
/****** Object:  StoredProcedure [dbo].[pipeline_inbound_del]    Script Date: 24.12.2018 ******/
CREATE PROCEDURE dbo.pipeline_inbound_del @Id [int],
@FDeleted [bit]
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.Filing_Headers_id
   ,@mapping_status = grid.Filing_Headers_MappingStatus
  FROM Pipeline_Inbound_Grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Pipeline_Inbound
    SET FDeleted = @FDeleted
    WHERE Id = @Id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Pipeline_Inbound
      SET FDeleted = @FDeleted
      WHERE Id IN (SELECT
          details.Pipeline_Inbounds_FK
        FROM Pipeline_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO
--
CREATE PROCEDURE dbo.pipeline_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.pipeline_sections ps
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
--
CREATE PROCEDURE dbo.pipeline_filing_param (@Filing_Headers_id INT)
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
  LEFT JOIN dbo.Pipeline_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.Filing_Headers_FK = @Filing_Headers_id
  AND v.FEditable = 1

  EXEC (@str);
END
GO
--- Pipeline post save action ---
CREATE PROCEDURE dbo.pipeline_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @importerCode VARCHAR(128) = dbo.fn_pipeline_GetImporterCode(@filingHeaderId);
  DECLARE @freight DECIMAL(18, 6)

  SELECT
    @freight = COALESCE(rulePriceExact.freight, rulePrice.freight)

  FROM dbo.Pipeline_Filing_Details details
  INNER JOIN dbo.Pipeline_Inbound inbound
    ON inbound.id = details.Pipeline_Inbounds_FK
  LEFT JOIN Clients clients
    ON clients.ClientCode = @ImporterCode
  LEFT JOIN Pipeline_Rule_Price rulePrice
    ON clients.id = rulePrice.importer_id
      AND rulePrice.crude_type_id IS NULL
  LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
    ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
  LEFT JOIN Pipeline_Rule_Price rulePriceExact
    ON clients.id = rulePriceExact.importer_id
      AND ruleBatch.id = rulePriceExact.crude_type_id
  WHERE details.Filing_Headers_FK = @filingHeaderId

  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
   ,api DECIMAL(18, 6)
  )

  INSERT INTO @tbl (
      record_id
     ,parent_record_id
     ,filing_header_id
     ,quantity
     ,unit_price
     ,api)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,a.Filing_Headers_FK
     ,CONVERT(DECIMAL(18, 6), a.value) AS Quantity
     ,CONVERT(DECIMAL(28, 15), b.value) AS UnitPrice
     ,CONVERT(DECIMAL(18, 6), c.value) AS api
    FROM Pipeline_DEFValues_Manual a
    JOIN Pipeline_DEFValues_Manual b
      ON a.record_id = b.record_id
    JOIN Pipeline_DEFValues_Manual c
      ON a.record_id = c.record_id
    WHERE a.Filing_Headers_FK = @filingHeaderId
    AND a.table_name = 'Pipeline_InvoiceLines'
    AND a.column_name = 'Invoice_Qty'
    AND b.column_name = 'PriceUnit'
    AND c.column_name = 'Attribute_2'

  DECLARE @total DECIMAL(28, 15)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  -- update invoice customs qty
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Customs_QTY'
  -- update invoice line price
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Line_Price'
  -- update invoice line amount
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * @freight, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Amount'
  -- update invoice line gross weight
  UPDATE defValues
  SET defValues.value = FORMAT(dbo.fn_pipeline_weight(t.Quantity, t.api), '0.###')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Gr_Weight'

  -- update invoice header invoice total
  UPDATE defValues
  SET value = FORMAT(@total, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl
    ON defValues.record_id = [@tbl].parent_record_id
  WHERE defValues.Filing_Headers_FK = @filingHeaderId
  AND table_name = 'Pipeline_InvoiceHeaders'
  AND column_name = 'Invoice_Total'
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

ALTER TABLE dbo.imp_pipeline_invoice_line
DROP COLUMN invoice_line_number;
DROP FUNCTION dbo.fn_imp_pipeline_invoice_line_number;
ALTER TABLE dbo.imp_pipeline_invoice_line
DROP COLUMN gr_weight_tons;
DROP FUNCTION dbo.fn_imp_pipeline_batch_code;
DROP FUNCTION dbo.fn_imp_pipeline_importer_code;
DROP FUNCTION dbo.fn_imp_pipeline_weight;
GO

PRINT ('drop tables')
GO

DROP TABLE dbo.imp_pipeline_declaration
DROP TABLE dbo.imp_pipeline_container
DROP TABLE dbo.imp_pipeline_misc
DROP TABLE dbo.imp_pipeline_document
DROP TABLE dbo.imp_pipeline_invoice_line
DROP TABLE dbo.imp_pipeline_invoice_header
DROP TABLE dbo.imp_pipeline_filing_detail
DROP TABLE dbo.imp_pipeline_filing_header
DROP TABLE dbo.imp_pipeline_inbound
DROP TABLE dbo.imp_pipeline_rule_api
DROP TABLE dbo.imp_pipeline_rule_consignee_importer
DROP TABLE dbo.imp_pipeline_rule_facility
DROP TABLE dbo.imp_pipeline_rule_price
DROP TABLE dbo.imp_pipeline_rule_batch_code
DROP TABLE dbo.imp_pipeline_rule_importer
DROP TABLE dbo.imp_pipeline_form_configuration
DROP TABLE dbo.imp_pipeline_form_section_configuration

PRINT ('drop views')
GO

DROP VIEW dbo.v_imp_pipeline_form_configuration
DROP VIEW dbo.v_imp_pipeline_field_configuration
DROP VIEW dbo.v_imp_pipeline_inbound_grid
DROP VIEW dbo.v_imp_pipeline_report
DROP VIEW dbo.v_imp_pipeline_review_grid


PRINT ('drop procedures')
GO

DROP PROCEDURE dbo.sp_imp_pipeline_create_entry_records
DROP PROCEDURE dbo.sp_imp_pipeline_add_declaration
DROP PROCEDURE dbo.sp_imp_pipeline_add_container
DROP PROCEDURE dbo.sp_imp_pipeline_add_misc
DROP PROCEDURE dbo.sp_imp_pipeline_add_invoice_header
DROP PROCEDURE dbo.sp_imp_pipeline_add_invoice_line
DROP PROCEDURE dbo.sp_imp_pipeline_review_entry
DROP PROCEDURE dbo.sp_imp_pipeline_delete_entry_records
DROP PROCEDURE dbo.sp_imp_pipeline_update_entry
DROP PROCEDURE dbo.sp_imp_pipeline_recalculate
DROP PROCEDURE dbo.sp_imp_pipeline_delete_inbound
