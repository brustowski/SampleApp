ALTER VIEW inbond.v_inbound_grid 
AS SELECT DISTINCT
  inbnd.id
 ,cfc.firms_code
 ,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbnd.port_of_arrival
 ,inbnd.port_of_destination
 ,inbnd.entry_date
 ,inbnd.export_conveyance
 ,consignee.ClientCode AS consignee_code
 ,inbnd.carrier
 ,inbnd.value
 ,inbnd.manifest_qty
 ,inbnd.manifest_qty_unit
 ,inbnd.weight
 ,inbnd.created_user
 ,inbnd.created_date
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,CASE
    WHEN rule_transport_mode.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_transport_mode_rule
 ,CASE
    WHEN rule_entry.id IS NULL THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS has_entry_rule
 ,CASE
    WHEN inbnd.export_conveyance IS NULL AND
      rule_entry.entry_type IN ('62', '63') THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS is_export_conveyance_valid
 ,inbnd.deleted AS is_deleted
FROM inbond.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
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
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code

LEFT JOIN cw_firms_codes AS cfc
  ON inbnd.firms_code_id = cfc.id
LEFT JOIN Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN Clients AS consignee
  ON inbnd.consignee_id = consignee.id

LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
  ON rule_transport_mode.importer_id = inbnd.importer_id
    AND rule_transport_mode.firms_code_id = inbnd.firms_code_id
    AND rule_transport_mode.consignee_id = inbnd.consignee_id
    AND rule_transport_mode.carrier = inbnd.carrier
LEFT JOIN inbond.rule_entry AS rule_entry
  ON rule_entry.firms_code_id = inbnd.firms_code_id
    AND rule_entry.importer_id = inbnd.importer_id
    AND rule_entry.carrier = inbnd.carrier
    AND rule_entry.consignee_id = inbnd.consignee_id
    AND rule_entry.us_port_of_destination = inbnd.port_of_destination

WHERE inbnd.deleted = 0
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
       ,entry_date
       ,eta
       ,port_of_presentation)
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
       ,IIF(rule_transport_mode.transport_mode = '10', NULL, 1)
       ,SUBSTRING(user_data.Branch, 1, 5)
       ,user_data.Broker
       ,inbnd.created_date
       ,GETDATE()
       ,rule_entry.port_of_presentation
      FROM inbond.filing_detail AS detail
      INNER JOIN inbond.inbound AS inbnd
        ON inbnd.id = detail.Z_FK
      LEFT JOIN dbo.cw_firms_codes AS firms_code
        ON inbnd.firms_code_id = firms_code.id
      LEFT JOIN dbo.Clients AS importer
        ON inbnd.importer_id = importer.id
      LEFT JOIN dbo.Clients AS consignee
        ON inbnd.consignee_id = consignee.id      
      LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
        ON rule_transport_mode.importer_id = inbnd.importer_id
          AND rule_transport_mode.firms_code_id = inbnd.firms_code_id
          AND rule_transport_mode.consignee_id = inbnd.consignee_id
          AND rule_transport_mode.carrier = inbnd.carrier
      LEFT JOIN dbo.handbook_transport_mode AS transport_mode
        ON rule_transport_mode.transport_mode = transport_mode.code_number
      LEFT JOIN inbond.rule_entry AS rule_entry
        ON rule_entry.firms_code_id = inbnd.firms_code_id
          AND rule_entry.importer_id = inbnd.importer_id
          AND rule_entry.carrier = inbnd.carrier
          AND rule_entry.consignee_id = inbnd.consignee_id
          AND rule_entry.us_port_of_destination = inbnd.port_of_destination
      LEFT JOIN dbo.app_users_data AS user_data
        ON user_data.UserAccount = @filingUser
      WHERE detail.Filing_Headers_FK = @filingHeaderId
  END
END;
GO
