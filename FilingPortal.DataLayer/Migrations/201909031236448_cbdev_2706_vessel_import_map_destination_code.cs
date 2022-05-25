using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2706_vessel_import_map_destination_code : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Rule_Port", "destination_code", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vessel_Rule_Port", "destination_code");

            ExecuteSqlFileDown();
        }
    }
}
