using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2649_update_vessel_intake_form : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFileUp();

            DropColumn("dbo.Vessel_Exports", "description");
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Exports", "description", c => c.String(nullable: false, maxLength: 128, unicode: false));

            ExecuteSqlFileDown();
        }
    }
}
