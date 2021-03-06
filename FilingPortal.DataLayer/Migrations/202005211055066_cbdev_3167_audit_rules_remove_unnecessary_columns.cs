// Generated Time: 05/21/2020 13:55:06
// Generated By: iapetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3167_audit_rules_remove_unnecessary_columns : FpMigration
    {
        public override void Up()
        {
            DropColumn("dbo.imp_rail_audit_daily_spi_rules", "last_modified_by");
            DropColumn("dbo.imp_rail_audit_daily_spi_rules", "last_modified_date");
            DropColumn("dbo.imp_rail_audit_daily_rules", "last_modified_by");
            DropColumn("dbo.imp_rail_audit_daily_rules", "last_modified_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.imp_rail_audit_daily_rules", "last_modified_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.imp_rail_audit_daily_rules", "last_modified_by", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.imp_rail_audit_daily_spi_rules", "last_modified_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.imp_rail_audit_daily_spi_rules", "last_modified_by", c => c.String(maxLength: 50, unicode: false));
        }
    }
}
