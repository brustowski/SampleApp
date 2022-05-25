ALTER VIEW dbo.v_imp_pipeline_inbound_grid
AS
SELECT
  inbnd.id
 ,inbnd.importer
 ,inbnd.batch
 ,inbnd.ticket_number
 ,inbnd.facility
 ,inbnd.site_name
 ,inbnd.quantity
 ,inbnd.api
 ,inbnd.entry_number
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.created_date
 ,inbnd.[deleted]
 ,filing_header.id AS filing_header_id
 ,filing_header.filing_number
 ,filing_header.job_link
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,mapping_status.id AS mapping_status
 ,mapping_status.[name] AS mapping_status_title
 ,filing_status.id AS filing_status
 ,filing_status.[name] AS filing_status_title
 ,rule_importer.found AS has_importer_rule
 ,batch.found AS has_batch_rule
 ,facility.found AS has_facility_rule
 ,rule_price.found AS has_price_rule
 ,(CAST(rule_importer.found & batch.found & facility.found & rule_price.found AS BIT)) AS has_all_required_rules
FROM dbo.imp_pipeline_inbound inbnd

OUTER APPLY (SELECT
    etfh.id
   ,etfh.filing_number
   ,etfh.job_link
   ,etfh.entry_status
   ,etfh.mapping_status
   ,etfh.filing_status
  FROM dbo.imp_pipeline_filing_header etfh
  JOIN dbo.imp_pipeline_filing_detail etfd
    ON etfh.id = etfd.filing_header_id
  WHERE etfd.inbound_id = inbnd.id
  AND etfh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'import'
LEFT JOIN dbo.imp_pipeline_rule_facility rule_facility
  ON inbnd.facility = rule_facility.facility
LEFT JOIN dbo.imp_pipeline_rule_batch_code rule_batch
  ON dbo.fn_imp_pipeline_batch_code(inbnd.batch) = rule_batch.batch_code
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_rule_importer rule_importer
      WHERE inbnd.importer = rule_importer.importer)
    , 1, 0) AS found) AS rule_importer
OUTER APPLY (SELECT
    IIF(rule_batch.id IS NOT NULL, 1, 0) AS found, rule_batch.id) AS batch
OUTER APPLY (SELECT
    IIF(rule_facility.id IS NOT NULL, 1, 0) AS found,rule_facility.id) AS facility
OUTER APPLY (SELECT
    IIF(EXISTS (SELECT
        1
      FROM dbo.imp_pipeline_rule_price rule_price
      INNER JOIN dbo.Clients clients
        ON rule_price.importer_id = clients.id
        AND ((rule_price.crude_type_id IS NULL
        AND rule_price.facility_id IS NULL)
        OR (rule_price.crude_type_id = batch.id
        AND rule_price.facility_id IS NULL)
        OR (rule_price.crude_type_id = batch.id
        AND rule_price.facility_id = facility.id))
      WHERE inbnd.importer = clients.ClientCode
      AND clients.id = rule_price.importer_id)
    , 1, 0) AS found) AS rule_price

WHERE inbnd.deleted = 0
GO