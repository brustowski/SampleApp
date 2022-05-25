-- Filing procedures

ALTER PROCEDURE dbo.truck_filing(
@Filing_Headers_id INT = 117,
@FilingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;

  -- Truck_Filing Data---------------------------------------

  INSERT INTO Truck_FilingData (TI_FK,
  Filing_Headers_FK,
  Supplier,
  Importer,
  Issuer,
  Master_Bill,
  SCAC,
  Discharge,
  entry_port,
  State,
  Description,
  Code,
  Invoice_No,
  Consignee,
  Manufacturer,
  Seller,
  Sold_To_Party,
  Ship_to_party,
  Tariff,
  Customs_QTY,
  Goods_Description,
  SPI,
  Gr_Weight,
  UQ,
  Price_Unit,
  Prod_ID_1,
  Attribute_1,
  Attribute_2_manual,
  Invoice_Qty,
  Invoice_Qty_Unit,
  Amount,
  recon_issue,
  FTA_Recon,
  Branch,
  Broker,
  Preparer_Dist_Port)
    SELECT
      tfd.BDP_FK AS TI_FK
     ,tfd.Filing_Headers_FK
     ,tri.cw_supplier AS Supplier
     ,tri.cw_ior AS Importer
     ,SUBSTRING(ti.PAPs, 1, 4) AS Issuer
     ,SUBSTRING(ti.PAPs, 5, LEN(ti.PAPs)) AS Master_Bill
     ,SUBSTRING(ti.PAPs, 1, 4) AS Scac
     ,tri.arrival_port AS Discharge
     ,tri.Entry_port AS Entry_port
     ,tri.destination_state AS State
     ,tri.Goods_description AS Description
     ,trp.firms_code AS Code
     ,ti.PAPs
     ,tri.cw_ior AS Consignee
     ,tri.cw_supplier AS Manufacturer
     ,tri.cw_supplier AS Seller
     ,tri.cw_ior AS Sold_to_party
     ,tri.cw_ior AS Ship_to_party
     ,tri.Tariff AS Tariff
     ,tri.custom_quantity AS Customs_qty
     ,tri.Goods_description AS Goods_description
     ,tri.SPI AS SPI
     ,tri.gross_weight AS Gr_weight
     ,tri.custom_uq AS UQ
     ,tri.Line_Price AS Price_unit
     ,tri.product_id AS Prod_id_1
     ,tri.custom_attrib1 AS Attribute_1
     ,tri.custom_attrib2 AS Attribute_2
     ,tri.Invoice_qty AS Invoice_qty
     ,tri.invoice_uq AS Invoice_qty_unit
     ,tri.charges AS Amount
     ,tri.Recon_issue AS Recon_issue
     ,tri.nafta_recon AS FTA_recon
     ,aud.Branch
     ,aud.Broker
     ,aud.Location
    FROM Truck_Filing_Details tfd
    INNER JOIN Truck_Inbound ti
      ON tfd.BDP_FK = ti.id
    LEFT JOIN Truck_Rule_Importers tri
      ON RTRIM(LTRIM(ti.Importer)) = RTRIM(LTRIM(tri.ior))
    LEFT JOIN Truck_Rule_Ports trp
      ON (RTRIM(LTRIM(tri.arrival_port)) = RTRIM(LTRIM(trp.arrival_port))
          AND RTRIM(LTRIM(tri.Entry_port)) = RTRIM(LTRIM(trp.Entry_port)))
    LEFT JOIN app_users_data aud
      ON aud.UserAccount = @FilingUser
    WHERE NOT EXISTS (SELECT
        *
      FROM Truck_FilingData r
      WHERE r.Filing_Headers_FK = tfd.Filing_Headers_FK
      AND tfd.BDP_FK = r.TI_FK)
    AND tfd.Filing_Headers_FK = @Filing_Headers_id

  DECLARE @str_val NVARCHAR(500)
         ,@ParmDefinition NVARCHAR(500)
         ,@DefvalOUT VARCHAR(128);

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
   ,td.id
  FROM INFORMATION_SCHEMA.columns i
  LEFT JOIN dbo.Truck_DEFValues td
    ON UPPER(i.column_name) = UPPER(td.ColName)
    AND UPPER(i.table_name) = 'TRUCK_FILINGDATA'
  WHERE i.table_schema = 'dbo'
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
  SET @str_val = N'SELECT @valOUT = try_cast(try_cast(''' + @DefValue + ''' as ' + @datatype + ') as varchar(128))';
  SET @ParmDefinition = N'@valOUT varchar(128) OUTPUT';
  EXECUTE sp_executesql @str_val
                       ,@ParmDefinition
                       ,@valOUT = @DefvalOUT OUTPUT;
  IF @DefvalOUT IS NULL
  BEGIN
    SET @str_val = N'SELECT @valOUT = try_cast(try_cast(' + @DefValue + ' as ' + @datatype + ') as varchar(128))';
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
  INSERT INTO Truck_DEFValues_Manual (Filing_Headers_FK,
  Display_on_UI,
  ValueLabel,
  ColName,
  FManual,
  FHasDefaultVal,
  FEditable,
  UI_Section,
  FMandatory,
  DefValue)
    SELECT
      @Filing_Headers_id
     ,Display_on_UI
     ,ValueLabel
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

  DECLARE @str VARCHAR(MAX) = '';

  SELECT
    @str = @str + 'update Truck_FilingData set ' + ColName + '= try_cast(''' + DefValue + ''' as ' +
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
    + ') where Filing_Headers_FK=' + CAST(@Filing_Headers_id AS VARCHAR(64)) + '; ' + CHAR(10)

  FROM INFORMATION_SCHEMA.columns i
  LEFT JOIN dbo.Truck_DEFValues_Manual v
    ON UPPER(i.column_name) = UPPER(v.ColName)
      AND UPPER(i.table_name) = 'TRUCK_FILINGDATA'
  WHERE i.table_schema = 'dbo'
  AND Filing_Headers_FK = @Filing_Headers_id
  AND DefValue IS NOT NULL
  AND FHasDefaultVal > 0
  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables  
  AND EXISTS (SELECT
      *
    FROM INFORMATION_SCHEMA.columns c
    WHERE (UPPER(i.table_name) = UPPER(c.table_name)
    AND UPPER(c.column_name) = 'FILING_HEADERS_FK')
    OR UPPER(c.table_name) = 'TRUCK_FILING_HEADERS')
  ORDER BY CASE
    WHEN UPPER(i.table_name) = 'TRUCK_FILING_HEADERS' THEN 0
    ELSE 1
  END
  --PRINT @str;
  EXEC (@str);

  DECLARE @List VARCHAR(MAX);
  DECLARE @List2 VARCHAR(MAX);
  DECLARE @sql VARCHAR(MAX);
  DECLARE @sql2 VARCHAR(1000) = '';
  DECLARE @s VARCHAR(MAX) = 'Truck_FilingData';

  SET @List = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.NAME) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.NAME) + ', ''###.######'')'
        ELSE QUOTENAME(c.NAME)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(c.NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  SET @List2 = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE object_id = OBJECT_ID(@s)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADERS_FK', 'BDP_FK', 'CREATEDDATE', 'CREATEDUSER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')

  -- we don't update table without Filing_Headers_FK, but we use the first value from this tables 
  SET @sql2 = '';
  IF EXISTS (SELECT
        *
      FROM sys.columns
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
ON (upper(t.ColName) = s.ColName AND t.Filing_Headers_FK = ' + CAST(@Filing_Headers_id AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
    UPDATE SET DefValue = case when s.value='''' then null else s.value end  ;';
  --PRINT @sql
  EXEC (@sql)

END
GO