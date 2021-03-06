// Generated Time: 09/22/2020 06:08:46
// Generated By: iapetrov

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3318_add_grid_view : FpMigration
    {
        public override void Up()
        {
            AddColumn("us_exp_rail.form_section_configuration", "display_as_grid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("us_exp_rail.form_section_configuration", "display_as_grid");
        }
    }
}
