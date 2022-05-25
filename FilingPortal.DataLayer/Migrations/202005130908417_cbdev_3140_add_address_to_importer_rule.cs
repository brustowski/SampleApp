// Generated Time: 05/13/2020 12:08:41
// Generated By: AIKravchenko

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_3140_add_address_to_importer_rule : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.imp_rail_rule_importer_supplier", "main_supplier_address", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.imp_rail_rule_importer_supplier", "manufacturer_address", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("dbo.imp_rail_rule_importer_supplier", "manufacturer_address");
            DropColumn("dbo.imp_rail_rule_importer_supplier", "main_supplier_address");

            ExecuteSqlFileDown();
        }
    }
}
