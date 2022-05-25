-- create section table ---
CREATE TABLE dbo.truck_sections (
  id INT NOT NULL IDENTITY
 ,[name] VARCHAR(128) NOT NULL
 ,title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128)
 ,is_array BIT NOT NULL
 ,parent_id INT
 ,[procedure_name] VARCHAR(128)
 ,CONSTRAINT PK_truck__sections_id PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE UNIQUE INDEX IX_name
ON dbo.truck_sections ([name])
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.truck_sections (parent_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.truck_sections
ADD CONSTRAINT FK_truck_sections__parent_id FOREIGN KEY (parent_id) REFERENCES dbo.truck_sections (id)
GO

SET IDENTITY_INSERT dbo.truck_sections ON
INSERT dbo.truck_sections(id, [name], title, table_name, is_array, parent_id, [procedure_name]) VALUES 
  (1, 'root', 'Root', NULL, CONVERT(bit, 'False'), NULL, NULL)
  ,(2, 'declaration', 'Declaration', 'Truck_DeclarationTab', CONVERT(bit, 'False'), 1, 'truck_add_declaration_record')
  ,(3, 'invoice', 'Invoices', NULL, CONVERT(bit, 'False'), 1, NULL)
  ,(4, 'invoice_header', 'Invoice', 'Truck_InvoiceHeaders', CONVERT(bit, 'True'), 3, 'truck_add_invoice_header_record')
  ,(5, 'invoice_line', 'Line', 'Truck_InvoiceLines', CONVERT(bit, 'True'), 4, 'truck_add_invoice_line_record')
  ,(6, 'container', 'Container', 'Truck_ContainersTab', CONVERT(bit, 'False'), 1, 'truck_add_container_record')
  ,(7, 'misc', 'MISC', 'Truck_MISC', CONVERT(bit, 'False'), 1, 'truck_add_misc_record')
SET IDENTITY_INSERT dbo.truck_sections OFF
GO

--- update Truck Default Values table ---
CREATE TABLE dbo.truck_def_values (
  id int NOT NULL IDENTITY
 ,label varchar(128) NOT NULL
 ,default_value varchar(512)
 ,[description] varchar(128)
 ,section_id int NOT NULL
 ,column_name varchar(128) NOT NULL
 ,editable bit NOT NULL
 ,mandatory bit NOT NULL
 ,has_default_value bit NOT NULL
 ,paired_field_table varchar(128)
 ,paired_field_column varchar(128)
 ,handbook_name varchar(128)
 ,display_on_ui tinyint NOT NULL
 ,[manual] tinyint NOT NULL
 ,single_filing_order tinyint
 ,created_date datetime NOT NULL CONSTRAINT DF_truck_def_values__created_date DEFAULT (GETDATE())
 ,created_user varchar(128) NOT NULL
 ,CONSTRAINT PK_truck_def_values PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE UNIQUE INDEX UK_truck_def_values__single_filing_order
ON dbo.truck_def_values (single_filing_order)
WHERE (single_filing_order IS NOT NULL)
ON [PRIMARY]
GO

CREATE INDEX IX_section_id
ON dbo.truck_def_values (section_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.truck_def_values
ADD CONSTRAINT FK_truck_def_values__truck_sections__section_id FOREIGN KEY (section_id) REFERENCES dbo.truck_sections (id)
GO

INSERT dbo.truck_def_values (
  label
 ,default_value
 ,[description]
 ,section_id
 ,column_name
 ,editable
 ,mandatory
 ,has_default_value
 ,paired_field_table
 ,paired_field_column
 ,handbook_name
 ,display_on_ui
 ,[manual]
 ,single_filing_order
 ,created_date
 ,created_user)
  SELECT
    td.ValueLabel
   ,td.DefValue
   ,td.ValueDesc
   ,ts.id
   ,td.ColName
   ,CAST(td.FEditable AS BIT)
   ,CAST(td.FMandatory AS BIT)
   ,CAST(td.FHasDefaultVal AS BIT)
   ,td.paired_field_table
   ,td.paired_field_column
   ,td.handbook_name
   ,td.Display_on_UI
   ,td.FManual
   ,td.SingleFilingOrder
   ,td.CreatedDate
   ,ISNULL(td.CreatedUser, SUSER_NAME())
  FROM dbo.Truck_DEFValues td
  INNER JOIN dbo.truck_sections ts
    ON td.TableName = ts.table_name
GO

IF OBJECT_ID('dbo.Truck_DEFValues', 'U') IS NOT NULL
	DROP TABLE dbo.Truck_DEFValues
GO
--- Update Truck Default Values view ---
IF OBJECT_ID(N'dbo.v_Truck_DEFValues', 'V') IS NOT NULL
  DROP VIEW dbo.v_Truck_DEFValues
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

--- update Truck Default Values manual table ---
-- create new table
CREATE TABLE dbo.truck_def_values_manual (
  id INT NOT NULL IDENTITY
 ,filing_header_id INT NOT NULL
 ,parent_record_id INT NOT NULL
 ,record_id INT NOT NULL
 ,section_name VARCHAR(128) NOT NULL
 ,section_title VARCHAR(128) NOT NULL
 ,table_name VARCHAR(128) NOT NULL
 ,column_name VARCHAR(128) NOT NULL
 ,label VARCHAR(128) NOT NULL
 ,[value] VARCHAR(512) NULL
 ,[description] VARCHAR(128) NULL
 ,editable BIT NOT NULL
 ,has_default_value BIT NOT NULL
 ,mandatory BIT NOT NULL
 ,display_on_ui TINYINT NOT NULL
 ,[manual] TINYINT NOT NULL
 ,paired_field_table VARCHAR(128) NULL
 ,paired_field_column VARCHAR(128) NULL
 ,handbook_name VARCHAR(128) NULL
 ,created_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,created_user VARCHAR(128) NOT NULL DEFAULT (SUSER_NAME())
 ,modification_date DATETIME NOT NULL DEFAULT (GETDATE())
 ,CONSTRAINT PK_truck_def_values_manual PRIMARY KEY CLUSTERED (id)
) ON [PRIMARY]
GO

CREATE INDEX Idx_FilingHeaderId
ON dbo.truck_def_values_manual (filing_header_id)
ON [PRIMARY]
GO

-- copy data 
INSERT INTO dbo.truck_def_values_manual (
    filing_header_id
   ,parent_record_id
   ,record_id
   ,section_name
   ,section_title
   ,table_name
   ,column_name
   ,label
   ,value
   ,editable
   ,has_default_value
   ,mandatory
   ,display_on_ui
   ,manual
   ,paired_field_table
   ,paired_field_column
   ,handbook_name
   ,created_date
   ,created_user
   ,modification_date)
  SELECT
    tdm.Filing_Headers_FK
   ,0
   ,0
   ,ts.name
   ,ts.title
   ,ts.table_name
   ,tdm.ColName
   ,tdm.ValueLabel
   ,tdm.DefValue
   ,CAST(tdm.FEditable AS BIT)
   ,CAST(tdm.FHasDefaultVal AS BIT)
   ,CAST(tdm.FMandatory AS BIT)
   ,tdm.Display_on_UI
   ,tdm.FManual
   ,tdm.paired_field_table
   ,tdm.paired_field_column
   ,tdm.handbook_name
   ,tdm.CreatedDate
   ,tdm.CreatedUser
   ,tdm.ModifiedDate
  FROM dbo.Truck_DEFValues_Manual tdm
  INNER JOIN dbo.truck_sections ts
    ON tdm.TableName = ts.table_name

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM truck_def_values_manual dest
INNER JOIN Truck_DeclarationTab source
  ON dest.filing_header_id = source.Filing_Headers_FK
WHERE dest.table_name = 'Truck_DeclarationTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM truck_def_values_manual dest
INNER JOIN Truck_ContainersTab source
  ON dest.filing_header_id = source.Filing_Headers_FK
WHERE dest.table_name = 'Truck_ContainersTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM truck_def_values_manual dest
INNER JOIN Truck_MISC source
  ON dest.filing_header_id = source.Filing_Headers_FK
WHERE dest.table_name = 'Truck_MISC'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM truck_def_values_manual dest
INNER JOIN Truck_InvoiceHeaders source
  ON dest.filing_header_id = source.Filing_Headers_FK
WHERE dest.table_name = 'Truck_InvoiceHeaders'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.InvoiceHeaders_FK
FROM truck_def_values_manual dest
INNER JOIN Truck_InvoiceLines source
  ON dest.filing_header_id = source.Filing_Headers_FK
WHERE dest.table_name = 'Truck_InvoiceLines'
GO

-- delete records withot corresponding result record
DELETE dbo.truck_def_values_manual
WHERE record_id = 0

-- delete old table 
IF OBJECT_ID('dbo.Truck_DEFValues_Manual', 'U') IS NOT NULL
	DROP TABLE dbo.Truck_DEFValues_Manual
GO

--- update Truck Default Values Manual view ---
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

IF OBJECT_ID(N'dbo.v_Truck_DEFValues_Manual ', 'V') IS NOT NULL
  DROP VIEW dbo.v_Truck_DEFValues_Manual 
GO

--- update Truck Tables view ---
IF OBJECT_ID(N'dbo.v_Truck_Tables ', 'V') IS NOT NULL
  DROP VIEW dbo.v_Truck_Tables 
GO

CREATE VIEW dbo.v_truck_tables 
AS SELECT
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

--- update Truck Invoice Header table ---
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
  id int IDENTITY,
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

SET IDENTITY_INSERT dbo.Truck_InvoiceHeaders ON

INSERT dbo.Truck_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Truck_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Truck_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Truck_InvoiceHeaders

SET IDENTITY_INSERT dbo.Truck_InvoiceHeaders OFF
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Truck_InvoiceHeaders
  ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id)
GO

--- update constraints 
--- Truck Invoice Lines
ALTER TABLE dbo.Truck_InvoiceLines
ADD CONSTRAINT FK_Truck_InvoiceLines_Truck_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Truck_InvoiceHeaders (id) ON DELETE CASCADE
GO

--- Truck Invoice Headers
IF (OBJECT_ID('FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_InvoiceHeaders
  DROP CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK
GO
ALTER TABLE dbo.Truck_InvoiceHeaders
ADD CONSTRAINT FK_Truck_InvoiceHeaders_Truck_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

--- Truck MISC
IF (OBJECT_ID('[FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_MISC
  DROP CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_MISC
ADD CONSTRAINT [FK_dbo.Truck_MISC.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

--- Truck Declaration Tab
IF (OBJECT_ID('[FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_DeclarationTab
  DROP CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_DeclarationTab
ADD CONSTRAINT [FK_dbo.Truck_DeclarationTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

--- Truck Containers Tab
IF (OBJECT_ID('[FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_ContainersTab
  DROP CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_ContainersTab
ADD CONSTRAINT [FK_dbo.Truck_ContainersTab_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

--- Truck Documents
IF (OBJECT_ID('[FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Truck_Documents
  DROP CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Truck_Documents
ADD CONSTRAINT [FK_dbo.Truck_Documents_dbo.Truck_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Truck_Filing_Headers (id) ON DELETE CASCADE
GO

--- add triggers ---
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

--- add new Truck filing procedures ---
CREATE PROCEDURE dbo.truck_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + s.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.default_value, '') + ''' as ' +
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
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.truck_def_values v
  INNER JOIN dbo.truck_sections s
    ON v.section_id = s.id
  LEFT JOIN information_schema.columns i
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(s.table_name)
  WHERE UPPER(s.table_name) = UPPER(@tableName)
  AND i.table_schema = 'dbo'
  AND v.has_default_value = 1

  EXEC (@str);
END
GO

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
  OR v.display_on_ui > 0)
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

  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_def_values_manual tdvm
      WHERE tdvm.record_id = @recordId
      AND tdvm.table_name = @tableName
      AND tdvm.column_name = @columnName)
  BEGIN
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
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

CREATE PROCEDURE dbo.truck_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add declarationTab data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_DeclarationTab declarationTab
      WHERE declarationTab.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
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
        WHERE details.Filing_Headers_FK = @filingHeaderId)
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
      SELECT
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
       ,firms_code
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_DeclarationTab declarationTab
    WHERE declarationTab.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.truck_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add сontainersTab data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_ContainersTab сontainersTab
      WHERE сontainersTab.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.BDP_FK AS TI_FK
         ,details.FILING_HEADERS_FK AS Filing_Headers_FK
        FROM Truck_Filing_Details details
        INNER JOIN Truck_Inbound inbound
          ON details.BDP_FK = inbound.id
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_ContainersTab (
        Filing_Headers_FK
       ,TI_FK)
      SELECT
        Filing_Headers_FK
       ,TI_FK       
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_ContainersTab сontainersTab
    WHERE сontainersTab.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.truck_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_MISC misc
      WHERE misc.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,details.BDP_FK AS TI_FK
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
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    INSERT INTO dbo.Truck_MISC (
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,Recon_Issue
       ,FTA_Recon)
      SELECT
        Filing_Headers_FK
       ,TI_FK
       ,Branch
       ,Broker
       ,Location
       ,recon_issue
       ,nafta_recon
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_MISC misc
    WHERE misc.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.truck_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add Invoice Lines data and apply rules  
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_InvoiceLines invoiceLines
      WHERE invoiceLines.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
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
        WHERE details.Filing_Headers_FK = @filingHeaderId)
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
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price)
      SELECT
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
       ,UQ
       ,PriceUnit
       ,Prod_ID_1
       ,Attribute_1
       ,Attribute_2
       ,Invoice_Qty
       ,Invoice_Qty_Unit
       ,Amount
       ,Line_Price
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_InvoiceLines invoiceLines
    WHERE invoiceLines.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.truck_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'Truck_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = sections.is_array
  FROM truck_sections sections
  WHERE sections.table_name = @tableName

  -- add invoiceHeaders data and apply rules
  -- invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total -- replace with computed field
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Truck_InvoiceHeaders invoiceHeaders
      WHERE invoiceHeaders.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,inbound.PAPs AS Invoice_No
         ,ruleImporters.cw_supplier AS Supplier
         ,ruleImporters.cw_ior AS Consignee
         ,ruleImporters.transactions_related AS Transaction_Related
         ,ruleImporters.cw_supplier AS Manufacturer
         ,ruleImporters.cw_supplier AS Seller
         ,ruleImporters.cw_ior AS Importer
         ,ruleImporters.cw_ior AS Sold_to_party
         ,ruleImporters.cw_ior AS Ship_to_party
         ,ruleimporters.ce AS Export
        FROM dbo.Truck_Filing_Details details
        INNER JOIN dbo.Truck_Inbound inbound
          ON inbound.id = details.BDP_FK
        LEFT JOIN Truck_Rule_Importers ruleImporters
          ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.ior))
        WHERE details.Filing_Headers_FK = @filingHeaderId)
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
       ,Export)
      SELECT
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
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Truck_InvoiceHeaders invoiceHeaders
    WHERE invoiceHeaders.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.truck_apply_def_values @tableName
                                 ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_add_def_values_manual @tableName
                                      ,@filingHeaderId
                                      ,@parentId
                                      ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_def_values_manual'
                                   ,@tableName
                                   ,@recordId
  -- add invoice line
  EXEC dbo.truck_add_invoice_line_record @filingHeaderId
                                        ,@recordId
                                        ,@filingUser

  RETURN @recordId
END;
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

--- update existing Truck Filing procedure ---
ALTER PROCEDURE dbo.truck_filing (@filingHeaderId INT,
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

ALTER PROCEDURE dbo.truck_filing_param (@Filing_Headers_id INT)
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
  LEFT JOIN dbo.truck_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO

ALTER PROCEDURE dbo.truck_filing_del (@Filing_Headers_id INT)
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