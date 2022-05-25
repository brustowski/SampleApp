namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2579_update_vessel_product_rule : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vessel_Rule_Product", "product_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vessel_Rule_Product", "product_id", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
