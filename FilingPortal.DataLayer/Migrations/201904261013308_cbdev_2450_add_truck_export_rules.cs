namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2450_add_truck_export_rules : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Truck_Export_Rule_Consignee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        consignee_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        destination = c.String(maxLength: 5, unicode: false),
                        country = c.String(maxLength: 2, unicode: false),
                        ultimate_consignee_type = c.String(maxLength: 1, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.consignee_code, unique: true);
            
            CreateTable(
                "dbo.Truck_Export_Rule_Exporter_Consignee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        exporter = c.String(nullable: false, maxLength: 128, unicode: false),
                        consignee_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        address = c.String(maxLength: 128, unicode: false),
                        contact = c.String(maxLength: 128, unicode: false),
                        phone = c.String(maxLength: 128, unicode: false),
                        tran_related = c.String(maxLength: 1, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => new { t.exporter, t.consignee_code }, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Truck_Export_Rule_Exporter_Consignee", new[] { "exporter", "consignee_code" });
            DropIndex("dbo.Truck_Export_Rule_Consignee", new[] { "consignee_code" });
            DropTable("dbo.Truck_Export_Rule_Exporter_Consignee");
            DropTable("dbo.Truck_Export_Rule_Consignee");
        }
    }
}
