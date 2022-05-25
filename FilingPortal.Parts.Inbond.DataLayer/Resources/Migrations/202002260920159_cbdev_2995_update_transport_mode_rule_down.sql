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
       ,@masterBill
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
ALTER PROCEDURE inbond.sp_add_commodities (@filingHeaderId INT,
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
       ,importing_carrier_port_of_arrival
       ,voyage_trip_no
       ,branch
       ,authorized_agent
       ,entry_date)
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
       ,IIF(rule_transport_mode.transport_mode = '10', NULL, 1)
       ,SUBSTRING(user_data.Branch, 1, 5)
       ,user_data.Broker
       ,GETDATE()
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
      LEFT JOIN dbo.app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO

-- add movement header record --
ALTER PROCEDURE inbond.sp_add_movement_header (@filingHeaderId INT,
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
  SELECT @inBondNum = bill.master_bill FROM inbond.bill AS bill;
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

ALTER VIEW inbond.v_inbound_grid 
AS SELECT DISTINCT
  inbnd.id
 ,cfc.firms_code
 ,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbnd.port_of_arrival
 ,inbnd.export_conveyance
 ,consignee.ClientCode AS consignee_code
 ,inbnd.value
 ,inbnd.manifest_qty
 ,inbnd.manifest_qty_unit
 ,inbnd.weight
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_carrier.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_carrier_rule
 ,CASE
    WHEN rule_transport_mode.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_transport_mode_rule
 ,CASE
    WHEN rule_firms_code.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_firms_code_rule
 ,CASE
    WHEN inbnd.export_conveyance IS NULL AND
      rule_firms_code.entry_type IN ('62', '63') THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS is_export_conveyance_valid
 ,inbnd.deleted AS is_deleted
FROM inbond.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.mapping_status
   ,fh.filing_status
  FROM inbond.filing_header AS fh
  JOIN inbond.filing_detail AS fd
    ON fh.id = fd.Filing_Headers_FK
  WHERE fd.Z_FK = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id

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

WHERE inbnd.deleted = 0
GO