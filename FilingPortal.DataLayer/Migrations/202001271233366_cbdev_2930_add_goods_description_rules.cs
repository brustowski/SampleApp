// Generated Time: 01/27/2020 15:33:36
// Generated By: IAPetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2930_add_goods_description_rules : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.imp_rail_audit_daily_goods_description_rules",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    goods_description = c.String(nullable: false, maxLength: 525, unicode: false),
                    api_from = c.Decimal(precision: 18, scale: 6),
                    api_to = c.Decimal(precision: 18, scale: 6),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => new { t.goods_description, t.api_from, t.api_to }, unique: true);

        }

        public override void Down()
        {
            DropIndex("dbo.imp_rail_audit_daily_goods_description_rules", new[] { "goods_description", "api_from", "api_to" });
            DropTable("dbo.imp_rail_audit_daily_goods_description_rules");
        }
    }
}
