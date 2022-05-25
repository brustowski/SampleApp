-- create section table ---
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

CREATE UNIQUE INDEX IX_name
ON dbo.pipeline_sections (name)
ON [PRIMARY]
GO

CREATE INDEX IX_parent_id
ON dbo.pipeline_sections (parent_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.pipeline_sections
ADD CONSTRAINT FK_pipeline_sections_parent_id FOREIGN KEY (parent_id) REFERENCES dbo.pipeline_sections (id)
GO

SET IDENTITY_INSERT dbo.pipeline_sections ON
INSERT dbo.pipeline_sections(id, name, title, table_name, is_array, parent_id, procedure_name) VALUES 
  (1, 'root', 'Root', NULL, CONVERT(bit, 'False'), NULL, NULL)
  ,(2, 'declaration', 'Declaration', 'Pipeline_DeclarationTab', CONVERT(bit, 'False'), 1, 'pipeline_add_declaration_record')
  ,(3, 'invoice', 'Invoices', NULL, CONVERT(bit, 'False'), 1, NULL)
  ,(4, 'invoice_header', 'Invoice', 'Pipeline_InvoiceHeaders', CONVERT(bit, 'True'), 3, 'pipeline_add_invoice_header_record')
  ,(5, 'invoice_line', 'Line', 'Pipeline_InvoiceLines', CONVERT(bit, 'True'), 4, 'pipeline_add_invoice_line_record')
  ,(6, 'container', 'Container', 'Pipeline_ContainersTab', CONVERT(bit, 'False'), 1, 'pipeline_add_container_record')
  ,(7, 'misc', 'MISC', 'Pipeline_MISC', CONVERT(bit, 'False'), 1, 'pipeline_add_misc_record')
SET IDENTITY_INSERT dbo.pipeline_sections OFF
GO

--- update Pipeline DefValues table ---
ALTER TABLE dbo.Pipeline_DEFValues
  ADD section_id int NULL
GO

UPDATE dbo.Pipeline_DEFValues
SET section_id = ps.id
FROM dbo.Pipeline_DEFValues AS pdv
INNER JOIN dbo.pipeline_sections ps
  ON pdv.TableName = ps.table_name

ALTER TABLE dbo.Pipeline_DEFValues
ALTER COLUMN section_id INT NOT NULL
GO

CREATE INDEX IX_section_id
ON dbo.Pipeline_DEFValues (section_id)
ON [PRIMARY]
GO

ALTER TABLE dbo.Pipeline_DEFValues
ADD CONSTRAINT [FK_dbo.Pipeline_DEFValues_dbo.pipeline_sections_section_id] FOREIGN KEY (section_id) REFERENCES dbo.pipeline_sections (id)
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues' AND COLUMN_NAME = 'UI_Section' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues
  DROP COLUMN UI_Section
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues' AND COLUMN_NAME = 'TableName' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues
  DROP COLUMN TableName
END
GO

--- update Pipeline DefValues view ---
ALTER VIEW dbo.v_Pipeline_DEFValues
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

--- update Pipeline DefValues Manual table ---
-- add new columns
ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD parent_record_id INT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD section_name VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD section_title VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD record_id INT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD [description] VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD table_name VARCHAR(128)  NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD column_name VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD [value] VARCHAR(512) NULL
GO

-- update data
UPDATE pdm 
SET pdm.section_name = ps.name
  ,pdm.section_title = ps.title
  ,pdm.table_name = pdm.TableName
  ,pdm.column_name = pdm.ColName
  ,pdm.value = pdm.DefValue
  FROM Pipeline_DEFValues_Manual pdm
  INNER JOIN pipeline_sections ps
  ON pdm.TableName = ps.table_name
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Pipeline_DEFValues_Manual dest
INNER JOIN Pipeline_DeclarationTab source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Pipeline_DeclarationTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Pipeline_DEFValues_Manual dest
INNER JOIN Pipeline_ContainersTab source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Pipeline_ContainersTab'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Pipeline_DEFValues_Manual dest
INNER JOIN Pipeline_MISC source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Pipeline_MISC'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.Filing_Headers_FK
FROM Pipeline_DEFValues_Manual dest
INNER JOIN Pipeline_InvoiceHeaders source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Pipeline_InvoiceHeaders'
GO

UPDATE dest
SET dest.record_id = source.id
   ,dest.parent_record_id = source.InvoiceHeaders_FK
FROM Pipeline_DEFValues_Manual dest
INNER JOIN Pipeline_InvoiceLines source
  ON dest.Filing_Headers_FK = source.Filing_Headers_FK
WHERE dest.table_name = 'Pipeline_InvoiceLines'
GO
-- update new columns
ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN parent_record_id INT NOT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN section_name VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN section_title VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN record_id INT NOT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN table_name VARCHAR(128) NOT NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ALTER COLUMN column_name VARCHAR(128) NOT NULL
GO

-- drop old columns
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'UI_Section' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN UI_Section
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'ColName' AND TABLE_SCHEMA = 'DBO') 
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN ColName
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'TableName' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN TableName
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'DefValue' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN DefValue
END
GO

-- delete records withot corresponding result record
DELETE dbo.Pipeline_DEFValues_Manual
WHERE record_id IS NULL

-- add index
CREATE INDEX Idx_FilingHeadersFK
ON dbo.Pipeline_DEFValues_Manual (Filing_Headers_FK)
ON [PRIMARY]
GO

--- update Pipeline DefValues Manual view ---
ALTER VIEW dbo.v_Pipeline_DEFValues_Manual 
AS SELECT
  v.id
 ,v.Filing_Headers_FK AS filing_header_id
 ,v.CreatedDate AS created_date
 ,v.CreatedUser AS created_user
 ,v.Display_on_UI AS display_on_ui
 ,v.FEditable AS editable
 ,v.FHasDefaultVal AS has_default_value
 ,v.FMandatory AS mandatory
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

--- update Pipeline Invoice Header table ---
IF OBJECT_ID('FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK') IS NOT NULL
ALTER TABLE dbo.Pipeline_InvoiceHeaders
  DROP CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK
GO

DECLARE @res int
EXEC @res = sp_rename N'dbo.Pipeline_InvoiceHeaders', N'tmp_Pipeline_InvoiceHeaders', 'OBJECT'
IF @res <> 0
  RAISERROR ('Error while Rename table [dbo].[Pipeline_InvoiceHeaders] to [dbo].[tmp_Pipeline_InvoiceHeaders]', 11, 1 );
GO

CREATE TABLE dbo.Pipeline_InvoiceHeaders (
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

SET IDENTITY_INSERT dbo.Pipeline_InvoiceHeaders ON

INSERT dbo.Pipeline_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Pipeline_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Pipeline_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Pipeline_InvoiceHeaders

SET IDENTITY_INSERT dbo.Pipeline_InvoiceHeaders OFF
GO

ALTER TABLE dbo.Pipeline_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Pipeline_InvoiceHeaders
  ADD CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- update constraints 
--- Pipeline Invoice Lines
ALTER TABLE dbo.Pipeline_InvoiceLines
ADD CONSTRAINT FK_Pipeline_InvoiceLines_Pipeline_InvoiceHeaders_InvoiceHeaders_FK FOREIGN KEY (InvoiceHeaders_FK) REFERENCES dbo.Pipeline_InvoiceHeaders (id) ON DELETE CASCADE
GO

--- Pipeline Invoice Headers
IF (OBJECT_ID('FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_InvoiceHeaders
  DROP CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK
GO
ALTER TABLE dbo.Pipeline_InvoiceHeaders
ADD CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

--- Pipeline MISC
IF (OBJECT_ID('[FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_MISC
  DROP CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_MISC
ADD CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

--- Pipeline Declaration Tab
IF (OBJECT_ID('[FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_DeclarationTab
  DROP CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_DeclarationTab
ADD CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

--- Pipeline Containers Tab
IF (OBJECT_ID('[FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_ContainersTab
  DROP CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_ContainersTab
ADD CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
GO

--- Pipeline Documents
IF (OBJECT_ID('[FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_Documents
  DROP CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_Documents
ADD CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id) ON DELETE CASCADE
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

--- add new Pipeline filing procedures ---
CREATE PROCEDURE dbo.pipeline_apply_def_values (@tableName VARCHAR(128)
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
  FROM dbo.Pipeline_DEFValues v
  INNER JOIN dbo.pipeline_sections s
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
      FROM dbo.Pipeline_DEFValues_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
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
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

CREATE PROCEDURE dbo.pipeline_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_DeclarationTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_DeclarationTab pdt
      WHERE pdt.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ruleImporters.Supplier AS Main_Supplier
         ,@ImporterCode AS Importer
         ,rulePorts.Issuer AS Issuer
         ,REPLACE(inbound.TicketNumber, '-', '') AS Batch_Ticket
         ,rulePorts.Pipeline AS Pipeline
         ,rulePorts.Issuer AS Carrier_SCAC
         ,rulePorts.port AS Discharge
         ,rulePorts.port AS Entry_Port
         ,inbound.ImportDate AS Dep
         ,inbound.ImportDate AS Arr
         ,inbound.ImportDate AS Arr_2
         ,rulePorts.Origin AS Origin
         ,rulePorts.Destination AS Destination
         ,rulePorts.Destination_State AS Destination_State
         ,inbound.ImportDate AS ETA
         ,inbound.ImportDate AS Export_Date
         ,CONCAT(rulePorts.customs_attribute3, ': ', inbound.Batch) AS Description
         ,inbound.TicketNumber AS Owner_Ref
         ,rulePorts.FIRMs_Code AS FIRMs_Code
         ,REPLACE(inbound.TicketNumber, '-', '') AS Master_Bill
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON (
              (--we have both tofacility-port and port-importer rule associated with record
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (
                  LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)
                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        WHERE details.Filing_Headers_FK = @filingHeaderId)

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
       ,Importer_of_record)
      SELECT
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
       ,@ImporterCode
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_DeclarationTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.pipeline_add_container_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_ContainersTab'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

  --Get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)

  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add container data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_ContainersTab pct
      WHERE pct.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,ROUND(inbound.Quantity, 0) AS Manifest_QTY
         ,REPLACE(inbound.TicketNumber, '-', '') AS Bill_Num
         ,rulePorts.Issuer AS Bill_Issuer_SCAC
        FROM Pipeline_Filing_Details details
        INNER JOIN Pipeline_Inbound inbound
          ON details.Pipeline_Inbounds_FK = inbound.id
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON ( --we have both tofacility-port and port-importer rule associated with record
              (
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)

                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_ContainersTab (
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Manifest_QTY
       ,Bill_Num
       ,Bill_Issuer_SCAC
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_ContainersTab pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.pipeline_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_MISC'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;  
  DECLARE @ImporterCode VARCHAR(128);

  -- get importer code for the inbound importer
  SELECT
    @ImporterCode = dbo.fn_pipeline_GetImporterCode(@filingHeaderId)


  -- get section property is_array
  SELECT
    @allowMultiple = ps.is_array
  FROM pipeline_sections ps
  WHERE ps.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_MISC pm
      WHERE pm.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Pipeline_Inbounds_FK AS PI_FK
         ,details.Filing_Headers_FK
         ,userData.Branch
         ,userData.Broker
         ,userData.Location
         ,ruleImporters.recon_issue
         ,ruleImporters.fta_recon
         ,ruleImporters.payment_type
         ,ruleImporters.broker_to_pay
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN app_users_data userData
          ON userData.UserAccount = @filingUser
        WHERE details.Filing_Headers_FK = @filingHeaderId)

    INSERT INTO Pipeline_MISC (
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Preparer_Dist_Port
       ,recon_issue
       ,fta_recon
       ,payment_type
       ,broker_to_pay)
      SELECT
        Filing_Headers_FK
       ,PI_FK
       ,Branch
       ,Broker
       ,Location
       ,recon_issue
       ,fta_recon
       ,payment_type
       ,broker_to_pay
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_MISC pdt
    WHERE pdt.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.pipeline_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceLines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_InvoiceLines pil
      WHERE pil.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT
          details.Filing_Headers_FK
         ,@parentId AS InvoiceHeaders_FK
         ,details.Pipeline_Inbounds_FK AS PI_FK
         ,inbound.Batch AS Invoice_No -- ?? is it ok?
         ,ruleImporters.Transaction_Related AS Transaction_Related
         ,ruleAPI.Tariff AS Tariff
         ,inbound.Quantity AS Customs_QTY
         ,CONCAT(ruleBatch.product, ' - ', ruleBatch.batch_code) AS Goods_Description
         ,ruleImporters.SPI AS SPI
         ,dbo.fn_pipeline_weight(inbound.Quantity, inbound.API) AS Gr_weight
         ,ruleImporters.value AS PriceUnit
         ,inbound.Batch AS Attribute_1
         ,'API Gravity @ 60 F° = "' + CONVERT(VARCHAR(128), inbound.API) + '"' AS Attribute_2
         ,rulePorts.customs_attribute3 AS Attribute_3
         ,inbound.Quantity AS Invoice_Qty
         ,ruleImporters.origin AS ORG
         ,inbound.Quantity * ruleImporters.value AS Line_Price
         ,inbound.Quantity * ruleImporters.freight AS Amount
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.origin AS Origin
         ,ruleImporters.country_of_export AS Export
         ,rulePorts.Destination_State AS Destination_State
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Facility ruleFacility
          ON inbound.facility = ruleFacility.facility
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        LEFT JOIN Pipeline_Rule_PortImporter rulePorts
          ON (
              --we have both tofacility-port and port-importer rule associated with record
              (
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
              )
              OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                AND (LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                  AND @ImporterCode NOT IN (SELECT
                      Importer
                    FROM Pipeline_Rule_PortImporter
                    WHERE port = ruleFacility.port)
                )
              )
              OR (--we dont have tofacility-port rule but port-importer rule exist for the record
                RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = ''
              )
            )
        LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
          ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
        LEFT JOIN @tariffs t
          ON inbound.id = t.pi
        LEFT JOIN Pipeline_Rule_API ruleAPI
          ON t.Tariff = ruleAPI.id
        WHERE details.Filing_Headers_FK = @filingHeaderId)

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
      SELECT
        Filing_Headers_FK
       ,InvoiceHeaders_FK
       ,PI_FK
       ,Invoice_No
       ,Transaction_Related
       ,Tariff
       ,Customs_QTY
       ,Goods_Description
       ,SPI
       ,Gr_weight
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
       ,Sold_To_Party
       ,Origin
       ,Export
       ,Destination_State
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_InvoiceLines pil
    WHERE pil.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END;
GO

CREATE PROCEDURE dbo.pipeline_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Pipeline_InvoiceHeaders'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT;
  DECLARE @ImporterCode VARCHAR(128);

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
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Pipeline_InvoiceHeaders pih
      WHERE pih.Filing_Headers_FK = @parentId)
    OR @allowMultiple = 1
  BEGIN
    WITH RichData
    AS
      (SELECT DISTINCT
          details.Filing_Headers_FK AS Filing_Headers_FK
         ,inbound.Batch AS Invoice_No
         ,ruleImporters.supplier AS supplier
         ,inbound.Quantity * ruleImporters.value AS Invoice_Total
         ,ruleImporters.Origin AS Origin
         ,ruleImporters.Consignee AS Consignee
         ,ruleImporters.transaction_related AS transaction_related
         ,ruleImporters.supplier AS Manufacturer
         ,ruleImporters.supplier AS Seller
         ,@ImporterCode AS Importer
         ,ruleImporters.Consignee AS Sold_To_Party
         ,ruleImporters.Consignee AS Ship_To_Party
         ,ruleImporters.seller AS Seller_Address
         ,ruleImporters.country_of_export AS Export
        FROM dbo.Pipeline_Filing_Details details
        INNER JOIN dbo.Pipeline_Inbound inbound
          ON inbound.Id = details.Pipeline_Inbounds_FK
        LEFT JOIN Pipeline_Rule_Importer ruleImporters
          ON RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        WHERE details.Filing_Headers_FK = @filingHeaderId)

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
       ,Export)
      SELECT
        Filing_Headers_FK
       ,Invoice_No
       ,supplier
       ,Invoice_Total
       ,Origin
       ,Consignee
       ,transaction_related
       ,Manufacturer
       ,Seller
       ,Importer
       ,Sold_To_Party
       ,Ship_To_Party
       ,Seller_Address
       ,Export
      FROM RichData
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM Pipeline_InvoiceHeaders pih
    WHERE pih.Filing_Headers_FK = @parentId
  END

  -- apply default values
  EXEC dbo.pipeline_apply_def_values @tableName
                                    ,@recordId

  -- fill the def value manual table
  EXEC dbo.pipeline_add_def_values_manual @tableName
                                         ,@filingHeaderId
                                         ,@parentId
                                         ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'Pipeline_DEFValues_Manual'
                                   ,@tableName
                                   ,@recordId

  -- add invoice line
  EXEC dbo.pipeline_add_invoice_line_record @filingHeaderId
                                               ,@recordId
                                               ,@filingUser
  RETURN @recordId
END;
GO

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

--- update existing Pipeline filing procedure ---
ALTER PROCEDURE dbo.pipeline_filing (@filingHeaderId INT,
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

ALTER PROCEDURE dbo.pipeline_filing_param (@Filing_Headers_id INT)
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

  EXEC (@str);
END
GO

ALTER PROCEDURE dbo.pipeline_filing_del (@Filing_Headers_id INT)
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