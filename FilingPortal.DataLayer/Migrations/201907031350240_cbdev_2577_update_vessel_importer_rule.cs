namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2577_update_vessel_importer_rule : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vessel_Rule_Importer", "cw_supplier");
            DropColumn("dbo.Vessel_Rule_Importer", "relationship");
            DropColumn("dbo.Vessel_Rule_Importer", "value_recon");
            DropColumn("dbo.Vessel_Rule_Importer", "fta_recon");
            DropColumn("dbo.Vessel_Rule_Importer", "spi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vessel_Rule_Importer", "spi", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Importer", "fta_recon", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Importer", "value_recon", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Importer", "relationship", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Importer", "cw_supplier", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
