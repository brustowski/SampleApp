ALTER VIEW inbond.v_inbound_grid
AS
SELECT DISTINCT
  z.id
 ,cfc.firms_code
 ,zfh.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,z.port_of_arrival
 ,z.export_conveyance
 ,consignee.ClientCode AS consignee_code
 ,z.value
 ,z.manifest_qty
 ,z.manifest_qty_unit
 ,z.weight
 ,z.created_user
 ,'' AS filing_number
 ,'' AS job_link
 ,z.created_date
 ,ISNULL(zfh.mapping_status, 0) AS mapping_status
 ,ISNULL(zfh.filing_status, 0) AS filing_status
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
    WHEN z.export_conveyance IS NULL AND
      rule_firms_code.entry_type IN ('62', '63') THEN CAST(0 AS BIT)
    ELSE CAST(1 AS BIT)
  END AS is_export_conveyance_valid
 ,z.deleted AS is_deleted
FROM inbond.inbound AS z
LEFT JOIN cw_firms_codes AS cfc
  ON z.firms_code_id = cfc.id
LEFT JOIN inbond.filing_detail AS zfd
  ON zfd.Z_FK = z.id
LEFT JOIN inbond.filing_header AS zfh
  ON zfh.id = zfd.Filing_Headers_FK
LEFT JOIN Clients AS importer
  ON z.importer_id = importer.id
LEFT JOIN Clients AS consignee
  ON z.consignee_id = consignee.id

LEFT JOIN inbond.rule_carrier AS rule_carrier
  ON rule_carrier.importer_id = z.importer_id
    AND rule_carrier.firms_code_id = z.firms_code_id
    AND rule_carrier.consignee_id = z.consignee_id
LEFT JOIN inbond.rule_transport_mode AS rule_transport_mode
  ON rule_transport_mode.carrier = rule_carrier.carrier
LEFT JOIN inbond.rule_firms_code AS rule_firms_code
  ON rule_firms_code.firms_code_id = z.firms_code_id
    AND rule_firms_code.carrier = rule_carrier.carrier
    AND rule_firms_code.consignee_id = z.consignee_id
    AND rule_firms_code.manifest_qty = z.manifest_qty
    AND rule_firms_code.transport_mode = rule_transport_mode.transport_mode

WHERE z.deleted = 0
GO