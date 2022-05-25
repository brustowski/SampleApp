-- Add Rail Import def values manual records --
ALTER PROCEDURE dbo.rail_add_def_values_manual (@tableName VARCHAR(128)
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
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Rail_DEFValues v
  INNER JOIN dbo.rail_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0)
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
      FROM dbo.Rail_DEFValues_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.Rail_DEFValues_Manual (
        Filing_Headers_FK
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,ModifiedDate
       ,value
       ,FEditable
       ,Display_on_UI
       ,FHasDefaultVal
       ,FMandatory
       ,FManual
       ,description
       ,ValueLabel
	   ,handbook_name
	   ,paired_field_table
	   ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,s.name
       ,s.title
       ,@recordId
       ,dv.ColName
       ,s.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.FEditable
       ,dv.Display_on_UI
       ,dv.FHasDefaultVal
       ,dv.FMandatory
       ,dv.FManual
       ,dv.ValueDesc
       ,dv.ValueLabel
	   ,dv.handbook_name
	   ,dv.paired_field_table
	   ,dv.paired_field_column
      FROM dbo.Rail_DEFValues dv
      INNER JOIN dbo.rail_sections s
        ON dv.section_id = s.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Add Pipeline Import def values manual records --
ALTER PROCEDURE dbo.pipeline_add_def_values_manual (@tableName VARCHAR(128)
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
   ,v.DefValue
   ,v.id
   ,v.ColName
  FROM dbo.Pipeline_DEFValues v
  INNER JOIN dbo.pipeline_sections s
    ON v.section_id = s.id
  INNER JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.ColName)
      AND UPPER(i.TABLE_NAME) = UPPER(s.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND UPPER(i.TABLE_NAME) = UPPER(@tableName)
  AND (v.FManual > 0
  OR v.FHasDefaultVal > 0
  OR v.Display_on_UI > 0)
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
        1
      FROM dbo.Pipeline_DEFValues_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.Pipeline_DEFValues_Manual (
        Filing_Headers_FK
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,ModifiedDate
       ,value
       ,FEditable
       ,Display_on_UI
       ,FHasDefaultVal
       ,FMandatory
       ,FManual
       ,description
       ,ValueLabel
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,s.name
       ,s.title
       ,@recordId
       ,dv.ColName
       ,s.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.FEditable
       ,dv.Display_on_UI
       ,dv.FHasDefaultVal
       ,dv.FMandatory
       ,dv.FManual
       ,dv.ValueDesc
       ,dv.ValueLabel
       ,dv.handbook_name
       ,dv.paired_field_table
       ,dv.paired_field_column
      FROM dbo.Pipeline_DEFValues dv
      INNER JOIN dbo.pipeline_sections s
        ON dv.section_id = s.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Add Truck Import def values manual records --
ALTER PROCEDURE dbo.truck_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.truck_def_values v
  INNER JOIN dbo.truck_sections s
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
      FROM dbo.truck_def_values_manual tdvm
      WHERE tdvm.record_id = @recordId
      AND tdvm.table_name = @tableName
      AND tdvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.truck_def_values_manual (
        filing_header_id
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,modification_date
       ,value
       ,editable
       ,display_on_ui
       ,has_default_value
       ,mandatory
       ,manual
       ,description
       ,label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,ts.name
       ,ts.title
       ,@recordId
       ,dv.column_name
       ,ts.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.editable
       ,dv.display_on_ui
       ,dv.has_default_value
       ,dv.mandatory
       ,dv.manual
       ,dv.description
       ,dv.label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column
      FROM dbo.truck_def_values dv
      INNER JOIN dbo.truck_sections ts
        ON dv.section_id = ts.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Add Vessel import def value manual values records --
ALTER PROCEDURE dbo.vessel_import_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.Vessel_Import_Def_Values v
  INNER JOIN dbo.Vessel_Import_Sections s
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
      FROM dbo.Vessel_Import_Def_Values_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.Vessel_Import_Def_Values_Manual (
        filing_header_id
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,modification_date
       ,value
       ,editable
       ,display_on_ui
       ,has_default_value
       ,mandatory
       ,manual
       ,description
       ,label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,s.name
       ,s.title
       ,@recordId
       ,dv.column_name
       ,s.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.editable
       ,dv.display_on_ui
       ,dv.has_default_value
       ,dv.mandatory
       ,dv.manual
       ,dv.description
       ,dv.label
       ,dv.handbook_name
       ,dv.paired_field_table
       ,dv.paired_field_column
      FROM dbo.Vessel_Import_Def_Values dv
      INNER JOIN dbo.Vessel_Import_Sections s
        ON dv.section_id = s.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO

-- Add Truck Export def values manual records --
ALTER PROCEDURE dbo.truck_export_add_def_values_manual (@tableName VARCHAR(128)
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
        1
      FROM dbo.truck_export_def_values_manual tedvm
      WHERE tedvm.record_id = @recordId
      AND tedvm.table_name = @tableName
      AND tedvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.truck_export_def_values_manual (
        filing_header_id
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,modification_date
       ,value
       ,editable
       ,display_on_ui
       ,has_default_value
       ,mandatory
       ,manual
       ,description
       ,label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
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
       ,handbook_name
       ,paired_field_table
       ,paired_field_column
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

-- Add Vessel export def value manual values records --
ALTER PROCEDURE dbo.vessel_export_add_def_values_manual (@tableName VARCHAR(128)
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
  FROM dbo.Vessel_Export_Def_Values v
  INNER JOIN dbo.Vessel_Export_Sections s
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
      FROM dbo.Vessel_Export_Def_Values_Manual dvm
      WHERE dvm.record_id = @recordId
      AND dvm.table_name = @tableName
      AND dvm.column_name = @columnName)
  BEGIN
    INSERT INTO dbo.Vessel_Export_Def_Values_Manual (
        filing_header_id
       ,parent_record_id
       ,section_name
       ,section_title
       ,record_id
       ,column_name
       ,table_name
       ,modification_date
       ,value
       ,editable
       ,display_on_ui
       ,has_default_value
       ,mandatory
       ,manual
       ,description
       ,label
       ,handbook_name
       ,paired_field_table
       ,paired_field_column)
      SELECT
        @filingHeaderId
       ,@parentId
       ,s.name
       ,s.title
       ,@recordId
       ,dv.column_name
       ,s.table_name
       ,GETDATE()
       ,@defValueOut
       ,dv.editable
       ,dv.display_on_ui
       ,dv.has_default_value
       ,dv.mandatory
       ,dv.manual
       ,dv.description
       ,dv.label
       ,dv.handbook_name
       ,dv.paired_field_table
       ,dv.paired_field_column
      FROM dbo.Vessel_Export_Def_Values dv
      INNER JOIN dbo.Vessel_Export_Sections s
        ON dv.section_id = s.id
      WHERE dv.id = @id
  END
  FETCH NEXT FROM c INTO @dataType, @defValue, @id, @columnName
  END
  CLOSE c
  DEALLOCATE c
END
GO