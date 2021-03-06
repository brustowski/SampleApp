// Generated Time: 04/13/2020 10:00:11
// Generated By: iapetrov

namespace FilingPortal.Parts.Isf.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3107_set_bill_number_length_16 : FpMigration
    {
        public override void Up()
        {
            AlterColumn("isf.inbound_bills", "bill_number", c => c.String(nullable: false, maxLength: 16, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("isf.inbound_bills", "bill_number", c => c.String(nullable: false, maxLength: 14, unicode: false));
        }
    }
}
