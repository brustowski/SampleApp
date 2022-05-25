--- create sections table ---
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

CREATE UNIQUE INDEX IX_name
ON dbo.rail_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.rail_sections (parent_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.rail_sections
ADD CONSTRAINT FK_rail_sections_parent_id FOREIGN KEY (parent_id) REFERENCES dbo.rail_sections (id)
GO

SET IDENTITY_INSERT dbo.rail_sections ON
INSERT dbo.rail_sections(id, name, title, table_name, is_array, parent_id, procedure_name) VALUES 
  (1, 'root', 'Root', NULL, CONVERT(bit, 'False'), NULL, NULL)
  ,(2, 'declaration', 'Declaration', 'Rail_DeclarationTab', CONVERT(bit, 'False'), 1, 'rail_add_declaration_record')
  ,(3, 'invoice', 'Invoices', NULL, CONVERT(bit, 'False'), 1, NULL)
  ,(4, 'invoice_header', 'Invoice', 'Rail_InvoiceHeaders', CONVERT(bit, 'True'), 3, 'rail_add_invoice_header_record')
  ,(5, 'invoice_line', 'Line', 'Rail_InvoiceLines', CONVERT(bit, 'True'), 4, 'rail_add_invoice_line_record')
  ,(6, 'container', 'Container', 'Rail_ContainersTab', CONVERT(bit, 'False'), 1, 'rail_add_container_record')
  ,(7, 'misc', 'MISC', 'Rail_MISC', CONVERT(bit, 'False'), 1, 'rail_add_misc_record')
SET IDENTITY_INSERT dbo.rail_sections OFF
GO

--- update Rail DefValues table ---
ALTER TABLE dbo.Rail_DEFValues
  ADD section_id int NULL
GO

UPDATE dbo.Rail_DEFValues
SET section_id = ps.id
FROM dbo.Rail_DEFValues AS pdv
INNER JOIN dbo.rail_sections ps
  ON pdv.TableName = ps.table_name

ALTER TABLE dbo.Rail_DEFValues
ALTER COLUMN section_id INT NOT NULL
GO

CREATE INDEX IX_section_id
ON dbo.Rail_DEFValues (section_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.Rail_DEFValues
ADD CONSTRAINT FK_Rail_DEFValues__rail_sections__section_id FOREIGN KEY (section_id) REFERENCES dbo.rail_sections (id)
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues' AND COLUMN_NAME = 'UI_Section' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues
  DROP COLUMN UI_Section
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues' AND COLUMN_NAME = 'TableName' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues
  DROP COLUMN TableName
END
GO

--- update Rail DefValues view ---
ALTER VIEW dbo.v_Rail_DEFValues
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

--- update Rail DefValues Manual table ---
-- add new columns
ALTER TABLE dbo.Rail_DEFValues_Manual
ADD parent_record_id INT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD section_name VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD section_title VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD record_id INT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD [description] VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD table_name VARCHAR(128)  NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD column_name VARCHAR(128) NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ADD [value] VARCHAR(512) NULL
GO

-- update data
UPDATE pdm 
SET pdm.section_name = ps.name
  ,pdm.section_title = ps.title
  ,pdm.table_name = pdm.TableName
  ,pdm.column_name = pdm.ColName
  ,pdm.value = pdm.DefValue
  FROM Rail_DEFValues_Manual pdm
  INNER JOIN rail_sections ps
  ON pdm.TableName = ps.table_name
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Rail_DEFValues_Manual dest
INNER JOIN Rail_DeclarationTab source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Rail_DeclarationTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Rail_DEFValues_Manual dest
INNER JOIN Rail_ContainersTab source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Rail_ContainersTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Rail_DEFValues_Manual dest
INNER JOIN Rail_MISC source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Rail_MISC'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Rail_DEFValues_Manual dest
INNER JOIN Rail_InvoiceHeaders source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Rail_InvoiceHeaders'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.InvoiceHeaders_FK
FROM Rail_DEFValues_Manual dest
INNER JOIN Rail_InvoiceLines source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Rail_InvoiceLines'
GO

-- delete records without corresponding result record
DELETE dbo.Rail_DEFValues_Manual
WHERE record_id IS NULL

-- update new columns
ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN parent_record_id INT NOT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN section_name VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN section_title VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN record_id INT NOT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN table_name VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Rail_DEFValues_Manual
ALTER COLUMN column_name VARCHAR(128) NOT NULL
GO

-- drop old columns
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'UI_Section' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN UI_Section
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'ColName' AND TABLE_SCHEMA = 'DBO') 
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN ColName
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'TableName' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN TableName
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rail_DEFValues_Manual' AND COLUMN_NAME = 'DefValue' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Rail_DEFValues_Manual
  DROP COLUMN DefValue
END
GO

-- add index
CREATE INDEX Idx_FilingHeadersFK
ON dbo.Rail_DEFValues_Manual (Filing_Headers_FK)
ON [PRIMARY]
GO

--- update Rail DefValues Manual view ---
ALTER VIEW dbo.v_Rail_DEFValues_Manual 
AS SELECT
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
OR i.COLUMN_NAME IS NULL
GO

--- update Rail Tables view ---
IF OBJECT_ID(N'dbo.v_Rail_Tables ', 'V') IS NOT NULL
  DROP VIEW dbo.v_Rail_Tables 
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
INNER JOIN truck_sections s
  ON i.TABLE_NAME = s.table_name
WHERE i.TABLE_SCHEMA = 'dbo'
AND UPPER(column_name) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
GO

--- update Rail Invoice Header table ---
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

SET IDENTITY_INSERT dbo.Rail_InvoiceHeaders ON

INSERT dbo.Rail_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Rail_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Rail_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Rail_InvoiceHeaders

SET IDENTITY_INSERT dbo.Rail_InvoiceHeaders OFF
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Rail_InvoiceHeaders
  ADD CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id)
GO

--- update constraints 
--- Rail Invoice Lines
ALTER TABLE dbo.Rail_InvoiceLines
ADD CONSTRAINT FK_Rail_InvoiceLines_Rail_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Rail_InvoiceHeaders (id) ON DELETE CASCADE
GO

--- Rail Invoice Headers
IF (OBJECT_ID('FK_Rail_InvoiceHeaders_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_InvoiceHeaders
  DROP CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers
GO
ALTER TABLE dbo.Rail_InvoiceHeaders
ADD CONSTRAINT FK_Rail_InvoiceHeaders_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

--- Rail MISC
IF (OBJECT_ID('FK_Rail_Rail_MISC_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_MISC
  DROP CONSTRAINT FK_Rail_Rail_MISC_Filing_Headers
GO
ALTER TABLE dbo.Rail_MISC
ADD CONSTRAINT FK_Rail_Rail_MISC_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

--- Rail Declaration Tab
IF (OBJECT_ID('FK_RAIL_DEC_REFERENCE_RAIL_FIL', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_DeclarationTab
  DROP CONSTRAINT FK_RAIL_DEC_REFERENCE_RAIL_FIL
GO
ALTER TABLE dbo.Rail_DeclarationTab
ADD CONSTRAINT FK_RAIL_DEC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

--- Rail Containers Tab
IF (OBJECT_ID('FK_Rail_Packing_Filing_Headers', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_ContainersTab
  DROP CONSTRAINT FK_Rail_Packing_Filing_Headers
GO
ALTER TABLE dbo.Rail_ContainersTab
ADD CONSTRAINT FK_Rail_Packing_Filing_Headers FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

--- Rail Documents
IF (OBJECT_ID('FK_RAIL_DOC_REFERENCE_RAIL_FIL', 'F')) IS NOT NULL
  ALTER TABLE dbo.Rail_Documents
  DROP CONSTRAINT FK_RAIL_DOC_REFERENCE_RAIL_FIL
GO
ALTER TABLE dbo.Rail_Documents
ADD CONSTRAINT FK_RAIL_DOC_REFERENCE_RAIL_FIL FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Rail_Filing_Headers (id) ON DELETE CASCADE
GO

--- add triggers ---
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

--- add new Rail filing procedures ---
CREATE PROCEDURE dbo.rail_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + s.table_name + ' set ' + v.ColName + ' = try_cast(''' + ISNULL(v.DefValue, '') + ''' as ' +
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
  FROM dbo.Rail_DEFValues v
  INNER JOIN dbo.rail_sections s
    ON v.section_id = s.id
  LEFT JOIN information_schema.columns i
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(s.table_name)
  WHERE UPPER(s.table_name) = UPPER(@tableName)
  AND i.table_schema = 'dbo'
  AND v.FHasDefaultVal = 1

  EXEC (@str);
END
GO

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
  OR v.Display_on_UI > 0)
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
      FROM dbo.Rail_DEFValues_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
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
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

CREATE PROCEDURE dbo.rail_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_DeclarationTab (
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
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_DeclarationTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.rail_apply_def_values @tableName
                                ,@recordId

  -- fill the def value manual table
  EXEC dbo.rail_add_def_values_manual @tableName
                                     ,@filingHeaderId
                                     ,@parentId
                                     ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.rail_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.BDP_FK
         ,p.IssuerCode AS Bill_Issuer_SCAC
         ,p.BillofLading AS Bill_Num
         ,CONCAT('MB:', p.BillofLading) AS Bill_Number
         ,CONCAT(EquipmentInitial, EquipmentNumber) AS Container_Number
         ,details.FILING_HEADERS_FK
        FROM dbo.Rail_Filing_Details details
        INNER JOIN dbo.Rail_BD_Parsed p
          ON p.BDP_PK = details.BDP_FK
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_ContainersTab (
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,FILING_HEADERS_FK)
      SELECT
        BDP_FK
       ,Bill_Issuer_SCAC
       ,Bill_Num
       ,Bill_Number
       ,Container_Number
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_ContainersTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.rail_apply_def_values @tableName
                                ,@recordId

  -- fill the def value manual table
  EXEC dbo.rail_add_def_values_manual @tableName
                                     ,@filingHeaderId
                                     ,@parentId
                                     ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.rail_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_MISC (
        BDP_FK
       ,Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK)
      SELECT
        BDP_FK
       ,Recon_Issue
       ,FTA_Recon
       ,Payment_Type
       ,Broker_to_Pay
       ,Submitter
       ,Filing_Headers_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_MISC pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.rail_apply_def_values @tableName
                                ,@recordId

  -- fill the def value manual table
  EXEC dbo.rail_add_def_values_manual @tableName
                                     ,@filingHeaderId
                                     ,@parentId
                                     ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.rail_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          d.BDP_FK
         ,@parentId AS InvoiceHeaders_FK
         ,rd.Attribute_1 AS Attribute_1
         ,rd.Attribute_2 AS Attribute_2
         ,p.Weight AS Gr_Weight
         ,p.WeightUnit AS Gr_Weight_Unit
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
        WHERE d.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Rail_InvoiceLines (
        BDP_FK
       ,InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Gr_Weight
       ,Gr_Weight_Unit
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
      SELECT
        BDP_FK
       ,InvoiceHeaders_FK
       ,Attribute_1
       ,Attribute_2
       ,Gr_Weight
       ,Gr_Weight_Unit
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
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_InvoiceLines pil
    WHERE pil.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.rail_apply_def_values @tableName
                                ,@recordId

  -- fill the def value manual table
  EXEC dbo.rail_add_def_values_manual @tableName
                                     ,@filingHeaderId
                                     ,@parentId
                                     ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.rail_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Rail_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM rail_sections ps
  WHERE ps.table_name = @tableName

  -- add invoice header data and apply rules  
  -- ,ri.PriceUnit * ri.Customs_QTY AS Invoice_Total -- replace with computed field
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Rail_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT DISTINCT
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
          ON p.Importer = rn.Importer_Name
            AND (p.Supplier = rn.Supplier_Name
              OR (p.Supplier IS NULL
                AND rn.Supplier_Name IS NULL))
        WHERE d.Filing_Headers_FK = @filingHeaderId)

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
      SELECT
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
       ,FILING_HEADERS_FK
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Rail_InvoiceHeaders pih
    WHERE pih.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.rail_apply_def_values @tableName
                                ,@recordId

  -- fill the def value manual table
  EXEC dbo.rail_add_def_values_manual @tableName
                                     ,@filingHeaderId
                                     ,@parentId
                                     ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Rail_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  -- add invoice line
  EXEC dbo.rail_add_invoice_line_record @filingHeaderId
                                       ,@recordId
                                       ,@filingUser
  RETURN @recordId
END;
GO

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

--- update existing Rail filing procedure ---
ALTER PROCEDURE dbo.rail_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.rail_add_declaration_record @filingHeaderId
                                          ,@filingHeaderId
                                          ,@filingUser
  -- add invoice header
  EXEC dbo.rail_add_invoice_header_record @filingHeaderId
                                             ,@filingHeaderId
                                             ,@filingUser
  -- add container
  EXEC dbo.rail_add_container_record @filingHeaderId
                                        ,@filingHeaderId
                                        ,@filingUser
  -- add misc
  EXEC dbo.rail_add_misc_record @filingHeaderId
                                   ,@filingHeaderId
                                   ,@filingUser
END;
GO

ALTER PROCEDURE dbo.rail_filing_param (@Filing_Headers_id INT)
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
  LEFT JOIN dbo.Rail_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.Filing_Headers_FK = @Filing_Headers_id

  EXEC (@str);
END
GO

ALTER PROCEDURE dbo.rail_filing_del (@Filing_Headers_id INT)
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