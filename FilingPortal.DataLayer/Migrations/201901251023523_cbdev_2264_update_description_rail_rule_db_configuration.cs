namespace FilingPortal.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2264_update_description_rail_rule_db_configuration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Rail_Rule_Desc1_Desc2", "Idx_Description1_Description2");

            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Description1", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Template_HTS_Quantity", c => c.Decimal(precision: 18, scale: 6));
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Template_Invoice_Quantity", c => c.Decimal(precision: 18, scale: 6));

            CreateIndex("dbo.Rail_Rule_Desc1_Desc2", new[] { "Description1", "Description2" }, unique: true, name: "Idx_Description1_Description2");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rail_Rule_Desc1_Desc2", "Idx_Description1_Description2");

            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Template_Invoice_Quantity", c => c.Decimal(precision: 18, scale: 6, storeType: "numeric"));
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Template_HTS_Quantity", c => c.Decimal(precision: 18, scale: 6, storeType: "numeric"));
            AlterColumn("dbo.Rail_Rule_Desc1_Desc2", "Description1", c => c.String(maxLength: 128, unicode: false));

            CreateIndex("dbo.Rail_Rule_Desc1_Desc2", new[] { "Description1", "Description2" }, unique: true, name: "Idx_Description1_Description2");
        }
    }
}
