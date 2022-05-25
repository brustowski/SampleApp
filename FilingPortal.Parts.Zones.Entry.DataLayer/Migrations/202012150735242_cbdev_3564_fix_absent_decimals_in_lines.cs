// Generated Time: 12/15/2020 10:35:24
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3564_fix_absent_decimals_in_lines : FpMigration
    {
        public override void Up()
        {
            AlterColumn("zones_entry.inbound_lines", "item_value", c => c.Decimal(precision: 18, scale: 6));
            AlterColumn("zones_entry.inbound_lines", "ftz_manifest_qty", c => c.Decimal(precision: 18, scale: 6));
        }

        public override void Down()
        {
            AlterColumn("zones_entry.inbound_lines", "ftz_manifest_qty", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AlterColumn("zones_entry.inbound_lines", "item_value", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
    }
}