using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2420_UpdateTruck_FilingProc : FpMigration
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
