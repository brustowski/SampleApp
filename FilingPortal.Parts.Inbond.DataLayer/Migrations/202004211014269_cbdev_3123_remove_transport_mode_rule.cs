// Generated Time: 04/21/2020 13:14:26
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3123_remove_transport_mode_rule : FpMigration
    {
        public override void Up()
        {
            // Transport mode rule was removed from the application without removing DB artifacts. Customer requirement.

            AddColumn("inbond.rule_entry", "transport_mode", c => c.String(maxLength: 2, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("inbond.rule_entry", "transport_mode");

            ExecuteSqlFileDown();
        }
    }
}
