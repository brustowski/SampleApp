ALTER VIEW recon.v_fta_recon_report 
AS SELECT
  fta_recon.id
 ,inbnd.job_number
 ,inbnd.filer
 ,inbnd.entry_no
 ,inbnd.owner_ref
 ,inbnd.master_bill
 ,fta_recon.fta_eligibility
 ,IIF(fta_recon.fta_eligibility = 'Y', 'File', 'Expired') AS fta_recon_filing
FROM recon.fta_recon AS fta_recon
JOIN recon.inbound AS inbnd
  ON inbnd.id = fta_recon.id
WHERE fta_recon.status = 1
GO

CREATE VIEW recon.v_value_recon_report
AS
SELECT
  value_recon.id
 ,inbnd.job_number
 ,inbnd.filer
 ,inbnd.entry_no
 ,inbnd.owner_ref
 ,inbnd.master_bill
 ,value_recon.final_unit_price
 ,value_recon.final_total_value
FROM recon.value_recon AS value_recon
JOIN recon.inbound AS inbnd
  ON inbnd.id = value_recon.id
WHERE value_recon.status = 1
GO