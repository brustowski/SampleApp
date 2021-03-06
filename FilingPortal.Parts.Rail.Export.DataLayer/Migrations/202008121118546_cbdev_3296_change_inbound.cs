// Generated Time: 08/12/2020 14:18:54
// Generated By: iapetrov

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3296_change_inbound : FpMigration
    {
        public override void Up()
        {
            AddColumn("us_exp_rail.inbound", "load_date", c => c.DateTime(nullable: false));
            AddColumn("us_exp_rail.inbound", "export_date", c => c.DateTime(nullable: false));
            AddColumn("us_exp_rail.inbound", "terminal_address", c => c.String(maxLength: 128, unicode: false));
            DropColumn("us_exp_rail.rule_exporter_consignee", "address");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("us_exp_rail.rule_exporter_consignee", "address", c => c.String(maxLength: 128, unicode: false));
            DropColumn("us_exp_rail.inbound", "terminal_address");
            DropColumn("us_exp_rail.inbound", "export_date");
            DropColumn("us_exp_rail.inbound", "load_date");

            ExecuteSqlFileDown();
        }
    }
}
