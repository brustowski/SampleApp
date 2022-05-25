using FilingPortal.Parts.Common.DataLayer.Base;
using Framework.DataLayer.Migrations;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2445_add_facility_rule : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pipeline_Rule_Facility",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        facility = c.String(nullable: false, maxLength: 128, unicode: false),
                        port = c.String(nullable: false, maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.facility, unique: true, name: "Idx_Facility");
            
            this.AddColumnIfNotExists("dbo.Pipeline_Inbound", "SiteName", c => c.String(maxLength: 128, unicode: false));
            this.AddColumnIfNotExists("dbo.Pipeline_Inbound", "Facility", c => c.String(maxLength: 128, unicode: false));
            DropColumn("dbo.Pipeline_Inbound", "Port");
            DropColumn("dbo.Pipeline_Inbound", "EntryNumber");

            ExecuteSqlFile("201904221706508_cbdev_2451_update_pipeline_inbound_table.sql");
            ExecuteSqlFile("201904231523555_cbdev_2455_update_mapping_procedure.sql");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pipeline_Inbound", "EntryNumber", c => c.String(maxLength: 11, unicode: false));
            AddColumn("dbo.Pipeline_Inbound", "Port", c => c.String(nullable: false, maxLength: 128, unicode: false));
            DropIndex("dbo.Pipeline_Rule_Facility", "Idx_Facility");
            DropColumn("dbo.Pipeline_Inbound", "Facility");
            DropColumn("dbo.Pipeline_Inbound", "SiteName");
            DropTable("dbo.Pipeline_Rule_Facility");

            ExecuteSqlFile("201904221706508_cbdev_2451_update_pipeline_inbound_table_down.sql");
            ExecuteSqlFile("201904231523555_cbdev_2455_update_mapping_procedure_down.sql");
        }
    }
}
