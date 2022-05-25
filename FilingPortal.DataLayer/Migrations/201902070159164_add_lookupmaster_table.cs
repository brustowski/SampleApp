namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_lookupmaster_table : DbMigration
    {
        public override void Up()
        {
            // Add lookup Table
            CreateTable(
                "dbo.LookupMaster",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        DisplayValue = c.String(nullable: false, maxLength: 50),
                        Value = c.String(maxLength: 100),
                        Type = c.String(maxLength: 100),
                        Description = c.String(maxLength: 100),
                        LastUpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LookupMaster");
        }
    }
}
