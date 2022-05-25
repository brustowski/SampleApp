INSERT INTO dbo.handbook_entry_status (status_type, code ,description) VALUES 
('in-bond', 'AAV', 'Awaiting In-Bond Arrival'),
('in-bond', 'ADA', 'Awaiting In-Bond Departure Replace'),
('in-bond', 'ADO', 'Awaiting In-Bond Departure Add'),
('in-bond', 'ADW', 'Awaiting In-Bond Departure Delete'),
('in-bond', 'AEX', 'Awaiting In-Bond Exportation'),
('in-bond', 'ATL', 'Awaiting In-Bond Transfer Of Liability'),
('in-bond', 'CAV', 'Clear In-Bond Arrival'),
('in-bond', 'CDA', 'Clear In-Bond Departure Replace'),
('in-bond', 'CDO', 'Clear In-Bond Departure Add'),
('in-bond', 'CPA', 'Clear In-Bond Departure Partial Replace (Check Messages tab for the status of each bill of lading)'),
('in-bond', 'CPO', 'Clear In-Bond Departure Partial Add (Check Messages tab for the status of each bill of lading)'),
('in-bond', 'CPW', 'Clear In-Bond Departure Partial Delete (Check Messages tab for the status of each bill of lading)'),
('in-bond', 'CDW', 'Clear In-Bond Departure Delete'),
('in-bond', 'CEX', 'Clear In-Bond Exportation'),
('in-bond', 'CTL', 'Clear In-Bond Transfer Of Liability'),
('in-bond', 'EAV', 'Error In-Bond Arrival'),
('in-bond', 'EDA', 'Error In-Bond Departure Replace'),
('in-bond', 'EDO', 'Error In-Bond Departure Add'),
('in-bond', 'EDW', 'Error In-Bond Departure Delete'),
('in-bond', 'EEX', 'Error In-Bond Exportation'),
('in-bond', 'ETL', 'Error In-Bond Transfer Of Liability'),
('in-bond', 'NOT', 'Not Sent - only valid for exact match');
GO

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
  AND entry_status.status_type = 'in-bond'

LEFT JOIN cw_firms_codes AS cfc
  ON inbnd.firms_code_id = cfc.id
LEFT JOIN Clients AS importer
  ON inbnd.importer_id = importer.id
LEFT JOIN Clients AS consignee
  ON inbnd.consignee_id = consignee.id

LEFT JOIN inbond.rule_entry AS rule_entry
  ON rule_entry.firms_code_id = inbnd.firms_code_id
    AND rule_entry.importer_id = inbnd.importer_id
    AND rule_entry.carrier = inbnd.carrier
    AND rule_entry.consignee_id = inbnd.consignee_id
    AND rule_entry.us_port_of_destination = inbnd.port_of_destination

WHERE inbnd.deleted = 0
GO