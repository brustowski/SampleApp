namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_columns_to_Clients_Table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Manufacturer", c => c.Boolean(nullable: true));
            AddColumn("dbo.Clients", "Consignee", c => c.Boolean(nullable: true));
            AddColumn("dbo.Clients", "Seller", c => c.Boolean(nullable: true));
            AddColumn("dbo.Clients", "ShipToParty", c => c.Boolean(nullable: true));
            AddColumn("dbo.Clients", "SoldToParty", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "SoldToParty");
            DropColumn("dbo.Clients", "ShipToParty");
            DropColumn("dbo.Clients", "Seller");
            DropColumn("dbo.Clients", "Consignee");
            DropColumn("dbo.Clients", "Manufacturer");
        }
    }
}
