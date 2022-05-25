--- rename view for Vessel Import read model ---
IF OBJECT_ID('dbo.Vessel_Inbound_Grid', 'V') IS NOT NULL
BEGIN
  DROP VIEW dbo.Vessel_Inbound_Grid
END
GO

CREATE VIEW dbo.Vessel_Import_Grid
AS
SELECT
  imports.id AS id
 ,headers.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,supplier.ClientCode AS supplier_code
 ,Vessels.name AS vessel
 ,states.StateCode AS state
 ,discharge.name AS discharge_terminal
 ,tariffs.USC_Tariff AS classification
 ,descriptions.name AS product_description
 ,imports.eta AS eta
 ,imports.user_id AS filer_id
 ,imports.container AS container
 ,imports.entry_type AS entry_type
 ,imports.created_date AS created_date
 ,ISNULL(headers.mapping_status, 0) AS mapping_status
 ,ISNULL(headers.filing_status, 0) AS filing_status
 ,headers.error_description AS error_description
 ,imports.deleted AS is_deleted
FROM dbo.Vessel_Imports imports
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Details details
  ON imports.id = details.VI_FK
LEFT OUTER JOIN dbo.Vessel_Import_Filing_Headers headers
  ON details.Filing_Headers_FK = headers.id
LEFT OUTER JOIN dbo.Clients AS importer
  ON imports.importer_id = importer.id
LEFT OUTER JOIN dbo.Clients AS supplier
  ON imports.supplier_id = supplier.id
LEFT OUTER JOIN dbo.US_States states
  ON imports.state_id = states.id
LEFT OUTER JOIN dbo.Tariff tariffs
  ON imports.classification_id = tariffs.id
LEFT OUTER JOIN dbo.Vessels
  ON imports.vessel_id = Vessels.id
LEFT OUTER JOIN dbo.Vessel_DischargeTerminals discharge
  ON imports.discharge_terminal_id = discharge.id
LEFT OUTER JOIN dbo.Vessel_ProductDescriptions descriptions
  ON imports.product_description_id = descriptions.id
GO

--- Vessel import add def value manual values ---
CREATE PROCEDURE dbo.vessel_import_add_def_values_manual (@tableName VARCHAR(128)
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

--- Vessel Import apply filing values for specified resulting table procedure ---
CREATE PROCEDURE dbo.vessel_import_apply_def_values (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  DECLARE @str VARCHAR(MAX) = ''
  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
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
    + ') ' + 'where id = ' + CAST(@recordId AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM dbo.Vessel_Import_Def_Values_Manual v
  LEFT JOIN INFORMATION_SCHEMA.COLUMNS i
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE UPPER(v.table_name) = UPPER(@tableName)
  AND i.TABLE_SCHEMA = 'dbo'
  AND v.has_default_value = 1

  EXEC (@str);
END
GO

--- Vessel import add declarations record ---
CREATE PROCEDURE dbo.vessel_import_add_declaration_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Declarations'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Declarations vid
      WHERE vid.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Declarations (
        filing_header_id
       ,main_supplier
       ,importer
       ,container
       ,ent_type
       ,eta
       ,arr
       ,vessel
       ,port_of_discharge
       ,entry_port
       ,dest_state
       ,firms_code
       ,description
       ,hmf)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,importer.ClientCode
       ,vi.container
       ,vi.entry_type
       ,vi.eta
       ,vi.eta
       ,vessel.name
       ,vrp.entry_port
       ,vrp.entry_port
       ,vrp.destination_state
       ,vrp.firms_code
       ,vpd.name
       ,vrp.hmf
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Vessels vessel
        ON vessel.id = vi.vessel_id
      LEFT JOIN dbo.Vessel_DischargeTerminals vdt
        ON vi.discharge_terminal_id = vdt.id
      LEFT JOIN dbo.Vessel_Rule_Port vrp
        ON vdt.name = vrp.discharge_name
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id

      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel import add invoice line record ---
CREATE PROCEDURE dbo.vessel_import_add_invoice_line_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Lines'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Invoice_Lines viil
      WHERE viil.invoice_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Lines (
        invoice_header_id
       ,classification
       ,tariff
       ,goods_description
       ,destination_state
       ,consignee
       ,seller
       ,attribute1
       ,attribute2
       ,epa_tsca_toxic_substance_control_act_indicator
       ,tsca_indicator
       ,certifying_individual
       ,hmf
       ,product
       ,customs_qty_unit
       ,manufacturer
       ,sold_to_party)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,tariff.USC_Tariff
       ,tariff.USC_Tariff
       ,rule_product.goods_description
       ,rule_port.destination_state
       ,importer.ClientCode
       ,supplier.ClientCode
       ,rule_product.customs_attribute1
       ,rule_product.customs_attribute2
       ,rule_product.tsca_requirement
       ,IIF(rule_product.tsca_requirement = 'D', '+', NULL)
       ,IIF(rule_product.tsca_requirement = 'D', 'CB', NULL)
       ,rule_port.hmf
       ,vpd.name
       ,tariff.Unit
       ,supplier.ClientCode
       ,importer.ClientCode
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Vessel_ProductDescriptions vpd
        ON vi.product_description_id = vpd.id
      LEFT JOIN dbo.Tariff tariff
        ON vi.classification_id = tariff.id
      LEFT JOIN dbo.Vessel_Rule_Product rule_product
        ON tariff.USC_Tariff = rule_product.tariff
      LEFT JOIN dbo.Vessel_DischargeTerminals vdt
        ON vi.discharge_terminal_id = vdt.id
      LEFT JOIN dbo.Vessel_Rule_Port rule_port
        ON vdt.name = rule_port.discharge_name
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel import add invoice header record ---
CREATE PROCEDURE dbo.vessel_import_add_invoice_header_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Invoice_Headers'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Invoice_Headers viih
      WHERE viih.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Invoice_Headers (
        filing_header_id
       ,supplier
       ,seller
       ,importer
       ,sold_to_party
       ,transaction_related)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,supplier.ClientCode
       ,supplier.ClientCode
       ,importer.ClientCode
       ,importer.ClientCode
       ,IIF(vi.importer_id = vi.supplier_id, 'Y', 'N')
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN dbo.Clients importer
        ON vi.importer_id = importer.id
      LEFT JOIN dbo.Clients supplier
        ON vi.supplier_id = supplier.id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add invoice line
    EXEC dbo.vessel_import_add_invoice_line_record @filingHeaderId
                                                  ,@recordId
                                                  ,@filingUser

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel import add misc record ---
CREATE PROCEDURE dbo.vessel_import_add_misc_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Miscs'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add misc data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Miscs vim
      WHERE vim.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Miscs (
        filing_header_id
       ,branch
       ,broker
       ,preparer_dist_port)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
       ,userData.Branch
       ,userData.Broker
       ,userData.Location
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      LEFT JOIN app_users_data userData
        ON userData.UserAccount = vi.user_id
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    PRINT @recordId

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

--- Vessel import add packing record ---
CREATE PROCEDURE dbo.vessel_import_add_packing_record (@filingHeaderId INT, @parentId INT, @filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  SET DATEFORMAT mdy;
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'Vessel_Import_Packings'
  DECLARE @allowMultiple BIT = 0
  DECLARE @IDs TABLE (
    ID INT
  );

  -- get section property is_array
  SELECT
    @allowMultiple = vis.is_array
  FROM dbo.Vessel_Import_Sections vis
  WHERE vis.table_name = @tableName

  -- add data and apply rules
  IF NOT EXISTS (SELECT
        *
      FROM dbo.Vessel_Import_Packings vip
      WHERE vip.filing_header_id = @parentId)
    OR @allowMultiple = 1
  BEGIN
    INSERT INTO Vessel_Import_Packings (
        filing_header_id)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT DISTINCT
        @parentId
      FROM dbo.Vessel_Import_Filing_Details vifd
      INNER JOIN dbo.Vessel_Imports vi
        ON vi.id = vifd.VI_FK
      WHERE vifd.Filing_Headers_FK = @filingHeaderId

    DECLARE @recordId INT
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- fill the def value manual table
    EXEC dbo.vessel_import_add_def_values_manual @tableName
                                                ,@filingHeaderId
                                                ,@parentId
                                                ,@recordId

    -- apply default values
    EXEC dbo.vessel_import_apply_def_values @tableName
                                           ,@recordId

    -- update def value manual table date with values from result table 
    EXEC dbo.update_def_values_manual 'Vessel_Import_Def_Values_Manual'
                                     ,@tableName
                                     ,@recordId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur

  END

END;
GO

-- add Vessel import filing procedure
CREATE PROCEDURE dbo.vessel_import_filing (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  -- add declaration
  EXEC dbo.vessel_import_add_declaration_record @filingHeaderId
                                               ,@filingHeaderId

  -- add invoice header
  EXEC dbo.vessel_import_add_invoice_header_record @filingHeaderId
                                                  ,@filingHeaderId

  -- add paking
  EXEC dbo.vessel_import_add_packing_record @filingHeaderId
                                           ,@filingHeaderId

  -- add misc
  EXEC dbo.vessel_import_add_misc_record @filingHeaderId
                                        ,@filingHeaderId

END;
GO

--- Vessel import inbound record soft delete proceure ---
CREATE PROCEDURE dbo.vessel_import_del (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mapping_status INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mapping_status = grid.mapping_status
  FROM Vessel_Import_Grid grid
  WHERE grid.id = @Id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE Vessel_Imports
    SET deleted = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mapping_status = 1
      OR @mapping_status = 0
    BEGIN
      UPDATE Vessel_Imports
      SET deleted = @deleted
      WHERE id IN (SELECT
          details.VI_FK
        FROM Vessel_Import_Filing_Details details
        WHERE details.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO

--- Vessel import delete record from specified resulting table procedure ---
CREATE PROCEDURE dbo.vessel_import_delete_record (@tableName VARCHAR(128)
, @recordId INT)
AS
BEGIN
  IF EXISTS (SELECT
        id
      FROM dbo.Vessel_Import_Sections vis
      WHERE vis.table_name = @tableName)
  BEGIN
    DECLARE @str VARCHAR(MAX)
    SET @str = 'DELETE FROM dbo.' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
    EXEC (@str)
  END
  ELSE
    THROW 51000, 'Invalid table name', 1
END
GO

-- Vessel import filing records delete procedure ---
CREATE PROCEDURE dbo.vessel_import_filing_del (@filingHeaderId INT)
AS
BEGIN

  DELETE FROM dbo.Vessel_Import_Def_Values_Manual
  WHERE filing_header_id = @filingHeaderId

  DELETE FROM dbo.Vessel_Import_Filing_Details
  WHERE Filing_Headers_FK = @filingHeaderId

  DELETE FROM dbo.Vessel_Import_Filing_Headers
  WHERE id = @filingHeaderId

END
GO

-- Vessel import apply filing params for all resulting tables procedure ---
CREATE PROCEDURE dbo.vessel_import_filing_param (@Filing_Headers_id INT)
AS
BEGIN

  DECLARE @str VARCHAR(MAX) = ''

  SELECT
    @str = @str + 'update  ' + v.table_name + ' set ' + v.column_name + ' = try_cast(''' + ISNULL(v.[value], '') + ''' as ' +
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
    + ') ' + 'where id = ' + CAST(v.record_id AS VARCHAR(64)) + ';'
    + CHAR(10)
  FROM INFORMATION_SCHEMA.COLUMNS i
  LEFT JOIN dbo.Vessel_Import_Def_Values_Manual v
    ON UPPER(i.COLUMN_NAME) = UPPER(v.column_name)
      AND UPPER(i.TABLE_NAME) = UPPER(v.table_name)
  WHERE i.TABLE_SCHEMA = 'dbo'
  AND v.filing_header_id = @Filing_Headers_id

  EXEC (@str);
END
GO