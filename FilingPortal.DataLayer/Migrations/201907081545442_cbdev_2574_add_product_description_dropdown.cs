using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2574_add_product_description_dropdown : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vessel_ProductDescriptions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128, unicode: false),
                        tariff_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Tariff", t => t.tariff_id)
                .Index(t => t.tariff_id);
            
            AddColumn("dbo.Vessel_Imports", "product_description_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Vessel_Imports", "product_description_id");
            AddForeignKey("dbo.Vessel_Imports", "product_description_id", "dbo.Vessel_ProductDescriptions", "id");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vessel_Imports", "product_description_id", "dbo.Vessel_ProductDescriptions");
            DropForeignKey("dbo.Vessel_ProductDescriptions", "tariff_id", "dbo.Tariff");
            DropIndex("dbo.Vessel_ProductDescriptions", new[] { "tariff_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "product_description_id" });
            DropColumn("dbo.Vessel_Imports", "product_description_id");
            DropTable("dbo.Vessel_ProductDescriptions");

            ExecuteSqlFileDown();
        }
    }
}
