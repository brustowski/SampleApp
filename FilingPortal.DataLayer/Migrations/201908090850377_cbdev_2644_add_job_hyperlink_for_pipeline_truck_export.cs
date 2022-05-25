using FilingPortal.Parts.Common.DataLayer.Base;
using Framework.DataLayer.Migrations;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2644_add_job_hyperlink_for_pipeline_truck_export : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("dbo.Vessel_Import_Filing_Headers", "job_link", c => c.String(maxLength: 128, unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Export_Filing_Headers", "job_link", c => c.String(maxLength: 128, unicode: false));
            this.AddColumnIfNotExists("dbo.Pipeline_Filing_Headers", "job_link", c => c.String(maxLength: 128, unicode: false));
            this.AddColumnIfNotExists("dbo.Truck_Filing_Headers", "job_link", c => c.String(maxLength: 128, unicode: false));
            this.AddColumnIfNotExists("dbo.truck_export_filing_headers", "job_link", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropColumn("dbo.truck_export_filing_headers", "job_link");
            DropColumn("dbo.Truck_Filing_Headers", "job_link");
            DropColumn("dbo.Pipeline_Filing_Headers", "job_link");
            DropColumn("dbo.Vessel_Export_Filing_Headers", "job_link");
            DropColumn("dbo.Vessel_Import_Filing_Headers", "job_link");
        }
    }
}
