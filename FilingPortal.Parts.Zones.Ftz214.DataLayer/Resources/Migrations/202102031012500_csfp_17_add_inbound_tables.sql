INSERT dbo.App_Permissions(id, description, name) VALUES (21200, 'View Zones Ftz214 Records Permission', 'ZonesFtz214ViewInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21201, 'Delete Zones Ftz214 Records Permission', 'ZonesFtz214DeleteInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21202, 'File Zones Ftz214 Records Permission', 'ZonesFtz214FileInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21203, 'Import Zones Ftz214 Records Permission', 'ZonesFtz214ImportInboundRecord');
INSERT dbo.App_Permissions(id, description, name) VALUES (21204, 'View Zones Ftz214 Rules Permission', 'ZonesFtz214ViewRules');
INSERT dbo.App_Permissions(id, description, name) VALUES (21205, 'Edit Zones Ftz214 Rules Permission', 'ZonesFtz214EditRules');
INSERT dbo.App_Permissions(id, description, name) VALUES (21206, 'Delete Zones Ftz214 Rules Permission', 'ZonesFtz214DeleteRules');

SET IDENTITY_INSERT dbo.App_Roles ON
INSERT dbo.App_Roles(id, title, description) VALUES (21200, 'ZonesFtz214User', 'The role with following permissions: View, Edit, and File Zones Ftz214 inbound data.')
INSERT dbo.App_Roles(id, title, description) VALUES (21201, 'ZonesFtz214PowerUser', 'The role with following permissions: View and Edit Zones Ftz214 rules data.')
SET IDENTITY_INSERT dbo.App_Roles OFF

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21200, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21201, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21202, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21203, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21204, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21205, 1)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21206, 1)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21200, 21200)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21201, 21200)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21202, 21200)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21203, 21200)

INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21200, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21201, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21202, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21203, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21204, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21205, 21201)
INSERT dbo.App_Permissions_Roles(App_Permissions_FK, App_Roles_FK) VALUES (21206, 21201)
GO

CREATE VIEW zones_ftz214.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,applicant.ClientCode AS applicant
 ,inbnd.ein
 ,ftz_operator.ClientCode AS ftz_operator
 ,inbnd.zone_id
 ,inbnd.admission_type
 ,job_status.id AS job_status
 ,job_status.[name] AS job_status_title
 ,job_status.code AS job_status_code
 ,inbnd.modified_date AS modified_date
 ,inbnd.modified_user AS modified_user
 ,inbnd.is_update
 ,inbnd.is_auto
 ,inbnd.is_auto_processed
 ,inbnd.validation_passed
 ,inbnd.validation_result
 ,filing_header.created_date AS entry_created_date
 ,filing_header.last_modified_date AS entry_modified_date
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,filing_header.filing_number
 ,filing_header.job_link
 ,inbnd.created_date
 ,inbnd.deleted AS is_deleted
 ,inbnd.created_user
FROM zones_ftz214.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.job_status
   ,fh.created_date
   ,fh.last_modified_date
  FROM zones_entry.filing_header AS fh
  JOIN zones_entry.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.job_status > 0) AS filing_header

LEFT JOIN common.job_statuses AS job_status
  ON ISNULL(filing_header.job_status, 0) = job_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS applicant
  ON inbnd.applicant_id = applicant.id
LEFT JOIN dbo.Clients AS ftz_operator
  ON inbnd.ftz_operator_id = ftz_operator.id

WHERE inbnd.deleted = 0

GO

CREATE VIEW zones_ftz214.v_form_configuration
AS
SELECT
  form.id
 ,form.label
 ,constr.[value] AS default_value
 ,form.description
 ,sections.table_name
 ,form.column_name AS column_name
 ,sections.name AS section_name
 ,sections.title AS section_title
 ,form.handbook_name
 ,form.paired_field_table
 ,form.paired_field_column
 ,form.display_on_ui
 ,form.manual
 ,form.single_filing_order
 ,CAST(form.has_default_value AS BIT) AS has_default_value
 ,CAST(form.editable AS BIT) AS editable
 ,CAST(form.mandatory AS BIT) AS mandatory
 ,form.overridden_type
 ,form.created_date
 ,form.created_user
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
 ,form.confirmation_needed
FROM zones_ftz214.form_configuration AS form
JOIN zones_ftz214.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
LEFT JOIN zones_ftz214.form_configuration AS dependency
  ON dependency.id = form.depends_on_id
OUTER APPLY (SELECT
    REPLACE(SUBSTRING(df.definition, 2, LEN(df.definition) - 2), '''', '') AS [value]
  FROM sys.columns AS c
  JOIN sys.default_constraints AS df
    ON df.parent_object_id = c.object_id
      AND df.parent_column_id = c.column_id
  WHERE c.object_id = OBJECT_ID(sections.table_name, 'U')
  AND c.Name = form.column_name) AS constr
WHERE form.column_name NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

CREATE VIEW zones_ftz214.v_field_configuration
AS
SELECT
  CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
 ,s.table_name AS table_name
 ,i.COLUMN_NAME AS column_name
 ,s.id AS section_id
 ,s.title AS section_title
FROM information_schema.columns AS i
INNER JOIN zones_ftz214.form_section_configuration AS s
  ON i.TABLE_SCHEMA + '.' + i.TABLE_NAME = s.table_name
WHERE i.COLUMN_NAME NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id')
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
CREATE PROCEDURE zones_ftz214.sp_update_entry (@json VARCHAR(MAX))
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

  INSERT INTO @result (id
  , record_id
  , [value]
  , table_name
  , column_name
  , row_num)
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
    INNER JOIN zones_ftz214.form_configuration config
      ON config.id = field.id
    INNER JOIN zones_ftz214.form_section_configuration section
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
END
GO

CREATE PROCEDURE zones_ftz214.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
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
  FROM zones_ftz214.form_section_configuration rs
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
     ,dependancy.column_name AS depends_on
     ,COALESCE(defValue.overridden_type, col.DATA_TYPE) AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM zones_ftz214.form_configuration AS defValue
    LEFT JOIN zones_ftz214.form_configuration AS dependancy
      ON defValue.depends_on_id = dependancy.id
    INNER JOIN zones_ftz214.form_section_configuration AS section
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

CREATE PROCEDURE zones_ftz214.sp_recalculate (@filingHeaderId INT
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
  INSERT INTO @config (id
  , record_id
  , parent_record_id
  , value
  , column_name
  , table_name)
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
    LEFT JOIN zones_ftz214.form_configuration conf
      ON inbnd.Id = conf.id
    INNER JOIN zones_ftz214.form_section_configuration section
      ON conf.section_id = section.id;

  -- calculate new values
 
  --DECLARE @tbl AS TABLE (
  --  record_id INT NOT NULL
  -- ,parent_record_id INT NOT NULL
  -- ,quantity DECIMAL(18, 6)
  -- ,unit_price DECIMAL(18, 6)
  --);mapping_status

  --INSERT INTO @tbl (record_id
  --, parent_record_id
  --, quantity
  --, unit_price)
  --  SELECT
  --    a.record_id
  --   ,a.parent_record_id
  --   ,CONVERT(DECIMAL(18, 6), a.value) AS quantity
  --   ,CONVERT(DECIMAL(18, 6), b.value) AS unit_price
  --  FROM @config a
  --  JOIN @config b
  --    ON a.record_id = b.record_id
  --  WHERE a.table_name = 'invoice_line'
  --  AND a.column_name = 'invoice_qty'
  --  AND b.column_name = 'unit_price';

  ---- calculate
  DECLARE @tblUpdatedFields field_update_list;
  ---- invoice line price
  --INSERT INTO @tblUpdatedFields (id
  --, record_id
  --, parent_record_id
  --, value)
  --  SELECT
  --    id
  --   ,fields.record_id
  --   ,fields.parent_record_id
  --   ,FORMAT(quantity * unit_price, '0.##############')
  --  FROM @config AS fields
  --  JOIN @tbl AS tbl
  --    ON fields.record_id = tbl.record_id
  --  WHERE table_name = 'invoice_line'
  --  AND column_name = 'price';
  ---- invoice header invoice total
  --DECLARE @total DECIMAL(18, 6);
  --SELECT
  --  @total = SUM(quantity * unit_price)
  --FROM @tbl;
  --INSERT INTO @tblUpdatedFields (id
  --, record_id
  --, parent_record_id
  --, value)
  --  SELECT
  --    id
  --   ,fields.record_id
  --   ,fields.parent_record_id
  --   ,FORMAT(@total, '0.##############')
  --  FROM @config AS fields
  --  WHERE table_name = 'invoice_header'
  --  AND column_name IN ('invoice_total', 'line_total');

  ---- declaration origin
  --INSERT INTO @tblUpdatedFields (id
  --, record_id
  --, parent_record_id
  --, value)
  --  SELECT
  --    field.id
  --   ,field.record_id
  --   ,field.parent_record_id
  --   ,foreign_port.unloco
  --  FROM @config AS field
  --  JOIN @config AS field2
  --    ON field.record_id = field2.record_id
  --      AND field2.column_name = 'port_of_loading'
  --  LEFT JOIN dbo.CW_Foreign_Ports foreign_port
  --    ON field2.value = foreign_port.port_code
  --  WHERE field.table_name = 'declaration'
  --  AND field.column_name IN ('origin', 'loading_unloco');


  -- return data
  SET @jsonUpdatedFields = (SELECT
      *
    FROM @tblUpdatedFields
    FOR JSON PATH, INCLUDE_NULL_VALUES);
END;
GO

CREATE PROCEDURE zones_ftz214.sp_delete_inbound (@id INT,
@deleted BIT)
AS
BEGIN
  DECLARE @filingHeaderId INT = NULL
  DECLARE @jobStatus INT = NULL

  SELECT
   @jobStatus = grid.job_status
  FROM zones_ftz214.v_inbound_grid grid
  WHERE grid.id = @id

  IF @filingHeaderId IS NULL
  BEGIN
    UPDATE zones_ftz214.inbound
    SET [deleted] = @deleted
    WHERE id = @id
  END
  ELSE
  BEGIN
    IF @jobStatus = 1
      OR @jobStatus = 0
    BEGIN
      UPDATE zones_ftz214.inbound
      SET [deleted] = @deleted
      WHERE id IN (SELECT
          detail.inbound_id
        FROM zones_ftz214.filing_detail detail
        WHERE detail.filing_header_id = @filingHeaderId)
    END
  END
END
GO

CREATE PROCEDURE zones_ftz214.sp_delete_entry_records (@recordId INT, @tableName VARCHAR(128) = NULL)
AS
BEGIN
  SET NOCOUNT ON
  IF @tableName IS NULL
  BEGIN
    DELETE FROM zones_ftz214.filing_detail
    WHERE filing_header_id = @recordId

    DELETE FROM zones_ftz214.filing_header
    WHERE id = @recordId
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT
          id
        FROM zones_ftz214.form_section_configuration ps
        WHERE ps.table_name = @tableName)
    BEGIN
      DECLARE @str VARCHAR(MAX)
      SET @str = 'DELETE FROM ' + @tableName + ' WHERE id=' + CAST(@recordId AS VARCHAR(64)) + '; '
      EXEC (@str)
    END
    ELSE
      THROW 51000, 'Invalid table name', 1
  END
END
GO

CREATE PROCEDURE zones_ftz214.sp_inbound_validate (@ids VARCHAR(MAX))
AS
BEGIN
  DECLARE @tbl TABLE (
    id INT NOT NULL
  )

  IF (@ids IS NULL)
    INSERT INTO @tbl (id)
      SELECT
        inbnd.id
      FROM zones_ftz214.inbound AS inbnd
  ELSE
  BEGIN
    INSERT INTO @tbl (id)
      SELECT
        CAST(value AS INT)
      FROM STRING_SPLIT(@ids, ',');
  END;

  WITH cte
  AS
  (SELECT
      inbnd.id
     ,applicant_set.found AS applicant_set
     ,(SELECT
          [name] AS message
        FROM (VALUES (
        CASE applicant_set.found
          WHEN 0 THEN applicant_set.message
          ELSE NULL
        END
        )) X ([name])
        FOR JSON PATH)
      AS validation_json_result
    FROM @tbl t
    INNER JOIN zones_ftz214.inbound inbnd
      ON t.id = inbnd.id
    LEFT JOIN dbo.Clients AS applicant
      ON inbnd.applicant_id = applicant.id   
    CROSS APPLY (SELECT
        IIF(inbnd.applicant_id IS NULL, 0, 1) AS found
       ,'Applicant is not set for EIN = "' + inbnd.ein + '"' AS message) AS applicant_set)

  UPDATE inbnd
  SET validation_passed = t.applicant_set
     ,validation_result = validation_json_result
  FROM cte t
  INNER JOIN zones_ftz214.inbound AS inbnd
    ON t.id = inbnd.id;
END
GO