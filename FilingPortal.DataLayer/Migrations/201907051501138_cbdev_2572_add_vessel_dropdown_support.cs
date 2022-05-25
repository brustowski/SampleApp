using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2572_add_vessel_dropdown_support : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Vessels",
                    c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);

            AddColumn("dbo.Vessel_Imports", "vessel_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Vessel_Imports", "vessel_id");
            AddForeignKey("dbo.Vessel_Imports", "vessel_id", "dbo.Vessels", "id");
            DropColumn("dbo.Vessel_Imports", "vessel");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Imports", "vessel", c => c.String(nullable: false, maxLength: 128, unicode: false));
            DropForeignKey("dbo.Vessel_Imports", "vessel_id", "dbo.Vessels");
            DropIndex("dbo.Vessel_Imports", new[] { "vessel_id" });
            DropColumn("dbo.Vessel_Imports", "vessel_id");
            DropTable("dbo.Vessels");

            ExecuteSqlFileDown();
        }
    }
}
