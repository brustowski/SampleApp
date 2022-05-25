using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_1003_descrition1removedescription2 : FpMigration
    {
        public override void Up()
        {
           
            DropIndex("dbo.Rail_Rule_Desc1_Desc2", "Idx_Description1_Description2");
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Description1", c => c.String(nullable: false, maxLength: 500, unicode: false));
            ExecuteSqlFile("201903220307597_cbdev_1003_update_description1_Rail_Rule_Desc1_Desc2.sql");
            DropColumn("dbo.Rail_Rule_Desc1_Desc2", "Description2");
            CreateIndex("dbo.Rail_Rule_Desc1_Desc2", "Description1", unique: true, name: "Idx_Description1");

            DropIndex("dbo.Rail_BD_Parsed", "Idx_Description1_Description2");
            AlterColumn("dbo.Rail_BD_Parsed", "Description1", c => c.String(maxLength: 500));
            DropColumn("dbo.Rail_BD_Parsed", "Description2");
            CreateIndex("dbo.Rail_BD_Parsed", "Description1", unique: false, name: "Idx_Description1");

            AlterColumn("dbo.Rail_DeclarationTab", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.Rail_DEFValues", "DefValue", c => c.String(maxLength: 500, unicode: false));
            AlterColumn("dbo.Rail_DEFValues_Manual", "DefValue", c => c.String(maxLength: 500, unicode: false));

            ExecuteSqlFile("201903220307597_cbdev_1003_descrition1removedescription2.sql");
        }

        public override void Down()
        {
            DropIndex("dbo.Rail_Rule_Desc1_Desc2", "Idx_Description1");
            AddColumn("dbo.Rail_Rule_Desc1_Desc2", "Description2", c => c.String(maxLength: 128, unicode: false));

            DropIndex("dbo.Rail_BD_Parsed", "Idx_Description1");
            AddColumn("dbo.Rail_BD_Parsed", "Description2", c => c.String(maxLength: 200));
            AlterColumn("dbo.Rail_DEFValues", "DefValue", c => c.String(maxLength: 128, unicode: false));
            ExecuteSqlFile("201903220307597_cbdev_1003_descrition1removedescription2_down.sql");
        }
    }
}
