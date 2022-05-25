ALTER VIEW inbond.v_form_configuration 
AS SELECT
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
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
 ,form.confirmation_needed
 ,clmn.DATA_TYPE AS value_type
 ,clmn.CHARACTER_MAXIMUM_LENGTH AS value_max_length
FROM inbond.form_configuration AS form
JOIN inbond.form_section_configuration AS sections
  ON form.section_id = sections.id
JOIN INFORMATION_SCHEMA.COLUMNS AS clmn
  ON clmn.column_name = form.column_name
    AND clmn.table_schema + '.' + clmn.table_name = sections.table_name
LEFT JOIN inbond.form_configuration AS dependency
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
AND form.marks_remarks <> 1
GO

-- review mapped data
ALTER PROCEDURE inbond.sp_review_entry (@filingHeaderIds VARCHAR(MAX)
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
     ,dependancy.column_name AS depends_on
	 ,defValue.confirmation_needed
     ,COALESCE(defValue.overridden_type, col.DATA_TYPE) AS value_type
     ,col.CHARACTER_MAXIMUM_LENGTH AS value_max_length
    FROM inbond.form_configuration AS defValue
    LEFT JOIN inbond.form_configuration AS dependancy
      ON defValue.depends_on_id = dependancy.id
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

-- add main detail record --
ALTER PROCEDURE inbond.sp_add_main_detail (@filingHeaderId INT,
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
    -- we need to know the confirmation status of the rule that was applied to this filing header
    DECLARE @confirmationNeeded BIT
    SET @confirmationNeeded = (SELECT
        rule_entry.confirmation_needed
      FROM inbond.filing_detail AS detail
      JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
        AND rule_entry.importer_id = inbnd.importer_id
        AND rule_entry.carrier = inbnd.carrier
        AND rule_entry.consignee_id = inbnd.consignee_id
        AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      WHERE detail.Filing_Headers_FK = @filingHeaderId)

    UPDATE inbond.filing_header SET confirmation_needed = @confirmationNeeded WHERE id=@filingHeaderId

    INSERT INTO inbond.main_detail (filing_header_id
    , parent_record_id
    , operation_id
    , created_date
    , created_user
    , importer
    , firms_code
    , transport_mode
    , carrier_code
    , conveyance
    , importing_carrier_port_of_arrival
    , branch
    , authorized_agent
    , entry_date
    , eta
    , port_of_presentation)
      SELECT
        @filingHeaderId
       ,@parentId
       ,@operationId
       ,GETDATE()
       ,@filingUser
       ,importer.ClientCode
       ,firms_code.firms_code
       ,transport_mode.code
       ,inbnd.carrier
       ,inbnd.export_conveyance
       ,inbnd.port_of_arrival
       ,SUBSTRING(user_data.Branch, 1, 5)
       ,user_data.Broker
       ,inbnd.created_date
       ,GETDATE()
       ,rule_entry.port_of_presentation
      FROM inbond.filing_detail AS detail
      JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.cw_firms_codes AS firms_code
        ON inbnd.firms_code_id = firms_code.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      LEFT JOIN dbo.handbook_transport_mode AS transport_mode
        ON rule_entry.transport_mode = transport_mode.code_number
      LEFT JOIN dbo.app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO