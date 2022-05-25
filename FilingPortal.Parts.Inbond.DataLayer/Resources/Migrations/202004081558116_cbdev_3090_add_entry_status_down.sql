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
  ON rule_transport_mode.importer_id = inbnd.importer_id
    AND rule_transport_mode.firms_code_id = inbnd.firms_code_id
    AND rule_transport_mode.consignee_id = inbnd.consignee_id
    AND rule_transport_mode.carrier = rule_carrier.carrier
LEFT JOIN inbond.rule_entry AS rule_entry
  ON rule_entry.firms_code_id = inbnd.firms_code_id
    AND rule_entry.importer_id = inbnd.importer_id
    AND rule_entry.carrier = rule_carrier.carrier
    AND rule_entry.consignee_id = inbnd.consignee_id
    AND rule_entry.us_port_of_destination = inbnd.port_of_destination

WHERE inbnd.deleted = 0
GO