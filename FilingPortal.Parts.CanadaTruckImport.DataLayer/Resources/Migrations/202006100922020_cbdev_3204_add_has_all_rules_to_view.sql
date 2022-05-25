ALTER VIEW canada_imp_truck.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,importer.ClientCode AS importer_code
 ,inbnd.carrier_at_import
 ,inbnd.port
 ,inbnd.pars_number
 ,inbnd.eta
 ,inbnd.owners_reference
 ,inbnd.direct_ship_date
 ,inbnd.line_price
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
 ,has_importer_rule.value AS has_importer_rule
 ,has_port_rule.value AS has_port_rule
 ,has_carrier_rule.value AS has_carrier_rule
 ,CAST(has_importer_rule.value & has_port_rule.value & has_carrier_rule.value AS BIT) AS has_all_required_rules
 ,inbnd.deleted AS is_deleted
FROM canada_imp_truck.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
  FROM canada_imp_truck.filing_header AS fh
  JOIN canada_imp_truck.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import_truck_canada'

LEFT JOIN Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN canada_imp_truck.rule_importer AS rule_importer
  ON inbnd.importer_id = rule_importer.importer_id
LEFT JOIN canada_imp_truck.rule_port AS rule_port
  ON inbnd.port = rule_port.port_of_clearance
LEFT JOIN canada_imp_truck.rule_carrier AS rule_carrier
  ON inbnd.carrier_at_import = rule_carrier.carrier

CROSS APPLY (SELECT
    IIF(rule_importer.id IS NULL, 0, 1) AS value) AS has_importer_rule
CROSS APPLY (SELECT
    IIF(rule_port.id IS NULL, 0, 1) AS value) AS has_port_rule
CROSS APPLY (SELECT
    IIF(rule_carrier.id IS NULL, 0, 1) AS value) AS has_carrier_rule

WHERE inbnd.deleted = 0
GO