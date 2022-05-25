IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.update_def_values_manual') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.update_def_values_manual
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_declaration_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_declaration_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_def_values_manual') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_def_values_manual
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_invoice_header_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_invoice_header_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_add_invoice_line_record') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_add_invoice_line_record
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_apply_def_values') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_apply_def_values
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing_del') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing_del
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('dbo.truck_export_filing_param') AND type in (N'P', N'PC'))
	DROP PROCEDURE dbo.truck_export_filing_param
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

CREATE PROCEDURE dbo.update_def_values_manual(@defValuesManualTableName VARCHAR(128) = 'truck_export_def_values_manual'
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
      WHEN t.name LIKE 'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
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

 -- add truck_export_apply_def_values procedure
CREATE PROCEDURE dbo.truck_export_apply_def_values (@tableName VARCHAR(128)
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
  FROM dbo.truck_export_def_values v
  INNER JOIN dbo.truck_export_sections s
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

 -- add truck_export_add_def_values_manual procedure
CREATE PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
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
    , label)
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

-- add truck_export_add_invoice_line_record procedure
CREATE PROCEDURE dbo.truck_export_add_invoice_line_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add invoice line data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_lines t
      WHERE t.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO dbo.truck_export_invoice_lines (invoice_header_id)
      VALUES (@parentId);
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_lines t
    WHERE t.invoice_header_id = @parentId
  END

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END
GO

-- add truck_export_add_invoice_header_record procedure
CREATE PROCEDURE dbo.truck_export_add_invoice_header_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_invoice_headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_invoice_headers t
      WHERE t.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO dbo.truck_export_invoice_headers (filing_header_id)
      VALUES (@parentId);
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_invoice_headers t
    WHERE t.filing_header_id = @parentId
  END

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  -- add invoice line
  EXEC dbo.truck_export_add_invoice_line_record @filingHeaderId
                                               ,@recordId

  RETURN @recordId
END
GO

-- add truck_export_add_declaration_record procedure
CREATE PROCEDURE dbo.truck_export_add_declaration_record (@filingHeaderId INT, @parentId INT)
AS
BEGIN
  DECLARE @tableName VARCHAR(128) = 'truck_export_declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @recordId INT

  -- get section property is_array
  SELECT
    @allowMultiple = tes.is_array
  FROM truck_export_sections tes
  WHERE tes.table_name = @tableName

  -- add declaration data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.truck_export_declarations ted
      WHERE ted.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO dbo.truck_export_declarations (filing_header_id)
      VALUES (@parentId);
    SET @recordId = SCOPE_IDENTITY()
  END
  ELSE
  BEGIN
    SELECT TOP (1)
      @recordId = id
    FROM truck_export_declarations ted
    WHERE ted.filing_header_id = @parentId
  END

  -- apply default values
  EXEC dbo.truck_export_apply_def_values @tableName
                                        ,@recordId

  -- fill the def value manual table
  EXEC dbo.truck_export_add_def_values_manual @tableName
                                             ,@filingHeaderId
                                             ,@parentId
                                             ,@recordId

  -- update def value manual table date with values from result table 
  EXEC dbo.update_def_values_manual 'truck_export_def_values_manual'
                                   ,@tableName
                                   ,@recordId

  RETURN @recordId
END
GO

-- add truck_export_filing procedure
CREATE PROCEDURE dbo.truck_export_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.truck_export_add_declaration_record @filingHeaderId
                                              ,@filingHeaderId

  -- add invoice header
  EXEC dbo.truck_export_add_invoice_header_record @filingHeaderId
                                                 ,@filingHeaderId
END;
GO

-- add truck_export_filing_param procedure
CREATE PROCEDURE dbo.truck_export_filing_param (@Filing_Headers_id INT)
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
  LEFT JOIN dbo.truck_export_def_values_manual v
    ON UPPER(i.column_name) = UPPER(v.column_name)
      AND UPPER(i.table_name) = UPPER(v.table_name)
  WHERE i.table_schema = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO

-- add truck_export_filing_del procedure
CREATE   PROCEDURE dbo.truck_export_filing_del (@filingHeaderId INT)
AS
BEGIN

  DELETE FROM dbo.truck_export_def_values_manual
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.truck_export_filing_details
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.truck_export_filing_headers
  WHERE id = @filingHeaderId

END;
GO