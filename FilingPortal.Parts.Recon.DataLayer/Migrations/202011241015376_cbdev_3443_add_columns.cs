// Generated Time: 11/25/2020 21:59:51
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3443_add_columns : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "recon.fta_recon",
                c => new
                    {
                        id = c.Int(nullable: false),
                        fta_eligibility = c.String(nullable: false, maxLength: 1, unicode: false),
                        client_note = c.String(maxLength: 8000, unicode: false),
                        status = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("recon.inbound", t => t.id, cascadeDelete: true)
                .Index(t => t.id);
            
            CreateTable(
                "recon.value_recon",
                c => new
                    {
                        id = c.Int(nullable: false),
                        final_unit_price = c.Decimal(precision: 18, scale: 6),
                        final_total_value = c.Decimal(precision: 18, scale: 6),
                        client_note = c.String(maxLength: 8000, unicode: false),
                        status = c.Int(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("recon.inbound", t => t.id, cascadeDelete: true)
                .Index(t => t.id);
            
            DropTable("recon.inbound_client");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            CreateTable(
                "recon.inbound_client",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        importer = c.String(maxLength: 100, unicode: false),
                        importer_no = c.String(maxLength: 259, unicode: false),
                        filer = c.String(maxLength: 100, unicode: false),
                        entry_no = c.String(maxLength: 35, unicode: false),
                        line_number7501 = c.String(maxLength: 8000, unicode: false),
                        job_number = c.String(maxLength: 25, unicode: false),
                        recon_issue = c.String(maxLength: 100, unicode: false),
                        nafta_recon = c.String(maxLength: 100, unicode: false),
                        calculated_client_recon_due_date = c.DateTime(),
                        calculated520_d_due_date = c.DateTime(),
                        export_date = c.DateTime(),
                        import_date = c.DateTime(),
                        release_date = c.DateTime(),
                        entry_port = c.String(maxLength: 4, unicode: false),
                        destination_state = c.String(maxLength: 2, unicode: false),
                        entry_type = c.String(maxLength: 100, unicode: false),
                        transport_mode = c.String(maxLength: 3, unicode: false),
                        vessel = c.String(maxLength: 35, unicode: false),
                        voyage = c.String(maxLength: 10, unicode: false),
                        owner_ref = c.String(maxLength: 35, unicode: false),
                        spi = c.String(maxLength: 8000, unicode: false),
                        co = c.String(maxLength: 2, unicode: false),
                        manufacturer_mid = c.String(maxLength: 254, unicode: false),
                        tariff = c.String(maxLength: 35, unicode: false),
                        goods_description = c.String(maxLength: 1000, unicode: false),
                        container = c.String(maxLength: 8000, unicode: false),
                        customs_attribute1 = c.String(maxLength: 128, unicode: false),
                        customs_qty1 = c.Decimal(precision: 18, scale: 6),
                        customs_uq1 = c.String(maxLength: 3, unicode: false),
                        master_bill = c.String(maxLength: 8000, unicode: false),
                        line_entered_value = c.Decimal(precision: 18, scale: 6),
                        invoice_line_charges = c.Decimal(precision: 18, scale: 6),
                        duty = c.Decimal(precision: 18, scale: 6),
                        hmf = c.Decimal(precision: 18, scale: 6),
                        mpf = c.Decimal(precision: 18, scale: 6),
                        payable_mpf = c.Decimal(precision: 18, scale: 6),
                        prior_disclosure_misc = c.String(maxLength: 100, unicode: false),
                        protest_petition_filed_stat_misc = c.String(maxLength: 100, unicode: false),
                        nafta303_claim_stat_misc = c.String(maxLength: 128, unicode: false),
                        final_unit_price = c.Decimal(precision: 18, scale: 6),
                        final_total_value = c.Decimal(precision: 18, scale: 6),
                        client_note = c.String(maxLength: 8000, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("recon.value_recon", "id", "recon.inbound");
            DropForeignKey("recon.fta_recon", "id", "recon.inbound");
            DropIndex("recon.value_recon", new[] { "id" });
            DropIndex("recon.fta_recon", new[] { "id" });
            DropTable("recon.value_recon");
            DropTable("recon.fta_recon");

            ExecuteSqlFileDown();
        }
    }
}