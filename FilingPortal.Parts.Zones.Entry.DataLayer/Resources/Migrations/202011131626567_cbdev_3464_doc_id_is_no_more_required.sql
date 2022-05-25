--
-- Drop index [IX_filer_code_entry_no] from table [zones_entry].[inbound]
--
DROP INDEX IF EXISTS IX_filer_code_entry_no ON zones_entry.inbound
GO

--
-- Create index [IX_filer_code_entry_no] on table [zones_entry].[inbound]
--
CREATE UNIQUE INDEX IX_filer_code_entry_no
  ON zones_entry.inbound (filer_code, entry_no)
  WHERE ([deleted] = (0))
  ON [PRIMARY]
GO

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
 ,inbnd.deleted AS is_deleted
 ,inbnd.created_user
 ,inbnd.created_date
 ,inbnd.modified_date AS modified_date
 ,inbnd.modified_user AS modified_user
 ,inbnd.is_update
 ,inbnd.is_auto
 ,inbnd.is_auto_processed
 ,inbnd.validation_passed
 ,inbnd.validation_result
 ,filing_header.filing_number
 ,filing_header.job_link
 ,filing_header.created_date AS entry_created_date
 ,filing_header.last_modified_date AS entry_modified_date
 ,entry_status.code AS entry_status
 ,entry_status.description AS entry_status_description
 ,job_status.id AS job_status
 ,job_status.[name] AS job_status_title
 ,job_status.code AS job_status_code
FROM zones_entry.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.job_status
   ,fh.created_date
   ,fh.last_modified_date
  FROM zones_entry.filing_header AS fh
  JOIN zones_entry.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.job_status > 0) AS filing_header

LEFT JOIN common.job_statuses AS job_status
  ON ISNULL(filing_header.job_status, 0) = job_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS importer
  ON inbnd.importer_id = importer.id

OUTER APPLY (SELECT TOP (1)
    doc.id
   ,doc.document_type
  FROM zones_entry.document AS doc
  WHERE doc.inbound_record_id = inbnd.id
  AND doc.document_type IN ('Form3461', 'Form7501')
  ORDER BY doc.id DESC) AS inbound_doc

WHERE inbnd.deleted = 0
GO