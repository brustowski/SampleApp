PRINT ('create tables')
GO

CREATE TABLE dbo.Truck_Inbound (
  Id INT IDENTITY
 ,Importer NVARCHAR(200) NOT NULL
 ,PAPs VARCHAR(20) NOT NULL
 ,CreatedDate DATETIME NOT NULL
 ,CreatedUser VARCHAR(128) NOT NULL
 ,FDeleted BIT NOT NULL DEFAULT (0)
 ,CONSTRAINT [PK_dbo.Truck_Inbound] PRIMARY KEY CLUSTERED (Id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Filing_Headers (
  id INT IDENTITY
 ,CreatedDate DATETIME NULL
 ,CreatedUser VARCHAR(128) NULL
 ,ErrorDescription VARCHAR(MAX) NULL
 ,FilingNumber VARCHAR(255) NULL
 ,FilingStatus TINYINT NULL
 ,MappingStatus TINYINT NULL
 ,job_link VARCHAR(1024) NULL
 ,RequestXML VARCHAR(MAX) NULL
 ,ResponseXML VARCHAR(MAX) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Filing_Details (
  Filing_Headers_FK INT NOT NULL
 ,BDP_FK INT NOT NULL
 ,CONSTRAINT [PK_dbo.Truck_Filing_Details] PRIMARY KEY CLUSTERED (Filing_Headers_FK, BDP_FK)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_DeclarationTab (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NOT NULL
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
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_ContainersTab (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NULL
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

CREATE TABLE dbo.Truck_InvoiceHeaders (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
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
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_InvoiceLines (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,InvoiceHeaders_FK INT NOT NULL
 ,TI_FK INT NULL
 ,Invoice_No VARCHAR(128) NULL
 ,Tariff VARCHAR(128) NULL
 ,Customs_QTY NUMERIC(18, 6) NULL
 ,Line_Price NUMERIC(18, 6) NULL
 ,Goods_Description VARCHAR(128) NULL
 ,ORG VARCHAR(128) NULL
 ,SPI VARCHAR(128) NULL
 ,Gr_Weight NUMERIC(18, 6) NULL
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
 ,Amount INT NULL
 ,Description VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,Gr_Weight_Unit VARCHAR(2) NULL
 ,PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_MISC (
  id INT IDENTITY
 ,Filing_Headers_FK INT NOT NULL
 ,TI_FK INT NULL
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

CREATE TABLE dbo.Truck_Documents (
  id INT IDENTITY
 ,CreatedDate DATETIME NULL
 ,CreatedUser VARCHAR(128) NULL
 ,document_type VARCHAR(128) NULL
 ,file_content VARBINARY(MAX) NULL
 ,file_desc VARCHAR(1000) NULL
 ,file_extension VARCHAR(128) NULL
 ,filename VARCHAR(255) NULL
 ,Filing_Headers_FK INT NULL
 ,Status VARCHAR(50) NULL
 ,inbound_record_id INT NULL
 ,CONSTRAINT [PK_dbo.Truck_Documents] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Rule_Importers (
  id INT IDENTITY
 ,arrival_port VARCHAR(128) NULL
 ,ce VARCHAR(128) NULL
 ,charges DECIMAL(18, 6) NULL
 ,co VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,custom_attrib1 VARCHAR(128) NULL
 ,custom_attrib2 VARCHAR(128) NULL
 ,custom_quantity DECIMAL(18, 6) NULL
 ,custom_uq VARCHAR(128) NULL
 ,cw_ior VARCHAR(128) NULL
 ,cw_supplier VARCHAR(128) NULL
 ,destination_state VARCHAR(128) NULL
 ,entry_port VARCHAR(128) NULL
 ,goods_description VARCHAR(128) NULL
 ,gross_weight DECIMAL(18, 6) NULL
 ,gross_weight_uq VARCHAR(128) NULL
 ,invoice_qty DECIMAL(18, 6) NULL
 ,invoice_uq VARCHAR(128) NULL
 ,ior VARCHAR(128) NOT NULL
 ,line_price DECIMAL(18, 6) NULL
 ,manufacturer_mid VARCHAR(128) NULL
 ,nafta_recon VARCHAR(128) NULL
 ,product_id VARCHAR(128) NULL
 ,recon_issue VARCHAR(128) NULL
 ,spi VARCHAR(128) NULL
 ,supplier_mid VARCHAR(128) NULL
 ,tariff VARCHAR(128) NULL
 ,transactions_related VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_Rule_Importers] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.Truck_Rule_Ports (
  id INT IDENTITY
 ,entry_port VARCHAR(128) NOT NULL
 ,arrival_port VARCHAR(128) NULL
 ,firms_code VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL
 ,created_user VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_Rule_Ports] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_sections (
  id INT IDENTITY
 ,name VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NULL
 ,is_array BIT NOT NULL
 ,parent_id INT NULL
 ,procedure_name VARCHAR(128) NULL
 ,CONSTRAINT PK_truck__sections_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_def_values (
  id INT IDENTITY
 ,label VARCHAR(128) NOT NULL
 ,default_value VARCHAR(512) NULL
 ,description VARCHAR(128) NULL
 ,section_id INT NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,editable BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,display_on_ui TINYINT NOT NULL
 ,manual TINYINT NOT NULL
 ,single_filing_order TINYINT NULL
 ,created_date DATETIME NOT NULL CONSTRAINT DF_truck_def_values__created_date DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL
 ,CONSTRAINT PK_truck_def_values PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE TABLE dbo.truck_def_values_manual (
  id INT IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,record_id INT NOT NULL
 ,section_name VARCHAR(128) NOT NULL
 ,section_title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,label VARCHAR(128) NOT NULL
 ,value VARCHAR(512) NULL
 ,description VARCHAR(128) NULL
 ,editable BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,display_on_ui TINYINT NOT NULL
 ,manual TINYINT NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,modification_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,CONSTRAINT PK_truck_def_values_manual PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

PRINT ('copy data')
GO
PRINT ('copy inbound data')
GO

SET IDENTITY_INSERT dbo.Truck_Inbound ON;
INSERT INTO dbo.Truck_Inbound (
    Id
   ,Importer
   ,PAPs
   ,FDeleted
   ,CreatedDate
   ,CreatedUser)
  SELECT
    ti.id
   ,ti.importer
   ,ti.paps
   ,ti.deleted
   ,ti.created_date
   ,ti.created_user
  FROM dbo.imp_truck_inbound ti;
SET IDENTITY_INSERT dbo.Truck_Inbound OFF;
GO

PRINT ('copy filing header data')
GO

SET IDENTITY_INSERT dbo.Truck_Filing_Headers ON;
INSERT INTO dbo.Truck_Filing_Headers (
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
  FROM dbo.imp_truck_filing_header fh;
SET IDENTITY_INSERT dbo.Truck_Filing_Headers OFF;
GO

PRINT ('copy filing details data')
GO
INSERT INTO dbo.Truck_Filing_Details (
    BDP_FK
   ,Filing_Headers_FK)
  SELECT
    fd.inbound_id
   ,fd.filing_header_id
  FROM dbo.imp_truck_filing_detail fd;
GO

PRINT ('copy declaration data')
GO
SET IDENTITY_INSERT dbo.Truck_DeclarationTab ON;
INSERT INTO dbo.Truck_DeclarationTab (
    id
   ,Filing_Headers_FK
   ,TI_FK
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
    declaration.id
   ,declaration.filing_header_id
   ,itfd.inbound_id
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
   ,declaration.created_date
   ,declaration.created_user
  FROM dbo.imp_truck_declaration declaration
  INNER JOIN imp_truck_filing_header itfh
    ON declaration.filing_header_id = itfh.id
  LEFT JOIN imp_truck_filing_detail itfd
    ON itfd.filing_header_id = itfh.id;
SET IDENTITY_INSERT dbo.Truck_DeclarationTab OFF;
GO


PRINT ('copy container data')
GO
SET IDENTITY_INSERT dbo.Truck_ContainersTab ON;
INSERT INTO dbo.Truck_ContainersTab (
    id
   ,Filing_Headers_FK
   ,TI_FK
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
   ,itfd.inbound_id
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
  FROM dbo.imp_truck_container container
  INNER JOIN imp_truck_filing_header itfh
    ON container.filing_header_id = itfh.id
  LEFT JOIN imp_truck_filing_detail itfd
    ON itfd.filing_header_id = itfh.id;
SET IDENTITY_INSERT dbo.Truck_ContainersTab OFF;
GO

PRINT ('copy invoice header data')
GO
SET IDENTITY_INSERT dbo.Truck_InvoiceHeaders ON;
INSERT INTO dbo.Truck_InvoiceHeaders (
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
    invoice.id
   ,invoice.filing_header_id
   ,invoice.invoice_no
   ,invoice.supplier
   ,invoice.supplier_address
   ,invoice.inco
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
   ,invoice.created_date
   ,invoice.created_user
  FROM dbo.imp_truck_invoice_header invoice;
SET IDENTITY_INSERT dbo.Truck_InvoiceHeaders OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.Truck_InvoiceLines ON;
INSERT INTO dbo.Truck_InvoiceLines (
    id
   ,Filing_Headers_FK
   ,InvoiceHeaders_FK
   ,TI_FK
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
   ,itfd.inbound_id
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
  FROM dbo.imp_truck_invoice_line line
  INNER JOIN imp_truck_filing_header itfh
    ON line.filing_header_id = itfh.id
  LEFT JOIN imp_truck_filing_detail itfd
    ON itfd.filing_header_id = itfh.id;
SET IDENTITY_INSERT dbo.Truck_InvoiceLines OFF;
GO

PRINT ('copy invoice lines data')
GO
SET IDENTITY_INSERT dbo.Truck_MISC ON;
INSERT INTO dbo.Truck_MISC (
    id
   ,Filing_Headers_FK
   ,TI_FK
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
   ,itfd.inbound_id
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
  FROM dbo.imp_truck_misc misc
  INNER JOIN imp_truck_filing_header itfh
    ON misc.filing_header_id = itfh.id
  LEFT JOIN imp_truck_filing_detail itfd
    ON itfd.filing_header_id = itfh.id;
SET IDENTITY_INSERT dbo.Truck_MISC OFF;
GO

PRINT ('copy documents data')
GO
SET IDENTITY_INSERT dbo.Truck_Documents ON;
INSERT INTO dbo.Truck_Documents (
    id
   ,Filing_Headers_FK
   ,inbound_record_id
   ,[filename]
   ,file_extension
   ,file_desc
   ,file_content
   ,document_type
   ,[Status]
   ,CreatedDate
   ,CreatedUser)
  SELECT
    doc.id
   ,doc.filing_header_id
   ,doc.inbound_record_id
   ,doc.[file_name]
   ,doc.file_extension
   ,doc.file_description
   ,doc.file_content
   ,doc.document_type
   ,[doc].[status]
   ,doc.created_date
   ,doc.created_user
  FROM dbo.imp_truck_document doc;
SET IDENTITY_INSERT dbo.Truck_Documents OFF;
GO

PRINT ('copy port rule data')
GO
SET IDENTITY_INSERT dbo.Truck_Rule_Ports ON;
INSERT INTO dbo.Truck_Rule_Ports (
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
  FROM dbo.imp_truck_rule_port r;
SET IDENTITY_INSERT dbo.Truck_Rule_Ports OFF;
GO

PRINT ('copy importer rule data')
GO
SET IDENTITY_INSERT dbo.Truck_Rule_Importers ON;
INSERT INTO dbo.Truck_Rule_Importers (
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
  FROM dbo.imp_truck_rule_importer r;
SET IDENTITY_INSERT dbo.Truck_Rule_Importers OFF;
GO

PRINT ('copy sections data')
GO
SET IDENTITY_INSERT dbo.truck_sections ON;
INSERT INTO dbo.truck_sections (
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
  FROM dbo.imp_truck_form_section_configuration section;
SET IDENTITY_INSERT dbo.truck_sections OFF;
GO

UPDATE truck_sections
SET table_name = 'Truck_DeclarationTab'
   ,procedure_name = 'truck_add_declaration_record'
WHERE table_name = 'imp_truck_declaration';
UPDATE imp_truck_form_section_configuration
SET table_name = 'Truck_InvoiceHeaders'
   ,procedure_name = 'truck_add_invoice_header_record'
WHERE table_name = 'imp_truck_invoice_header';
UPDATE imp_truck_form_section_configuration
SET table_name = 'Truck_InvoiceLines'
   ,procedure_name = 'truck_add_invoice_line_record'
WHERE table_name = 'imp_truck_invoice_line';
UPDATE imp_truck_form_section_configuration
SET table_name = 'Truck_ContainersTab'
   ,procedure_name = 'truck_add_container_record'
WHERE table_name = 'imp_truck_container';
UPDATE imp_truck_form_section_configuration
SET table_name = 'Truck_MISC'
   ,procedure_name = 'truck_add_misc_record'
WHERE table_name = 'imp_truck_misc';
GO

PRINT ('copy def-values data')
GO
SET IDENTITY_INSERT dbo.truck_def_values ON;
INSERT INTO dbo.truck_def_values (
    id
   ,section_id
   ,column_name
   ,[label]
   ,[description]
   ,default_value
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
  FROM dbo.imp_truck_form_configuration dv;
SET IDENTITY_INSERT dbo.truck_def_values OFF;
GO

UPDATE dbo.imp_truck_form_configuration
SET column_name =
CASE
  WHEN column_name = 'arr2' AND
    section_id = 2 THEN 'Arr_2'
  WHEN column_name = 'attribute1' AND
    section_id = 5 THEN 'Attribute_1'
  WHEN column_name = 'attribute2' AND
    section_id = 5 THEN 'Attribute_2'
  WHEN column_name = 'prod_id1' AND
    section_id = 5 THEN 'Prod_ID_1'
  WHEN column_name = 'price_unit' AND
    section_id = 5 THEN 'PriceUnit'
  ELSE column_name
END;
GO

PRINT ('add constraints')
GO

ALTER TABLE dbo.Truck_Filing_Details
ADD CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_Filing_Details
ADD CONSTRAINT [FK_dbo.Truck_Filing_Details_dbo.Truck_Inbound_BDP_FK] FOREIGN KEY (BDP_FK) REFERENCES dbo.Truck_Inbound (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT [FK_dbo.Truck_InvoiceLines.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT [FK_dbo.Truck_InvoiceLines.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT FK_Truck_InvoiceLines_Truck_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Truck_InvoiceHeaders (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Inbound_TI_FK] FOREIGN KEY (TI_FK) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.Truck_Documents
ADD CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Truck_Documents
ADD CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Inbound_inbound_record_id] FOREIGN KEY (inbound_record_id) REFERENCES dbo.Truck_Inbound (Id)
GO

ALTER TABLE dbo.truck_sections
ADD CONSTRAINT FK_truck_sections__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.truck_sections (id)
GO

ALTER TABLE dbo.truck_def_values
ADD CONSTRAINT FK_truck_def_values__truck_sections__section_id FOREIGN KEY (section_id) REFERENCES dbo.truck_sections (id)
GO

PRINT ('add indexes')
GO

CREATE INDEX IX_BDP_FK
ON dbo.Truck_Filing_Details (BDP_FK)
ON [PRIMARY]
GO

CREATE INDEX IX_Filing_Headers_FK
ON dbo.Truck_Filing_Details (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckDeclarationTab_filingHeadersFK
ON dbo.Truck_DeclarationTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckContainersTab_filingHeadersFK
ON dbo.Truck_ContainersTab (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckInvoiceHeaders_filingHeadersFK
ON dbo.Truck_InvoiceHeaders (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckInvoiceLines_invoiceHeadersFK
ON dbo.Truck_InvoiceLines (InvoiceHeaders_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckMISC_filingHeadersFK
ON dbo.Truck_MISC (Filing_Headers_FK)
INCLUDE (id)
ON [PRIMARY]
GO

CREATE INDEX IX_Filing_Headers_FK
ON dbo.Truck_Documents (Filing_Headers_FK)
ON [PRIMARY]
GO

CREATE INDEX IX_inbound_record_id
ON dbo.Truck_Documents (inbound_record_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.truck_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.truck_sections (parent_id)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.truck_def_values (section_id)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX UK_truck_def_values__single_filing_order
ON dbo.truck_def_values (single_filing_order)
WHERE ([single_filing_order] IS NOT NULL)
ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeaderId
ON dbo.truck_def_values_manual (filing_header_id)
ON [PRIMARY]
GO

CREATE INDEX Idx_TruckDefValuesManual_recordId_tableName_columnName
ON dbo.truck_def_values_manual (record_id, table_name, column_name)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_ior
ON dbo.Truck_Rule_Importers (ior)
ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_entry_port
ON dbo.Truck_Rule_Ports (entry_port)
ON [PRIMARY]
GO

PRINT ('create functions and computed columns')
GO
-- gets truck export invoice header number
CREATE FUNCTION dbo.truck_invoice_line_number (@invoiceHeaderId INT
, @recordId INT)
RETURNS INT WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result INT = NULL;
  SELECT
    @result = tmp.RowNum
  FROM (SELECT
      til.id
     ,RowNum = ROW_NUMBER() OVER (ORDER BY til.id)
    FROM dbo.Truck_InvoiceLines til
    WHERE til.InvoiceHeaders_FK = @invoiceHeaderId) AS tmp
  WHERE tmp.id = @recordId
  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.Truck_InvoiceLines
ADD invoice_line_number AS ([dbo].[truck_invoice_line_number]([InvoiceHeaders_FK], [id]));
ALTER TABLE dbo.Truck_InvoiceLines
ADD Gr_Weight_Tons AS ([dbo].[weightToTon]([Gr_Weight], [Gr_Weight_Unit]));
GO
-- gets truck import invoice total
CREATE FUNCTION dbo.truck_invoice_total (@invoiceHeaderId INT)
RETURNS NUMERIC(18, 6) WITH SCHEMABINDING
AS
BEGIN
  DECLARE @result NUMERIC(18, 6) = NULL;

  SELECT
    @result = SUM(til.Invoice_Qty * til.PriceUnit)
  FROM dbo.Truck_InvoiceLines til
  WHERE til.InvoiceHeaders_FK = @invoiceHeaderId
  GROUP BY til.InvoiceHeaders_FK

  RETURN @result
END
GO
-- alter truck export invoice header table
ALTER TABLE dbo.Truck_InvoiceHeaders
ADD Invoice_Total AS ([dbo].[truck_invoice_total]([id]));
GO

PRINT ('create triggers')
GO
CREATE TRIGGER dbo.truck_declaration_tab_befor_delete
ON dbo.Truck_DeclarationTab
FOR DELETE
AS
  DELETE FROM dbo.truck_def_values_manual
  WHERE table_name = 'Truck_DeclarationTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.truck_containers_tab_befor_delete
ON dbo.Truck_ContainersTab
FOR DELETE
AS
  DELETE FROM dbo.truck_def_values_manual
  WHERE table_name = 'Truck_ContainersTab'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.truck_invoice_headers_befor_delete
ON dbo.Truck_InvoiceHeaders
FOR DELETE
AS
  DELETE FROM dbo.truck_def_values_manual
  WHERE table_name = 'Truck_InvoiceHeaders'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.truck_invoice_lines_befor_delete
ON dbo.Truck_InvoiceLines
FOR DELETE
AS
  DELETE FROM dbo.truck_def_values_manual
  WHERE table_name = 'Truck_InvoiceLines'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO
CREATE TRIGGER dbo.truck_misc_befor_delete
ON dbo.Truck_MISC
FOR DELETE
AS
  DELETE FROM dbo.truck_def_values_manual
  WHERE table_name = 'Truck_MISC'
    AND record_id IN (SELECT
        deleted.id
      FROM deleted)
GO

PRINT ('create views')
GO
CREATE VIEW dbo.Truck_Inbound_Grid
AS
SELECT
  ti.Id AS ID
 ,tfh.id AS Filing_Headers_Id
 ,ti.Importer AS BaseImporter
 ,tri.cw_ior AS Importer
 ,ti.PAPs
 ,'' AS FilingNumber
 ,'' AS job_link
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,ti.FDeleted
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Truck_Rule_Importers rule_importers
        WHERE RTRIM(LTRIM(rule_importers.ior)) = RTRIM(LTRIM(ti.Importer))) THEN 1
    ELSE 0
  END AS has_importer_rule
 ,CASE
    WHEN EXISTS (SELECT
          1
        FROM Truck_Rule_Importers rule_importers
        INNER JOIN Truck_Rule_Ports rulePorts
          ON RTRIM(LTRIM(rule_importers.entry_port)) = RTRIM(LTRIM(rulePorts.entry_port))
            AND RTRIM(LTRIM(rule_importers.arrival_port)) = RTRIM(LTRIM(rulePorts.arrival_port))
        WHERE RTRIM(LTRIM(rule_importers.ior)) = RTRIM(LTRIM(ti.Importer))) THEN 1
    ELSE 0
  END AS has_port_rule
FROM dbo.Truck_Inbound ti
LEFT JOIN dbo.Truck_Rule_Importers tri
  ON ti.Importer = tri.ior
LEFT JOIN dbo.Truck_Filing_Details tfd
  ON tfd.BDP_FK = ti.Id
LEFT JOIN dbo.Truck_Filing_Headers tfh
  ON tfh.id = tfd.Filing_Headers_FK
    AND tfh.MappingStatus <> 0
WHERE NOT EXISTS (SELECT
    1
  FROM dbo.Truck_Filing_Headers tfh
  INNER JOIN dbo.Truck_Filing_Details tfd
    ON tfh.id = tfd.Filing_Headers_FK
  WHERE tfh.MappingStatus > 0
  AND ti.Id = tfd.BDP_FK)
AND ti.FDeleted = 0

UNION

SELECT
  ti.Id AS ID
 ,tfh.id AS Filing_Headers_Id
 ,ti.Importer AS BaseImporter
 ,declaration.Importer AS Importer
 ,ti.PAPs
 ,tfh.FilingNumber AS FilingNumber
 ,tfh.job_link
 ,ti.CreatedDate
 ,ISNULL(tfh.MappingStatus, 0) AS Filing_Headers_MappingStatus
 ,ISNULL(tfh.FilingStatus, 0) AS Filing_Headers_FilingStatus
 ,ti.FDeleted
 ,1 AS has_importer_rule
 ,1 AS has_port_rule
FROM dbo.Truck_Filing_Headers tfh
INNER JOIN dbo.Truck_Filing_Details tfd
  ON tfh.id = tfd.Filing_Headers_FK
INNER JOIN dbo.Truck_Inbound ti
  ON tfd.BDP_FK = ti.Id
LEFT JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = tfh.id
    AND tfd.BDP_FK = declaration.TI_FK
WHERE tfh.MappingStatus > 0
AND ti.FDeleted = 0
GO

CREATE VIEW dbo.Truck_Report
AS
SELECT
  headers.id
 ,declaration.Main_Supplier
 ,declaration.Importer
 ,declaration.Shipment_Type
 ,declaration.Transport
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
 ,containers.Bill_Type
 ,containers.Bill_Num
 ,containers.Bill_Number
 ,containers.UQ AS Containers_UQ
 ,containers.Manifest_QTY
 ,containers.Packing_UQ
 ,containers.Bill_Issuer_SCAC
 ,invheaders.Invoice_No
 ,invheaders.Consignee_Address
 ,invheaders.Invoice_Total
 ,invheaders.Curr
 ,invheaders.Payment_Date
 ,invheaders.Consignee
 ,invheaders.Inv_Date
 ,invheaders.Agreed_Place
 ,invheaders.Inv_Gross_Weight
 ,invheaders.Net_Weight
 ,invheaders.Manufacturer
 ,invheaders.Seller
 ,invheaders.Sold_to_party
 ,invheaders.Ship_to_party
 ,invheaders.Broker_PGA_Contact_Name
 ,invheaders.Broker_PGA_Contact_Phone
 ,invheaders.Broker_PGA_Contact_Email
 ,invlines.invoice_line_number AS LNO
 ,invlines.Tariff
 ,invlines.Customs_QTY
 ,invlines.Line_Price
 ,invlines.Goods_Description
 ,invlines.ORG
 ,invlines.SPI
 ,invlines.Gr_Weight
 ,invlines.Gr_Weight_Unit
 ,invlines.Gr_Weight_Tons
 ,invlines.UQ
 ,invlines.PriceUnit
 ,invlines.Prod_ID_1
 ,invlines.Attribute_1
 ,invlines.Attribute_2
 ,invlines.Export
 ,invlines.Invoice_Qty
 ,invlines.Invoice_Qty_Unit
 ,invlines.Code
 ,invlines.Amount
 ,invlines.CIF_Component
 ,invlines.EPA_TSCA_Toxic_Substance_Control_Act_Indicator
 ,invlines.TSCA_Indicator
 ,invlines.Certifying_Individual
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
 ,invlines.Manufacturer AS Truck_InvoiceLines_Manufacturer
 ,invheaders.Transaction_Related AS Truck_InvoiceHeaders_Transaction_Related
 ,invlines.Transaction_Related AS Truck_InvoiceLines_Transaction_Related
 ,invheaders.EPA_TSCA_Cert_Date AS Truck_InvoiceHeaders_EPA_TSCA_Cert_Date
 ,declaration.Container AS Truck_DeclarationTab_Container
 ,invheaders.Supplier AS Truck_InvoiceHeaders_Supplier
 ,invheaders.Origin AS Truck_InvoiceHeaders_Origin
FROM dbo.Truck_Filing_Headers headers
LEFT OUTER JOIN dbo.Truck_ContainersTab containers
  ON containers.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_DeclarationTab declaration
  ON declaration.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceHeaders invheaders
  ON invheaders.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_InvoiceLines invlines
  ON invlines.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Truck_MISC misc
  ON misc.Filing_Headers_FK = headers.id
WHERE headers.MappingStatus = 2
GO

CREATE VIEW dbo.v_truck_def_values
AS
SELECT
  tdv.id
 ,tdv.column_name
 ,tdv.created_date
 ,tdv.created_user
 ,tdv.default_value
 ,tdv.display_on_ui
 ,tdv.editable
 ,tdv.has_default_value
 ,tdv.mandatory
 ,tdv.[manual]
 ,tdv.[description]
 ,tdv.label
 ,tdv.single_filing_order
 ,tdv.paired_field_table
 ,tdv.paired_field_column
 ,tdv.handbook_name
 ,ts.table_name
 ,ts.[name] AS section_name
 ,ts.title AS section_title
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.truck_def_values tdv
INNER JOIN dbo.truck_sections ts
  ON tdv.section_id = ts.id
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(tdv.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(ts.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_truck_def_values_manual
AS
SELECT
  tdvm.id
 ,tdvm.filing_header_id
 ,tdvm.parent_record_id
 ,tdvm.record_id
 ,tdvm.section_name
 ,tdvm.section_title
 ,tdvm.table_name
 ,tdvm.column_name
 ,tdvm.label
 ,tdvm.[value]
 ,tdvm.description
 ,tdvm.editable
 ,tdvm.has_default_value
 ,tdvm.mandatory
 ,tdvm.paired_field_table
 ,tdvm.paired_field_column
 ,tdvm.handbook_name
 ,tdvm.display_on_ui
 ,tdvm.manual
 ,tdvm.modification_date
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.truck_def_values_manual tdvm
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(tdvm.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(tdvm.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

CREATE VIEW dbo.v_Truck_Filing_Data
AS
SELECT
  ti.Id AS id
 ,tfh.id AS Filing_header_id
 ,tdt.Importer AS Importer
 ,ti.PAPs
FROM dbo.Truck_Filing_Headers tfh
LEFT OUTER JOIN dbo.Truck_DeclarationTab tdt
  ON tdt.FILING_HEADERS_FK = tfh.id
INNER JOIN Truck_Inbound ti
  ON ti.Id = tdt.TI_FK
GO

CREATE VIEW dbo.v_truck_tables
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,i.TABLE_NAME AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns i
INNER JOIN truck_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'TI_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

PRINT ('create stored procedures')
GO
-- Add Truck Import def values manual records --
CREATE PROCEDURE dbo.truck_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.truck_def_values v
  INNER JOIN dbo.truck_sections s
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

  INSERT INTO dbo.truck_def_values_manual (
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
     ,ts.name
     ,ts.title
     ,@recordId
     ,dv.column_name
     ,ts.table_name
     ,GETDATE()
     ,@defValueOut
     ,dv.editable
     ,dv.display_on_ui
     ,dv.has_default_value
     ,dv.mandatory
     ,dv.manual
     ,dv.description
     ,dv.label
     ,handbook_name
     ,paired_field_table
     ,paired_field_column
    FROM dbo.truck_def_values dv
    INNER JOIN dbo.truck_sections ts
      ON dv.section_id = ts.id
    WHERE dv.id = @id

  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO
--- Truck apply dafault values procedure for specified table---
CREATE PROCEDURE dbo.truck_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.[value] IS NULL THEN 'NULL '
      ELSE 'try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
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
  FROM dbo.truck_def_values_manual v
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
-- add truck declaration tab record --
CREATE PROCEDURE dbo.truck_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add declarationTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_DeclarationTab declarationTab
      WHERE declarationTab.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_DeclarationTab (
        Filing_Headers_FK
       ,TI_FK
       ,Main_Supplier
       ,Importer
       ,Issuer
       ,Master_Bill
       ,Carrier_SCAC
       ,Discharge
       ,Entry_Port
       ,Destination_State
       ,Description
       ,FIRMs_Code)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK AS TI_FK
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs))
       ,SUBSTRING(inbound.PAPs, 1, 4)
       ,ruleImporters.arrival_port
       ,ruleImporters.Entry_Port
       ,ruleImporters.Destination_State
       ,ruleImporters.Goods_Description
       ,rulePorts.FIRMs_Code
      FROM Truck_Filing_Details details
      INNER JOIN Truck_Inbound inbound
        ON details.BDP_FK = inbound.id
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      LEFT JOIN Truck_Rule_Ports rulePorts
        ON (RTRIM(LTRIM(ruleImporters.arrival_port)) = RTRIM(LTRIM(rulePorts.arrival_port))
            AND RTRIM(LTRIM(ruleImporters.Entry_Port)) = RTRIM(LTRIM(rulePorts.Entry_Port)))
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
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck containers tab record --
CREATE PROCEDURE dbo.truck_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add сontainersTab data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_ContainersTab сontainersTab
      WHERE сontainersTab.Filing_Headers_FK = @parentId)

  BEGIN
    INSERT INTO dbo.Truck_ContainersTab (
        Filing_Headers_FK
       ,TI_FK)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK
      FROM Truck_Filing_Details details
      INNER JOIN Truck_Inbound inbound
        ON details.BDP_FK = inbound.id
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
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck invoice line record --
CREATE PROCEDURE dbo.truck_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add Invoice Lines data and apply rules  
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_InvoiceLines invoiceLines
      WHERE invoiceLines.Filing_Headers_FK = @parentId)

  BEGIN
    INSERT INTO dbo.Truck_InvoiceLines (
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,TI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,ORG
       ,Export
       ,Gr_Weight
       ,Gr_Weight_Unit
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,details.BDP_FK
       ,inbound.PAPs
       ,ruleImporters.transactions_related
       ,ruleImporters.Tariff
       ,ruleImporters.custom_quantity
       ,ruleImporters.Goods_Description
       ,ruleImporters.SPI
       ,ruleImporters.co
       ,ruleimporters.ce
       ,ruleImporters.gross_weight
       ,ruleImporters.gross_weight_uq
       ,ruleImporters.custom_uq
       ,ruleImporters.Line_Price
       ,ruleImporters.product_id
       ,ruleImporters.custom_attrib1
       ,ruleImporters.custom_attrib2
       ,ruleImporters.Invoice_Qty
       ,ruleImporters.invoice_uq
       ,ruleImporters.charges
       ,ruleImporters.invoice_qty * ruleImporters.line_price
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
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
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
-- add truck invoice header record --
CREATE PROCEDURE dbo.truck_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add invoiceHeaders data and apply rules
  -- invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_InvoiceHeaders invoiceHeaders
      WHERE invoiceHeaders.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_InvoiceHeaders (
        Filing_Headers_FK
       ,Invoice_No
       ,Supplier
       ,Consignee
       ,Transaction_Related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_to_party
       ,Ship_to_party
       ,Export
       ,Origin)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,inbound.PAPs
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,ruleImporters.transactions_related
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_supplier
       ,ruleImporters.cw_ior
       ,ruleImporters.cw_ior
       ,ruleImporters.cw_ior
       ,ruleimporters.ce
       ,ruleImporters.co
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
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
    EXEC dbo.truck_add_invoice_line_record @filingHeaderId
                                          ,@recordId
                                          ,@filingUser

    -- fill the def value manual table
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END
GO
-- add truck misc record --
CREATE PROCEDURE dbo.truck_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add misc data and apply rules
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM dbo.Truck_MISC misc
      WHERE misc.Filing_Headers_FK = @parentId)
  BEGIN
    INSERT INTO dbo.Truck_MISC (
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,Recon_Issue
       ,FTA_Recon)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,details.BDP_FK
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
       ,ruleImporters.Recon_Issue
       ,ruleImporters.nafta_recon
      FROM dbo.Truck_Filing_Details details
      INNER JOIN dbo.Truck_Inbound inbound
        ON inbound.id = details.BDP_FK
      LEFT JOIN Truck_Rule_Importers ruleImporters
        ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = @FilingUser
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
    EXEC dbo.truck_add_def_values_manual @tableName
                                        ,@filingHeaderId
                                        ,@parentId
                                        ,@recordId

    -- apply default values
    EXEC dbo.truck_apply_def_values @tableName
                                   ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END
END
GO
--- update existing Truck Filing procedure ---
CREATE PROCEDURE dbo.truck_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  -- add declaration
  EXEC dbo.truck_add_declaration_record @filingHeaderId
                                       ,@filingHeaderId
                                       ,@filingUser
  -- add invoice header
  EXEC dbo.truck_add_invoice_header_record @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
  -- add container
  EXEC dbo.truck_add_container_record @filingHeaderId
                                     ,@filingHeaderId
                                     ,@filingUser
  -- add misc
  EXEC dbo.truck_add_misc_record @filingHeaderId
                                ,@filingHeaderId
                                ,@filingUser
END;
GO
CREATE PROCEDURE dbo.truck_filing_del (@Filing_Headers_id INT)
AS
BEGIN

  DELETE FROM dbo.truck_def_values_manual
  WHERE filing_header_id = @Filing_Headers_id

  DELETE FROM dbo.Truck_Filing_Details
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM dbo.Truck_Filing_Headers
  WHERE id = @Filing_Headers_id

END;
GO
/****** Object:  StoredProcedure [dbo].[truck_inbound_del]    Script Date: 24.12.2018 ******/
CREATE PROCEDURE dbo.truck_inbound_del (@id INT,
@FDeleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.Filing_Headers_id
   ,@mapping_status = grid.Filing_Headers_MappingStatus
  FROM Truck_Inbound_Grid grid
  WHERE grid.Id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Truck_Inbound
    SET FDeleted = @FDeleted
    WHERE Id = @Id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Truck_Inbound
      SET FDeleted = @FDeleted
      WHERE Id IN (SELECT
          details.BDP_FK
        FROM Truck_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO
CREATE PROCEDURE dbo.truck_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.truck_sections tes
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
--- Truck apply values to result table---
CREATE PROCEDURE dbo.truck_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = ' +
    CASE
      WHEN v.[value] IS NULL THEN 'NULL '
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
  LEFT JOIN dbo.truck_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id
  AND v.editable = 1

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
 ,truck_doc.[filename] AS filename
 ,truck_doc.file_extension AS file_extension
 ,truck_doc.file_content AS file_Content
 ,truck_doc.file_desc AS file_desc
 ,truck_doc.document_type AS document_type
 ,'Truck_Imp' AS transport_shipment_type
FROM dbo.Truck_Documents truck_doc
INNER JOIN dbo.Truck_Filing_Headers truck_header
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

ALTER TABLE dbo.imp_truck_invoice_header
DROP COLUMN invoice_total
DROP FUNCTION dbo.fn_imp_truck_invoice_total
ALTER TABLE dbo.imp_truck_invoice_line
DROP COLUMN invoice_line_number
DROP FUNCTION dbo.fn_imp_truck_invoice_line_number
ALTER TABLE dbo.imp_truck_invoice_line
DROP COLUMN gr_weight_tons
GO

PRINT ('drop tables')
GO

DROP TABLE dbo.imp_truck_declaration
DROP TABLE dbo.imp_truck_container
DROP TABLE dbo.imp_truck_misc
DROP TABLE dbo.imp_truck_document
DROP TABLE dbo.imp_truck_invoice_line
DROP TABLE dbo.imp_truck_invoice_header
DROP TABLE dbo.imp_truck_filing_detail
DROP TABLE dbo.imp_truck_filing_header
DROP TABLE dbo.imp_truck_inbound
DROP TABLE dbo.imp_truck_rule_port
DROP TABLE dbo.imp_truck_rule_importer
DROP TABLE dbo.imp_truck_form_configuration
DROP TABLE dbo.imp_truck_form_section_configuration

PRINT ('drop views')
GO

DROP VIEW dbo.v_imp_truck_form_configuration
DROP VIEW dbo.v_imp_truck_field_configuration
DROP VIEW dbo.v_imp_truck_inbound_grid
DROP VIEW dbo.v_imp_truck_report


PRINT ('drop procedures')
GO

DROP PROCEDURE dbo.sp_imp_truck_create_entry_records
DROP PROCEDURE dbo.sp_imp_truck_add_declaration
DROP PROCEDURE dbo.sp_imp_truck_add_container
DROP PROCEDURE dbo.sp_imp_truck_add_misc
DROP PROCEDURE dbo.sp_imp_truck_add_invoice_header
DROP PROCEDURE dbo.sp_imp_truck_add_invoice_line
DROP PROCEDURE dbo.sp_imp_truck_review_entry
DROP PROCEDURE dbo.sp_imp_truck_delete_entry_records
DROP PROCEDURE dbo.sp_imp_truck_update_entry
DROP PROCEDURE dbo.sp_imp_truck_recalculate
DROP PROCEDURE dbo.sp_imp_truck_delete_inbound
