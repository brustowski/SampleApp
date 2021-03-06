// Generated Time: 09/03/2020 19:49:39
// Generated By: iapetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3321_fix_chgs_type : FpMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.imp_rail_audit_daily", "chgs", c => c.Decimal(precision: 18, scale: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.imp_rail_audit_daily", "chgs", c => c.Decimal(nullable: false, precision: 18, scale: 6));
        }
    }
}
