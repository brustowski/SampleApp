// Generated Time: 02/05/2020 18:19:01
// Generated By: IAPetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2955_add_facility_to_pipline_price_rule : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.imp_pipeline_rule_price", "facility_id", c => c.Int());
            CreateIndex("dbo.imp_pipeline_rule_price", "facility_id");
            AddForeignKey("dbo.imp_pipeline_rule_price", "facility_id", "dbo.imp_pipeline_rule_facility", "id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.imp_pipeline_rule_price", "facility_id", "dbo.imp_pipeline_rule_facility");
            DropIndex("dbo.imp_pipeline_rule_price", new[] { "facility_id" });
            DropColumn("dbo.imp_pipeline_rule_price", "facility_id");
        }
    }
}