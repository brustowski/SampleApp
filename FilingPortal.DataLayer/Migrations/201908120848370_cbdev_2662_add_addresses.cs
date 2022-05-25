using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2662_add_addresses : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client_Addresses",
                c => new
                {
                    id = c.Guid(nullable: false, identity: true),
                    client_id = c.Guid(),
                    code = c.String(nullable: false, maxLength: 128, unicode: false),
                    last_updated_time = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.client_id, name: "Idx_ClientCode")
                .Index(t => t.code, name: "Idx_Code");

            AddColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address_id", c => c.Guid());
            AddColumn("dbo.Vessel_Export_Rule_USPPI", "address_id", c => c.Guid());
            CreateIndex("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address_id");
            CreateIndex("dbo.Vessel_Export_Rule_USPPI", "address_id");
            AddForeignKey("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("dbo.Vessel_Export_Rule_USPPI", "address_id", "dbo.Client_Addresses", "id");
            DropColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address");
            DropColumn("dbo.Vessel_Export_Rule_USPPI", "address");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Export_Rule_USPPI", "address", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address", c => c.String(maxLength: 128, unicode: false));
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI", "address_id", "dbo.Client_Addresses");
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address_id", "dbo.Client_Addresses");
            DropIndex("dbo.Vessel_Export_Rule_USPPI", new[] { "address_id" });
            DropIndex("dbo.Client_Addresses", "Idx_Code");
            DropIndex("dbo.Client_Addresses", "Idx_ClientCode");
            DropIndex("dbo.Vessel_Export_Rule_USPPI_Consignee", new[] { "consignee_address_id" });
            DropColumn("dbo.Vessel_Export_Rule_USPPI", "address_id");
            DropColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_address_id");
            DropTable("dbo.Client_Addresses");

            ExecuteSqlFileDown();
        }
    }
}
