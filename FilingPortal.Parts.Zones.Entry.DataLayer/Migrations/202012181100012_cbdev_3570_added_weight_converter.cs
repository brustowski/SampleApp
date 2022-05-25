// Generated Time: 12/18/2020 14:00:01
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3570_added_weight_converter : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound_lines", "gross_weight", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("zones_entry.inbound_lines", "gross_weight_unit", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound_parsed_data", "gross_wgt_unit", c => c.String(maxLength: 128, unicode: false));
        }

        public override void Down()
        {
            DropColumn("zones_entry.inbound_parsed_data", "gross_wgt_unit");
            DropColumn("zones_entry.inbound_lines", "gross_weight_unit");
            DropColumn("zones_entry.inbound_lines", "gross_weight");
        }
    }
}