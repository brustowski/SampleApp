CREATE PROCEDURE zones_entry.sp_import_entry (@json VARCHAR(MAX)
, @inserted INT OUTPUT
, @updated INT OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,filing_header_id INT NOT NULL
   ,[value] VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);
  DECLARE @values VARCHAR(MAX);
  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  INSERT INTO @result (
      id
     ,record_id
     ,parent_record_id
     ,filing_header_id
     ,[value]
     ,table_name
     ,column_name)
    SELECT
      config.id
     ,[row_number] AS record_id
     ,field.parent_record_id
     ,field.parent_record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
    FROM OPENJSON(@json)
    WITH (section VARCHAR(128)
    , [column] VARCHAR(128)
    , [row_number] INT
    , parent_record_id INT
    , filing_header_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN zones_entry.form_section_configuration AS section
      ON section.name = field.section
    INNER JOIN zones_entry.form_configuration AS config
      ON config.column_name = field.[column]
        AND config.section_id = section.id;

  SET @inserted = 0;
  SET @updated = 0;

  DECLARE @recordId INT;
  DECLARE @parentRecordId INT;
  DECLARE @filingHeaderId INT;
  DECLARE @tableName VARCHAR(128);
  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    table_name
   ,record_id
   ,parent_record_id
   ,filing_header_id
  FROM @result
  GROUP BY table_name
          ,record_id
          ,parent_record_id
          ,filing_header_id;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName, @recordId, @parentRecordId, @filingHeaderId;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  DECLARE @id INT = NULL;
  SELECT
    @id = invoice_line.id
  FROM zones_entry.invoice_line AS invoice_line
  JOIN @result AS fields
    ON invoice_line.line_no = fields.value
      AND invoice_line.parent_record_id = fields.parent_record_id
      AND fields.record_id = @recordId
      AND fields.table_name = @tableName
      AND fields.column_name = 'line_no'

  IF @tableName = 'zones_entry.invoice_line'
    AND @id IS NOT NULL
  BEGIN
    SET @columns = 'operation_id = ''' + CAST(@operationId AS CHAR(36)) + '''';
    SELECT
      @columns = COALESCE(@columns + ', ', '') + field.column_name + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
    FROM @result AS field
    WHERE field.record_id = @recordId
    AND field.table_name = @tableName;

    SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@id AS VARCHAR(10)) + ';' + CHAR(10);
    SET @updated = @updated + 1;
  END
  ELSE
  BEGIN
    SET @columns = 'filing_header_id, parent_record_id, operation_id';
    SELECT
      @columns = COALESCE(@columns + ', ', '') + field.column_name
    FROM @result AS field
    WHERE field.record_id = @recordId
    AND field.table_name = @tableName;

    SET @values = CAST(@filingHeaderId AS VARCHAR(20)) + ', ' + CAST(@parentRecordId AS VARCHAR(20)) + ', ''' + CAST(@operationId AS CHAR(36)) + '''';
    SELECT
      @values = COALESCE(@values + ', ', '') + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
    FROM @result AS field
    WHERE field.record_id = @recordId
    AND field.table_name = @tableName;

    SET @command = COALESCE(@command, '') + 'insert into ' + @tableName + ' (' + @columns + ') values (' + @values + ')' + ';' + CHAR(10);
    SET @inserted = @inserted + 1;
  END

  FETCH NEXT FROM cur INTO @tableName, @recordId, @parentRecordId, @filingHeaderId;
  END;

  CLOSE cur;
  DEALLOCATE cur;

  EXEC (@command);

END
GO