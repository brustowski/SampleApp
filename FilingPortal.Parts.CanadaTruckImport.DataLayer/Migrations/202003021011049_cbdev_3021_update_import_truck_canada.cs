// Generated Time: 03/02/2020 13:11:04
// Generated By: aikravchenko

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3021_update_import_truck_canada : FpMigration
    {
        public override void Up()
        {
            AddColumn("canada_imp_truck.inbound", "line_price", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("canada_imp_truck.rule_importer", "export_state", c => c.String(maxLength: 2, unicode: false));
            DropColumn("canada_imp_truck.rule_carrier", "customs_qty");
            DropColumn("canada_imp_truck.rule_importer", "customs_uq");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("canada_imp_truck.rule_importer", "customs_uq", c => c.String(maxLength: 3, unicode: false));
            AddColumn("canada_imp_truck.rule_carrier", "customs_qty", c => c.Decimal(precision: 18, scale: 6));
            DropColumn("canada_imp_truck.rule_importer", "export_state");
            DropColumn("canada_imp_truck.inbound", "line_price");

            ExecuteSqlFileDown();
        }
    }
}
