using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2665_filer_id_fix : FpMigration
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
