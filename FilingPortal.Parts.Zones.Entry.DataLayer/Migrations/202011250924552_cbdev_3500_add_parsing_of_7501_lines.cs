// Generated Time: 11/25/2020 12:26:17
// Generated By: iapetrov

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3500_add_parsing_of_7501_lines : FpMigration
    {
        public override void Up()
        {
            AddColumn("zones_entry.inbound_lines", "invoice_qty_uq", c => c.String(maxLength: 3, unicode: false));
            AddColumn("zones_entry.inbound_lines", "transaction_related", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("zones_entry.inbound_lines", "transaction_related");
            DropColumn("zones_entry.inbound_lines", "invoice_qty_uq");
        }
    }
}
