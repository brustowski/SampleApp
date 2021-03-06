// Generated Time: 06/10/2020 15:01:42
// Generated By: iapetrov

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3194_add_recon_carcass : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "recon.inbound",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        job_number = c.String(maxLength: 25, unicode: false),
                        importer = c.String(maxLength: 100, unicode: false),
                        importer_no = c.String(maxLength: 259, unicode: false),
                        bond_type = c.String(maxLength: 8000, unicode: false),
                        surety_code = c.String(maxLength: 100, unicode: false),
                        entry_type = c.String(maxLength: 100, unicode: false),
                        filer = c.String(maxLength: 100, unicode: false),
                        entry_no = c.String(maxLength: 35, unicode: false),
                        line_no = c.Int(),
                        line_number7501 = c.String(maxLength: 8000, unicode: false),
                        recon_issue = c.String(maxLength: 100, unicode: false),
                        nafta_recon = c.String(maxLength: 100, unicode: false),
                        recon_job_numbers = c.String(maxLength: 8000, unicode: false),
                        main_recon_issues = c.String(maxLength: 8000, unicode: false),
                        calculated_value_recon_due_date = c.DateTime(),
                        calculated520_d_due_date = c.DateTime(),
                        fta_recon_filing = c.String(maxLength: 100, unicode: false),
                        co = c.String(maxLength: 2, unicode: false),
                        spi = c.String(maxLength: 8000, unicode: false),
                        manufacturer_mid = c.String(maxLength: 254, unicode: false),
                        tariff = c.String(maxLength: 35, unicode: false),
                        customs_qty1 = c.Decimal(precision: 18, scale: 6),
                        customs_uq1 = c.String(maxLength: 3, unicode: false),
                        line_entered_value = c.Decimal(precision: 18, scale: 6),
                        duty = c.Decimal(precision: 18, scale: 6),
                        mpf = c.Decimal(precision: 18, scale: 6),
                        payable_mpf = c.Decimal(precision: 18, scale: 6),
                        hmf = c.Decimal(precision: 18, scale: 6),
                        import_date = c.DateTime(),
                        cancellation = c.String(maxLength: 100, unicode: false),
                        psa_reason = c.String(maxLength: 100, unicode: false),
                        psa_filed_date = c.String(maxLength: 100, unicode: false),
                        psa_reason520d = c.String(maxLength: 100, unicode: false),
                        psa_filed_date520d = c.String(maxLength: 100, unicode: false),
                        psa_filed_by = c.String(maxLength: 100, unicode: false),
                        psc_explanation = c.String(maxLength: 1024, unicode: false),
                        psc_reason_codes_header = c.String(maxLength: 350, unicode: false),
                        psc_reason_codes_line = c.String(maxLength: 350, unicode: false),
                        liq_date = c.DateTime(),
                        liq_type = c.String(maxLength: 1, unicode: false),
                        duty_liquidated = c.Decimal(precision: 18, scale: 6),
                        total_liquidated_fees = c.Decimal(precision: 18, scale: 6),
                        cbp_form28_action = c.String(maxLength: 100, unicode: false),
                        cbp_form29_action = c.String(maxLength: 100, unicode: false),
                        prior_disclosure_misc = c.String(maxLength: 100, unicode: false),
                        protest_petition_filed_stat_misc = c.String(maxLength: 100, unicode: false),
                        transport_mode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.id);
         
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();

            DropTable("recon.inbound");
        }
    }
}
