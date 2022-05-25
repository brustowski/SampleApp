using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2573_add_vessel_discharge_terminal_dropdown : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vessel_DischargeTerminals",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128, unicode: false),
                        state_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.US_States", t => t.state_id)
                .Index(t => t.state_id);
            
            AddColumn("dbo.Vessel_Imports", "discharge_terminal_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Vessel_Imports", "discharge_terminal_id");
            AddForeignKey("dbo.Vessel_Imports", "discharge_terminal_id", "dbo.Vessel_DischargeTerminals", "id");
            DropColumn("dbo.Vessel_Imports", "discharge_terminal");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vessel_Imports", "discharge_terminal", c => c.String(nullable: false, maxLength: 1, unicode: false));
            DropForeignKey("dbo.Vessel_Imports", "discharge_terminal_id", "dbo.Vessel_DischargeTerminals");
            DropForeignKey("dbo.Vessel_DischargeTerminals", "state_id", "dbo.US_States");
            DropIndex("dbo.Vessel_DischargeTerminals", new[] { "state_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "discharge_terminal_id" });
            DropColumn("dbo.Vessel_Imports", "discharge_terminal_id");
            DropTable("dbo.Vessel_DischargeTerminals");

            ExecuteSqlFileDown();
        }
    }
}
