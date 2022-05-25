
/****** Object:  View [zones_ftz214].[v_inbound_grid]    Script Date: 22.4.2021 г. 12:28:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER   VIEW [zones_ftz214].[v_inbound_grid]
AS
SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
 ,applicant.ClientCode AS applicant
 ,inbnd.ein
 ,operator.ClientCode as ftz_operator
 ,inbnd.zone_id
 ,inbnd.admission_type
 ,inbound_doc.document_type
 ,inbound_doc.id AS doc_id
 ,inbnd.created_user
 ,inbnd.created_date
 ,inbnd.modified_date
 ,inbnd.modified_user
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
 ,inbnd.deleted AS is_deleted
 ,job_status.id AS job_status
 ,job_status.[name] AS job_status_title
 ,job_status.code AS job_status_code
FROM zones_ftz214.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.job_status
   ,fh.last_modified_date
   ,fh.created_date
  FROM zones_ftz214.filing_header AS fh
  JOIN zones_ftz214.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.job_status > 0) AS filing_header
  
LEFT JOIN common.job_statuses AS job_status
  ON ISNULL(filing_header.job_status, 0) = job_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
    AND entry_status.status_type = 'in-bond'
LEFT JOIN dbo.Clients AS applicant
  ON inbnd.applicant_id = applicant.id
LEFT JOIN dbo.Clients AS operator
  ON inbnd.applicant_id = operator.id

OUTER APPLY (SELECT
    doc.id
   ,doc.document_type
  FROM zones_ftz214.document AS doc
  WHERE doc.inbound_record_id = inbnd.id) AS inbound_doc

WHERE inbnd.deleted = 0
GO

