-- review mapped data
ALTER PROCEDURE dbo.sp_imp_rail_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,value VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.imp_rail_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.name AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.manual
     ,defValue.description
     ,defValue.label
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.imp_rail_form_configuration defValue
    INNER JOIN dbo.imp_rail_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- review mapped data
ALTER PROCEDURE dbo.sp_imp_pipeline_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.imp_pipeline_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.imp_pipeline_form_configuration defValue
    INNER JOIN dbo.imp_pipeline_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- review mapped data
ALTER PROCEDURE dbo.sp_imp_truck_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,value VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.imp_truck_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.imp_truck_form_configuration defValue
    INNER JOIN dbo.imp_truck_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- review mapped data
ALTER PROCEDURE dbo.sp_imp_vessel_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.imp_vessel_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.imp_vessel_form_configuration defValue
    INNER JOIN dbo.imp_vessel_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- review mapped data
ALTER PROCEDURE dbo.sp_exp_truck_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,value VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.exp_truck_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.name AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.manual
     ,defValue.description
     ,defValue.label
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.exp_truck_form_configuration defValue
    INNER JOIN dbo.exp_truck_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- review mapped data
ALTER PROCEDURE dbo.sp_exp_vessel_review_entry (@filingHeaderIds VARCHAR(MAX)
, @operationId UNIQUEIDENTIFIER = NULL
, @values VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- get values from resulting tables
  DECLARE @result TABLE (
    id INT NULL
   ,filing_header_id INT NULL
   ,parent_record_id INT NULL
   ,table_name VARCHAR(128) NULL
   ,column_name VARCHAR(128) NULL
   ,[value] VARCHAR(512)
  );
  DECLARE @tableName VARCHAR(128);

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    rs.table_name
  FROM dbo.exp_vessel_form_section_configuration rs
  WHERE rs.table_name IS NOT NULL;

  OPEN cur;

  FETCH NEXT FROM cur INTO @tableName;

  WHILE @@FETCH_STATUS = 0
  BEGIN

  INSERT INTO @result EXEC dbo.sp_app_transpose @tableName
                                               ,@filingHeaderIds
                                               ,@operationId;

  FETCH NEXT FROM cur INTO @tableName;

  END;

  CLOSE cur;
  DEALLOCATE cur;

  SET @values = (SELECT
      r.filing_header_id
     ,r.parent_record_id
     ,section.[name] AS section_name
     ,section.title AS section_title
     ,r.id AS record_id
     ,r.column_name
     ,r.table_name
     ,r.[value]
     ,defValue.id
     ,defValue.editable
     ,defValue.display_on_ui
     ,defValue.has_default_value
     ,defValue.mandatory
     ,defValue.[manual]
     ,defValue.[description]
     ,defValue.[label]
     ,defValue.handbook_name
     ,defValue.paired_field_table
     ,defValue.paired_field_column
     ,col.DATA_TYPE AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM dbo.exp_vessel_form_configuration defValue
    INNER JOIN dbo.exp_vessel_form_section_configuration section
      ON defValue.section_id = section.id
    JOIN @result r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO