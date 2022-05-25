using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{

    public partial class cbdev_2675_discharge_terminal_to_firms_code : FpMigration
    {
        public override void Up()
        {
            Sql("TRUNCATE TABLE Vessel_Rule_Port");

            DropForeignKey("dbo.Vessel_Imports", "discharge_terminal_id", "dbo.Vessel_DischargeTerminals");
            DropIndex("dbo.Vessel_Imports", new[] { "state_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "discharge_terminal_id" });
            DropIndex("dbo.Vessel_Rule_Port", "Idx_DischargeName");
            CreateTable(
                "dbo.cw_firms_codes",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    firms_code = c.String(nullable: false, maxLength: 4, unicode: false),
                    name = c.String(nullable: false, maxLength: 128, unicode: false),
                    is_active = c.Boolean(nullable: false),
                    country_id = c.Int(),
                    state_id = c.Int(),
                    last_updated_time = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Countries", t => t.country_id)
                .ForeignKey("dbo.US_States", t => t.state_id)
                .Index(t => t.firms_code, unique: true)
                .Index(t => t.country_id)
                .Index(t => t.state_id);

            AddColumn("dbo.Vessel_Imports", "firms_code_id", c => c.Int(nullable: false));
            AddColumn("dbo.Vessel_Rule_Port", "discharge_port", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.Vessel_Rule_Port", "firms_code_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Vessel_Imports", "state_id", c => c.Int());
            AlterColumn("dbo.Vessel_Rule_Port", "discharge_name", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Vessel_Rule_Port", "hmf", c => c.String(maxLength: 1, unicode: false));
            CreateIndex("dbo.Vessel_Imports", "state_id");
            CreateIndex("dbo.Vessel_Imports", "firms_code_id");
            CreateIndex("dbo.Vessel_Rule_Port", "firms_code_id", unique: true);
            AddForeignKey("dbo.Vessel_Rule_Port", "firms_code_id", "dbo.cw_firms_codes", "id");
            AddForeignKey("dbo.Vessel_Imports", "firms_code_id", "dbo.cw_firms_codes", "id");
            DropColumn("dbo.Vessel_Imports", "discharge_terminal_id");
            DropColumn("dbo.Vessel_Rule_Port", "arrival_port");
            DropColumn("dbo.Vessel_Rule_Port", "destination_state");
            DropColumn("dbo.Vessel_Rule_Port", "firms_code");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            AddColumn("dbo.Vessel_Rule_Port", "firms_code", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Port", "destination_state", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Vessel_Rule_Port", "arrival_port", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.Vessel_Imports", "discharge_terminal_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Vessel_Imports", "firms_code_id", "dbo.cw_firms_codes");
            DropForeignKey("dbo.Vessel_Rule_Port", "firms_code_id", "dbo.cw_firms_codes");
            DropForeignKey("dbo.cw_firms_codes", "state_id", "dbo.US_States");
            DropForeignKey("dbo.cw_firms_codes", "country_id", "dbo.Countries");
            DropIndex("dbo.Vessel_Rule_Port", new[] { "firms_code_id" });
            DropIndex("dbo.cw_firms_codes", new[] { "state_id" });
            DropIndex("dbo.cw_firms_codes", new[] { "country_id" });
            DropIndex("dbo.cw_firms_codes", new[] { "firms_code" });
            DropIndex("dbo.Vessel_Imports", new[] { "firms_code_id" });
            DropIndex("dbo.Vessel_Imports", new[] { "state_id" });
            AlterColumn("dbo.Vessel_Rule_Port", "hmf", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Vessel_Rule_Port", "discharge_name", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AlterColumn("dbo.Vessel_Imports", "state_id", c => c.Int(nullable: false));
            DropColumn("dbo.Vessel_Rule_Port", "firms_code_id");
            DropColumn("dbo.Vessel_Rule_Port", "discharge_port");
            DropColumn("dbo.Vessel_Imports", "firms_code_id");
            DropTable("dbo.cw_firms_codes");
            CreateIndex("dbo.Vessel_Rule_Port", "discharge_name", unique: true, name: "Idx_DischargeName");
            CreateIndex("dbo.Vessel_Imports", "discharge_terminal_id");
            CreateIndex("dbo.Vessel_Imports", "state_id");
            AddForeignKey("dbo.Vessel_Imports", "discharge_terminal_id", "dbo.Vessel_DischargeTerminals", "id");

            ExecuteSqlFileDown();
        }
    }
}
