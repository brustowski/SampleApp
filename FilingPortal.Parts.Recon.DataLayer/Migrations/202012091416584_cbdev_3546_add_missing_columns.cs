// Generated Time: 12/09/2020 17:16:58
// Generated By: AIKravchenko


using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3546_add_missing_columns : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("recon.fta_recon", "request_xml", c => c.String(unicode: false));
            this.AddColumnIfNotExists("recon.fta_recon", "response_xml", c => c.String(unicode: false));
            this.AddColumnIfNotExists("recon.fta_recon", "error_description", c => c.String(unicode: false));
        }

        public override void Down()
        {
            DropColumn("recon.fta_recon", "request_xml");
            DropColumn("recon.fta_recon", "response_xml");
            DropColumn("recon.fta_recon", "error_description");
        }
    }
}
