using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2587_update_vessel_entity : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Imports", "importer_id", c => c.Guid(nullable: false));
            AddColumn("dbo.Vessel_Imports", "state_id", c => c.Int(nullable: false));
            AddColumn("dbo.Vessel_Imports", "classification_id", c => c.Int(nullable: false));
            AddColumn("dbo.Vessel_Imports", "user_id", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Vessel_Imports", "importer_id");
            CreateIndex("dbo.Vessel_Imports", "state_id");
            CreateIndex("dbo.Vessel_Imports", "classification_id");
            CreateIndex("dbo.Vessel_Imports", "user_id");
            AddForeignKey("dbo.Vessel_Imports", "classification_id", "dbo.Tariff", "id");
            AddForeignKey("dbo.Vessel_Imports", "user_id", "dbo.app_users", "UserAccount");
            AddForeignKey("dbo.Vessel_Imports", "importer_id", "dbo.Clients", "id");
            AddForeignKey("dbo.Vessel_Imports", "state_id", "dbo.US_States", "id");
            DropColumn("dbo.Vessel_Imports", "importer_code");
            DropColumn("dbo.Vessel_Imports", "state");
            DropColumn("dbo.Vessel_Imports", "classification");
            DropColumn("dbo.Vessel_Imports", "filer_id");
            DropColumn("dbo.Vessel_Imports", "master_bill");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vessel_Imports", "master_bill", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Imports", "filer_id", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Vessel_Imports", "classification", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Imports", "state", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Imports", "importer_code", c => c.String(nullable: false, maxLength: 128, unicode: false));
            DropForeignKey("dbo.Vessel_Imports", "state_id", "dbo.US_States");
            DropForeignKey("dbo.Vessel_Imports", "importer_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Imports", "user_id", "dbo.app_users");
            DropForeignKey("dbo.Vessel_Imports", "classification_id", "dbo.Tariff");
            DropIndex("dbo.Vessel_Imports", new[] { "user_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "classification_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "state_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "importer_id" });
            DropColumn("dbo.Vessel_Imports", "user_id");
            DropColumn("dbo.Vessel_Imports", "classification_id");
            DropColumn("dbo.Vessel_Imports", "state_id");
            DropColumn("dbo.Vessel_Imports", "importer_id");

            ExecuteSqlFileDown();
        }
    }
}
