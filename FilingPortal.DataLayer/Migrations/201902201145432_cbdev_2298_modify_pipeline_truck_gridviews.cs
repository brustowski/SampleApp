using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2298_modify_pipeline_truck_gridviews : FpMigration
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
