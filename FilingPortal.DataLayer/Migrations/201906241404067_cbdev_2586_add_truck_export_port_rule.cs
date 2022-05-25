using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2586_add_truck_export_port_rule : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Truck_Export_Rule_Port",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        port = c.String(nullable: false, maxLength: 128, unicode: false),
                        origin_code = c.String(maxLength: 10, unicode: false),
                        state_of_origin = c.String(maxLength: 10, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.port, unique: true);
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropIndex("dbo.Truck_Export_Rule_Port", new[] { "port" });
            DropTable("dbo.Truck_Export_Rule_Port");
            ExecuteSqlFileDown();
        }
    }
}
