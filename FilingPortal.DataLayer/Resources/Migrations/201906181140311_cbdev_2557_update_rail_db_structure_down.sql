--- delete triggers ---
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.rail_invoice_lines_befor_delete'))
  DROP TRIGGER dbo.rail_invoice_lines_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.rail_invoice_headers_befor_delete'))
  DROP TRIGGER dbo.rail_invoice_headers_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.rail_misc_befor_delete'))
  DROP TRIGGER dbo.rail_misc_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.rail_containers_tab_befor_delete'))
  DROP TRIGGER dbo.rail_containers_tab_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.rail_declaration_tab_befor_delete'))
  DROP TRIGGER dbo.rail_declaration_tab_befor_delete
GO

--- restore constraints 
--- Rail Invoice Lines
IF (OBJECT_ID('FK_Rail_InvoiceLines_Rail_InvoiceHeaders_InvoiceHeaders_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_InvoiceLines
  DROP CONSTRAINT FK_Rail_InvoiceLines_Rail_InvoiceHeaders_InvoiceHeaders_FK
GO

--- Rail Invoice Headers
IF (OBJECT_ID('FK_Rail_InvoiceHeaders_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_InvoiceHeaders
  DROP CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers
GO
ALTER TABLE dbo.Rail_InvoiceHeaders
ADD CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- Rail MISC
IF (OBJECT_ID('FK_Rail_Rail_MISC_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_MISC
  DROP CONSTRAINT FK_Rail_Rail_MISC_Filing_Headers
GO
ALTER TABLE dbo.Rail_MISC
ADD CONSTRAINT FK_Rail_Rail_MISC_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- Rail Declaration Tab
IF (OBJECT_ID('FK_RAIL_DEC_REFERENCE_RAIL_FIL', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_DeclarationTab
  DROP CONSTRAINT FK_RAIL_DEC_REFERENCE_RAIL_FIL
GO
ALTER TABLE dbo.Rail_DeclarationTab
ADD CONSTRAINT FK_RAIL_DEC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- Rail Containers Tab
IF (OBJECT_ID('FK_Rail_Packing_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_ContainersTab
  DROP CONSTRAINT FK_Rail_Packing_Filing_Headers
  GO
ALTER TABLE dbo.Rail_ContainersTab
ADD CONSTRAINT FK_Rail_Packing_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- Rail Documents
IF (OBJECT_ID('FK_RAIL_DOC_REFERENCE_RAIL_FIL', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_Documents
  DROP CONSTRAINT FK_RAIL_DOC_REFERENCE_RAIL_FIL
GO
ALTER TABLE dbo.Rail_Documents
ADD CONSTRAINT FK_RAIL_DOC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- restore Rail Invoice Header table ---
IF OBJECT_ID('FK_Rail_InvoiceHeaders_Filing_Headers') IS NOT NULL
ALTER TABLE dbo.Rail_InvoiceHeaders
  DROP CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers
GO

DECLARE @res int
EXEC @res = sp_rename N'dbo.Rail_InvoiceHeaders', N'tmp_Rail_InvoiceHeaders', 'OBJECT'
IF @res <> 0
  RAISERROR ('Error while Rename table [dbo].[Rail_InvoiceHeaders] to [dbo].[tmp_Rail_InvoiceHeaders]', 11, 1 );
GO

CREATE TABLE dbo.Rail_InvoiceHeaders (
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

INSERT dbo.Rail_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Rail_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Rail_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Rail_InvoiceHeaders
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
  ADD CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- restore Rail DefValues Manual table ---
ALTER TABLE dbo.Rail_DEFValues_Manual
ADD UI_Section VARCHAR(32) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD ColName VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD TableName VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD DefValue VARCHAR(128) NULL
GO

UPDATE dbo.Rail_DEFValues_Manual
SET ColName = column_name
   ,TableName = table_name
   ,DefValue = value
   ,UI_Section = section_title

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'parent_record_id' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN parent_record_id
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'section_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN section_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'section_title' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN section_title
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'record_id' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN record_id
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'description' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN [description]
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'table_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN table_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'column_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN column_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'value' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN [value]
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes i WHERE i.name = 'Idx_FilingHeadersFK')
  DROP INDEX Idx_FilingHeadersFK ON dbo.Rail_DEFValues_Manual
GO

--- restore Rail DefValues Manual view ---
ALTER VIEW dbo.v_Rail_DEFValues_Manual
AS
SELECT
  v.id
 ,v.Filing_Headers_FK
 ,v.Display_on_UI
 ,v.ValueLabel
 ,v.DefValue
 ,v.TableName
 ,v.ColName
 ,v.FManual
 ,v.FHasDefaultVal
 ,v.FEditable
 ,v.FMandatory
 ,v.UI_Section
 ,v.ModifiedDate
 ,v.CreatedDate
 ,v.CreatedUser
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Rail_DEFValues_Manual v
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
WHERE (UPPER(i.column_name) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

--- restore Rail DefValues table ---
ALTER TABLE dbo.Rail_DEFValues
ADD UI_Section VARCHAR(32) NULL
GO

ALTER TABLE dbo.Rail_DEFValues
ADD TableName VARCHAR(128) NULL
GO

UPDATE dbo.Rail_DEFValues
SET UI_Section = ps.title
   ,TableName = ps.table_name
FROM dbo.Rail_DEFValues AS pdv
INNER JOIN dbo.rail_sections ps
  ON pdv.section_id = ps.id

ALTER TABLE dbo.Rail_DEFValues
DROP CONSTRAINT FK_Rail_DEFValues__rail_sections__section_id
GO

DROP INDEX IX_section_id ON dbo.Rail_DEFValues
GO

ALTER TABLE dbo.Rail_DEFValues
DROP COLUMN section_id
GO

--- restore Rail DefValues view ---
ALTER VIEW dbo.v_Rail_DEFValues
AS
SELECT
  v.id
 ,v.Display_on_UI
 ,v.ValueLabel
 ,v.ValueDesc
 ,v.DefValue
 ,v.TableName
 ,v.ColName
 ,v.FManual
 ,v.FHasDefaultVal
 ,v.FEditable
 ,v.UI_Section
 ,v.FMandatory
 ,v.CreatedDate
 ,v.CreatedUser
 ,v.SingleFilingOrder
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Rail_DEFValues v
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
WHERE (UPPER(i.column_name)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Rail_Inbounds_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

--- restore rail tables view ---
ALTER VIEW dbo.v_Rail_Tables 
AS select ROW_NUMBER() over (ORDER BY table_name, column_name) as id, table_name as TableName, column_name as ColumnName
from information_schema.columns i
where  i.table_schema = 'dbo' and i.table_name like 'Rail_%'
and i.table_name not in (
	'Rail_BD_Parsed',
	'Rail_DEFValues',
	'Rail_DEFValues_Manual',
	'Rail_Documents',
	'Rail_EDIMessage',
	'Rail_Rule_Desc1_Desc2',
	'Rail_Rule_ImporterSupplier',
	'Rail_Rule_Port',
	'Rail_Inbound_Grid',
	'Rail_Report',
	'v_Client_CargoWise',
	'v_Rail_DEFValues_Manual'
)
and upper(column_name) not in ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER' )
GO


--- drop Rail Section table ---
IF OBJECT_ID('dbo.rail_sections', 'U') IS NOT NULL
	DROP TABLE dbo.rail_sections
GO

-- delete unneseccary Rail filing proceduers --- 
IF OBJECT_ID('dbo.rail_add_invoice_header_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_invoice_header_record
GO
IF OBJECT_ID('dbo.rail_add_invoice_line_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_invoice_line_record
GO

IF OBJECT_ID('dbo.rail_add_misc_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_misc_record
GO

IF OBJECT_ID('dbo.rail_add_container_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_container_record
GO

IF OBJECT_ID('dbo.rail_add_declaration_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_declaration_record
GO

IF OBJECT_ID('dbo.rail_add_def_values_manual') IS NOT NULL
  DROP PROCEDURE dbo.rail_add_def_values_manual
GO

IF OBJECT_ID('dbo.rail_apply_def_values') IS NOT NULL
  DROP PROCEDURE dbo.rail_apply_def_values
GO

IF OBJECT_ID('dbo.rail_delete_record') IS NOT NULL
  DROP PROCEDURE dbo.rail_delete_record
GO

--- restore Rail filing procedures ---
ALTER PROCEDURE dbo.rail_filing (@Filing_Headers_id INT)
AS
BEGIN
  SET DATEFORMAT mdy;
  DECLARE @str VARCHAR(MAX) = '';
  -- RAIL_DECLARATIONTAB---------------------------------------
  INSERT INTO RAIL_DECLARATIONTAB (
      BDP_FK
     ,Carrier_SCAC
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
     ,Master_Bill
     ,Origin
     ,FILING_HEADERS_FK)
    SELECT
      d.BDP_FK
     ,p.IssuerCode AS Carrier_SCAC
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
     ,p.BillofLading AS Master_Bill
     ,rp.Origin AS Origin
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
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_DECLARATIONTAB r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  -- PRINT 'RAIL_DECLARATIONTAB'
  -- RAIL_CONTAINERSTAB---------------------------------------
  INSERT INTO RAIL_CONTAINERSTAB (
      BDP_FK
     ,Bill_Issuer_SCAC
     ,Bill_Num
     ,Bill_Number
     ,Container_Number
     ,FILING_HEADERS_FK)
    SELECT
      d.BDP_FK
     ,p.IssuerCode AS Bill_Issuer_SCAC
     ,p.BillofLading AS Bill_Num
     ,CONCAT('MB:', p.BillofLading) AS Bill_Number
     ,CONCAT(EquipmentInitial, EquipmentNumber) AS Container_Number
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_CONTAINERSTAB r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_CONTAINERSTAB'
  -- RAIL_INVOICELINES---------------------------------------
  INSERT INTO RAIL_INVOICELINES (
      BDP_FK
     ,Attribute_1
     ,Attribute_2
     ,Gr_Weight
     ,Gr_Weight_Unit
     ,Consignee
     ,Dest_State
     ,Export
     ,Goods_Description
     ,InvoiceHeaders_FK
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
    SELECT
      d.BDP_FK
     ,rd.Attribute_1 AS Attribute_1
     ,rd.Attribute_2 AS Attribute_2
     ,p.Weight AS Gr_Weight
     ,p.WeightUnit AS Gr_Weight_Unit
     ,rn.Consignee AS Consignee
     ,rn.Destination_State AS Dest_State
     ,rp.Export AS Export
     ,rd.Goods_Description AS Goods_Description
     ,ISNULL(ih.max_id, 0) + DENSE_RANK() OVER (ORDER BY p.Importer, p.Supplier, p.ReferenceNumber1, PortofUnlading) AS InvoiceHeaders_FK
     ,rn.Manufacturer AS Manufacturer
     ,rn.CountryofOrigin AS ORG
     ,rn.CountryofOrigin AS Origin
     ,rd.Prod_ID_1 AS Prod_ID_1
     ,rd.Tariff AS Tariff
     ,rn.Relationship AS Transaction_Related
     ,rd.Template_Invoice_Quantity AS Customs_QTY
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

        ,(SELECT
             MAX(id) max_id
           FROM Rail_InvoiceHeaders) ih
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_INVOICELINES r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_INVOICELINES'
  -- RAIL_INVOICEHEADERS---------------------------------------
  INSERT INTO RAIL_INVOICEHEADERS (
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
     ,Invoice_Total
     ,FILING_HEADERS_FK
     ,id)
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
     ,ri.PriceUnit * ri.Customs_QTY AS Invoice_Total
     ,d.FILING_HEADERS_FK
     ,ri.InvoiceHeaders_FK AS id
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
    INNER JOIN RAIL_INVOICELINES ri
      ON ri.Filing_Headers_FK = d.Filing_Headers_FK
        AND d.BDP_FK = ri.BDP_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM RAIL_INVOICEHEADERS r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND ri.InvoiceHeaders_FK = r.id)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'RAIL_INVOICEHEADERS'
  -- Rail_MISC---------------------------------------
  INSERT INTO Rail_MISC (
      BDP_FK
     ,Recon_Issue
     ,FTA_Recon
     ,Payment_Type
     ,Broker_to_Pay
     ,Submitter
     ,Filing_Headers_FK)
    SELECT
      d.BDP_FK
     ,ISNULL(rn.Value_Recon, 'N/A') AS Recon_Issue
     ,rn.FTA_Recon AS FTA_Recon
     ,rn.Payment_Type AS Payment_Type
     ,rn.Broker_to_Pay AS Broker_to_Pay
     ,rn.Importer AS Submitter
     ,d.FILING_HEADERS_FK
    FROM dbo.Rail_Filing_Details d
    INNER JOIN dbo.Rail_BD_Parsed p
      ON p.BDP_PK = d.BDP_FK
    LEFT JOIN dbo.Rail_Rule_ImporterSupplier rn
      ON RTRIM(LTRIM(p.Importer)) = RTRIM(LTRIM(rn.Importer_Name))
        AND (RTRIM(LTRIM(p.Supplier)) = RTRIM(LTRIM(rn.Supplier_Name))
          OR (p.Supplier IS NULL
            AND rn.Supplier_Name IS NULL))
    WHERE NOT EXISTS (SELECT
        *
      FROM Rail_MISC r
      WHERE r.Filing_Headers_FK = d.Filing_Headers_FK
      AND d.BDP_FK = r.BDP_FK)
    AND d.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'Rail_MISC'

  DECLARE @str_val NVARCHAR(500);
  DECLARE @ParmDefinition NVARCHAR(500);
  DECLARE @DefvalOUT VARCHAR(128);

  DECLARE @datatype VARCHAR(128)
         ,@DefValue VARCHAR(500)
         ,@id INT;
  DECLARE c CURSOR FOR SELECT DISTINCT
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
   ,DefValue
   ,v.id
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND (FManual > 0
  OR FHasDefaultVal > 0
  OR Display_on_UI > 0)
  AND NOT EXISTS (SELECT
      *
    FROM Rail_DEFValues_Manual r
    WHERE r.Filing_Headers_FK = @Filing_Headers_id)
  ORDER BY id
  OPEN c
  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  WHILE @@fetch_status = 0
  BEGIN
  SET @DefvalOUT = NULL;
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as  varchar(500))';
  SET @ParmDefinition = N'@valOUT varchar(500) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as  varchar(500))';
    SET @ParmDefinition = N'@valOUT varchar(500) OUTPUT';

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
  INSERT INTO Rail_DEFValues_Manual (
      Filing_Headers_FK
     ,Display_on_UI
     ,ValueLabel
     ,TableName
     ,ColName
     ,FManual
     ,FHasDefaultVal
     ,FEditable
     ,UI_Section
     ,FMandatory
     ,DefValue
     ,handbook_name
     ,paired_field_table
     ,paired_field_column)
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
    FROM Rail_DEFValues
    WHERE id = @id

  FETCH NEXT FROM c INTO @datatype, @DefValue, @id
  END
  CLOSE c
  DEALLOCATE c

  SET @str = ''
  SELECT
    @str = @str + 'update  ' + TableName + ' set ' + ColName + '= try_cast(''' + DefValue + ''' as ' +
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
    + ') ' +
    CASE
      WHEN UPPER(TableName) = 'RAIL_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM information_schema.columns c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'RAIL_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.table_name) = 'RAIL_FILING_HEADERS' THEN 0
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
  FROM Rail_DEFValues_Manual V
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
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE objecT_id = OBJECT_ID(@s)
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE objecT_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.columns
      WHERE (objecT_id = OBJECT_ID(@s)
      AND UPPER(NAME) = 'FILING_HEADERS_FK')
      OR objecT_id = OBJECT_ID('Rail_Filing_Headers'))
    SET @sql2 =
    CASE
      WHEN UPPER(@s) = 'RAIL_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END;

  SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  ' + @s + ' ' + @sql2 + ')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
  --PRINT @sql
  SET @sql = '
MERGE Rail_DEFValues_Manual AS t 
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

ALTER PROCEDURE dbo.rail_filing_param (@Filing_Headers_id INT = 117)
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
      WHEN UPPER(TableName) = 'RAIL_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Rail_DEFValues_Manual v
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
    OR UPPER(c.table_name) = 'RAIL_FILING_HEADERS')
  --print @str;
  EXEC (@str);
END;
GO

ALTER PROCEDURE dbo.rail_filing_del (@Filing_Headers_id INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = '';
  SET @str = ''

  SELECT
    @str = @str + 'delete from ' + TableName + ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64)) + '; '
    + CHAR(10)
  FROM information_schema.columns i
  INNER JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND EXISTS (SELECT
      *
    FROM information_schema.columns c
    WHERE UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
  GROUP BY TableName
  --print @str;
  EXEC (@str);
  DELETE FROM [dbo].[Rail_Documents]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Rail_DEFValues_Manual]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Rail_Filing_Details]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Rail_Filing_Headers]
  WHERE id = @Filing_Headers_id

END;
GO