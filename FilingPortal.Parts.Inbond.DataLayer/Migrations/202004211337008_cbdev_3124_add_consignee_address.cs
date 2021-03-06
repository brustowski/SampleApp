// Generated Time: 04/21/2020 16:37:00
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3124_add_consignee_address : FpMigration
    {
        public override void Up()
        {
            AddColumn("inbond.rule_entry", "consignee_address_id", c => c.Guid());
            CreateIndex("inbond.rule_entry", "consignee_address_id");
            AddForeignKey("inbond.rule_entry", "consignee_address_id", "dbo.Client_Addresses", "id");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropForeignKey("inbond.rule_entry", "consignee_address_id", "dbo.Client_Addresses");
            DropIndex("inbond.rule_entry", new[] { "consignee_address_id" });
            DropColumn("inbond.rule_entry", "consignee_address_id");

            ExecuteSqlFileDown();
        }
    }
}
