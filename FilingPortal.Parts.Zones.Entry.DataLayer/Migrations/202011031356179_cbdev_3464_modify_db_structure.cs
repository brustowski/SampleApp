// Generated Time: 11/06/2020 15:25:53
// Generated By: iapetrov

using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3464_modify_db_structure : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("zones_entry.filing_header", "job_status", c => c.Int());
            AddColumn("zones_entry.filing_header", "last_modified_date", c => c.DateTime());
            AddColumn("zones_entry.filing_header", "last_modified_user", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "is_update", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("zones_entry.inbound", "is_auto", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("zones_entry.inbound", "is_auto_processed", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("zones_entry.inbound", "validation_passed", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("zones_entry.inbound", "validation_result", c => c.String(maxLength: 128, unicode: false));
            AddColumn("zones_entry.inbound", "modified_date", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AddColumn("zones_entry.inbound", "modified_user", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            this.AddColumnIfNotExists("zones_entry.filing_header", "filing_status", c => c.Byte());
            this.AddColumnIfNotExists("zones_entry.filing_header", "mapping_status", c => c.Byte());
            DropColumn("zones_entry.inbound", "modified_user");
            DropColumn("zones_entry.inbound", "modified_date");
            DropColumn("zones_entry.inbound", "validation_result");
            DropColumn("zones_entry.inbound", "validation_passed");
            DropColumn("zones_entry.inbound", "is_auto_processed");
            DropColumn("zones_entry.inbound", "is_auto");
            DropColumn("zones_entry.inbound", "is_update");
            DropColumn("zones_entry.filing_header", "last_modified_user");
            DropColumn("zones_entry.filing_header", "last_modified_date");

            ExecuteSqlFileDown();
        }
    }
}
