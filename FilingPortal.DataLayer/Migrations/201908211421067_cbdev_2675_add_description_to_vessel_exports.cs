using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2675_add_description_to_vessel_exports : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Exports", "description", c => c.String(nullable: false, maxLength: 512, unicode: false));

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();

            DropColumn("dbo.Vessel_Exports", "description");
        }
    }
}
