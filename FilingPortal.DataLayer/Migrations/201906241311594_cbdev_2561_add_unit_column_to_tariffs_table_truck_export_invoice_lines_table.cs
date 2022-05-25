using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2561_add_unit_column_to_tariffs_table_truck_export_invoice_lines_table : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariff", "Tariff_Type", c => c.String(maxLength: 128));
            AddColumn("dbo.Tariff", "Unit", c => c.String(maxLength: 128));
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariff", "Unit");
            DropColumn("dbo.Tariff", "Tariff_Type");
            ExecuteSqlFileDown();
        }
    }
}
