using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2652_add_validation_to_main_screen : FpMigration
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
