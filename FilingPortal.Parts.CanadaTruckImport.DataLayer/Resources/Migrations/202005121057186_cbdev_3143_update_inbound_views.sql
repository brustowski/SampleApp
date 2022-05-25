INSERT INTO dbo.handbook_entry_status (status_type, code ,description) VALUES 
('import_truck_canada','ACC','Acknowledged Change'),
('import_truck_canada','ACD','Acknowledged Cancel'),
('import_truck_canada','ACO','Acknowledged Original'),
('import_truck_canada','ACR','Acknowledged Amendment'),
('import_truck_canada','AWC','Awaiting Change'),
('import_truck_canada','AWD','Awaiting Cancel'),
('import_truck_canada','AWO','Awaiting Original'),
('import_truck_canada','AWR','Awaiting Amendment'),
('import_truck_canada','CLC','Accepted Change'),
('import_truck_canada','CLD','Accepted Cancel'),
('import_truck_canada','CLO','Accepted Original'),
('import_truck_canada','CLR','Error Amendment'),
('import_truck_canada','ERC','Error Change'),
('import_truck_canada','ERD','Error Cancel'),
('import_truck_canada','ERO','Error Original'),
('import_truck_canada','ERR','Error Amendment'),
('import_truck_canada','UNK','Unknown'),
('import_truck_canada','NOT','Not Sent'),
('import_truck_canada','SNT','Sent');
GO

ALTER VIEW canada_imp_truck.v_inbound_grid 
AS SELECT
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

WHERE inbnd.deleted = 0
GO