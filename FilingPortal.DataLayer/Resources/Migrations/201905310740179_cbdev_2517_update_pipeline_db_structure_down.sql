--- delete unneseccary Pipeline filing procuders --- 
IF OBJECT_ID('dbo.pipeline_add_invoice_header_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_invoice_header_record
GO
IF OBJECT_ID('dbo.pipeline_add_invoice_line_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_invoice_line_record
GO

IF OBJECT_ID('dbo.pipeline_add_misc_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_misc_record
GO

IF OBJECT_ID('dbo.pipeline_add_container_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_container_record
GO

IF OBJECT_ID('dbo.pipeline_add_declaration_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_declaration_record
GO

IF OBJECT_ID('dbo.pipeline_add_def_values_manual') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_add_def_values_manual
GO

IF OBJECT_ID('dbo.pipeline_apply_def_values') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_apply_def_values
GO

IF OBJECT_ID('dbo.pipeline_delete_record') IS NOT NULL
  DROP PROCEDURE dbo.pipeline_delete_record
GO

--- delete triggers ---
IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.pipeline_invoice_lines_befor_delete'))
  DROP TRIGGER dbo.pipeline_invoice_lines_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.pipeline_invoice_headers_befor_delete'))
  DROP TRIGGER dbo.pipeline_invoice_headers_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.pipeline_misc_befor_delete'))
  DROP TRIGGER dbo.pipeline_misc_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.pipeline_containers_tab_befor_delete'))
  DROP TRIGGER dbo.pipeline_containers_tab_befor_delete
GO

IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'dbo.pipeline_declaration_tab_befor_delete'))
  DROP TRIGGER dbo.pipeline_declaration_tab_befor_delete
GO

--- restore constraints 
--- Pipeline Invoice Lines
IF (OBJECT_ID('FK_Pipeline_InvoiceLines_Pipeline_InvoiceHeaders_InvoiceHeaders_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_InvoiceLines
  DROP CONSTRAINT FK_Pipeline_InvoiceLines_Pipeline_InvoiceHeaders_InvoiceHeaders_FK
GO

--- Pipeline Invoice Headers
IF (OBJECT_ID('FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_InvoiceHeaders
  DROP CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK
GO
ALTER TABLE dbo.Pipeline_InvoiceHeaders
ADD CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- Pipeline MISC
IF (OBJECT_ID('[FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_MISC
  DROP CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_MISC
ADD CONSTRAINT [FK_dbo.Pipeline_MISC.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- Pipeline Declaration Tab
IF (OBJECT_ID('[FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_DeclarationTab
  DROP CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_DeclarationTab
ADD CONSTRAINT [FK_dbo.Pipeline_DeclarationTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- Pipeline Containers Tab
IF (OBJECT_ID('[FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_ContainersTab
  DROP CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
  GO
ALTER TABLE dbo.Pipeline_ContainersTab
ADD CONSTRAINT [FK_dbo.Pipeline_ContainersTab_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- Pipeline Documents
IF (OBJECT_ID('[FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]', 'F')) IS NOT NULL
  ALTER TABLE dbo.Pipeline_Documents
  DROP CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK]
GO
ALTER TABLE dbo.Pipeline_Documents
ADD CONSTRAINT [FK_dbo.Pipeline_Documents_dbo.Pipeline_Filing_Headers_Filing_Headers_FK] FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- restore Pipeline Invoice Header table ---
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

INSERT dbo.Pipeline_InvoiceHeaders(id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser)
  SELECT id, Filing_Headers_FK, Invoice_No, Supplier, Supplier_Address, INCO, Invoice_Total, Curr, Origin, Payment_Date, Consignee, Consignee_Address, Inv_Date, Agreed_Place, Inv_Gross_Weight, Net_Weight, Export, Export_Date, First_Sale, Transaction_Related, Packages, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Broker_PGA_Contact_Name, Broker_PGA_Contact_Phone, Broker_PGA_Contact_Email, EPA_PST_Cert_Date, EPA_TSCA_Cert_Date, EPA_VNE_Cert_Date, FSIS_Cert_Date, FWS_Cert_Date, LACEY_ACT_Cert_Date, NHTSA_Cert_Date, Landed_Costing_Ex_Rate, CreatedDate, CreatedUser FROM dbo.tmp_Pipeline_InvoiceHeaders WITH (NOLOCK)

IF OBJECT_ID('dbo.tmp_Pipeline_InvoiceHeaders', 'U') IS NOT NULL
  DROP TABLE dbo.tmp_Pipeline_InvoiceHeaders
GO

ALTER TABLE dbo.Pipeline_InvoiceHeaders
  ADD PRIMARY KEY CLUSTERED (id)
GO

ALTER TABLE dbo.Pipeline_InvoiceHeaders
  ADD CONSTRAINT FK_Pipeline_InvoiceHeaders_Pipeline_Filing_Headers_Filing_Headers_FK FOREIGN KEY (Filing_Headers_FK) REFERENCES dbo.Pipeline_Filing_Headers (id)
GO

--- restore Pipeline DefValues Manual table ---
ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD UI_Section VARCHAR(32) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD ColName VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD TableName VARCHAR(128) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues_Manual
ADD DefValue VARCHAR(128) NULL
GO

UPDATE dbo.Pipeline_DEFValues_Manual
SET ColName = column_name
   ,TableName = table_name
   ,DefValue = value
   ,UI_Section = section_title

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'parent_record_id' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN parent_record_id
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'section_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN section_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'section_title' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN section_title
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'record_id' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN record_id
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'description' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN [description]
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'table_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN table_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'column_name' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN column_name
END
GO

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Pipeline_DEFValues_Manual' AND COLUMN_NAME = 'value' AND TABLE_SCHEMA = 'DBO')
BEGIN
  ALTER TABLE Pipeline_DEFValues_Manual
  DROP COLUMN [value]
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes i WHERE i.name = 'Idx_FilingHeadersFK')
  DROP INDEX Idx_FilingHeadersFK ON dbo.Pipeline_DEFValues_Manual
GO

--- restore Pipeline DefValues Manual view ---
ALTER VIEW dbo.v_Pipeline_DEFValues_Manual
AS
SELECT
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
 ,v.TableName
 ,v.UI_Section
 ,v.ValueLabel
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS ValueType
 ,i.CHARACTER_MAXIMUM_LENGTH AS ValueMaxLength
FROM dbo.Pipeline_DEFValues_Manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
    AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
WHERE (UPPER(i.COLUMN_NAME)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

--- restore Pipeline DefValues table ---
ALTER TABLE dbo.Pipeline_DEFValues
ADD UI_Section VARCHAR(32) NULL
GO

ALTER TABLE dbo.Pipeline_DEFValues
ADD TableName VARCHAR(128) NULL
GO

UPDATE dbo.Pipeline_DEFValues
SET UI_Section = ps.title
   ,TableName = ps.table_name
FROM dbo.Pipeline_DEFValues AS pdv
INNER JOIN dbo.pipeline_sections ps
  ON pdv.section_id = ps.id

ALTER TABLE dbo.Pipeline_DEFValues
DROP CONSTRAINT [FK_dbo.Pipeline_DEFValues_dbo.pipeline_sections_section_id]
GO

DROP INDEX IX_section_id ON dbo.Pipeline_DEFValues
GO

ALTER TABLE dbo.Pipeline_DEFValues
DROP COLUMN section_id
GO

--- restore Pipeline DefValues view ---
ALTER VIEW dbo.v_Pipeline_DEFValues
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
FROM dbo.Pipeline_DEFValues v
LEFT JOIN information_schema.columns i
  ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
WHERE (UPPER(i.column_name)
NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK', 'Pipeline_Inbounds_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

--- drop Pipeline Section table ---
IF OBJECT_ID('dbo.pipeline_sections', 'U') IS NOT NULL
	DROP TABLE dbo.pipeline_sections
GO

--- restore Pipeline filing procedures ---
ALTER PROCEDURE dbo.pipeline_filing (@Filing_Headers_id INT,
@FilingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;
  DECLARE @str VARCHAR(MAX) = '';
  DECLARE @ImporterCode VARCHAR(128);
  --Get Importercode for the inbound Importer
  SELECT
    @ImporterCode = [dbo].[fn_pipeline_GetImporterCode](@Filing_Headers_id)

  -- PIPELINE_DECLARATIONTAB---------------------------------------
  INSERT INTO Pipeline_DeclarationTab (Filing_Headers_FK,
  PI_FK,
  Main_Supplier,
  Importer,
  Issuer,
  Batch_Ticket,
  Pipeline,
  Carrier_SCAC,
  Discharge,
  Entry_Port,
  Dep,
  Arr,
  Arr_2,
  Origin,
  Destination,
  Destination_State,
  ETA,
  Export_Date,
  Description,
  Owner_Ref,
  FIRMs_Code,
  Master_Bill,
  Importer_of_record)
    SELECT
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
     ,@ImporterCode
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound inbound
      ON details.Pipeline_Inbounds_FK = inbound.id
    LEFT JOIN Pipeline_Rule_Facility ruleFacility
      ON inbound.facility = ruleFacility.facility
    LEFT JOIN Pipeline_Rule_Importer ruleImporters
      ON (RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(ruleImporters.Importer))
        )
    LEFT JOIN Pipeline_Rule_PortImporter rulePorts
      ON (
          --we have both tofacility-port and port-importer rule associated with record
          (
            (RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer)))
            AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
          )
          OR (-- we have tofacility-port rule but no port-importer rule associated with record 
            RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
            AND ((LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
              AND @ImporterCode NOT IN (SELECT
                  Importer
                FROM Pipeline_Rule_PortImporter
                WHERE port = ruleFacility.port)
            )
            )
          )
          OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
          (
            (
            RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
            )
            AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = '')
          )
        )
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_DeclarationTab declaration
      WHERE declaration.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.Pipeline_Inbounds_FK = declaration.PI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id

  INSERT INTO dbo.Pipeline_ContainersTab (Filing_Headers_FK
  , PI_FK
  , Manifest_QTY
  , Bill_Num
  , Bill_Issuer_SCAC)
    SELECT
      details.Filing_Headers_FK -- Filing_Headers_FK - int NOT NULL
     ,details.Pipeline_Inbounds_FK -- PI_FK - int
     ,ROUND(inbound.Quantity, 0) AS Manifest_QTY -- Manifest_QTY - varchar(128)
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
      ON (
          --we have both tofacility-port and port-importer rule associated with record
          (
            (RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer)))
            AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
          )
          OR (-- we have tofacility-port rule but no port-importer rule associated with record 
            RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
            AND ((LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
              AND @ImporterCode NOT IN (SELECT
                  Importer
                FROM Pipeline_Rule_PortImporter
                WHERE port = ruleFacility.port)
            )
            )
          )
          OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
          (
            (
            RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
            )
            AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = '')
          )
        )
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_ContainersTab containers
      WHERE containers.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.Pipeline_Inbounds_FK = containers.PI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id

  -- Get current tariff ---------------

  DECLARE @tariffs TABLE (
    pi INT NOT NULL
   ,tariff INT NOT NULL
  )

  INSERT INTO @tariffs (pi, tariff)
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

    WHERE details.Filing_Headers_FK = @Filing_Headers_id

  -- Get current tariff ---------------

  -- PIPELINE_INVOICELINES---------------------------------------
  INSERT INTO Pipeline_InvoiceLines (Filing_Headers_FK
  , InvoiceHeaders_FK
  , PI_FK
  , Invoice_No
  , Transaction_Related
  , tariff
  , Customs_QTY
  , Goods_Description
  , spi
  , Gr_Weight
  , PriceUnit
  , Attribute_1
  , Attribute_2
  , Attribute_3
  , Invoice_Qty
  , ORG
  , Line_Price
  , Amount
  , Manufacturer
  , Consignee
  , Sold_to_party
  , Origin
  , Export
  , Dest_State)
    SELECT
      details.Filing_Headers_FK
     ,ISNULL(ih.max_id, 0) + 1 AS InvoiceHeaders_FK
     ,details.Pipeline_Inbounds_FK AS PI_FK
     ,inbound.Batch AS Invoice_No
     ,ruleImporters.Transaction_Related AS Transaction_Related
     ,pra.Tariff AS Tariff
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
     ,inbound.Quantity * ruleImporters.value
     ,inbound.Quantity * ruleImporters.freight
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
                 (RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer)))
                 AND RTRIM(LTRIM(ruleFacility.port)) = RTRIM(LTRIM(rulePorts.port))
               )
               OR (-- we have tofacility-port rule but no port-importer rule associated with record 
                 RTRIM(LTRIM(rulePorts.port)) = RTRIM(LTRIM(ruleFacility.port))
                 AND ((LOWER(RTRIM(LTRIM(rulePorts.Importer))) = LOWER('N/A')
                   AND @ImporterCode NOT IN (SELECT
                       Importer
                     FROM Pipeline_Rule_PortImporter
                     WHERE port = ruleFacility.port)
                 )
                 )
               )
               OR (--we dont have tofacility-port rule but port-importer rule exist for the record 
               (
                 (
                 RTRIM(LTRIM(@ImporterCode)) = RTRIM(LTRIM(rulePorts.Importer))
                 )
                 AND ISNULL(RTRIM(LTRIM(ruleFacility.port)), '') = '')
               )
             )
         LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
           ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
         LEFT JOIN @tariffs t
           ON inbound.id = t.pi
         LEFT JOIN Pipeline_Rule_API pra
           ON t.Tariff = pra.id
        ,(SELECT
             MAX(id) max_id
           FROM Pipeline_InvoiceHeaders) ih
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_InvoiceLines invLines
      WHERE invLines.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.Pipeline_Inbounds_FK = invLines.PI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'PIPELINE_INVOICELINES'

  -- PIPELINE_INVOICEHEADERS---------------------------------------
  INSERT INTO Pipeline_InvoiceHeaders (id, Filing_Headers_FK, Invoice_No, Supplier, Invoice_Total, Origin, Consignee, Transaction_Related, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Supplier_Address, Export)
    SELECT DISTINCT
      invLines.InvoiceHeaders_FK AS Id
     ,details.Filing_Headers_FK AS Filing_Headers_FK
     ,inbound.Batch AS Invoice_No
     ,ruleImporters.supplier AS supplier
     ,inbound.Quantity * ruleImporters.value - invLines.PriceUnit AS Invoice_Total
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
    INNER JOIN Pipeline_InvoiceLines invLines
      ON invLines.Filing_Headers_FK = details.Filing_Headers_FK
        AND details.Pipeline_Inbounds_FK = invLines.PI_FK
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_InvoiceHeaders invHeaders
      WHERE invHeaders.Filing_Headers_FK = details.Filing_Headers_FK
      AND invLines.InvoiceHeaders_FK = invHeaders.Id)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'PIPELINE_INVOICEHEADERS'

  -- Pipeline_MISC---------------------------------------
  INSERT INTO Pipeline_MISC (PI_FK, Filing_Headers_FK, Branch, Broker, Preparer_Dist_Port, recon_issue, fta_recon, payment_type, broker_to_pay)
    SELECT
      details.Pipeline_Inbounds_FK
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
      ON userData.UserAccount = @FilingUser
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_MISC misc
      WHERE misc.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.Pipeline_Inbounds_FK = misc.PI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id
  --PRINT 'Pipeline_MISC'

  DECLARE @str_val NVARCHAR(500);
  DECLARE @ParmDefinition NVARCHAR(500);
  DECLARE @DefvalOUT VARCHAR(128);

  DECLARE @datatype VARCHAR(128)
         ,@DefValue VARCHAR(128)
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
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Pipeline_DEFValues v
    ON UPPER(i.column_name) = UPPER(v.ColName)
    AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND (FManual > 0
  OR FHasDefaultVal > 0
  OR Display_on_UI > 0)
  AND NOT EXISTS (SELECT
      *
    FROM Pipeline_DEFValues_Manual r
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
INSERT INTO Pipeline_DEFValues_Manual (Filing_Headers_FK, Display_on_UI, ValueLabel, TableName, ColName, FManual, FHasDefaultVal
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
    FROM Pipeline_DEFValues
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
      WHEN UPPER(TableName) = 'PIPELINE_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Pipeline_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = UPPER(v.TableName)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM INFORMATION_SCHEMA.COLUMNS c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'PIPELINE_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.table_name) = 'PIPELINE_FILING_HEADERS' THEN 0
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
  FROM Pipeline_DEFValues_Manual V
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
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'PIPELINE_INBOUNDS_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.COLUMNS
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'PIPELINE_INBOUNDS_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.COLUMNS
      WHERE (object_id = OBJECT_ID(@s)
      AND UPPER(NAME) = 'FILING_HEADERS_FK')
      OR object_id = OBJECT_ID('Pipeline_Filing_Headers'))
    SET @sql2 =
    CASE
      WHEN UPPER(@s) = 'PIPELINE_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END;

  SET @sql = 'SELECT  ColName,value                  
               FROM  (SELECT top 1 ' + @List + '  FROM  ' + @s + ' ' + @sql2 + ')  p
               UNPIVOT (value  FOR  ColName  IN  (' + @List2 + '))  as  unpvt'
  --PRINT @sql
  SET @sql = '
MERGE Pipeline_DEFValues_Manual AS t 
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
END
GO

ALTER PROCEDURE dbo.pipeline_filing_param (@Filing_Headers_id INT = 117)
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
      WHEN UPPER(TableName) = 'PIPELINE_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM information_schema.columns i
  LEFT JOIN dbo.Pipeline_DEFValues_Manual v
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
    OR UPPER(c.table_name) = 'PIPELINE_FILING_HEADERS')
  --print @str;
  EXEC (@str);
END;
GO

ALTER PROCEDURE dbo.pipeline_filing_del (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = '';

  SET @str = ''
  SELECT
    @str = @str + 'delete from ' + TableName + ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64)) + '; '
    + CHAR(10)
  FROM information_schema.columns i
  INNER JOIN dbo.Pipeline_DEFValues_Manual v
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
  DELETE FROM [dbo].[Pipeline_Documents]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Pipeline_DEFValues_Manual]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Pipeline_Filing_Details]
  WHERE Filing_Headers_FK = @Filing_Headers_id
  DELETE FROM [dbo].[Pipeline_Filing_Headers]
  WHERE id = @Filing_Headers_id

END;
GO
