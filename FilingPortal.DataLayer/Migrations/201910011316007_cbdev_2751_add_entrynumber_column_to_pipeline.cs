using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2751_add_entrynumber_column_to_pipeline : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pipeline_Inbound", "entry_number", c => c.String(nullable: false, maxLength: 11, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("dbo.Pipeline_Inbound", "entry_number");

            ExecuteSqlFileDown();
        }
    }
}
