namespace FilingPortal.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_2220_pipeline_rules : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pipeline_Rule_BatchCode",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    batch_code = c.String(nullable: false, maxLength: 128, unicode: false),
                    product = c.String(maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.batch_code, unique: true, name: "Idx_BatchCode");

            CreateTable(
                "dbo.Pipeline_Rule_Importer",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    broker_to_pay = c.String(maxLength: 128, unicode: false),
                    consignee = c.String(maxLength: 128, unicode: false),
                    country_of_export = c.String(maxLength: 128, unicode: false),
                    freight = c.Decimal(precision: 18, scale: 6),
                    fta_recon = c.String(maxLength: 128, unicode: false),
                    importer = c.String(nullable: false, maxLength: 128, unicode: false),
                    manufacturer = c.String(maxLength: 128, unicode: false),
                    manufacturer_address = c.String(maxLength: 128, unicode: false),
                    mid = c.String(maxLength: 128, unicode: false),
                    origin = c.String(maxLength: 128, unicode: false),
                    payment_type = c.Int(),
                    recon_issue = c.String(maxLength: 128, unicode: false),
                    seller = c.String(maxLength: 128, unicode: false),
                    spi = c.String(maxLength: 128, unicode: false),
                    supplier = c.String(maxLength: 128, unicode: false),
                    transaction_related = c.String(maxLength: 128, unicode: false),
                    value = c.Decimal(precision: 18, scale: 6),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.importer, unique: true, name: "Idx_Importer");

            CreateTable(
                "dbo.Pipeline_Rule_PortImporter",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    customs_attribute3 = c.String(maxLength: 128, unicode: false),
                    description = c.String(maxLength: 128, unicode: false),
                    destination = c.String(maxLength: 128, unicode: false),
                    destination_state = c.String(maxLength: 128, unicode: false),
                    firms_code = c.String(maxLength: 128, unicode: false),
                    importer = c.String(maxLength: 128, unicode: false),
                    issuer = c.String(maxLength: 128, unicode: false),
                    origin = c.String(maxLength: 128, unicode: false),
                    pipeline = c.String(maxLength: 128, unicode: false),
                    port = c.String(nullable: false, maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => new { t.port, t.importer }, unique: true, name: "Idx_Port_Importer");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropIndex("dbo.Pipeline_Rule_PortImporter", "Idx_Port_Importer");
            DropIndex("dbo.Pipeline_Rule_Importer", "Idx_Importer");
            DropIndex("dbo.Pipeline_Rule_BatchCode", "Idx_BatchCode");
            DropTable("dbo.Pipeline_Rule_PortImporter");
            DropTable("dbo.Pipeline_Rule_Importer");
            DropTable("dbo.Pipeline_Rule_BatchCode");
        }
    }
}
