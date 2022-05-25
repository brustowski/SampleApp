// Generated Time: 05/07/2020 12:54:42
// Generated By: iapetrov

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3146_make_port_carrier_mandatory : FpMigration
    {
        public override void Up()
        {
            AlterColumn("inbond.inbound", "port_of_destination", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("inbond.inbound", "carrier", c => c.String(nullable: false, maxLength: 128, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("inbond.inbound", "carrier", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("inbond.inbound", "port_of_destination", c => c.String(maxLength: 4, unicode: false));
        }
    }
}