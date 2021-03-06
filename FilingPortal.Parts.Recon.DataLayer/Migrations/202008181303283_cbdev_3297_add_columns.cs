// Generated Time: 08/24/2020 19:17:38
// Generated By: aikravchenko

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3297_add_columns : FpMigration
    {
        public override void Up()
        {
            Sql("TRUNCATE TABLE recon.inbound");
            Sql("TRUNCATE TABLE recon.inbound_client");

            AddColumn("recon.inbound", "calculated_client_recon_due_date", c => c.DateTime());
            AddColumn("recon.inbound", "preliminary_statement_date", c => c.DateTime());
            AddColumn("recon.inbound", "export_date", c => c.DateTime());
            AddColumn("recon.inbound", "release_date", c => c.DateTime());
            AddColumn("recon.inbound", "duty_paid_date", c => c.DateTime());
            AddColumn("recon.inbound", "payment_due_date", c => c.DateTime());
            AddColumn("recon.inbound", "entry_port", c => c.String(maxLength: 4, unicode: false));
            AddColumn("recon.inbound", "destination_state", c => c.String(maxLength: 2, unicode: false));
            AddColumn("recon.inbound", "vessel", c => c.String(maxLength: 35, unicode: false));
            AddColumn("recon.inbound", "voyage", c => c.String(maxLength: 10, unicode: false));
            AddColumn("recon.inbound", "owner_ref", c => c.String(maxLength: 35, unicode: false));
            AddColumn("recon.inbound", "ens_status_description", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound", "goods_description", c => c.String(maxLength: 1000, unicode: false));
            AddColumn("recon.inbound", "container", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound", "customs_attribute1", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound", "master_bill", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound", "invoice_line_charges", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound", "ens_status", c => c.String(maxLength: 3, unicode: false));
            AddColumn("recon.inbound", "nafta303_claim_stat_misc", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound", "recon_job_numbers_vl", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound", "recon_job_numbers_nf", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "calculated_client_recon_due_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "preliminary_statement_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "export_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "release_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "duty_paid_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "payment_due_date", c => c.DateTime());
            AddColumn("recon.inbound_client", "entry_port", c => c.String(maxLength: 4, unicode: false));
            AddColumn("recon.inbound_client", "destination_state", c => c.String(maxLength: 2, unicode: false));
            AddColumn("recon.inbound_client", "vessel", c => c.String(maxLength: 35, unicode: false));
            AddColumn("recon.inbound_client", "voyage", c => c.String(maxLength: 10, unicode: false));
            AddColumn("recon.inbound_client", "owner_ref", c => c.String(maxLength: 35, unicode: false));
            AddColumn("recon.inbound_client", "ens_status_description", c => c.String(maxLength: 100, unicode: false));
            AddColumn("recon.inbound_client", "goods_description", c => c.String(maxLength: 1000, unicode: false));
            AddColumn("recon.inbound_client", "container", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound_client", "customs_attribute1", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "master_bill", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("recon.inbound_client", "invoice_line_charges", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("recon.inbound_client", "ens_status", c => c.String(maxLength: 3, unicode: false));
            AddColumn("recon.inbound_client", "nafta303_claim_stat_misc", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "recon_job_numbers_vl", c => c.String(maxLength: 128, unicode: false));
            AddColumn("recon.inbound_client", "recon_job_numbers_nf", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("recon.inbound", "psa_filed_date", c => c.DateTime());
            AlterColumn("recon.inbound", "psa_filed_date520d", c => c.DateTime());
            AlterColumn("recon.inbound_client", "psa_filed_date", c => c.DateTime());
            AlterColumn("recon.inbound_client", "psa_filed_date520d", c => c.DateTime());

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AlterColumn("recon.inbound_client", "psa_filed_date520d", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("recon.inbound_client", "psa_filed_date", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("recon.inbound", "psa_filed_date520d", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("recon.inbound", "psa_filed_date", c => c.String(maxLength: 100, unicode: false));
            DropColumn("recon.inbound_client", "recon_job_numbers_nf");
            DropColumn("recon.inbound_client", "recon_job_numbers_vl");
            DropColumn("recon.inbound_client", "nafta303_claim_stat_misc");
            DropColumn("recon.inbound_client", "ens_status");
            DropColumn("recon.inbound_client", "invoice_line_charges");
            DropColumn("recon.inbound_client", "master_bill");
            DropColumn("recon.inbound_client", "customs_attribute1");
            DropColumn("recon.inbound_client", "container");
            DropColumn("recon.inbound_client", "goods_description");
            DropColumn("recon.inbound_client", "ens_status_description");
            DropColumn("recon.inbound_client", "owner_ref");
            DropColumn("recon.inbound_client", "voyage");
            DropColumn("recon.inbound_client", "vessel");
            DropColumn("recon.inbound_client", "destination_state");
            DropColumn("recon.inbound_client", "entry_port");
            DropColumn("recon.inbound_client", "payment_due_date");
            DropColumn("recon.inbound_client", "duty_paid_date");
            DropColumn("recon.inbound_client", "release_date");
            DropColumn("recon.inbound_client", "export_date");
            DropColumn("recon.inbound_client", "preliminary_statement_date");
            DropColumn("recon.inbound_client", "calculated_client_recon_due_date");
            DropColumn("recon.inbound", "recon_job_numbers_nf");
            DropColumn("recon.inbound", "recon_job_numbers_vl");
            DropColumn("recon.inbound", "nafta303_claim_stat_misc");
            DropColumn("recon.inbound", "ens_status");
            DropColumn("recon.inbound", "invoice_line_charges");
            DropColumn("recon.inbound", "master_bill");
            DropColumn("recon.inbound", "customs_attribute1");
            DropColumn("recon.inbound", "container");
            DropColumn("recon.inbound", "goods_description");
            DropColumn("recon.inbound", "ens_status_description");
            DropColumn("recon.inbound", "owner_ref");
            DropColumn("recon.inbound", "voyage");
            DropColumn("recon.inbound", "vessel");
            DropColumn("recon.inbound", "destination_state");
            DropColumn("recon.inbound", "entry_port");
            DropColumn("recon.inbound", "payment_due_date");
            DropColumn("recon.inbound", "duty_paid_date");
            DropColumn("recon.inbound", "release_date");
            DropColumn("recon.inbound", "export_date");
            DropColumn("recon.inbound", "preliminary_statement_date");
            DropColumn("recon.inbound", "calculated_client_recon_due_date");

            ExecuteSqlFileDown();
        }
    }
}
