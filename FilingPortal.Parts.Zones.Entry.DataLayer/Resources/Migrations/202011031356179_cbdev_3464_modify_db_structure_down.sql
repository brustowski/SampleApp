ALTER VIEW zones_entry.v_inbound_grid
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id

 ,importer.ClientCode AS importer
 ,inbnd.entry_type
 ,inbnd.entry_port
 ,inbnd.arrival_date
 ,inbnd.owner_ref
 ,inbnd.firms_code
 ,inbound_doc.document_type
 ,inbound_doc.id AS doc_id
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
 ,CAST(has_importer_rule.value AS BIT) AS has_importer_rule
 ,CAST(has_importer_rule.value AS BIT) AS has_all_required_rules
 ,inbnd.deleted AS is_deleted
FROM zones_entry.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
  FROM zones_entry.filing_header AS fh
  JOIN zones_entry.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id

OUTER APPLY (SELECT
    doc.id
   ,doc.document_type
  FROM zones_entry.document AS doc
  WHERE doc.inbound_record_id = inbnd.id) AS inbound_doc

CROSS APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM zones_entry.rule_importer rule_importer
      WHERE rule_importer.importer_id = inbnd.importer_id)
    , 1, 0) AS value) AS has_importer_rule

WHERE inbnd.deleted = 0
GO

DROP PROCEDURE zones_entry.sp_inbound_validate
GO