ALTER VIEW dbo.Rail_Inbound_Grid
AS
SELECT DISTINCT
  bd_parsed.BDP_PK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,bd_parsed.PortOfUnlading BD_Parsed_PortOfUnlading
 ,bd_parsed.Description1 BD_Parsed_Description1
 ,bd_parsed.Description2 BD_Parsed_Description2
 ,bd_parsed.BillofLading BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,CONCAT(bd_parsed.EquipmentInitial, bd_parsed.EquipmentNumber) AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,importer_supplier.Importer Rule_ImporterSupplier_Importer
 ,importer_supplier.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,rail_description.Tariff Rule_Desc1_Desc2_Tariff
 ,rail_port.Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,rail_description.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_BD_Parsed bd_parsed
LEFT JOIN dbo.Rail_Rule_Port rail_port
  ON bd_parsed.PortOfUnlading = rail_port.Port
LEFT JOIN dbo.Rail_Rule_ImporterSupplier importer_supplier
  ON bd_parsed.Importer = importer_supplier.Importer_Name
    AND (bd_parsed.Supplier = importer_supplier.Supplier_Name
      OR (bd_parsed.Supplier IS NULL
        AND importer_supplier.Supplier_Name IS NULL))
LEFT JOIN dbo.Rail_Rule_Desc1_Desc2 rail_description
  ON rail_description.Description1 = bd_parsed.Description1
    AND (bd_parsed.Description2 = rail_description.Description2
      OR (bd_parsed.Description2 IS NULL
        AND rail_description.Description2 IS NULL))
LEFT JOIN dbo.Rail_Filing_Details filing_details
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.Rail_Filing_Headers filing_headers
  ON filing_headers.id = filing_details.Filing_Headers_FK
    AND filing_headers.MappingStatus <> 0
LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_headers.MappingStatus, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_headers.FilingStatus, 0) = filing_status.id
WHERE NOT EXISTS (SELECT
    *
  FROM dbo.Rail_Filing_Headers filing_headers
  INNER JOIN dbo.Rail_Filing_Details filing_details
    ON filing_headers.id = filing_details.Filing_Headers_FK
  WHERE filing_headers.MappingStatus > 0
  AND bd_parsed.BDP_PK = filing_details.BDP_FK)

UNION ALL

SELECT
  filing_details.BDP_FK BD_Parsed_Id
 ,bd_parsed.BDP_EM BD_Parsed_EDIMessage_Id
 ,filing_headers.id Filing_Headers_id
 ,bd_parsed.Importer BD_Parsed_Importer
 ,bd_parsed.Supplier BD_Parsed_Supplier
 ,declaration_tab.Entry_Port BD_Parsed_PortOfUnlading
 ,declaration_tab.Description BD_Parsed_Description1
 ,invoice_lines.Attribute_1 BD_Parsed_Description2
 ,declaration_tab.Master_Bill BD_Parsed_BillofLading
 ,bd_parsed.IssuerCode BD_Parsed_Issuer_Code
 ,containers_tab.Container_Number AS BD_Parsed_Container_Number
 ,bd_parsed.ReferenceNumber1 BD_Parsed_ReferenceNumber1
 ,bd_parsed.CreatedDate BD_Parsed_CreatedDate
 ,bd_parsed.FDeleted BD_Parsed_FDeleted
 ,CONVERT(BIT, ISNULL(bd_parsed.DuplicateOf, 0)) BD_Parsed_Is_Duplicated
 ,declaration_tab.Importer Rule_ImporterSupplier_Importer
 ,declaration_tab.Main_Supplier Rule_ImporterSupplier_Main_Supplier
 ,invoice_lines.Tariff Rule_Desc1_Desc2_Tariff
 ,declaration_tab.Entry_Port Rule_Port_Port
 ,filing_headers.FilingNumber Filing_Headers_FilingNumber
 ,filing_headers.JobPKHyperlink AS Filing_Headers_JobLink
 ,ISNULL(filing_headers.MappingStatus, 0) Filing_Headers_MappingStatus
 ,mapping_status.name Filing_Headers_MappingStatus_Title
 ,ISNULL(filing_headers.FilingStatus, 0) Filing_Headers_FilingStatus
 ,filing_status.name Filing_Headers_FilingStatus_Title
 ,invoice_lines.Description
 ,filing_headers.ErrorDescription

FROM dbo.Rail_Filing_Headers filing_headers
INNER JOIN dbo.Rail_Filing_Details filing_details
  ON filing_headers.id = filing_details.Filing_Headers_FK
INNER JOIN dbo.Rail_BD_Parsed bd_parsed
  ON filing_details.BDP_FK = bd_parsed.BDP_PK
LEFT JOIN dbo.MappingStatus mapping_status
  ON filing_headers.MappingStatus = mapping_status.id
LEFT JOIN dbo.FilingStatus filing_status
  ON filing_headers.FilingStatus = filing_status.id
LEFT JOIN dbo.Rail_DeclarationTab declaration_tab
  ON declaration_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = declaration_tab.BDP_FK
LEFT JOIN dbo.Rail_ContainersTab containers_tab
  ON containers_tab.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = containers_tab.BDP_FK
LEFT JOIN dbo.Rail_InvoiceLines invoice_lines
  ON invoice_lines.Filing_Headers_FK = filing_headers.id
    AND filing_details.BDP_FK = invoice_lines.BDP_FK
WHERE filing_headers.MappingStatus > 0
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
  INSERT INTO Truck_InvoiceHeaders (Id, Filing_Headers_FK, Invoice_No, Supplier, Consignee, Transaction_Related, Manufacturer, Seller, Importer, Sold_to_party, Ship_to_party, Invoice_Total)
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
      , invLines.Invoice_Qty * invLines.PriceUnit AS Invoice_Total
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