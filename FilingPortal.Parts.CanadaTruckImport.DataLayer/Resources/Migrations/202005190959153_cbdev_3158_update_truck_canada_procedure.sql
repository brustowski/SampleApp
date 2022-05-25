-- update filing entry
ALTER PROCEDURE canada_imp_truck.sp_update_entry (@json VARCHAR(MAX))
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

  INSERT INTO @result (
      id
     ,record_id
     ,parent_record_id
     ,[value]
     ,table_name
     ,column_name
     ,row_num)
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
    INNER JOIN canada_imp_truck.form_configuration AS config
      ON config.id = field.id
    INNER JOIN canada_imp_truck.form_section_configuration AS section
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
    @columns = COALESCE(@columns + ', ', '') + QUOTENAME(field.column_name) + ' = ' + IIF(field.value IS NULL, 'NULL', '''' + field.value + '''')
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

---  update inbound record  ---
  UPDATE inbnd
  SET inbnd.owners_reference = src.value
  FROM canada_imp_truck.inbound AS inbnd
  JOIN canada_imp_truck.filing_detail AS detail
    ON inbnd.id = detail.inbound_id
  JOIN canada_imp_truck.filing_header AS header
    ON detail.filing_header_id = header.id
  JOIN @result AS src
    ON header.id = src.parent_record_id
    AND src.table_name = 'canada_imp_truck.declaration'
    AND src.column_name = 'sd_owners_reference';

  UPDATE inbnd
  SET inbnd.pars_number = src.value
  FROM canada_imp_truck.inbound AS inbnd
  JOIN canada_imp_truck.filing_detail AS detail
    ON inbnd.id = detail.inbound_id
  JOIN canada_imp_truck.filing_header AS header
    ON detail.filing_header_id = header.id
  JOIN @result AS src
    ON header.id = src.parent_record_id
    AND src.table_name = 'canada_imp_truck.declaration'
    AND src.column_name = 'td_cargo_control_no';

END
GO