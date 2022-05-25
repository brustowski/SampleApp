// Generated Time: 12/11/2020 16:16:54
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Recon.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3553_add_status_entities : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "recon.fta_recon_status",
                c => new
                {
                    id = c.Int(nullable: false),
                    name = c.String(nullable: false, maxLength: 20, unicode: false),
                    code = c.String(maxLength: 3, unicode: false),
                    description = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "recon.value_recon_status",
                c => new
                {
                    id = c.Int(nullable: false),
                    name = c.String(nullable: false, maxLength: 20, unicode: false),
                    code = c.String(maxLength: 3, unicode: false),
                    description = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropTable("recon.value_recon_status");
            DropTable("recon.fta_recon_status");
        }
    }
}