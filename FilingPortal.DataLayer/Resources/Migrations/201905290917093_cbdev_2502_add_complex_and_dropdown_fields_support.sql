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

ALTER VIEW dbo.v_truck_export_def_values_manual
AS
SELECT
  v.id
 ,v.filing_header_id
 ,v.parent_record_id
 ,v.section_name
 ,v.section_title
 ,v.table_name
 ,v.column_name
 ,v.record_id
 ,v.modification_date
 ,v.label
 ,v.description
 ,v.value
 ,v.editable
 ,v.has_default_value
 ,v.mandatory
 ,v.display_on_ui
 ,v.manual
 ,v.handbook_name
 ,v.paired_field_table
 ,v.paired_field_column
 ,i.DATA_TYPE AS value_type
 ,i.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM dbo.truck_export_def_values_manual v
LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
  ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
    AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
WHERE (UPPER(i.COLUMN_NAME) NOT IN ('ID', 'CREATED_DATE', 'CREATED_USER', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID')
AND i.TABLE_SCHEMA = 'dbo')
OR i.COLUMN_NAME IS NULL
GO

ALTER VIEW dbo.v_Truck_DEFValues_Manual
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
WHERE (UPPER(i.column_name) NOT IN ('ID', 'CREATEDDATE', 'CREATEDUSER', 'FILING_HEADERS_FK', 'BDP_FK', 'BDP_PK', 'BDP_EM', 'EM_PK', 'InvoiceHeaders_FK')
AND i.table_schema = 'dbo')
OR i.column_name IS NULL
GO

ALTER PROCEDURE dbo.rail_filing (@Filing_Headers_id INT)
AS
BEGIN
  SET DATEFORMAT mdy;
  DECLARE @str VARCHAR(MAX) = '';
  -- RAIL_DECLARATIONTAB---------------------------------------
  INSERT INTO RAIL_DECLARATIONTAB (BDP_FK, Carrier_SCAC, Country_of_Export, Description, Destination, Destination_State, Discharge, Entry_Port, FIRMs_Code, Importer, Issuer, Main_Supplier, Master_Bill, Origin, FILING_HEADERS_FK)
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
  INSERT INTO RAIL_CONTAINERSTAB (BDP_FK, Bill_Issuer_SCAC, Bill_Num, Bill_Number, Container_Number, FILING_HEADERS_FK)
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
  INSERT INTO RAIL_INVOICELINES (BDP_FK, Attribute_1, Attribute_2, Gr_Weight, Gr_Weight_Unit, Consignee, Dest_State, Export, Goods_Description, InvoiceHeaders_FK, Manufacturer, ORG, Origin,
  Prod_ID_1, Tariff, Transaction_Related, Customs_QTY, SPI, UQ, PriceUnit, Invoice_Qty, Invoice_Qty_Unit, Amount, Line_Price, Description, FILING_HEADERS_FK)
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
  INSERT INTO RAIL_INVOICEHEADERS (Consignee, Export, Importer, Manufacturer, Origin, Seller, Ship_to_party, Sold_to_party, Supplier,
  Transaction_Related, Invoice_Total, FILING_HEADERS_FK, id)
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
  INSERT INTO Rail_MISC (BDP_FK, Recon_Issue, FTA_Recon, Payment_Type, Broker_to_Pay, Submitter,
  Filing_Headers_FK)
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
INSERT INTO Rail_DEFValues_Manual (Filing_Headers_FK, Display_on_UI, ValueLabel, TableName, ColName, FManual, FHasDefaultVal
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

ALTER PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.truck_export_def_values v
  INNER JOIN dbo.truck_export_sections s
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
      FROM dbo.truck_export_def_values_manual tedvm
      WHERE tedvm.record_id = @recordId
      AND tedvm.table_name = @tableName
      AND tedvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.truck_export_def_values_manual (filing_header_id
    , parent_record_id
    , section_name
    , section_title
    , record_id
    , column_name
    , table_name
    , modification_date
    , value
    , editable
    , display_on_ui
    , has_default_value
    , mandatory
    , manual
    , description
    , label
	, handbook_name
	, paired_field_table
	, paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,tes.name
       ,tes.title
       ,@recordId
       ,tedv.column_name
       ,tes.table_name
       ,GETDATE()
       ,@defValueOut
       ,tedv.editable
       ,tedv.display_on_ui
       ,tedv.has_default_value
       ,tedv.mandatory
       ,tedv.manual
       ,tedv.description
       ,tedv.label
	   ,handbook_name
	   ,paired_field_table
	   ,paired_field_column
      FROM dbo.truck_export_def_values tedv
      INNER JOIN dbo.truck_export_sections tes
        ON tedv.section_id = tes.id
      WHERE tedv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO