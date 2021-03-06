// Generated Time: 02/07/2020 13:21:26
// Generated By: IAPetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2978_audit_add_supplier_mid_validation : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.imp_rail_audit_daily_rules", "supplier_mid", c => c.String(maxLength: 16, unicode: false));
            RenameColumn("dbo.imp_rail_audit_daily_rules", "mid", "manufacturer_mid");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            RenameColumn("dbo.imp_rail_audit_daily_rules", "manufacturer_mid", "mid");
            DropColumn("dbo.imp_rail_audit_daily_rules", "supplier_mid");

            ExecuteSqlFileDown();
        }
    }
}
