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
 ,form.created_date
 ,form.created_user
 ,dependency.id AS depends_on_id
 ,dependency.column_name AS depends_on
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
     ,col.DATA_TYPE AS value_type
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

ALTER TABLE inbond.bill
DROP COLUMN consignee_address_id;
GO

DELETE FROM inbond.form_configuration
WHERE section_id = 3
  AND column_name = 'consignee_address_id';
GO

ALTER VIEW inbond.v_report 
AS SELECT
  header.id AS filing_header_id
 ,detail.Z_FK AS inbound_record_id

 ,main_detail.importer AS main_detail_importer
 ,main_detail.supplier AS main_detail_supplier
 ,main_detail.branch AS main_detail_branch
 ,main_detail.mode AS main_detail_mode
 ,main_detail.move_from_whs_ftz AS main_detail_move_from_whs_ftz
 ,main_detail.firms_code AS main_detail_firms_code
 ,main_detail.transport_mode AS main_detail_transport_mode
 ,main_detail.carrier_code AS main_detail_carrier_code
 ,main_detail.conveyance AS main_detail_conveyance
 ,main_detail.voyage_trip_no AS main_detail_voyage_trip_no
 ,main_detail.carrier_country AS main_detail_carrier_country
 ,main_detail.port_of_loading AS main_detail_port_of_loading
 ,main_detail.country_of_export AS main_detail_country_of_export
 ,main_detail.importing_carrier_port_of_arrival AS main_detail_importing_carrier_port_of_arrival
 ,main_detail.date_of_sailing AS main_detail_date_of_sailing
 ,main_detail.date_of_export AS main_detail_date_of_export
 ,main_detail.eta AS main_detail_eta
 ,main_detail.port_of_presentation AS main_detail_port_of_presentation

 ,bill.issuer_code AS bill_issuer_code
 ,bill.master_bill AS bill_master_bill
 ,bill.manifest_qty AS bill_manifest_qty
 ,bill.manifest_qty_unit AS bill_manifest_qty_unit
 ,bill.weight AS bill_weight
 ,bill.weight_unit AS bill_weight_unit
 ,bill.port_of_lading_schedule_k AS bill_port_of_lading_schedule_k
 ,bill.shipper AS bill_shipper
 ,bill.consignee AS bill_consignee
 ,bill.consignee_address AS consignee_address
 ,bill.notify_party AS bill_notify_party

 ,movement_heder.in_bond_number AS movement_heder_in_bond_number
 ,movement_heder.in_bond_entry_type AS movement_heder_in_bond_entry_type
 ,movement_heder.us_port_of_destination AS movement_heder_us_port_of_destination
 ,movement_heder.foreign_destination AS movement_heder_foreign_destination
 ,movement_heder.in_bond_carrier AS movement_heder_in_bond_carrier
 ,movement_heder.bta_indicator AS movement_heder_bta_indicator
 ,movement_heder.value_in_whole_dollars AS movement_heder_value_in_whole_dollars

 ,movement_detail.in_bond_number AS movement_detail_in_bond_number
 ,movement_detail.in_bond_qty AS movement_detail_in_bond_qty
 ,movement_detail.master_bill AS movement_detail_master_bill

 ,commodities.tariff AS commodities_tariff
 ,commodities.monetary_value AS commodities_monetary_value
 ,commodities.piece_count AS commodities_piece_count
 ,commodities.manifest_unit AS commodities_manifest_unit
 ,REPLACE(commodities.description, CHAR(10), CHAR(32)) AS commodities_description
 ,REPLACE(commodities.marks_and_numbers, CHAR(10), CHAR(32)) AS commodities_marks_and_numbers
 ,commodities.weight AS commodities_weight
 ,commodities.weight_unit AS commodities_weight_unit

FROM inbond.filing_header AS header
JOIN inbond.filing_detail AS detail
  ON header.id = detail.Filing_Headers_FK
JOIN inbond.main_detail AS main_detail
  ON header.id = main_detail.filing_header_id
JOIN inbond.bill AS bill
  ON header.id = bill.filing_header_id
JOIN inbond.movement_header AS movement_heder
  ON header.id = movement_heder.filing_header_id
JOIN inbond.movement_detail AS movement_detail
  ON header.id = movement_detail.filing_header_id
JOIN inbond.commodities AS commodities
  ON movement_detail.id = commodities.parent_record_id
WHERE header.mapping_status = 2
GO

-- add bill record --
ALTER PROCEDURE inbond.sp_add_bill (@filingHeaderId INT,
@parentId INT,
@filingUser NVARCHAR(255) = NULL,
@operationId UNIQUEIDENTIFIER = NULL OUTPUT)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @tableName VARCHAR(128) = 'inbond.bill'
  DECLARE @allowMultiple BIT = 0;
  DECLARE @masterBill VARCHAR(10);
  SET @masterBill = inbond.fn_inbond_number();
  IF @masterBill IS NULL
    THROW 60000, 'Unable to generate new Master Bill number because sequence is exceeded', 1;

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
       ,consignee
       ,consignee_address
       ,master_bill)
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
       ,consignee_address.code
       ,@masterBill
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      LEFT JOIN dbo.Clients AS shipper
        ON shipper.id = rule_entry.shipper_id
      LEFT JOIN dbo.Client_Addresses AS consignee_address
        ON consignee_address.id = rule_entry.consignee_address_id

      WHERE detail.Filing_Headers_FK = @filingHeaderId;
  END
END;
GO