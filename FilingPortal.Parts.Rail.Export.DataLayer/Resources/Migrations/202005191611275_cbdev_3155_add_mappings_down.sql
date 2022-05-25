ALTER VIEW us_exp_rail.v_inbound_grid 
AS SELECT
  inbnd.id
 ,filing_header.id AS filing_header_id
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
 ,inbnd.deleted AS is_deleted
FROM us_exp_rail.inbound AS inbnd

OUTER APPLY (SELECT
    fh.id
   ,fh.filing_number
   ,fh.job_link
   ,fh.entry_status
   ,fh.mapping_status
   ,fh.filing_status
  FROM us_exp_rail.filing_header AS fh
  JOIN us_exp_rail.filing_detail AS fd
    ON fh.id = fd.filing_header_id
  WHERE fd.inbound_id = inbnd.id
  AND fh.mapping_status > 0) AS filing_header

LEFT JOIN MappingStatus AS mapping_status
  ON ISNULL(filing_header.mapping_status, 0) = mapping_status.id
LEFT JOIN FilingStatus AS filing_status
  ON ISNULL(filing_header.filing_status, 0) = filing_status.id
LEFT JOIN dbo.handbook_entry_status AS entry_status
  ON filing_header.entry_status = entry_status.code
  AND entry_status.status_type = 'export'

WHERE inbnd.deleted = 0
GO

ALTER TABLE us_exp_rail.invoice_line DROP COLUMN IF EXISTS invoice_line_number
DROP FUNCTION IF EXISTS us_exp_rail.fn_invoice_line_number
DROP TABLE IF EXISTS us_exp_rail.invoice_line
ALTER TABLE us_exp_rail.invoice_header DROP COLUMN IF EXISTS invoice_number
DROP FUNCTION IF EXISTS us_exp_rail.fn_invoice_header_number
DROP TABLE IF EXISTS us_exp_rail.invoice_header
DROP TABLE IF EXISTS us_exp_rail.declaration
DROP TABLE IF EXISTS us_exp_rail.containers
GO

-- add filing records --
ALTER PROCEDURE us_exp_rail.sp_create_entry_records (@filingHeaderId INT,
@filingUser NVARCHAR(255) = NULL)
AS
BEGIN
  DECLARE @operationId UNIQUEIDENTIFIER = NEWID();
END;
GO

DELETE FROM us_exp_rail.form_configuration
DELETE FROM us_exp_rail.form_section_configuration
GO
DROP PROCEDURE IF EXISTS us_exp_rail.sp_add_container
GO
DROP PROCEDURE IF EXISTS us_exp_rail.sp_add_declaration
GO
DROP PROCEDURE IF EXISTS us_exp_rail.sp_add_invoice_header
GO
DROP PROCEDURE IF EXISTS us_exp_rail.sp_add_invoice_line
GO