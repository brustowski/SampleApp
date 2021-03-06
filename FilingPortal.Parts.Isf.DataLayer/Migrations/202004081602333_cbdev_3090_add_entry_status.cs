// Generated Time: 04/08/2020 19:02:33
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Isf.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3090_add_entry_status : FpMigration
    {
        public override void Up()
        {
            AddColumn("isf.filing_header", "entry_status", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("isf.filing_header", "entry_status");

            ExecuteSqlFileDown();
        }
    }
}
