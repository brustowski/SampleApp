ALTER VIEW recon.v_inbound_grid 
AS SELECT
  i.id
 ,i.job_number
 ,i.importer
 ,iar.importer_name AS ace_importer
 ,i.importer_no
 ,iar.importer_number AS ace_importer_no
 ,i.bond_type
 ,i.surety_code
 ,i.entry_type
 ,iar.entry_type_code AS ace_entry_type
 ,i.filer
 ,i.entry_no
 ,i.line_no
 ,i.line_number7501
 ,i.recon_issue
 ,iar.reconciliation_issue_code AS ace_recon_issue
 ,i.nafta_recon
 ,iar.nafta_reconciliation_indicator AS ace_nafta_recon
 ,i.recon_job_numbers
 ,i.main_recon_issues
 ,i.calculated_value_recon_due_date
 ,i.calculated520_d_due_date
 ,i.calculated_client_recon_due_date
 ,i.fta_recon_filing
 ,DATEADD(YEAR, 1, i.import_date) AS fta_deadline
 ,DATEADD(MONTH, 21, i.preliminary_statement_date) AS other_deadline
 ,i.co
 ,iar.country_of_origin_code AS ace_co
 ,i.spi
 ,iar.line_spi_code AS ace_spi
 ,i.manufacturer_mid
 ,iar.manufacturer_id AS ace_manufacturer_mid
 ,i.tariff
 ,i.customs_qty1
 ,i.customs_uq1
 ,i.line_entered_value
 ,i.duty
 ,i.mpf
 ,iar.line_mpf_amount AS ace_mpf
 ,i.payable_mpf
 ,i.hmf
 ,i.import_date
 ,iar.importation_date AS ace_import_date
 ,i.cancellation
 ,i.psa_reason
 ,i.psa_filed_date
 ,i.psa_reason520d
 ,i.psa_filed_date520d
 ,i.psa_filed_by
 ,i.psc_explanation
 ,i.psc_reason_codes_header
 ,i.psc_reason_codes_line
 ,i.liq_date
 ,i.liq_type
 ,i.duty_liquidated
 ,i.total_liquidated_fees
 ,i.cbp_form28_action
 ,i.cbp_form29_action
 ,i.prior_disclosure_misc
 ,i.protest_petition_filed_stat_misc
 ,i.transport_mode
 ,i.preliminary_statement_date
 ,i.export_date
 ,i.release_date
 ,i.duty_paid_date
 ,i.payment_due_date
 ,i.entry_port
 ,i.destination_state
 ,i.vessel
 ,i.voyage
 ,i.owner_ref
 ,i.ens_status_description
 ,i.goods_description
 ,i.container
 ,i.customs_attribute1
 ,i.master_bill
 ,i.invoice_line_charges
 ,i.ens_status
 ,i.nafta303_claim_stat_misc
 ,i.recon_job_numbers_vl
 ,i.recon_job_numbers_nf
 ,i.status
 ,CAST(IIF(iar.id IS NOT NULL, 1, 0) AS BIT) AS ace_found
 ,CAST(IIF(iar.importer_name IS NOT NULL AND NOT i.importer = iar.importer_name, 1, 0) AS BIT) AS err_importer_name
 ,CAST(IIF(iar.importer_number IS NOT NULL AND NOT i.importer_no = iar.importer_number, 1, 0) AS BIT) AS err_importer_no
 ,CAST(IIF(iar.entry_type_code IS NOT NULL AND NOT i.entry_type = iar.entry_type_code, 1, 0) AS BIT) AS err_entry_type
 ,CAST(IIF(iar.importation_date IS NOT NULL AND NOT i.import_date = iar.importation_date, 1, 0) AS BIT) AS err_import_date
 ,CAST(IIF(iar.reconciliation_issue_code IS NOT NULL AND NOT i.recon_issue = iar.reconciliation_issue_code, 1, 0) AS BIT) AS err_recon_issue
 ,CAST(IIF(iar.nafta_reconciliation_indicator IS NOT NULL AND NOT i.nafta_recon = iar.nafta_reconciliation_indicator, 1, 0) AS BIT) AS err_nafta_recon
 ,CAST(IIF(iar.country_of_origin_code IS NOT NULL AND NOT i.co = iar.country_of_origin_code, 1, 0) AS BIT) AS err_country_of_origin
 ,CAST(IIF(iar.line_spi_code IS NOT NULL AND NOT i.spi = iar.line_spi_code, 1, 0) AS BIT) AS err_spi
 ,CAST(IIF(iar.manufacturer_id IS NOT NULL AND NOT i.manufacturer_mid = iar.manufacturer_id, 1, 0) AS BIT) AS err_manufacturer_id
 ,CAST(IIF(iar.line_mpf_amount IS NOT NULL AND NOT i.mpf = iar.line_mpf_amount, 1, 0) AS BIT) AS err_mpf
FROM recon.inbound AS i
LEFT JOIN recon.inbound_ace_report as iar
  ON CONCAT(i.filer, i.entry_no) = iar.entry_summary_number
    AND i.line_no = iar.entry_summary_line_number
GO