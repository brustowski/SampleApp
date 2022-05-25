-- update zones  filing entry
ALTER   PROCEDURE [zones_entry].[sp_update_entry] (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,[value] VARCHAR(512)
   ,table_name VARCHAR(128)
   ,column_name VARCHAR(128)
   ,row_num INT NOT NULL
  );

  DECLARE @command VARCHAR(MAX);
  DECLARE @columns VARCHAR(MAX);

  INSERT INTO @result (id
  , record_id
   ,parent_record_id 
  , [value]
  , table_name
  , column_name
  , row_num)
    SELECT
      field.id
     ,field.record_id
	 ,field.parent_record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name, field.record_id ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
	 , parent_record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN zones_entry.form_configuration config
      ON config.id = field.id
    INNER JOIN zones_entry.form_section_configuration section
      ON config.section_id = section.id
    WHERE config.editable = 1;

  DECLARE @recordId INT;
  DECLARE @tableName VARCHAR(128);
  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    table_name
   ,record_id
  FROM @result
  GROUP BY table_name
          ,record_id;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  SET @columns = NULL;

  SELECT
    @columns = COALESCE(@columns + ', ', '') + field.column_name + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
  FROM @result AS field
  WHERE field.record_id = @recordId
  AND field.table_name = @tableName
  AND field.row_num = 1;

  SET @command = COALESCE(@command, '') + 'update ' + @tableName + ' set ' + @columns + ' where id = ' + CAST(@recordId AS VARCHAR(10)) + ';' + CHAR(10);

  FETCH NEXT FROM cur INTO @tableName, @recordId;

  END;

  CLOSE cur;
  DEALLOCATE cur

  EXEC (@command);

   declare @filingHeaderId int;
  select top(1) @filingHeaderId = filing_header_id from zones_entry.declaration as declaration
  join @result as src
  on declaration.id=src.record_id and src.table_name='zones_entry.declaration'

   -----  update inbound record  ---
  UPDATE inbnd
  SET


		inbnd.owner_ref= decl.owner_ref,
		inbnd.entry_no=decl.entry_number,
		inbnd.firms_code=decl.firms_code,
		inbnd.entry_port=decl.entry_port,
		inbnd.vessel_name=decl.ftz_number,
		inbnd.arrival_date=decl.arr

  FROM [zones_entry].[inbound] AS inbnd
  JOIN [zones_entry].filing_detail AS detail
    ON inbnd.id = detail.inbound_id
	and detail.filing_header_id = @filingHeaderId
  JOIN [zones_entry].filing_header AS header
    ON detail.filing_header_id = header.id  
 join zones_entry.declaration decl
	 on  decl.filing_header_id=header.id

	

END;
GO