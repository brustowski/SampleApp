// Generated Time: 11/19/2020 18:32:19
// Generated By: iapetrov

using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3474_ensure_conveyance : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("inbond.inbound", "export_conveyance", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
        }
    }
}
