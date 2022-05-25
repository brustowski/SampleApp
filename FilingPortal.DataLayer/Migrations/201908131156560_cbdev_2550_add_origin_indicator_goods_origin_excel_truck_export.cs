using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2550_add_origin_indicator_goods_origin_excel_truck_export : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.truck_exports", "origin_indicator", c => c.String(nullable: false, maxLength: 1, unicode: false));
            AddColumn("dbo.truck_exports", "goods_origin", c => c.String(nullable: false, maxLength: 10, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropColumn("dbo.truck_exports", "goods_origin");
            DropColumn("dbo.truck_exports", "origin_indicator");
        }
    }
}
