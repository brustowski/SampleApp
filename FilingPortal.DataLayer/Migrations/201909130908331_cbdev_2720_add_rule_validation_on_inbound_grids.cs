using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2720_add_rule_validation_on_inbound_grids : FpMigration
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
