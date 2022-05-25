// Generated Time: 11/12/2019 11:26:29
// Generated By: aikravchenko

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2798_remove_vessel_export_usppi_rule : FpMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI", "address_id", "dbo.Client_Addresses");
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI", "usppi_id", "dbo.Clients");
            DropIndex("dbo.Vessel_Export_Rule_USPPI", new[] { "usppi_id" });
            DropIndex("dbo.Vessel_Export_Rule_USPPI", new[] { "address_id" });
            DropTable("dbo.Vessel_Export_Rule_USPPI");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Vessel_Export_Rule_USPPI",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usppi_id = c.Guid(nullable: false),
                        address_id = c.Guid(),
                        contact = c.String(maxLength: 128, unicode: false),
                        phone = c.String(maxLength: 128, unicode: false),
                        last_ref_number = c.Int(nullable: false),
                        last_ref_year = c.String(maxLength: 2, unicode: false),
                        last_ref = c.String(maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateIndex("dbo.Vessel_Export_Rule_USPPI", "address_id");
            CreateIndex("dbo.Vessel_Export_Rule_USPPI", "usppi_id", unique: true);
            AddForeignKey("dbo.Vessel_Export_Rule_USPPI", "usppi_id", "dbo.Clients", "id");
            AddForeignKey("dbo.Vessel_Export_Rule_USPPI", "address_id", "dbo.Client_Addresses", "id");

            ExecuteSqlFileDown();
        }
    }
}