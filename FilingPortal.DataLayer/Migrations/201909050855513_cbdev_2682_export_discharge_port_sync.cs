using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2682_export_discharge_port_sync : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Discharge_Ports", "Idx_Port");
            DropTable("dbo.Discharge_Ports");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Discharge_Ports",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    port = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);

            CreateIndex("dbo.Discharge_Ports", "port", unique: true, name: "Idx_Port");
        }
    }
}
