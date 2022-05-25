// Generated Time: 04/06/2021 13:36:56
// Generated By: kkrastev

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoice_line_number_not_editable : FpMigration
    {
        public override void Up()
        {
            Sql("UPDATE [zones_ftz214].[form_configuration] " +
                "SET editable = 0 " +
                "WHERE column_name = 'invoice_line_number'");
        }
        
        public override void Down()
        {
        }
    }
}