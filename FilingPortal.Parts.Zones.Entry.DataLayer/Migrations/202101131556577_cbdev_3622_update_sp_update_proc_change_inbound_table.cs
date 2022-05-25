// Generated Time: 01/13/2021 10:56:57
// Generated By: ssugathan

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3622_update_sp_update_proc_change_inbound_table : FpMigration
    {
        public override void Up()
        {
            AlterColumn("zones_entry.inbound", "entry_port", c => c.String(maxLength: 5, unicode: false));
            AlterColumn("zones_entry.inbound", "entry_no", c => c.String(maxLength: 11, unicode: false));
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AlterColumn("zones_entry.inbound", "entry_no", c => c.String(maxLength: 8, unicode: false));
            AlterColumn("zones_entry.inbound", "entry_port", c => c.String(maxLength: 4, unicode: false));
            ExecuteSqlFileDown();
        }
    }
}