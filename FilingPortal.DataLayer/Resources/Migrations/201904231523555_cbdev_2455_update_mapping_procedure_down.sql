﻿DELETE FROM Pipeline_DEFValues WHERE UI_Section = '01.Declaration' AND TableName = 'PIPELINE_DECLARATIONTAB' AND ValueLabel = 'Entry #'
GO

ALTER TABLE dbo.Pipeline_DeclarationTab
  DROP COLUMN EntryNumber
GO

ALTER VIEW dbo.v_Pipeline_Filing_Data
AS SELECT
  inbound.Id AS id
  ,headers.id AS Filing_header_id 
  ,declaration.Importer AS Importer
  ,inbound.Batch AS Batch
  ,inbound.TicketNumber AS TicketNumber
  ,inbound.Port AS Port
  ,inbound.Quantity AS Quantity
  ,inbound.API AS API
  ,inbound.ExportDate AS ExportDate
  ,inbound.ImportDate AS ImportDate
  ,inbound.EntryNumber AS EntryNumber
FROM Pipeline_Filing_Headers headers
LEFT OUTER JOIN dbo.Pipeline_DeclarationTab declaration
  ON declaration.FILING_HEADERS_FK = headers.id
  INNER JOIN Pipeline_Inbound inbound
  ON inbound.Id = declaration.PI_FK
GO

ALTER PROCEDURE dbo.pipeline_filing (@Filing_Headers_id INT,
@FilingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;
  DECLARE @str VARCHAR(MAX) = '';

  -- PIPELINE_DECLARATIONTAB---------------------------------------
  INSERT INTO Pipeline_DeclarationTab (Filing_Headers_FK,
  PI_FK,
  Main_Supplier,
  importer,
  issuer,
  Batch_Ticket,
  pipeline,
  Carrier_SCAC,
  Discharge,
  Entry_Port,
  Dep,
  Arr,
  Arr_2,
  origin,
  destination,
  destination_state,
  ETA,
  Export_Date,
  description,
  Owner_Ref,
  firms_code)
    SELECT
      details.Filing_Headers_FK AS Filing_Headers_FK
     ,details.Pipeline_Inbounds_FK AS PI_FK
     ,ruleImporters.supplier AS Main_Supplier
     ,inbound.Importer AS Importer
     ,rulePorts.Issuer AS Issuer
     ,REPLACE(inbound.TicketNumber, '-', '') AS Batch_Ticket
     ,rulePorts.Pipeline AS Pipeline
     ,rulePorts.Issuer AS Carrier_SCAC
     ,inbound.port AS Discharge
     ,inbound.port AS Entry_Port
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
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound inbound
      ON details.Pipeline_Inbounds_FK = inbound.id
    LEFT JOIN Pipeline_Rule_Importer ruleImporters
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.Importer))
    LEFT JOIN Pipeline_Rule_PortImporter rulePorts
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(rulePorts.Importer))
        AND RTRIM(LTRIM(inbound.port)) = RTRIM(LTRIM(rulePorts.port))
    WHERE NOT EXISTS (SELECT
        *
      FROM Pipeline_DeclarationTab declaration
      WHERE declaration.Filing_Headers_FK = details.Filing_Headers_FK
      AND details.Pipeline_Inbounds_FK = declaration.PI_FK)
    AND details.Filing_Headers_FK = @Filing_Headers_id

  INSERT INTO dbo.Pipeline_ContainersTab (Filing_Headers_FK
  , PI_FK
  , Bill_Number
  , Manifest_QTY)
    SELECT
      details.Filing_Headers_FK -- Filing_Headers_FK - int NOT NULL
     ,details.Pipeline_Inbounds_FK -- PI_FK - int
     ,inbound.EntryNumber -- Bill_Number - varchar(128)
     ,ROUND(inbound.Quantity, 0) AS Manifest_QTY -- Manifest_QTY - varchar(128)
    FROM Pipeline_Filing_Details details
    INNER JOIN Pipeline_Inbound inbound
      ON details.Pipeline_Inbounds_FK = inbound.id
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
  , SPI
  , Gr_Weight
  , PriceUnit
  , Attribute_1
  , Attribute_2
  , Attribute_3
  , Invoice_Qty)
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
    FROM dbo.Pipeline_Filing_Details details
         INNER JOIN dbo.Pipeline_Inbound inbound
           ON inbound.id = details.Pipeline_Inbounds_FK
         LEFT JOIN Pipeline_Rule_Importer ruleImporters
           ON RTRIM(LTRIM(inbound.importer)) = RTRIM(LTRIM(ruleImporters.importer))
         LEFT JOIN Pipeline_Rule_PortImporter rulePorts
           ON RTRIM(LTRIM(inbound.importer)) = RTRIM(LTRIM(rulePorts.importer))
             AND RTRIM(LTRIM(inbound.port)) = RTRIM(LTRIM(rulePorts.port))
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
  INSERT INTO Pipeline_InvoiceHeaders (id, Filing_Headers_FK, Invoice_No, supplier, Invoice_Total, origin, Consignee, Transaction_Related, Manufacturer, seller, importer, Sold_to_party, Ship_to_party)
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
     ,inbound.Importer AS Importer
     ,ruleImporters.Consignee AS Sold_To_Party
     ,ruleImporters.Consignee AS Ship_To_Party
    FROM dbo.Pipeline_Filing_Details details
    INNER JOIN dbo.Pipeline_Inbound inbound
      ON inbound.Id = details.Pipeline_Inbounds_FK
    LEFT JOIN Pipeline_Rule_Importer ruleImporters
      ON RTRIM(LTRIM(inbound.Importer)) = RTRIM(LTRIM(ruleImporters.Importer))
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
  INSERT INTO Pipeline_MISC (PI_FK, Filing_Headers_FK, Branch, Broker, Preparer_Dist_Port, Recon_Issue, FTA_Recon, Payment_Type, Broker_to_Pay)
    SELECT
      details.Pipeline_Inbounds_FK
     ,details.Filing_Headers_FK
     ,userData.Branch
     ,userData.Broker
     ,userData.Location
     ,ruleImporters.Recon_Issue
     ,ruleImporters.FTA_Recon
     ,ruleImporters.Payment_Type
     ,ruleImporters.Broker_to_Pay
    FROM dbo.Pipeline_Filing_Details details
    INNER JOIN dbo.Pipeline_Inbound inbound
      ON inbound.id = details.Pipeline_Inbounds_FK
    LEFT JOIN Pipeline_Rule_Importer ruleImporters
      ON RTRIM(LTRIM(inbound.importer)) = RTRIM(LTRIM(ruleImporters.importer))
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
  LEFT JOIN dbo.Pipeline_DEFValues v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
    AND UPPER(i.TABLE_NAME) = UPPER(v.TableName)
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
  , FEditable, UI_Section, FMandatory, DefValue)
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
    FROM Pipeline_DEFValues
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
      WHEN UPPER(TableName) = 'PIPELINE_FILING_HEADERS' THEN ' where id=' + CAST(@Filing_Headers_id AS VARCHAR(64))
      ELSE ' where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64))
    END
    + '; '
    + CHAR(10)
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Pipeline_DEFValues_Manual v
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
    OR UPPER(c.TABLE_NAME) = 'PIPELINE_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.TABLE_NAME) = 'PIPELINE_FILING_HEADERS' THEN 0
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
END;
GO