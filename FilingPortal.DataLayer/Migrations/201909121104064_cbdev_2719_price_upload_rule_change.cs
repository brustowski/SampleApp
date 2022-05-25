using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2719_price_upload_rule_change : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Pipeline_Rule_Price", new[] { "crude_type_id" });
            AlterColumn("dbo.Pipeline_Rule_Price", "crude_type_id", c => c.Int());
            CreateIndex("dbo.Pipeline_Rule_Price", "crude_type_id");

            ExecuteSqlFile("201909121104064_cbdev_2719_price_upload_rule_change_migration.sql");

            DropColumn("dbo.Pipeline_Rule_Importer", "freight");
            DropColumn("dbo.Pipeline_Rule_Importer", "value");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Pipeline_Rule_Importer", "value", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("dbo.Pipeline_Rule_Importer", "freight", c => c.Decimal(precision: 18, scale: 6));
            DropIndex("dbo.Pipeline_Rule_Price", new[] { "crude_type_id" });
            AlterColumn("dbo.Pipeline_Rule_Price", "crude_type_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Pipeline_Rule_Price", "crude_type_id");

            ExecuteSqlFileDown();
        }
    }
}
