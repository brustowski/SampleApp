using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2633_correct_rail_inbound_view_master_bill : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
