ALTER VIEW recon.v_inbound_grid 
AS SELECT
  inbnd.id
 ,inbnd.job_number
 ,inbnd.importer
 ,inbnd.importer_no
 ,inbnd.bond_type
 ,inbnd.surety_code
 ,inbnd.entry_type
 ,inbnd.filer
 ,inbnd.entry_no
 ,inbnd.line_no
 ,inbnd.line_number7501
 ,inbnd.recon_issue
 ,ace_report.reconciliation_indicator AS ace_recon_indicator
 ,inbnd.nafta_recon
 ,ace_report.nafta_reconciliation_indicator AS ace_nafta_recon_indicator
 ,inbnd.recon_job_numbers
 ,inbnd.main_recon_issues
 ,inbnd.calculated_value_recon_due_date
 ,inbnd.calculated520_d_due_date
 ,inbnd.calculated_client_recon_due_date
 ,inbnd.fta_recon_filing
 ,DATEADD(YEAR, 1, inbnd.import_date) AS fta_deadline
 ,DATEADD(MONTH, 21, inbnd.preliminary_statement_date) AS other_deadline
 ,inbnd.co
 ,inbnd.spi
 ,inbnd.manufacturer_mid
 ,inbnd.tariff
 ,ace_report.hts_number_full AS ace_hts_number_full
 ,inbnd.customs_qty1
 ,ROUND(ace_report.line_tariff_quantity, 0) AS ace_line_tariff_quantity
 ,inbnd.customs_uq1
 ,inbnd.line_entered_value
 ,ace_report.line_goods_value_amount AS ace_line_goods_value_amount
 ,inbnd.duty
 ,ace_report.line_duty_amount AS ace_line_duty_amount
 ,inbnd.mpf
 ,ace_report.line_mpf_amount AS ace_line_mpf_amount
 ,inbnd.payable_mpf
 ,ace_report.total_paid_mpf_amount AS ace_total_paid_mpf_amount
 ,inbnd.hmf
 ,inbnd.import_date
 ,inbnd.cancellation
 ,inbnd.psa_reason
 ,inbnd.psa_filed_date
 ,inbnd.psa_reason520d
 ,inbnd.psa_filed_date520d
 ,inbnd.psa_filed_by
 ,inbnd.psc_explanation
 ,inbnd.psc_reason_codes_header
 ,inbnd.psc_reason_codes_line
 ,inbnd.liq_date
 ,inbnd.liq_type
 ,inbnd.duty_liquidated
 ,inbnd.total_liquidated_fees
 ,inbnd.cbp_form28_action
 ,inbnd.cbp_form29_action
 ,inbnd.prior_disclosure_misc
 ,inbnd.protest_petition_filed_stat_misc
 ,inbnd.transport_mode
 ,inbnd.preliminary_statement_date
 ,inbnd.export_date
 ,inbnd.release_date
 ,inbnd.duty_paid_date
 ,inbnd.payment_due_date
 ,inbnd.entry_port
 ,inbnd.destination_state
 ,inbnd.vessel
 ,inbnd.voyage
 ,inbnd.owner_ref
 ,inbnd.ens_status_description
 ,inbnd.goods_description
 ,inbnd.container
 ,inbnd.customs_attribute1
 ,inbnd.master_bill
 ,inbnd.invoice_line_charges
 ,inbnd.ens_status
 ,inbnd.nafta303_claim_stat_misc
 ,inbnd.recon_job_numbers_vl
 ,inbnd.recon_job_numbers_nf
 ,ace_report.id AS ace_id
 ,ace_report.entry_summary_number AS ace_entry_summary_number
 ,CAST(IIF(ace_report.id IS NOT NULL, 1, 0) AS BIT) AS ace_found
 ,CAST(IIF(ace_report.id IS NOT NULL AND (ace_report.reconciliation_indicator IS NULL OR ace_report.reconciliation_indicator = '')
  OR (ace_report.reconciliation_indicator = 'Y' AND (inbnd.recon_issue IS NULL OR inbnd.recon_issue = '')
  OR (ace_report.reconciliation_indicator = 'N' AND inbnd.recon_issue <> '')), 1, 0) AS BIT) AS mismatch_recon_value_flag
 ,CAST(IIF(ace_report.id IS NOT NULL AND (ace_report.nafta_reconciliation_indicator IS NULL OR ace_report.nafta_reconciliation_indicator = '')
  OR ace_report.nafta_reconciliation_indicator = 'Y' AND (inbnd.nafta_recon IS NULL OR inbnd.nafta_recon <> 'Y'), 1, 0) AS BIT) AS mismatch_recon_fta_flag
 ,CAST(IIF(ace_report.id IS NOT NULL AND NOT ace_report.line_goods_value_amount = inbnd.line_entered_value, 1, 0) AS BIT) AS mismatch_entry_value
 ,CAST(IIF(ace_report.id IS NOT NULL AND NOT ace_report.line_duty_amount = inbnd.duty, 1, 0) AS BIT) AS mismatch_duty
 ,CAST(IIF(ace_report.id IS NOT NULL AND NOT ace_report.line_mpf_amount = inbnd.mpf, 1, 0) AS BIT) AS mismatch_mpf
 ,CAST(IIF(ace_report.id IS NOT NULL AND NOT ROUND(ace_report.line_tariff_quantity, 0) = ROUND(inbnd.customs_qty1, 0) , 1, 0) AS BIT) AS mismatch_quantity
 ,CAST(IIF(ace_report.id IS NOT NULL AND (ace_report.hts_number_full IS NULL OR ace_report.hts_number_full = '' OR NOT ace_report.hts_number_full = inbnd.tariff), 1, 0) AS BIT) AS mismatch_hts
 ,CAST(IIF(ace_report.id IS NOT NULL AND (ace_report.total_paid_mpf_amount IS NULL
  OR ace_report.total_paid_mpf_amount <> (SELECT
      SUM(calc.payable_mpf)
    FROM (SELECT DISTINCT
        i.payable_mpf
       ,i.line_number7501
      FROM recon.inbound AS i
      WHERE i.entry_no = inbnd.entry_no
      AND i.filer = inbnd.filer) AS calc))
  , 1, 0) AS BIT) AS mismatch_payable_mpf

FROM recon.inbound AS inbnd
LEFT JOIN recon.inbound_ace_report AS ace_report
  ON CONCAT(inbnd.filer, inbnd.entry_no) = ace_report.entry_summary_number
    AND inbnd.line_number7501 = ace_report.entry_summary_line_number
GO