using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2705_add_port_entities : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Vessel_Export_Rule_LoadPort", new[] { "load_port" });
            DropTable("dbo.Vessel_Export_Rule_LoadPort");

            CreateTable(
                "dbo.CW_Domestic_Ports",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    port_code = c.String(nullable: false, maxLength: 10, unicode: false),
                    unloco = c.String(nullable: false, maxLength: 5, unicode: false),
                    country = c.String(maxLength: 2, unicode: false),
                    state = c.String(maxLength: 3, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.port_code, unique: true);

            CreateTable(
                "dbo.CW_Foreign_Ports",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    port_code = c.String(nullable: false, maxLength: 10, unicode: false),
                    unloco = c.String(nullable: false, maxLength: 5, unicode: false),
                    country = c.String(maxLength: 2, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.port_code, unique: true);

            ExecuteSqlFileUp();
            ExecuteSqlFile("201909121013145_cbdev_2704_add_port_tables_down.sql");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Vessel_Export_Rule_LoadPort",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    load_port = c.String(nullable: false, maxLength: 128, unicode: false),
                    unloco_code = c.String(maxLength: 10, unicode: false),
                    state_of_origin = c.String(maxLength: 10, unicode: false),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);
            CreateIndex("dbo.Vessel_Export_Rule_LoadPort", "load_port", unique: true);

            DropIndex("dbo.CW_Foreign_Ports", new[] { "port_code" });
            DropIndex("dbo.CW_Domestic_Ports", new[] { "port_code" });
            DropTable("dbo.CW_Foreign_Ports");
            DropTable("dbo.CW_Domestic_Ports");

            ExecuteSqlFile("201909121013145_cbdev_2704_add_port_tables.sql");
            ExecuteSqlFileDown();
        }
    }
}
