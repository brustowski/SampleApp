CREATE FUNCTION inbond.fn_inbond_number ()
RETURNS VARCHAR(10)
AS
BEGIN
  DECLARE @current VARCHAR(10);
  DECLARE @currentNum INT;
  DECLARE @next VARCHAR(10);
  DECLARE @nextNum INT = 1;

  SELECT
    @current = movement_header.in_bond_number
  FROM inbond.filing_header AS filing_header
  JOIN inbond.movement_header AS movement_header
    ON filing_header.id = movement_header.filing_header_id
  WHERE filing_header.mapping_status IS NOT NULL
  AND filing_header.mapping_status <> 0
  ORDER BY movement_header.created_date DESC
  OFFSET 0 ROWS FETCH NEXT 1 ROW ONLY;

  IF @current IS NOT NULL
  BEGIN
    DECLARE @part VARCHAR(2) = RIGHT(@current, 2);
    SET @currentNum = CAST(@part AS INT);
    SET @nextNum = @currentNum + 1;
    IF @nextNum > 99
      RETURN NULL;
  END;

  SET @next = FORMAT(GETDATE(), N'MMddyyyy') + FORMAT(@nextNum, N'D2');
  RETURN @next;
END
GO

-- update filing entry
CREATE PROCEDURE inbond.sp_update_entry (@json VARCHAR(MAX))
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @result TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
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
     ,[value]
     ,table_name
     ,column_name
     ,row_num)
    SELECT
      field.id
     ,field.record_id
     ,field.[value]
     ,section.table_name
     ,config.column_name
     ,ROW_NUMBER() OVER (PARTITION BY config.column_name, section.table_name, field.record_id ORDER BY field.id)
    FROM OPENJSON(@json)
    WITH (id INT
    , record_id INT
    , value VARCHAR(512)) AS field
    INNER JOIN inbond.form_configuration AS config
      ON config.id = field.id
    INNER JOIN inbond.form_section_configuration AS section
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
END
GO

-- review mapped data
CREATE PROCEDURE inbond.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM inbond.form_section_configuration AS rs
  WHERE rs.table_name IS NOT NULL
  AND rs.is_hidden = 0;

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
    FROM inbond.form_configuration AS defValue
    INNER JOIN inbond.form_section_configuration AS section
      ON defValue.section_id = section.id
    JOIN @result AS r
      ON defValue.column_name = r.column_name
        AND section.table_name = r.table_name
    LEFT JOIN INFORMATION_SCHEMA.COLUMNS AS col
      ON col.COLUMN_NAME = r.column_name
        AND col.TABLE_SCHEMA + '.' + col.TABLE_NAME = r.table_name
    FOR JSON PATH, INCLUDE_NULL_VALUES)

  RETURN 0;
END;
GO

-- recalculate rail fileds
CREATE PROCEDURE inbond.sp_recalculate (@filingHeaderId INT
, @jsonFields VARCHAR(MAX)
, @jsonUpdatedFields VARCHAR(MAX) OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  -- parse and enrich data
  DECLARE @config TABLE (
    id INT NOT NULL
   ,record_id INT NOT NULL
   ,parent_record_id INT NOT NULL
   ,value VARCHAR(512)
   ,column_name VARCHAR(100)
   ,table_name VARCHAR(100)
  );
  INSERT INTO @config (
      id
     ,record_id
     ,parent_record_id
     ,value
     ,column_name
     ,table_name)
    SELECT
      inbnd.id
     ,inbnd.record_id
     ,inbnd.parent_record_id
     ,inbnd.value
     ,conf.column_name
     ,section.table_name
    FROM OPENJSON(@jsonFields)
    WITH (id INT
    , record_id INT
    , parent_record_id INT
    , value VARCHAR(512)) AS inbnd
    LEFT JOIN inbond.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN inbond.form_section_configuration section
      ON conf.section_id = section.id;

  DECLARE @tblUpdatedFields field_update_list;
  -- calculate new values

  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

-- soft delete inbound record
CREATE PROCEDURE inbond.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @mappingStatus INT = NULL

  SELECT
    @filingHeaderId = grid.filing_header_id
   ,@mappingStatus = grid.mapping_status
  FROM inbond.v_inbound_grid AS grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE inbond.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @mappingStatus = 1
      OR @mappingStatus = 0
    BEGIN
      UPDATE inbond.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.Z_FK
        FROM inbond.filing_detail AS detail
        WHERE detail.Filing_Headers_FK = @filingHeaderId)
    END
  END
END
GO

-- delete filing entry
CREATE PROCEDURE inbond.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON;
  IF @tableName IS NULL
  BEGIN
    DELETE FROM inbond.filing_detail
    WHERE Filing_Headers_FK = @recordId;

    DELETE FROM inbond.filing_header
    WHERE id = @recordId;
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM inbond.form_section_configuration AS ps
        WHERE ps.table_name = @tableName)
    BEGIN
      DECLARE @str VARCHAR(MAX);
      SET @str = 'DELETE FROM ' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; ';
      EXEC (@str);
    END
    ELSE
      THROW 51000, 'Invalid table name', 1;
  END
END
GO

-- add main detail record --
CREATE PROCEDURE inbond.sp_add_main_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.main_detail'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.main_detail AS main_detail
      WHERE main_detail.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.main_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,importer
       ,firms_code
       ,transport_mode
       ,carrier_code
       ,conveyance
       ,importing_carrier_port_of_arrival)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,importer.ClientCode
       ,cfc.firms_code
       ,rule_transport_mode.transport_mode
       ,rule_carrier.carrier
       ,inbnd.export_conveyance
       ,inbnd.port_of_arrival
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN cw_firms_codes AS cfc
        ON inbnd.firms_code_id = cfc.id
      LEFT JOIN Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_carrier AS rule_carrier
        ON rule_carrier.importer_id = inbnd.importer_id
          AND rule_carrier.firms_code_id = inbnd.firms_code_id
          AND rule_carrier.consignee_id = inbnd.consignee_id
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.carrier = rule_carrier.carrier
      LEFT JOIN inbond.rule_firms_code AS rule_firms_code
        ON rule_firms_code.firms_code_id = inbnd.firms_code_id
          AND rule_firms_code.carrier = rule_carrier.carrier
          AND rule_firms_code.consignee_id = inbnd.consignee_id
          AND rule_firms_code.manifest_qty = inbnd.manifest_qty
          AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

-- add bill record --
CREATE PROCEDURE inbond.sp_add_bill (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.bill'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.bill AS bill
      WHERE bill.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.bill (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,manifest_qty
       ,manifest_qty_unit
       ,weight
       ,shipper
       ,consignee)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,inbnd.manifest_qty
       ,inbnd.manifest_qty_unit
       ,inbnd.weight
       ,shipper.ClientCode
       ,consignee.ClientCode
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_carrier AS rule_carrier
        ON rule_carrier.importer_id = inbnd.importer_id
          AND rule_carrier.firms_code_id = inbnd.firms_code_id
          AND rule_carrier.consignee_id = inbnd.consignee_id
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.carrier = rule_carrier.carrier
      LEFT JOIN inbond.rule_firms_code AS rule_firms_code
        ON rule_firms_code.firms_code_id = inbnd.firms_code_id
          AND rule_firms_code.carrier = rule_carrier.carrier
          AND rule_firms_code.consignee_id = inbnd.consignee_id
          AND rule_firms_code.manifest_qty = inbnd.manifest_qty
          AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode
      LEFT JOIN Clients AS shipper
        ON shipper.id = rule_firms_code.shipper_id
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

-- add commodities record --
CREATE PROCEDURE inbond.sp_add_commodities (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.commodities'
  DECLARE @allowMultiple BIT = 0;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add commodities data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.commodities AS commodities
      WHERE commodities.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.commodities (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,tariff
       ,monetary_value
       ,piece_count
       ,manifest_unit
       ,weight)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,rule_firms_code.tariff
       ,inbnd.value
       ,inbnd.manifest_qty
       ,inbnd.manifest_qty_unit
       ,inbnd.weight
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN inbond.rule_carrier AS rule_carrier
        ON rule_carrier.importer_id = inbnd.importer_id
          AND rule_carrier.firms_code_id = inbnd.firms_code_id
          AND rule_carrier.consignee_id = inbnd.consignee_id
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.carrier = rule_carrier.carrier
      LEFT JOIN inbond.rule_firms_code AS rule_firms_code
        ON rule_firms_code.firms_code_id = inbnd.firms_code_id
          AND rule_firms_code.carrier = rule_carrier.carrier
          AND rule_firms_code.consignee_id = inbnd.consignee_id
          AND rule_firms_code.manifest_qty = inbnd.manifest_qty
          AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

-- add movement detail record --
CREATE PROCEDURE inbond.sp_add_movement_detail (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.movement_detail'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );
  DECLARE @billNum VARCHAR(9);
  DECLARE @inBondNum VARCHAR(10);

  SET @operationId = COALESCE(@operationId, NEWID());

  SELECT
    @billNum = bill.master_bill
  FROM inbond.bill AS bill
  WHERE bill.filing_header_id = @filingHeaderId
  ORDER BY bill.created_date DESC
  OFFSET 0 ROWS FETCH NEXT 1 ROW ONLY;

  SELECT
    @inBondNum = movement_header.in_bond_number
  FROM inbond.movement_header AS movement_header
  WHERE movement_header.filing_header_id = @filingHeaderId
  ORDER BY movement_header.created_date DESC
  OFFSET 0 ROWS FETCH NEXT 1 ROW ONLY;

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add declaration data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.movement_detail AS movement_detail
      WHERE movement_detail.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.movement_detail (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,master_bill
       ,in_bond_number
       ,in_bond_qty)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,@billNum
       ,@inBondNum
       ,inbnd.manifest_qty
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      WHERE detail.Filing_Headers_FK = @filingHeaderId;

    DECLARE @recordId INT;
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add commodities
    EXEC inbond.sp_add_commodities @filingHeaderId
                                  ,@recordId
                                  ,@filingUser
                                  ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO

-- add movement header record --
CREATE PROCEDURE inbond.sp_add_movement_header (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.movement_header'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @IDs TABLE (
    ID INT
  );
  DECLARE @inBondNum VARCHAR(10);
  SET @inBondNum = inbond.fn_inbond_number();
  IF @inBondNum IS NULL
    THROW 60000, 'Unable to generate new In-Bond number because sequence is exceeded', 1;

  SET @operationId = COALESCE(@operationId, NEWID());

  -- get section property is_array
  SELECT
    @allowMultiple = section.is_array
  FROM inbond.form_section_configuration AS section
  WHERE section.table_name = @tableName

  -- add movement header data
  IF @allowMultiple = 1
    OR NOT EXISTS (SELECT
        1
      FROM inbond.movement_header AS movement_header
      WHERE movement_header.filing_header_id = @filingHeaderId)
  BEGIN
    INSERT INTO inbond.movement_header (
        filing_header_id
       ,parent_record_id
       ,operation_id
       ,created_date
       ,created_user
       ,in_bond_entry_type
       ,us_port_of_destination
       ,foreign_destination
       ,in_bond_carrier
       ,value_in_whole_dollars
       ,in_bond_number)
    OUTPUT INSERTED.ID INTO @IDs (ID)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,rule_firms_code.entry_type
       ,rule_firms_code.us_port_of_destination
       ,rule_firms_code.foreign_destination
       ,rule_carrier.carrier
       ,inbnd.value
       ,@inBondNum
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN inbond.rule_carrier AS rule_carrier
        ON rule_carrier.importer_id = inbnd.importer_id
          AND rule_carrier.firms_code_id = inbnd.firms_code_id
          AND rule_carrier.consignee_id = inbnd.consignee_id
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.carrier = rule_carrier.carrier
      LEFT JOIN inbond.rule_firms_code AS rule_firms_code
        ON rule_firms_code.firms_code_id = inbnd.firms_code_id
          AND rule_firms_code.carrier = rule_carrier.carrier
          AND rule_firms_code.consignee_id = inbnd.consignee_id
          AND rule_firms_code.manifest_qty = inbnd.manifest_qty
          AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode
      WHERE detail.Filing_Headers_FK = @filingHeaderId;

    DECLARE @recordId INT;
    DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
      ID
    FROM @IDs

    OPEN cur

    FETCH NEXT FROM cur INTO @recordId
    WHILE @@FETCH_STATUS = 0
    BEGIN

    -- add commodities
    EXEC inbond.sp_add_movement_detail @filingHeaderId
                                      ,@recordId
                                      ,@filingUser
                                      ,@operationId

    FETCH NEXT FROM cur INTO @recordId

    END

    CLOSE cur
    DEALLOCATE cur
  END
END;
GO

-- add filing records --
CREATE PROCEDURE inbond.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN

  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();

  -- add main detail
  EXEC inbond.sp_add_main_detail @filingHeaderId
                                ,@filingHeaderId
                                ,@filingUser
                                ,@operationId
  -- add bill
  EXEC inbond.sp_add_bill @filingHeaderId
                         ,@filingHeaderId
                         ,@filingUser
                         ,@operationId
  -- add movement header
  EXEC inbond.sp_add_movement_header @filingHeaderId
                                    ,@filingHeaderId
                                    ,@filingUser
                                    ,@operationId
END;
GO