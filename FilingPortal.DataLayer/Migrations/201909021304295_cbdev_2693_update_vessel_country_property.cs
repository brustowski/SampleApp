using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2693_update_vessel_country_property : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Exports", "country_of_destination_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Vessel_Exports", "country_of_destination_id");
            AddForeignKey("dbo.Vessel_Exports", "country_of_destination_id", "dbo.Countries", "id");
            DropColumn("dbo.Vessel_Exports", "country_of_destination");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Exports", "country_of_destination", c => c.String(nullable: false, maxLength: 128, unicode: false));
            DropForeignKey("dbo.Vessel_Exports", "country_of_destination_id", "dbo.Countries");
            DropIndex("dbo.Vessel_Exports", new[] { "country_of_destination_id" });
            DropColumn("dbo.Vessel_Exports", "country_of_destination_id");

            ExecuteSqlFileDown();
        }
    }
}
