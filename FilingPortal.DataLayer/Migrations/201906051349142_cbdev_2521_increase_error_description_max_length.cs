namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2521_increase_error_description_max_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pipeline_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Rail_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Truck_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.truck_export_filing_headers", "error_description", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Vessel_Import_Filing_Headers", "error_description", c => c.String(maxLength: null, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vessel_Import_Filing_Headers", "error_description", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.truck_export_filing_headers", "error_description", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Truck_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Rail_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
            AlterColumn("dbo.Pipeline_Filing_Headers", "ErrorDescription", c => c.String(maxLength: null, unicode: false));
        }
    }
}
