using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2430_vessel_import_filing : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFile("201907221244155_cbdev_2602_add_vessel_import_resulting_tables.sql");

            ExecuteSqlFile("201907231400593_cbdev_2603_fill_vessel_import_configuration.sql");

            ExecuteSqlFile("201907240912226_cbdev_2604_add_vessel_import_filing_procedures.sql");

            ExecuteSqlFile("201907251614244_cbdev_2606_fix_vessel_imports_grid_view.sql");
        }
        
        public override void Down()
        {
            ExecuteSqlFile("201907251614244_cbdev_2606_fix_vessel_imports_grid_view_down.sql");

            ExecuteSqlFile("201907240912226_cbdev_2604_add_vessel_import_filing_procedures_down.sql");

            ExecuteSqlFile("201907231400593_cbdev_2603_fill_vessel_import_configuration_down.sql");

            ExecuteSqlFile("201907221244155_cbdev_2602_add_vessel_import_resulting_tables_down.sql");
        }
    }
}
