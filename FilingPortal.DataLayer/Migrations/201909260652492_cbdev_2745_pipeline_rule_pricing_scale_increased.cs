namespace FilingPortal.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cbdev_2745_pipeline_rule_pricing_scale_increased : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pipeline_Rule_Price", "pricing", c => c.Decimal(nullable: false, precision: 28, scale: 15));
            Sql("ALTER TABLE dbo.Pipeline_InvoiceHeaders ALTER COLUMN Invoice_Total decimal(28, 15)");
            Sql("ALTER TABLE dbo.Pipeline_InvoiceLines ALTER COLUMN PriceUnit decimal(28, 15)");
            Sql("ALTER TABLE dbo.Pipeline_InvoiceLines ALTER COLUMN Line_Price decimal(28, 15)");
        }

        public override void Down()
        {
            AlterColumn("dbo.Pipeline_Rule_Price", "pricing", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            Sql("SET ANSI_WARNINGS OFF");
            Sql("ALTER TABLE dbo.Pipeline_InvoiceHeaders ALTER COLUMN Invoice_Total decimal(18, 6)");
            Sql("ALTER TABLE dbo.Pipeline_InvoiceLines ALTER COLUMN PriceUnit decimal(18, 6)");
            Sql("ALTER TABLE dbo.Pipeline_InvoiceLines ALTER COLUMN Line_Price decimal(18, 6)");
            Sql("SET ANSI_WARNINGS ON");
        }
    }
}
