using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2233_truck_results_tables_structure : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Truck_DEFValues", "TableName", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues", "SingleFilingOrder", c => c.Byte());
            AddColumn("dbo.Truck_DEFValues_Manual", "TableName", c => c.String(maxLength: 128, unicode: false));
            
            ExecuteSqlFile("201812271134010_cbdev_2241_update_truck_defValues.sql");
            ExecuteSqlFile("201812271238285_cbdev_2234_add_truck_result_tables.sql");
            ExecuteSqlFile("201812271314119_cbdev_2235_add_v_truck_tables.sql");
            ExecuteSqlFile("201812271445372_cbdev_2238_update_truck_report.sql");
            ExecuteSqlFile("201812281310198_cbdev_2239_truck_filing_procedures_update.sql");
            ExecuteSqlFile("201812281509538_cbdev_2233_update_truck_defval_data.sql");

            DropTable(@"dbo.Truck_FilingData");
        }
        
        public override void Down()
        {
            ExecuteSqlFile("201812281310198_cbdev_2239_truck_filing_procedures_update_down.sql");
            ExecuteSqlFile("201812271445372_cbdev_2238_update_truck_report_down.sql");
            ExecuteSqlFile("201812271314119_cbdev_2235_add_v_truck_tables_down.sql");
            ExecuteSqlFile("201812271238285_cbdev_2234_add_truck_result_tables_down.sql");
            ExecuteSqlFile("201812271134010_cbdev_2241_update_truck_defValues_down.sql");

            DropColumn("dbo.Truck_DEFValues_Manual", "TableName");
            DropColumn("dbo.Truck_DEFValues", "SingleFilingOrder");
            DropColumn("dbo.Truck_DEFValues", "TableName");
        }
    }
}
