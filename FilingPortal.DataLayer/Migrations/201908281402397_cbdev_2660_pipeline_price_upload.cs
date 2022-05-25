namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2660_pipeline_price_upload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pipeline_Rule_Price",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        importer_id = c.Guid(nullable: false),
                        crude_type_id = c.Int(nullable: false),
                        pricing = c.Decimal(nullable: false, precision: 18, scale: 6),
                        freight = c.Decimal(nullable: false, precision: 18, scale: 6),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Pipeline_Rule_BatchCode", t => t.crude_type_id)
                .ForeignKey("dbo.Clients", t => t.importer_id)
                .Index(t => t.importer_id)
                .Index(t => t.crude_type_id);
            
            AlterColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Vessel_Export_Rule_USPPI", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rail_Rule_ImporterSupplier", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Rail_Rule_Port", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Truck_Export_Rule_Consignee", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Truck_Export_Rule_Exporter_Consignee", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Truck_Rule_Importers", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Truck_Rule_Ports", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Truck_Export_Rule_Port", "created_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Vessel_Export_Rule_LoadPort", "created_date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pipeline_Rule_Price", "importer_id", "dbo.Clients");
            DropForeignKey("dbo.Pipeline_Rule_Price", "crude_type_id", "dbo.Pipeline_Rule_BatchCode");
            DropIndex("dbo.Pipeline_Rule_Price", new[] { "crude_type_id" });
            DropIndex("dbo.Pipeline_Rule_Price", new[] { "importer_id" });
            AlterColumn("dbo.Vessel_Export_Rule_LoadPort", "created_date", c => c.DateTime());
            AlterColumn("dbo.Truck_Export_Rule_Port", "created_date", c => c.DateTime());
            AlterColumn("dbo.Truck_Rule_Ports", "created_date", c => c.DateTime());
            AlterColumn("dbo.Truck_Rule_Importers", "created_date", c => c.DateTime());
            AlterColumn("dbo.Truck_Export_Rule_Exporter_Consignee", "created_date", c => c.DateTime());
            AlterColumn("dbo.Truck_Export_Rule_Consignee", "created_date", c => c.DateTime());
            AlterColumn("dbo.Rail_Rule_Port", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Rail_Rule_ImporterSupplier", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "CreatedDate", c => c.DateTime());
            AlterColumn("dbo.Vessel_Export_Rule_USPPI", "created_date", c => c.DateTime());
            AlterColumn("dbo.Vessel_Export_Rule_USPPI_Consignee", "created_date", c => c.DateTime());
            DropTable("dbo.Pipeline_Rule_Price");
        }
    }
}
