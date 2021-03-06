// Generated Time: 02/08/2021 23:15:45
// Generated By: ssugathan

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class csfp_24_add_invoice_line_desclaimer_epa_pst : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound_lines", "disclaimer", c => c.String(maxLength: 5, unicode: false));
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropColumn("zones_entry.inbound_lines", "disclaimer");
            ExecuteSqlFileDown();
        }
    }
}
