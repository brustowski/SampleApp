--- delete triggers ---
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.truck_invoice_lines_befor_delete'))
  DROP TRIGGER dbo.truck_invoice_lines_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.truck_invoice_headers_befor_delete'))
  DROP TRIGGER dbo.truck_invoice_headers_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.truck_misc_befor_delete'))
  DROP TRIGGER dbo.truck_misc_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.truck_containers_tab_befor_delete'))
  DROP TRIGGER dbo.truck_containers_tab_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.truck_declaration_tab_befor_delete'))
  DROP TRIGGER dbo.truck_declaration_tab_befor_delete
GO

--- restore constraints 
--- Truck Invoice Lines
IF (OBJECT_ID('FK_Truck_InvoiceLines_Truck_InvoiceHeaders_InvoiceHeaders_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_InvoiceLines
  DROP CONSTRAINT FK_Truck_InvoiceLines_Truck_InvoiceHeaders_InvoiceHeaders_FK
GO

--- Truck Invoice Headers
IF (OBJECT_ID('FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_InvoiceHeaders
  DROP CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK
GO
ALTER TABLE dbo.Truck_InvoiceHeaders
ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- Truck MISC
IF (OBJECT_ID('[FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_MISC
  DROP CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- Truck Declaration Tab
IF (OBJECT_ID('[FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_DeclarationTab
  DROP CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- Truck Containers Tab
IF (OBJECT_ID('[FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_ContainersTab
  DROP CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]
  GO
ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- Truck Documents
IF (OBJECT_ID('[FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_Documents
  DROP CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_Documents
ADD CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- restore Truck Invoice Header table ---
IF OBJECT_ID('FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK') IS NOT NULL
ALTER TABLE dbo.Truck_InvoiceHeaders
  DROP CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK
GO

DECLARE @res int
EXEC @res = sp_rename N'dbo.Truck_InvoiceHeaders', N'tmp_Truck_InvoiceHeaders', 'OBJECT'
IF @res <> 0
  RAISERROR ('Error while Rename table [dbo].[Truck_InvoiceHeaders] to [dbo].[tmp_Truck_InvoiceHeaders]', 11, 1 );
GO

CREATE TABLE dbo.Truck_InvoiceHeaders (
  id int NOT NULL,
  Filing_Headers_FK int NOT NULL,
  Invoice_No varchar(128) NULL,
  Supplier varchar(128) NULL,
  Supplier_Address varchar(128) NULL,
  INCO varchar(128) NULL,
  Invoice_Total numeric(18, 6) NULL,
  Curr varchar(128) NULL,
  Origin varchar(128) NULL,
  Payment_Date varchar(128) NULL,
  Consignee varchar(128) NULL,
  Consignee_Address varchar(128) NULL,
  Inv_Date varchar(128) NULL,
  Agreed_Place varchar(128) NULL,
  Inv_Gross_Weight varchar(128) NULL,
  Net_Weight varchar(128) NULL,
  Export varchar(128) NULL,
  Export_Date date NULL,
  First_Sale varchar(128) NULL,
  Transaction_Related varchar(128) NULL,
  Packages varchar(128) NULL,
  Manufacturer varchar(128) NULL,
  Seller varchar(128) NULL,
  Importer varchar(128) NULL,
  Sold_to_party varchar(128) NULL,
  Ship_to_party varchar(128) NULL,
  Broker_PGA_Contact_Name varchar(128) NULL,
  Broker_PGA_Contact_Phone varchar(128) NULL,
  Broker_PGA_Contact_Email varchar(128) NULL,
  EPA_PST_Cert_Date varchar(128) NULL,
  EPA_TSCA_Cert_Date varchar(128) NULL,
  EPA_VNE_Cert_Date varchar(128) NULL,
  FSIS_Cert_Date varchar(128) NULL,
  FWS_Cert_Date varchar(128) NULL,
  LACEY_ACT_Cert_Date varchar(128) NULL,
  NHTSA_Cert_Date varchar(128) NULL,
  Landed_Costing_Ex_Rate varchar(128) NULL,
  CreatedDate datetime NULL DEFAULT (getdate()),
  CreatedUser varchar(128) NULL DEFAULT (suser_name())
)
ON [PRIMARY]
GO

INSERT dbo.Truck_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Truck_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Truck_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Truck_InvoiceHeaders

ALTER TABLE dbo.Truck_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
  ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- restore Truck Tables view ---
IF OBJECT_ID(N'dbo.v_truck_tables ', 'V') IS NOT NULL
  DROP VIEW dbo.v_truck_tables 
GO

CREATE VIEW dbo.v_Truck_Tables 
AS SELECT
  ROW_NUMBER() OVER (ORDER BY table_name, column_name) AS id
 ,table_name AS TableName
 ,column_name AS ColumnName
FROM information_schema.columns i
WHERE i.table_schema = 'dbo'
AND i.table_name LIKE 'Truck_%'
AND i.TABLE_NAME NOT LIKE 'Truck_export_%'
AND i.TABLE_NAME NOT LIKE 'Truck_Rule_%'
AND i.table_name NOT IN (
'Truck_Inbound',
'Truck_DEFValues',
'Truck_DEFValues_Manual',
'Truck_Documents',
'Truck_Inbound_Grid',
'Truck_Report'
)
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'TI_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

--- restore Truck Default Values Manual table ---
CREATE TABLE dbo.Truck_DEFValues_Manual (
  id INT IDENTITY
 ,ColName VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL DEFAULT (GETDATE())
 ,CreatedUser VARCHAR(128) NULL DEFAULT (SUSER_NAME())
 ,DefValue VARCHAR(128) NULL
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Truck_DEFValues_Manual_Display_on_UI] DEFAULT (0)
 ,FEditable TINYINT NOT NULL
 ,FHasDefaultVal TINYINT NOT NULL
 ,Filing_Headers_FK INT NOT NULL
 ,FMandatory TINYINT NOT NULL
 ,FManual TINYINT NOT NULL
 ,ModifiedDate DATETIME NOT NULL DEFAULT (GETDATE())
 ,UI_Section VARCHAR(128) NULL
 ,ValueLabel VARCHAR(128) NULL
 ,TableName VARCHAR(128) NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_DEFValues_Manual] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO
-- copy data
INSERT INTO dbo.Truck_DEFValues_Manual (
    ColName
   ,CreatedDate
   ,CreatedUser
   ,DefValue
   ,Display_on_UI
   ,FEditable
   ,FHasDefaultVal
   ,Filing_Headers_FK
   ,FMandatory
   ,FManual
   ,ModifiedDate
   ,UI_Section
   ,ValueLabel
   ,TableName
   ,paired_field_table
   ,paired_field_column
   ,handbook_name)
  SELECT
    tdvm.column_name
   ,tdvm.created_date
   ,tdvm.created_user
   ,LEFT(tdvm.[value], 128)
   ,tdvm.display_on_ui
   ,CAST(tdvm.editable AS TINYINT)
   ,CAST(tdvm.has_default_value AS TINYINT)
   ,tdvm.filing_header_id
   ,CAST(tdvm.mandatory AS TINYINT)
   ,tdvm.manual
   ,tdvm.modification_date
   ,tdvm.section_title
   ,tdvm.label
   ,tdvm.table_name
   ,tdvm.paired_field_table
   ,tdvm.paired_field_column
   ,tdvm.handbook_name
  FROM dbo.truck_def_values_manual tdvm

-- delete new table
IF OBJECT_ID('dbo.truck_def_values_manual', 'U') IS NOT NULL
	DROP TABLE dbo.truck_def_values_manual
GO

--- restore Truck Default Values Manual view ---
CREATE VIEW dbo.v_Truck_DEFValues_Manual 
AS SELECT
  v.id
 ,v.ColName
 ,v.CreatedDate
 ,v.CreatedUser
 ,v.DefValue
 ,v.Display_on_UI
 ,v.FEditable
 ,v.FHasDefaultVal
 ,v.Filing_Headers_FK
 ,v.FMandatory
 ,v.FManual
 ,v.ModifiedDate
 ,v.UI_Section
 ,v.ValueLabel
 ,v.TableName
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Truck_DEFValues_Manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
    AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

IF OBJECT_ID(N'dbo.v_truck_def_values_manual', 'V') IS NOT NULL
  DROP VIEW dbo.v_truck_def_values_manual
GO

--- restore Truck Default Values table ---
CREATE TABLE Truck_DEFValues (
  id INT IDENTITY
 ,ColName VARCHAR(128) NULL
 ,CreatedDate DATETIME NULL CONSTRAINT [DF_dbo.Truck_DEFValues_CreatedDate] DEFAULT (GETDATE())
 ,CreatedUser NVARCHAR(MAX) NULL CONSTRAINT [DF_dbo.Truck_DEFValues_CreatedUser] DEFAULT (SUSER_NAME())
 ,DefValue VARCHAR(128) NULL
 ,Display_on_UI TINYINT NOT NULL CONSTRAINT [DF_dbo.Truck_DEFValues_Display_on_UI] DEFAULT (0)
 ,FEditable TINYINT NOT NULL
 ,FHasDefaultVal TINYINT NOT NULL
 ,FMandatory TINYINT NOT NULL
 ,FManual TINYINT NOT NULL
 ,UI_Section VARCHAR(32) NULL
 ,ValueDesc VARCHAR(128) NULL
 ,ValueLabel VARCHAR(128) NULL
 ,TableName VARCHAR(128) NULL
 ,SingleFilingOrder TINYINT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,CONSTRAINT [PK_dbo.Truck_DEFValues] PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE INDEX UK_Truck_DEFValues_SingleFilingOrder
ON Truck_DEFValues (SingleFilingOrder)
WHERE ([SingleFilingOrder] IS NOT NULL)
ON [PRIMARY]
GO

INSERT dbo.Truck_DEFValues (
  ColName
 ,CreatedDate
 ,CreatedUser
 ,DefValue
 ,Display_on_UI
 ,FEditable
 ,FHasDefaultVal
 ,FMandatory
 ,FManual
 ,UI_Section
 ,ValueDesc
 ,ValueLabel
 ,TableName
 ,SingleFilingOrder
 ,paired_field_table
 ,paired_field_column
 ,handbook_name)
  SELECT
    tdv.column_name
   ,tdv.created_date
   ,tdv.created_user
   ,LEFT(tdv.default_value, 128)
   ,tdv.display_on_ui
   ,CAST(tdv.editable AS TINYINT)
   ,CAST(tdv.has_default_value AS TINYINT)
   ,CAST(tdv.mandatory AS TINYINT)
   ,tdv.[manual]
   ,ts.title
   ,tdv.[description]
   ,tdv.label
   ,ts.table_name
   ,tdv.single_filing_order
   ,tdv.paired_field_table
   ,tdv.paired_field_column
   ,tdv.handbook_name
  FROM dbo.truck_def_values tdv
  INNER JOIN dbo.truck_sections ts
    ON tdv.section_id = ts.id
GO

IF OBJECT_ID('dbo.truck_def_values', 'U') IS NOT NULL
	DROP TABLE dbo.truck_def_values
GO

--- drop Truck Sections table ---
IF OBJECT_ID('dbo.truck_sections', 'U') IS NOT NULL
	DROP TABLE dbo.truck_sections
GO

--- restore Truck Default Values view ---
IF OBJECT_ID(N'dbo.v_truck_def_values', 'V') IS NOT NULL
  DROP VIEW dbo.v_truck_def_values
GO

CREATE VIEW dbo.v_Truck_DEFValues 
AS SELECT
  v.id
 ,v.ColName
 ,v.CreatedDate
 ,v.CreatedUser
 ,v.DefValue
 ,v.Display_on_UI
 ,v.FEditable
 ,v.FHasDefaultVal
 ,v.FMandatory
 ,v.FManual
 ,v.UI_Section
 ,v.ValueDesc
 ,v.ValueLabel
 ,v.TableName
 ,v.SingleFilingOrder
 ,v.paired_field_table
 ,v.paired_field_column
 ,v.handbook_name
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Truck_DEFValues v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
    AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

--- delete unneseccary Truck Filing procuders --- 
IF OBJECT_ID('dbo.truck_add_invoice_header_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_invoice_header_record
GO
IF OBJECT_ID('dbo.truck_add_invoice_line_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_invoice_line_record
GO

IF OBJECT_ID('dbo.truck_add_misc_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_misc_record
GO

IF OBJECT_ID('dbo.truck_add_container_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_container_record
GO

IF OBJECT_ID('dbo.truck_add_declaration_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_declaration_record
GO

IF OBJECT_ID('dbo.truck_add_def_values_manual') IS NOT NULL
  DROP PROCEDURE dbo.truck_add_def_values_manual
GO

IF OBJECT_ID('dbo.truck_apply_def_values') IS NOT NULL
  DROP PROCEDURE dbo.truck_apply_def_values
GO

IF OBJECT_ID('dbo.truck_delete_record') IS NOT NULL
  DROP PROCEDURE dbo.truck_delete_record
GO

--- restore Truck Filing procedures ---
ALTER PROCEDURE dbo.truck_filing (@Filing_Headers_id INT,
@FilingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  DECLARE @str VARCHAR(MAX) = '';

  -- TRUCK_DECLARATIONTAB---------------------------------------
  INSERT INTO Truck_DeclarationTab (Filing_Headers_FK,
  TI_FK,
  Main_Supplier,
  Importer,
  Issuer,
  Master_Bill,
  Carrier_SCAC,
  Discharge,
  Entry_Port,
  Destination_State,
  Description,
  FIRMs_Code)
    SELECT
      details.Filing_Headers_FK AS Filing_Headers_FK
     ,details.BDP_FK AS TI_FK
     ,ruleImporters.cw_supplier AS Main_Supplier
     ,ruleImporters.cw_ior AS Importer
     ,SUBSTRING(inbound.PAPs, 1, 4) AS Issuer
     ,SUBSTRING(inbound.PAPs, 5, LEN(inbound.PAPs)) AS Master_Bill
     ,SUBSTRING(inbound.PAPs, 1, 4) AS Carrier_SCAC
     ,ruleImporters.arrival_port AS Discharge
     ,ruleImporters.Entry_Port AS Entry_Port
     ,ruleImporters.Destination_State AS Destination_State
     ,ruleImporters.Goods_Description AS Description
     ,rulePorts.FIRMs_Code
    FROM Truck_Filing_Details details
    INNER JOIN Truck_Inbound inbound
      ON details.BDP_FK = inbound.id
    LEFT JOIN Truck_Rule_Importers ruleImporters
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
    LEFT JOIN Truck_Rule_Ports rulePorts
      ON (RTRIM(LTRIM(ruleImporters.arrival_port)) = RTRIM(LTRIM(rulePorts.arrival_port))
          AND RTRIM(LTRIM(ruleImporters.Entry_Port)) = RTRIM(LTRIM(rulePorts.Entry_Port)))
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_DeclarationTab declaration
      WHERE declaration.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.BDP_FK = declaration.TI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id

  -- Truck_ContainersTab---------------------------------------
  INSERT INTO Truck_ContainersTab (TI_FK, FILING_HEADERS_FK)
    SELECT
      details.BDP_FK
     ,details.FILING_HEADERS_FK
    FROM Truck_Filing_Details details
    INNER JOIN Truck_Inbound inbound
      ON details.BDP_FK = inbound.id
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_ContainersTab containers
      WHERE containers.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.BDP_FK = containers.TI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'Truck_ContainersTab'

  -- TRUCK_INVOICELINES---------------------------------------
  INSERT INTO Truck_InvoiceLines (Filing_Headers_FK
  , InvoiceHeaders_FK
  , TI_FK
  , Invoice_No
  , Transaction_Related
  , Tariff
  , Customs_QTY
  , Goods_Description
  , SPI
  , ORG
  , Export
  , Gr_Weight
  , UQ
  , PriceUnit
  , Prod_ID_1
  , Attribute_1
  , Attribute_2
  , Invoice_Qty
  , Invoice_Qty_Unit
  , Amount
  , Line_Price)
    SELECT
      details.Filing_Headers_FK
     ,ISNULL(ih.max_id, 0) + 1 AS InvoiceHeaders_FK
     ,details.BDP_FK AS TI_FK
     ,inbound.PAPs AS Invoice_No
     ,ruleImporters.transactions_related AS Transaction_Related
     ,ruleImporters.Tariff AS Tariff
     ,ruleImporters.custom_quantity AS Customs_QTY
     ,ruleImporters.Goods_Description AS Goods_Description
     ,ruleImporters.SPI AS SPI
     ,ruleImporters.co AS ORG
     ,ruleimporters.ce AS Export
     ,ruleImporters.Gross_Weight AS Gr_Weight
     ,ruleImporters.custom_uq AS UQ
     ,ruleImporters.Line_Price AS PriceUnit
     ,ruleImporters.product_id AS Prod_ID_1
     ,ruleImporters.custom_attrib1 AS Attribute_1
     ,ruleImporters.custom_attrib2 AS Attribute_2
     ,ruleImporters.Invoice_Qty AS Invoice_Qty
     ,ruleImporters.invoice_uq AS Invoice_Qty_Unit
     ,ruleImporters.charges AS Amount
     ,ruleImporters.invoice_qty * ruleImporters.line_price AS Line_Price
    FROM dbo.Truck_Filing_Details details
         INNER JOIN dbo.Truck_Inbound inbound
           ON inbound.id = details.BDP_FK
         LEFT JOIN Truck_Rule_Importers ruleImporters
           ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        ,(SELECT
             MAX(id) max_id
           FROM Truck_InvoiceHeaders) ih
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_InvoiceLines invLines
      WHERE invLines.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.BDP_FK = invLines.TI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'TRUCK_INVOICELINES'

  -- TRUCK_INVOICEHEADERS---------------------------------------
  INSERT INTO Truck_InvoiceHeaders (Id, Filing_Headers_FK, Invoice_No, Supplier, Consignee, Transaction_Related, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Invoice_Total, Export)
    SELECT DISTINCT
      invLines.InvoiceHeaders_FK AS Id
     ,details.Filing_Headers_FK AS Filing_Headers_FK
     ,inbound.PAPs AS Invoice_No
     ,ruleImporters.cw_supplier AS Supplier
     ,ruleImporters.cw_ior AS Consignee
     ,ruleImporters.transactions_related AS Transaction_Related
     ,ruleImporters.cw_supplier AS Manufacturer
     ,ruleImporters.cw_supplier AS Seller
     ,ruleImporters.cw_ior AS Importer
     ,ruleImporters.cw_ior AS Sold_to_party
     ,ruleImporters.cw_ior AS Ship_to_party
     ,invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total
     ,ruleimporters.ce AS Export
    FROM dbo.Truck_Filing_Details details
    INNER JOIN dbo.Truck_Inbound inbound
      ON inbound.id = details.BDP_FK
    LEFT JOIN Truck_Rule_Importers ruleImporters
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
    INNER JOIN Truck_InvoiceLines invLines
      ON invLines.Filing_Headers_FK = details.Filing_Headers_FK
        AND details.BDP_FK = invLines.TI_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_InvoiceHeaders invHeaders
      WHERE invHeaders.Filing_Headers_FK = details.Filing_Headers_FK
      AND invLines.InvoiceHeaders_FK = invHeaders.id)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'TRUCK_INVOICEHEADERS'

  -- Truck_MISC---------------------------------------
  INSERT INTO Truck_MISC (TI_FK, Branch, Broker, Preparer_Dist_Port, Recon_Issue, FTA_Recon, Filing_Headers_FK)
    SELECT
      details.BDP_FK
     ,userData.Branch
     ,userData.Broker
     ,userData.Location
     ,ruleImporters.Recon_Issue
     ,ruleImporters.nafta_recon
     ,details.Filing_Headers_FK
    FROM dbo.Truck_Filing_Details details
    INNER JOIN dbo.Truck_Inbound inbound
      ON inbound.id = details.BDP_FK
    LEFT JOIN Truck_Rule_Importers ruleImporters
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
    LEFT JOIN app_users_data userData
      ON userData.UserAccount = @FilingUser
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_MISC misc
      WHERE misc.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.BDP_FK = misc.TI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'Truck_MISC'


  DECLARE @str_val NVARCHAR(500);
  DECLARE @ParmDefinition NVARCHAR(500);
  DECLARE @DefvalOUT VARCHAR(128);

  DECLARE @datatype VARCHAR(128)
         ,@DefValue VARCHAR(128)
         ,@id INT;
  DECLARE c CURSOR FOR SELECT DISTINCT
    DATA_TYPE +
    CASE
      WHEN DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
   ,DefValue
   ,v.id
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Truck_DEFValues v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
    AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND (FManual > 0
  OR FHasDefaultVal > 0
  OR Display_on_UI > 0)
  AND NOT EXISTS (SELECT
      *
    FROM Truck_DEFValues_Manual r
    WHERE r.Filing_Headers_FK = @Filing_Headers_id)
  ORDER BY id
  OPEN c
  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  WHILE @@fetch_status = 0
  BEGIN
  SET @DefvalOUT = NULL;
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as  varchar(128))';
  SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as  varchar(128))';
    SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';

    DECLARE @result FLOAT
  BEGIN TRY
    EXECUTE sp_executesql @str_val
                         ,@ParmDefinition
                         ,@valOUT = @DefvalOUT OUTPUT;
  END TRY
  BEGIN CATCH
    SET @DefvalOUT = NULL;
  END CATCH;
  END;
  --PRINT concat(@id,' - ',@DefvalOUT, ' ', @datatype);

  IF @datatype LIKE 'date%'
  BEGIN
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS DATETIME), 'MM/dd/yyyy');
  END;
  IF @datatype LIKE 'numeric'
  BEGIN
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS FLOAT), '0.######');
  END;
  INSERT INTO Truck_DEFValues_Manual (Filing_Headers_FK, Display_on_UI, ValueLabel, TableName, ColName, FManual, FHasDefaultVal
  , FEditable, UI_Section, FMandatory, DefValue, handbook_name, paired_field_table, paired_field_column)
    SELECT
      @Filing_Headers_id
     ,Display_on_UI
     ,ValueLabel
     ,TableName
     ,ColName
     ,FManual
     ,FHasDefaultVal
     ,FEditable
     ,UI_Section
     ,FMandatory
     ,@DefvalOUT
	 ,handbook_name
	 ,paired_field_table
	 ,paired_field_column
    FROM Truck_DEFValues
    WHERE id = @id

  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  END
  CLOSE c
  DEALLOCATE c

  SET @str = ''
  SELECT
    @str = @str + 'update  ' + TableName + ' set ' + ColName + '= try_cast(''' + DefValue + ''' as ' +
    DATA_TYPE +
    CASE
      WHEN DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN '(' +
        CASE
          WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'MAX'
          ELSE CONVERT(VARCHAR(4),
            CASE
              WHEN DATA_TYPE IN ('nchar', 'nvarchar') THEN CHARACTER_MAXIMUM_LENGTH / 2
              ELSE CHARACTER_MAXIMUM_LENGTH
            END)
        END + ')'
      WHEN DATA_TYPE IN ('decimal', 'numeric') THEN '(' + CONVERT(VARCHAR(4), NUMERIC_PRECISION) + ','
        + CONVERT(VARCHAR(4), NUMERIC_SCALE) + ')'
      ELSE ''
    END
    + ') ' +
    CASE
      WHEN UPPER(TableName) = 'TRUCK_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Truck_DEFValues_Manual v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM INFORMATION_SCHEMA.COLUMNS c
    WHERE (UPPER(i.TABLE_NAME) = UPPER(c.TABLE_NAME)
    AND UPPER(c.COLUMN_NAME) = 'FILING_HEADERS_FK')
    OR UPPER(c.TABLE_NAME) = 'TRUCK_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.TABLE_NAME) = 'TRUCK_FILING_HEADERS' THEN 0
    ELSE 1
  END
  --PRINT @str;
  EXEC (@str);

  DECLARE @List VARCHAR(MAX);
  DECLARE @List2 VARCHAR(MAX);
  DECLARE @sql VARCHAR(MAX);
  DECLARE @sql2 VARCHAR(1000) = '';
  DECLARE @s VARCHAR(MAX);

  DECLARE c CURSOR FOR SELECT DISTINCT
    TableName
  FROM Truck_DEFValues_Manual V
  WHERE Filing_Headers_FK = @Filing_Headers_id
  OPEN c
  FETCH NEXT FROM c INTO @s
  WHILE @@fetch_status = 0
  BEGIN

  SET @List = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.NAME) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.NAME) + ', ''0.######'')'
        ELSE QUOTENAME(c.NAME)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.COLUMNS c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.COLUMNS
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.COLUMNS
      WHERE (object_id = OBJECT_ID(@s)
      AND UPPER(NAME) = 'FILING_HEADERS_FK')
      OR object_id = OBJECT_ID('Truck_Filing_Headers'))
    SET @sql2 =
    CASE
      WHEN UPPER(@s) = 'TRUCK_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END;

  SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  ' + @s + ' ' + @sql2 + ')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
  --PRINT @sql
  SET @sql = '
MERGE Truck_DEFValues_Manual AS t 
USING (' + @sql + ' ) AS s 
ON (upper(t.ColName) = s.ColName and upper(t.TableName) = ''' + @s + ''' AND t.Filing_Headers_FK = ' + CAST(@Filing_Headers_id AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
    UPDATE SET DefValue = case when s.value='''' then null else s.value end  ;';
  --PRINT @sql
  EXEC (@sql)

  FETCH NEXT FROM c INTO @s
  END
  CLOSE c
  DEALLOCATE c
END;
GO

ALTER PROCEDURE dbo.truck_filing_del (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = '';

  DELETE FROM Truck_DeclarationTab
  WHERE Filing_Headers_FK = @Filing_Headers_id;

  DELETE FROM Truck_ContainersTab
  WHERE Filing_Headers_FK = @Filing_Headers_id;

  DELETE FROM Truck_InvoiceHeaders
  WHERE Filing_Headers_FK = @Filing_Headers_id;

  DELETE FROM Truck_InvoiceLines
  WHERE Filing_Headers_FK = @Filing_Headers_id;

  DELETE FROM Truck_MISC
  WHERE Filing_Headers_FK = @Filing_Headers_id;

  DELETE FROM [dbo].[Truck_Documents]
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM [dbo].[Truck_DEFValues_Manual]
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM [dbo].[Truck_Filing_Details]
  WHERE Filing_Headers_FK = @Filing_Headers_id

  DELETE FROM [dbo].[Truck_Filing_Headers]
  WHERE id = @Filing_Headers_id

END;
GO

ALTER PROCEDURE dbo.truck_filing_param (@Filing_Headers_id INT = 117)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = '';

  SET @str = ''

  SELECT
    @str = @str + 'update  ' + TableName + ' set ' + ColName + '= ' +
    CASE
      WHEN DefValue IS NULL THEN 'NULL'
      ELSE 'cast(''' + REPLACE(DefValue, '''', '''''') + ''' as ' +
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
        + ')'
    END
    +
    CASE
      WHEN UPPER(TableName) = 'TRUCK_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Truck_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  -- we don't update table without Filing_Headers_FK, but we use the first value from this table
  AND EXISTS (SELECT
      *
    FROM information_schema.columns c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'TRUCK_FILING_HEADERS')
  --print @str;
  EXEC (@str);
END;
GO