// Generated Time: 09/18/2020 16:19:09
// Generated By: AIKravchenko

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_3334_add_columns_update_procedures : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.exp_truck_update_record", "load_port", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.exp_truck_update_record", "goods_description", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("dbo.exp_truck_update_record", "goods_description");
            DropColumn("dbo.exp_truck_update_record", "load_port");

            ExecuteSqlFileDown();
        }
    }
}
