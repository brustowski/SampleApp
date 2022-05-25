// Generated Time: 01/21/2020 17:27:15
// Generated By: AIKravchenko

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_2935_update_column_length : FpMigration
    {
        public override void Up()
        {
            AlterColumn("canada_imp_truck.rule_carrier", "carrier", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "transport", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "service_option", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "gross_weight_unit", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "packages_unit", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "port_of_clearance", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "sub_location", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "first_port_of_arrival", c => c.String(maxLength: 5, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "final_destination", c => c.String(maxLength: 5, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("canada_imp_truck.rule_port", "final_destination", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "first_port_of_arrival", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "sub_location", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_port", "port_of_clearance", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "packages_unit", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "gross_weight_unit", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "service_option", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_importer", "transport", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("canada_imp_truck.rule_carrier", "carrier", c => c.String(nullable: false, maxLength: 128, unicode: false));
        }
    }
}
