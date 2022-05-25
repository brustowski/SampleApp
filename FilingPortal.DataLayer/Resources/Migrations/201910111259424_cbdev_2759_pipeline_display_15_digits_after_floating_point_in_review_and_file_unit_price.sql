--- Pipeline post save action ---
ALTER PROCEDURE dbo.pipeline_filing_post_save (@filingHeaderId INT)
AS
BEGIN
  DECLARE @importerCode VARCHAR(128) = dbo.fn_pipeline_GetImporterCode(@filingHeaderId);
  DECLARE @api DECIMAL(18, 6)
         ,@freight DECIMAL(18, 6)

  SELECT
    @api = inbound.API
   ,@freight = COALESCE(rulePriceExact.freight, rulePrice.freight)

  FROM dbo.Pipeline_Filing_Details details
  INNER JOIN dbo.Pipeline_Inbound inbound
    ON inbound.id = details.Pipeline_Inbounds_FK
  LEFT JOIN Clients clients
    ON clients.ClientCode = @ImporterCode
  LEFT JOIN Pipeline_Rule_Price rulePrice
    ON clients.id = rulePrice.importer_id
      AND rulePrice.crude_type_id IS NULL
  LEFT JOIN Pipeline_Rule_BatchCode ruleBatch
    ON dbo.extractBatchCode(inbound.Batch) = ruleBatch.batch_code
  LEFT JOIN Pipeline_Rule_Price rulePriceExact
    ON clients.id = rulePriceExact.importer_id
      AND ruleBatch.id = rulePriceExact.crude_type_id
  WHERE details.Filing_Headers_FK = @filingHeaderId

  DECLARE @tbl AS TABLE (
    filing_header_id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,quantity DECIMAL(18, 6)
   ,unit_price DECIMAL(28, 15)
  )

  INSERT INTO @tbl (record_id
  , parent_record_id
  , filing_header_id
  , quantity
  , unit_price)
    SELECT
      a.record_id
     ,a.parent_record_id
     ,a.Filing_Headers_FK
     ,CONVERT(DECIMAL(18, 6), a.value) AS Quantity
     ,CONVERT(DECIMAL(28, 15), b.value) AS UnitPrice
    FROM Pipeline_DEFValues_Manual a
    JOIN Pipeline_DEFValues_Manual b
      ON a.record_id = b.record_id
    WHERE a.Filing_Headers_FK = @filingHeaderId
    AND b.Filing_Headers_FK = @filingHeaderId
    AND a.table_name = 'Pipeline_InvoiceLines'
    AND a.column_name = 'Invoice_Qty'
    AND b.column_name = 'PriceUnit'

  DECLARE @total DECIMAL(28, 15)
  SELECT
    @total = SUM([@tbl].quantity * [@tbl].unit_price)
  FROM @tbl

  -- update invoice customs qty
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Customs_QTY'
  -- update invoice line price
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * t.unit_price, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Line_Price'
  -- update invoice line amount
  UPDATE defValues
  SET defValues.value = FORMAT(t.quantity * @freight, '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Amount'
  -- update invoice line gross weight
  UPDATE defValues
  SET defValues.value = FORMAT(dbo.fn_pipeline_weight(t.Quantity, @api), '0.######')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl t
    ON defValues.record_id = t.record_id
  WHERE defValues.table_name = 'Pipeline_InvoiceLines'
  AND defValues.column_name = 'Gr_Weight'

  -- update invoice header invoice total
  UPDATE defValues
  SET value = FORMAT(@total, '0.##############')
  FROM Pipeline_DEFValues_Manual defValues
  JOIN @tbl
    ON defValues.record_id = [@tbl].parent_record_id
  WHERE defValues.Filing_Headers_FK = @filingHeaderId
  AND table_name = 'Pipeline_InvoiceHeaders'
  AND column_name = 'Invoice_Total'
END
GO

-- update def values manual data --
ALTER PROCEDURE dbo.update_def_values_manual (@defValuesManualTableName VARCHAR(128) = 'truck_export_def_values_manual'
, @tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  /*
    Update values into def value manual table with data from result table
  */
  DECLARE @selectColumnsList VARCHAR(MAX);
  DECLARE @columnsList VARCHAR(MAX);
  DECLARE @selectStatment VARCHAR(MAX);
  DECLARE @mergeStatment VARCHAR(MAX);

  -- get table column names with type converion for select statment
  SET @selectColumnsList = STUFF((SELECT DISTINCT
      ',' + 'isnull(cast(' +
      CASE
        WHEN t.name LIKE 'date%' THEN 'format(' + QUOTENAME(c.name) + ', ''MM/dd/yyyy'')'
        WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.###############'')'
        WHEN t.name LIKE 'decimal' THEN 'format(' + QUOTENAME(c.name) + ', ''0.###############'')'
        ELSE QUOTENAME(c.name)
      END
      + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.NAME)
    FROM sys.columns c
    INNER JOIN sys.types t
      ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(@tableName)
    AND UPPER(c.name) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @selectColumnsList

  -- get table column names for UNPIVOT statment
  SET @columnsList = STUFF((SELECT DISTINCT
      ',' + QUOTENAME(NAME)
    FROM sys.columns
    WHERE objecT_id = OBJECT_ID(@tableName)
    AND UPPER(NAME) NOT IN ('ID', 'FILING_HEADER_ID', 'INVOICE_HEADER_ID', 'CREATED_DATE', 'CREATED_USER')
    FOR XML PATH (''), TYPE)
  .value('.', 'VARCHAR(MAX)'), 1, 1, '')
  --PRINT @columnsList

  -- set sselect statment
  SET @selectStatment =
  ' SELECT column_name, value
  FROM (SELECT top 1 ' + @selectColumnsList + ' FROM  ' + @tableName + ' where id=' + CAST(@recordId AS VARCHAR(32)) + ') p
  UNPIVOT (value FOR column_name IN (' + @columnsList + ')) as unpvt'
  --PRINT @selectStatment

  --set merge statment
  SET @mergeStatment = '
MERGE ' + @defValuesManualTableName + ' AS t 
USING (' + @selectStatment + ') AS s 
ON (upper(t.column_name) = upper(s.column_name) and upper(t.table_name) = ''' + UPPER(@tableName) + ''' AND t.record_id = ' + CAST(@recordId AS VARCHAR(32)) + ')
WHEN MATCHED THEN 
  UPDATE SET t.value = case when s.value='''' then null else s.value end  ;';
  --PRINT @mergeStatment

  EXEC (@mergeStatment)

END
GO