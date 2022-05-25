// Generated Time: 12/11/2020 19:22:56
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3537_split_inbound_table : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "zones_entry.inbound_parsed_data",
                c => new
                    {
                        id = c.Int(nullable: false),
                        filer_code = c.String(maxLength: 128, unicode: false),
                        check_digit = c.String(maxLength: 128, unicode: false),
                        import_date = c.DateTime(),
                        team_no = c.String(maxLength: 128, unicode: false),
                        nafta_recon = c.Boolean(),
                        recon_issue = c.String(maxLength: 128, unicode: false),
                        ultimate_destination_state = c.String(maxLength: 128, unicode: false),
                        application_begin_date = c.DateTime(),
                        application_end_date = c.DateTime(),
                        total_entered_value = c.Decimal(precision: 18, scale: 6),
                        gross_wgt = c.Decimal(precision: 18, scale: 6),
                        ftz_number = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("zones_entry.inbound", t => t.id)
                .Index(t => t.id);
            
            DropColumn("zones_entry.inbound", "check_digit");
            DropColumn("zones_entry.inbound", "entry_type");
            DropColumn("zones_entry.inbound", "import_date");
            DropColumn("zones_entry.inbound", "team_no");
            DropColumn("zones_entry.inbound", "summary_date");
            DropColumn("zones_entry.inbound", "statement_date");
            DropColumn("zones_entry.inbound", "nafta_recon");
            DropColumn("zones_entry.inbound", "recon_issue");
            DropColumn("zones_entry.inbound", "ultimate_destination_state");
            DropColumn("zones_entry.inbound", "unlading_port");
            DropColumn("zones_entry.inbound", "application_begin_date");
            DropColumn("zones_entry.inbound", "application_end_date");
            DropColumn("zones_entry.inbound", "nafta_deferred");
            DropColumn("zones_entry.inbound", "live_entry_flag");
            DropColumn("zones_entry.inbound", "ach");
            DropColumn("zones_entry.inbound", "paperless_flag");
            DropColumn("zones_entry.inbound", "prorate_weight");
            DropColumn("zones_entry.inbound", "total_entered_value");
            DropColumn("zones_entry.inbound", "merchandise_description3461");
            DropColumn("zones_entry.inbound", "surcharge_flag");
            DropColumn("zones_entry.inbound", "government_contract_flag");
            DropColumn("zones_entry.inbound", "gross_wgt");
            DropColumn("zones_entry.inbound", "freight_charges_advanced");
            DropColumn("zones_entry.inbound", "freight_charges_not_advanced");
            DropColumn("zones_entry.inbound", "importer_of_record_declaration");
            DropColumn("zones_entry.inbound", "purchase_agreement_declaration");
            DropColumn("zones_entry.inbound", "created_date_time");
            DropColumn("zones_entry.inbound", "calculate_duty_fees");
            DropColumn("zones_entry.inbound", "duty_amount");
            DropColumn("zones_entry.inbound", "mpf_amount");
            DropColumn("zones_entry.inbound", "total_value");
            DropColumn("zones_entry.inbound", "total_fee_amount");
            DropColumn("zones_entry.inbound", "total_amount_due");
            DropColumn("zones_entry.inbound", "currency_code");
            DropColumn("zones_entry.inbound", "ftz_number");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("zones_entry.inbound", "ftz_number", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "currency_code", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "total_amount_due", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "total_fee_amount", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "total_value", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "mpf_amount", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "duty_amount", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "calculate_duty_fees", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "created_date_time", c => c.DateTime());
            AddColumn("zones_entry.inbound", "purchase_agreement_declaration", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "importer_of_record_declaration", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "freight_charges_not_advanced", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "freight_charges_advanced", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "gross_wgt", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "government_contract_flag", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "surcharge_flag", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "merchandise_description3461", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "total_entered_value", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound", "prorate_weight", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "paperless_flag", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "ach", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "live_entry_flag", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "nafta_deferred", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "application_end_date", c => c.DateTime());
            AddColumn("zones_entry.inbound", "application_begin_date", c => c.DateTime());
            AddColumn("zones_entry.inbound", "unlading_port", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "ultimate_destination_state", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "recon_issue", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "nafta_recon", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "statement_date", c => c.DateTime());
            AddColumn("zones_entry.inbound", "summary_date", c => c.DateTime());
            AddColumn("zones_entry.inbound", "team_no", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "import_date", c => c.DateTime());
            AddColumn("zones_entry.inbound", "entry_type", c => c.String(maxLength: 2, unicode: false));
            AddColumn("zones_entry.inbound", "check_digit", c => c.String(maxLength: 128, unicode: false));
            DropForeignKey("zones_entry.inbound_parsed_data", "id", "zones_entry.inbound");
            DropIndex("zones_entry.inbound_parsed_data", new[] { "id" });
            DropTable("zones_entry.inbound_parsed_data");

            ExecuteSqlFileDown();
        }
    }
}