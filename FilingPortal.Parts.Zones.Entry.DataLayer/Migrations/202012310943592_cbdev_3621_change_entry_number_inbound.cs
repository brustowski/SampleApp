// Generated Time: 12/31/2020 12:53:44
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3621_change_entry_number_inbound : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound_parsed_data", "entry_number", c => c.String(maxLength: 7, unicode: false));
            AlterColumn("zones_entry.inbound", "entry_no", c => c.String(maxLength: 8, unicode: false));
            AlterColumn("zones_entry.inbound_parsed_data", "check_digit", c => c.String(maxLength: 1, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AlterColumn("zones_entry.inbound_parsed_data", "check_digit", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("zones_entry.inbound", "entry_no", c => c.String(maxLength: 7, unicode: false));
            DropColumn("zones_entry.inbound_parsed_data", "entry_number");

            ExecuteSqlFileDown();
        }
    }
}
