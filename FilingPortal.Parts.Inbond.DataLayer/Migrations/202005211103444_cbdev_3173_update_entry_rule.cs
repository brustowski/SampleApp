// Generated Time: 05/21/2020 14:03:44
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3173_update_entry_rule : FpMigration
    {
        public override void Up()
        {
            AddColumn("inbond.rule_entry", "importer_address_id", c => c.Guid());
            CreateIndex("inbond.rule_entry", "importer_address_id");
            AddForeignKey("inbond.rule_entry", "importer_address_id", "dbo.Client_Addresses", "id");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropForeignKey("inbond.rule_entry", "importer_address_id", "dbo.Client_Addresses");
            DropIndex("inbond.rule_entry", new[] { "importer_address_id" });
            DropColumn("inbond.rule_entry", "importer_address_id");

            ExecuteSqlFileDown();
        }
    }
}
