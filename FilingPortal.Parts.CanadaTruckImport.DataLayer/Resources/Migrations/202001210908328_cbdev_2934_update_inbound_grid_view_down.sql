ALTER VIEW canada_imp_truck.v_inbound_grid 
AS SELECT DISTINCT
  inbound.id
 ,fh.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbound.carrier_at_import
 ,inbound.port
 ,inbound.pars_number
 ,inbound.eta
 ,inbound.owners_reference
 ,inbound.direct_ship_date
 ,inbound.created_user
 ,'' AS filing_number
 ,'' AS job_link
 ,inbound.created_date
 ,ISNULL(fh.mapping_status, 0) AS mapping_status
 ,ISNULL(fh.filing_status, 0) AS filing_status
 ,CASE
    WHEN rule_importer.id IS NULL THEN 0
    ELSE 1
  END AS has_importer_rule
  ,CASE
    WHEN rule_port.id IS NULL THEN 0
    ELSE 1
  END AS has_port_rule
  ,CASE
    WHEN rule_carrier.id IS NULL THEN 0
    ELSE 1
  END AS has_carrier_rule
 ,inbound.deleted AS is_deleted
FROM canada_imp_truck.inbound inbound
LEFT JOIN canada_imp_truck.filing_detail fd
  ON fd.inbound_id = inbound.id
LEFT JOIN canada_imp_truck.filing_header fh
  ON fh.id = fd.filing_header_id
LEFT JOIN Clients importer
  ON inbound.importer_id = importer.id
LEFT JOIN canada_imp_truck.rule_importer rule_importer
  ON inbound.importer_id = rule_importer.importer_id
LEFT JOIN canada_imp_truck.rule_port rule_port
  ON inbound.port = rule_port.port_of_clearance
LEFT JOIN canada_imp_truck.rule_carrier rule_carrier
  ON inbound.carrier_at_import = rule_carrier.carrier
WHERE inbound.deleted = 0
GO