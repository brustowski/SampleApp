// Generated Time: 05/08/2020 16:37:32
// Generated By: iapetrov

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3127_add_confirmation_needed_field : FpMigration
    {
        public override void Up()
        {
            AddColumn("inbond.filing_header", "confirmation_needed", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("inbond.rule_entry", "confirmation_needed", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("inbond.form_configuration", "confirmation_needed", c => c.Boolean(nullable: false, defaultValue: false));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("inbond.form_configuration", "confirmation_needed");
            DropColumn("inbond.rule_entry", "confirmation_needed");
            DropColumn("inbond.filing_header", "confirmation_needed");

            ExecuteSqlFileDown();
        }
    }
}
