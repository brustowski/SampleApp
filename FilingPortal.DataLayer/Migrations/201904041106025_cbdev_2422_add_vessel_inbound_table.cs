using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2422_add_vessel_inbound_table : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vessel_Import_Filing_Headers",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    error_description = c.String(maxLength: 255, unicode: false),
                    filing_number = c.String(maxLength: 255, unicode: false),
                    mapping_status = c.Byte(),
                    filing_status = c.Byte(),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.Vessel_Imports",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    created_user = c.String(maxLength: 128, unicode: false, defaultValueSql: "SUSER_NAME()"),
                    deleted = c.Boolean(nullable: false, defaultValue: false),
                    importer_code = c.String(nullable: false, maxLength: 128, unicode: false),
                    vessel = c.String(nullable: false, maxLength: 128, unicode: false),
                    state = c.String(nullable: false, maxLength: 128, unicode: false),
                    discharge_terminal = c.String(nullable: false, maxLength: 1, unicode: false),
                    classification = c.String(nullable: false, maxLength: 128, unicode: false),
                    eta = c.DateTime(nullable: false),
                    filer_id = c.String(nullable: false, maxLength: 255),
                    container = c.String(nullable: false, maxLength: 128, unicode: false, defaultValue: "BLK"),
                    entry_type = c.String(nullable: false, maxLength: 128, unicode: false, defaultValue: "01"),
                    master_bill = c.String(nullable: false, maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.Vessel_Import_Filing_Details",
                c => new
                {
                    Filing_Headers_FK = c.Int(nullable: false),
                    VI_FK = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Filing_Headers_FK, t.VI_FK })
                .ForeignKey("dbo.Vessel_Import_Filing_Headers", t => t.Filing_Headers_FK, cascadeDelete: true)
                .ForeignKey("dbo.Vessel_Imports", t => t.VI_FK, cascadeDelete: true)
                .Index(t => t.Filing_Headers_FK)
                .Index(t => t.VI_FK);

            ExecuteSqlFile("201904041106025_cbdev_2422_add_vessel_inbound_table.sql");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Vessel_Import_Filing_Details", "VI_FK", "dbo.Vessel_Imports");
            DropForeignKey("dbo.Vessel_Import_Filing_Details", "Filing_Headers_FK", "dbo.Vessel_Import_Filing_Headers");
            DropIndex("dbo.Vessel_Import_Filing_Details", new[] { "VI_FK" });
            DropIndex("dbo.Vessel_Import_Filing_Details", new[] { "Filing_Headers_FK" });
            DropTable("dbo.Vessel_Import_Filing_Details");
            DropTable("dbo.Vessel_Imports");
            DropTable("dbo.Vessel_Import_Filing_Headers");

            ExecuteSqlFile("201904041106025_cbdev_2422_add_vessel_inbound_table_down.sql");
        }
    }
}
