using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2700_vessel_exports_remove_invoice_total_from_intake : FpMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vessel_Exports", "invoice_total");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Exports", "invoice_total",
                c => c.Decimal(nullable: false, precision: 18, scale: 6, defaultValue: 0));

            ExecuteSqlFileDown();
        }
    }
}
