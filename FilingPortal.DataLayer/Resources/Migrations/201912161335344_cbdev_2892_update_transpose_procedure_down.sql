-- transpose table row
ALTER PROCEDURE dbo.sp_app_transpose (@tableName VARCHAR(128)
, @filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL)
AS
BEGIN
  DECLARE @selectColumnsList VARCHAR(MAX);
  DECLARE @columnsList VARCHAR(MAX);
  DECLARE @selectStatment VARCHAR(MAX);
  DECLARE @mergeStatment VARCHAR(MAX);

  -- get table column names with type converion for select statment
  SELECT
    @selectColumnsList = COALESCE(@selectColumnsList + ',', '') + 'isnull(cast(' +
    CASE
      WHEN t.name LIKE N'date%' THEN 'format(' + QUOTENAME(c.name) + ', ''MM/dd/yyyy'')'
      WHEN t.name LIKE N'numeric' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
      WHEN t.name LIKE N'decimal' THEN 'format(' + QUOTENAME(c.name) + ', ''0.######'')'
      ELSE QUOTENAME(c.name)
    END
    + ' as VARCHAR(MAX)),'''') AS ' + QUOTENAME(c.name)
  FROM sys.columns c
  INNER JOIN sys.types t
    ON c.user_type_id = t.user_type_id
  WHERE c.object_id = OBJECT_ID(@tableName)

  -- get table column names for UNPIVOT statment
  SELECT
    @columnsList = COALESCE(@columnsList + ',', '') + QUOTENAME(c.name)
  FROM sys.columns c
  WHERE objecT_id = OBJECT_ID(@tableName)
  AND (c.name) NOT IN (N'id', N'filing_header_id', N'parent_record_id', N'operation_id', N'source_record_id', N'created_date', N'created_user')

  -- set sselect statment
  SET @selectStatment =
  ' SELECT unpvt.id, unpvt.filing_header_id ,unpvt.parent_record_id, ''' + @tableName + ''' AS tableName, unpvt.column_name, unpvt.value
  FROM (SELECT ' + @selectColumnsList + ' FROM  ' + @tableName + ' where filing_header_id IN (' + @filingHeaderIds + ') ' +
  IIF(@operationId IS NOT NULL, ' AND operation_id=''' + CAST(@operationId AS VARCHAR(38)) + '''', '') + ') p
  UNPIVOT (value FOR column_name IN (' + @columnsList + ')) as unpvt'

  EXEC (@selectStatment)
END
GO