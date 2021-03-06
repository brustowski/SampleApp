// Generated Time: 05/25/2020 22:18:42
// Generated By: iapetrov

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3175_remove_value_field_from_form_configs : FpMigration
    {
        public override void Up()
        {
            DropColumn("us_exp_rail.form_configuration", "value");
        }
        
        public override void Down()
        {
            AddColumn("us_exp_rail.form_configuration", "value", c => c.String(maxLength: 512, unicode: false));
        }
    }
}
