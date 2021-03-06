// Generated Time: 07/29/2020 17:31:26
// Generated By: iapetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3272_truck_export_remove_port_rule : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.exp_truck_rule_port", "Idx__port");
            CreateTable(
                "dbo.cw_unloco_dictionary",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        unloco = c.String(nullable: false, maxLength: 5, unicode: false),
                        port_name = c.String(maxLength: 128, unicode: false),
                        port_diacriticals = c.String(maxLength: 128, unicode: false),
                        country_code = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.unloco, unique: true);
            
            DropTable("dbo.exp_truck_rule_port");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.exp_truck_rule_port",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        port = c.String(nullable: false, maxLength: 128, unicode: false),
                        origin_code = c.String(maxLength: 10, unicode: false),
                        state_of_origin = c.String(maxLength: 10, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            DropIndex("dbo.cw_unloco_dictionary", new[] { "unloco" });
            DropTable("dbo.cw_unloco_dictionary");
            CreateIndex("dbo.exp_truck_rule_port", "port", unique: true, name: "Idx__port");

            ExecuteSqlFileDown();
        }
    }
}
