// Generated Time: 12/23/2020 16:02:21
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3602_change_xml_storage_place : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound", "xml_file", c => c.Binary());
            AddColumn("zones_entry.inbound", "xml_file_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "xml_type", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("zones_entry.inbound", "xml_type");
            DropColumn("zones_entry.inbound", "xml_file_name");
            DropColumn("zones_entry.inbound", "xml_file");

            ExecuteSqlFileDown();
        }
    }
}