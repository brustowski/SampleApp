// Generated Time: 01/31/2020 16:25:16
// Generated By: iapetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2961_add_packs_uom_handbook : FpMigration
    {
        public override void Up()
        {
            Sql("EXECUTE app_create_handbook @name = 'cw_packtype'");
        }

        public override void Down()
        {
            Sql("DROP TABLE IF EXISTS dbo.Handbook_cw_packtype");
        }
    }
}