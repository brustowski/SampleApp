using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2198_add_single_filing_support : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rail_DEFValues", "SingleFilingOrder", c => c.Byte());

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropColumn("dbo.Rail_DEFValues", "SingleFilingOrder");
        }
    }
}
