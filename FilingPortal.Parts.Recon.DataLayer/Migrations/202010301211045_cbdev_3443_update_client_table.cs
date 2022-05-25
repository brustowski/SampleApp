// Generated Time: 10/30/2020 15:12:31
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3443_update_client_table : FpMigration
    {
        public override void Up()
        {
            AddColumn("recon.inbound_client", "final_unit_price", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound_client", "final_total_value", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound_client", "client_note", c => c.String(maxLength: 8000, unicode: false));
            DropColumn("recon.inbound_client", "bond_type");
            DropColumn("recon.inbound_client", "surety_code");
            DropColumn("recon.inbound_client", "line_no");
            DropColumn("recon.inbound_client", "recon_job_numbers");
            DropColumn("recon.inbound_client", "main_recon_issues");
            DropColumn("recon.inbound_client", "calculated_value_recon_due_date");
            DropColumn("recon.inbound_client", "fta_recon_filing");
            DropColumn("recon.inbound_client", "cancellation");
            DropColumn("recon.inbound_client", "psa_reason");
            DropColumn("recon.inbound_client", "psa_filed_date");
            DropColumn("recon.inbound_client", "psa_reason520d");
            DropColumn("recon.inbound_client", "psa_filed_date520d");
            DropColumn("recon.inbound_client", "psa_filed_by");
            DropColumn("recon.inbound_client", "psc_explanation");
            DropColumn("recon.inbound_client", "psc_reason_codes_header");
            DropColumn("recon.inbound_client", "psc_reason_codes_line");
            DropColumn("recon.inbound_client", "liq_date");
            DropColumn("recon.inbound_client", "liq_type");
            DropColumn("recon.inbound_client", "duty_liquidated");
            DropColumn("recon.inbound_client", "total_liquidated_fees");
            DropColumn("recon.inbound_client", "cbp_form28_action");
            DropColumn("recon.inbound_client", "cbp_form29_action");
            DropColumn("recon.inbound_client", "preliminary_statement_date");
            DropColumn("recon.inbound_client", "duty_paid_date");
            DropColumn("recon.inbound_client", "payment_due_date");
            DropColumn("recon.inbound_client", "ens_status_description");
            DropColumn("recon.inbound_client", "ens_status");
            DropColumn("recon.inbound_client", "recon_job_numbers_vl");
            DropColumn("recon.inbound_client", "recon_job_numbers_nf");
        }
        
        public override void Down()
        {
            AddColumn("recon.inbound_client", "recon_job_numbers_nf", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "recon_job_numbers_vl", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "ens_status", c => c.String(maxLength: 3, unicode: false));
            AddColumn("recon.inbound_client", "ens_status_description", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "payment_due_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "duty_paid_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "preliminary_statement_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "cbp_form29_action", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "cbp_form28_action", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "total_liquidated_fees", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound_client", "duty_liquidated", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound_client", "liq_type", c => c.String(maxLength: 1, unicode: false));
            AddColumn("recon.inbound_client", "liq_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "psc_reason_codes_line", c => c.String(maxLength: 350, unicode: false));
            AddColumn("recon.inbound_client", "psc_reason_codes_header", c => c.String(maxLength: 350, unicode: false));
            AddColumn("recon.inbound_client", "psc_explanation", c => c.String(maxLength: 1024, unicode: false));
            AddColumn("recon.inbound_client", "psa_filed_by", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "psa_filed_date520d", c => c.DateTime());
            AddColumn("recon.inbound_client", "psa_reason520d", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "psa_filed_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "psa_reason", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "cancellation", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "fta_recon_filing", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "calculated_value_recon_due_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "main_recon_issues", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound_client", "recon_job_numbers", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound_client", "line_no", c => c.Int());
            AddColumn("recon.inbound_client", "surety_code", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "bond_type", c => c.String(maxLength: 8000, unicode: false));
            DropColumn("recon.inbound_client", "client_note");
            DropColumn("recon.inbound_client", "final_total_value");
            DropColumn("recon.inbound_client", "final_unit_price");
        }
    }
}
