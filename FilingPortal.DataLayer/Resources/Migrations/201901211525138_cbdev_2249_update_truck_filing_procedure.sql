IF OBJECT_ID(N'dbo.truck_filing', 'P') IS NOT NULL
  DROP PROCEDURE dbo.truck_filing
GO

CREATE PROCEDURE dbo.truck_filing (@Filing_Headers_id INT,
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
  , Gr_Weight
  , UQ
  , PriceUnit
  , Prod_ID_1
  , Attribute_1
  , Attribute_2
  , Invoice_Qty
  , Invoice_Qty_Unit
  , Amount)
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
     ,ruleImporters.Gross_Weight AS Gr_Weight
     ,ruleImporters.custom_uq AS UQ
     ,ruleImporters.Line_Price AS PriceUnit
     ,ruleImporters.product_id AS Prod_ID_1
     ,ruleImporters.custom_attrib1 AS Attribute_1
     ,ruleImporters.custom_attrib2 AS Attribute_2
     ,ruleImporters.Invoice_Qty AS Invoice_Qty
     ,ruleImporters.invoice_uq AS Invoice_Qty_Unit
     ,ruleImporters.charges AS Amount
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
  INSERT INTO Truck_InvoiceHeaders (Id, Filing_Headers_FK, Invoice_No, Supplier, Consignee, Transaction_Related, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party)
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
    SET @DefvalOUT = FORMAT(TRY_CAST(@DefvalOUT AS FLOAT), '###.######');
  END;
  INSERT INTO Truck_DEFValues_Manual (Filing_Headers_FK, Display_on_UI, ValueLabel, TableName, ColName, FManual, FHasDefaultVal
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
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.NAME) + ', ''###.######'')'
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