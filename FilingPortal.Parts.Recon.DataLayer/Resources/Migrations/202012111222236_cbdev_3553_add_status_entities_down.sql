ALTER VIEW recon.v_fta_recon 
AS SELECT
  fta_recon.id
 ,inbnd.importer
 ,inbnd.importer_no
 ,inbnd.filer
 ,inbnd.entry_no
 ,inbnd.line_number7501
 ,inbnd.job_number
 ,inbnd.recon_issue
 ,inbnd.nafta_recon
 ,inbnd.calculated_client_recon_due_date
 ,inbnd.calculated520_d_due_date
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.release_date
 ,inbnd.entry_port
 ,inbnd.destination_state
 ,inbnd.entry_type
 ,inbnd.transport_mode
 ,inbnd.vessel
 ,inbnd.voyage
 ,inbnd.owner_ref
 ,inbnd.spi
 ,inbnd.co
 ,inbnd.manufacturer_mid
 ,inbnd.tariff
 ,inbnd.goods_description
 ,inbnd.container
 ,inbnd.customs_attribute1
 ,inbnd.customs_qty1
 ,inbnd.customs_uq1
 ,inbnd.master_bill
 ,inbnd.line_entered_value
 ,inbnd.invoice_line_charges
 ,inbnd.duty
 ,inbnd.hmf
 ,inbnd.mpf
 ,inbnd.payable_mpf
 ,inbnd.prior_disclosure_misc
 ,inbnd.protest_petition_filed_stat_misc
 ,inbnd.nafta303_claim_stat_misc
 ,inbnd.psa_reason
 ,inbnd.psa_filed_date
 ,fta_recon.fta_eligibility
 ,fta_recon.client_note
 ,fta_recon.status
 ,fta_recon.created_date
 ,fta_recon.created_user
FROM recon.fta_recon AS fta_recon
JOIN recon.inbound AS inbnd
  ON fta_recon.id = inbnd.id
GO

ALTER VIEW recon.v_value_recon 
AS SELECT
  value_recon.id
 ,inbnd.importer
 ,inbnd.importer_no
 ,inbnd.filer
 ,inbnd.entry_no
 ,inbnd.line_number7501
 ,inbnd.job_number
 ,inbnd.recon_issue
 ,inbnd.calculated_client_recon_due_date
 ,inbnd.export_date
 ,inbnd.import_date
 ,inbnd.release_date
 ,inbnd.entry_port
 ,inbnd.destination_state
 ,inbnd.entry_type
 ,inbnd.transport_mode
 ,inbnd.vessel
 ,inbnd.voyage
 ,inbnd.owner_ref
 ,inbnd.spi
 ,inbnd.co
 ,inbnd.manufacturer_mid
 ,inbnd.tariff
 ,inbnd.goods_description
 ,inbnd.container
 ,inbnd.customs_attribute1
 ,inbnd.customs_qty1
 ,inbnd.customs_uq1
 ,inbnd.master_bill
 ,inbnd.line_entered_value
 ,inbnd.invoice_line_charges
 ,inbnd.duty
 ,inbnd.hmf
 ,inbnd.mpf
 ,inbnd.payable_mpf
 ,inbnd.prior_disclosure_misc
 ,inbnd.protest_petition_filed_stat_misc
 ,inbnd.psa_reason
 ,inbnd.psa_filed_date
 ,inbnd.psa_reason520d
 ,inbnd.psa_filed_date520d
 ,value_recon.final_unit_price
 ,value_recon.final_total_value
 ,value_recon.client_note
 ,value_recon.status
 ,value_recon.created_date
 ,value_recon.created_user
FROM recon.value_recon AS value_recon
JOIN recon.inbound AS inbnd
  ON value_recon.id = inbnd.id
GO