// Generated Time: 10/08/2020 13:50:57
// Generated By: AIKravchenko

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_3380_add_entry_modification_columns : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.exp_truck_filing_header", "last_modified_date", c => c.DateTime());
            AddColumn("dbo.exp_truck_filing_header", "last_modified_user", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("dbo.exp_truck_filing_header", "last_modified_user");
            DropColumn("dbo.exp_truck_filing_header", "last_modified_date");

            ExecuteSqlFileDown();
        }
    }
}
