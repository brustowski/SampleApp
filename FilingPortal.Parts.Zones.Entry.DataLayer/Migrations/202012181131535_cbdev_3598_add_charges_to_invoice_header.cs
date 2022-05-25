// Generated Time: 12/18/2020 14:31:53
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3598_add_charges_to_invoice_header : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound_parsed_data", "charges", c => c.Decimal(precision: 18, scale: 6));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("zones_entry.inbound_parsed_data", "charges");

            ExecuteSqlFileDown();
        }
    }
}
