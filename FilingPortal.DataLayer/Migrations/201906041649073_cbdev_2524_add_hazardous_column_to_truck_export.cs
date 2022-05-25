using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2524_add_hazardous_column_to_truck_export : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.truck_exports", "hazardous", c => c.String(nullable: false, maxLength: 3, unicode: false));
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
            DropColumn("dbo.truck_exports", "hazardous");
        }
    }
    
}
