// Generated Time: 12/18/2019 11:49:45
// Generated By: AIKravchenko

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2898_add_rail_inbound_created_date_default_constraint : FpMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.imp_rail_inbound", "created_date",x=>x.DateTime(nullable:false,defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.imp_rail_inbound", "created_date", x => x.DateTime(nullable: false));
        }
    }
}