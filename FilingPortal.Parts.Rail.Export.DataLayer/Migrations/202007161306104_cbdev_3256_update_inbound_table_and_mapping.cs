// Generated Time: 07/16/2020 16:06:10
// Generated By: aikravchenko

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3256_update_inbound_table_and_mapping : FpMigration
    {
        public override void Up()
        {
            AddColumn("us_exp_rail.inbound", "export_port", c => c.String(maxLength: 4, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("us_exp_rail.inbound", "export_port");

            ExecuteSqlFileDown();
        }
    }
}