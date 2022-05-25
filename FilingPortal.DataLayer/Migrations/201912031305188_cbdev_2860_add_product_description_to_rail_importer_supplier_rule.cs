// Generated Time: 12/03/2019 16:05:18
// Generated By: IAPetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2860_add_product_description_to_rail_importer_supplier_rule : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.imp_rail_rule_importer_supplier", "product_description", c => c.String(maxLength: 500));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();
            DropColumn("dbo.imp_rail_rule_importer_supplier", "product_description");

            
        }
    }
}