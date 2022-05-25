using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2223_add_pipeline_filing : FpMigration
    {
        public override void Up()
        {
            // Add pipeline API rule table
            CreateTable(
                "dbo.Pipeline_Rule_API",
                t => new
                {
                    Id = t.Int(nullable: false),
                    API = t.String(nullable: false, maxLength: 128),
                    Tariff = t.String(maxLength: 128),
                    created_date = t.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = t.String(maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                })
                .PrimaryKey(t => t.Id);

            Sql(@"
            INSERT Pipeline_Rule_API(Id, API, Tariff) VALUES (1, N'<25', '2709001000')
            INSERT Pipeline_Rule_API(Id, API, Tariff) VALUES (2, N'>=25', '2709002090')
            ");

            // CBDEV-2224_add_pipeline_filing_headers_and_def_values
            CreateTable(
                "dbo.Pipeline_DEFValues_Manual",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    ColName = c.String(maxLength: 128, unicode: false),
                    CreatedDate = c.DateTime(defaultValueSql: "getdate()"),
                    CreatedUser = c.String(maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    DefValue = c.String(maxLength: 128, unicode: false),
                    Display_on_UI = c.Byte(),
                    FEditable = c.Byte(nullable: false, defaultValue: 1),
                    FHasDefaultVal = c.Byte(nullable: false, defaultValue: 0),
                    Filing_Headers_FK = c.Int(nullable: false),
                    FMandatory = c.Byte(nullable: false, defaultValue: 0),
                    FManual = c.Byte(nullable: false, defaultValue: 0),
                    ModifiedDate = c.DateTime(nullable: false, defaultValueSql: "getdate()" ),
                    TableName = c.String(maxLength: 128, unicode: false),
                    UI_Section = c.String(maxLength: 32, unicode: false),
                    ValueLabel = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.Pipeline_DEFValues",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    Display_on_UI = c.Byte(),
                    ValueLabel = c.String(maxLength: 128, unicode: false),
                    ValueDesc = c.String(maxLength: 128, unicode: false),
                    DefValue = c.String(maxLength: 128, unicode: false),
                    TableName = c.String(maxLength: 128, unicode: false),
                    ColName = c.String(maxLength: 128, unicode: false),
                    FManual = c.Byte(nullable: false),
                    FHasDefaultVal = c.Byte(nullable: false),
                    FEditable = c.Byte(nullable: false),
                    UI_Section = c.String(maxLength: 32, unicode: false),
                    FMandatory = c.Byte(nullable: false),
                    CreatedDate = c.DateTime(defaultValueSql: "getdate()"),
                    CreatedUser = c.String(maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    SingleFilingOrder = c.Byte(),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.Pipeline_Documents",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    CreatedDate = c.DateTime(defaultValueSql: "getdate()"),
                    CreatedUser = c.String(maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    DocumentType = c.String(maxLength: 128, unicode: false),
                    file_content = c.Binary(),
                    file_desc = c.String(maxLength: 1000, unicode: false),
                    file_extension = c.String(maxLength: 128, unicode: false),
                    filename = c.String(maxLength: 255, unicode: false),
                    Filing_Headers_FK = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Pipeline_Filing_Headers", t => t.Filing_Headers_FK)
                .Index(t => t.Filing_Headers_FK);

            CreateTable(
                "dbo.Pipeline_Filing_Headers",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    CreatedDate = c.DateTime(defaultValueSql: "getdate()"),
                    CreatedUser = c.String(maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    ErrorDescription = c.String(maxLength: 255, unicode: false),
                    FilingNumber = c.String(maxLength: 255, unicode: false),
                    FilingStatus = c.Int(),
                    MappingStatus = c.Int(),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.Pipeline_Filing_Details",
                c => new
                {
                    Filing_Headers_FK = c.Int(nullable: false),
                    Pipeline_Inbounds_FK = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Filing_Headers_FK, t.Pipeline_Inbounds_FK })
                .ForeignKey("dbo.Pipeline_Filing_Headers", t => t.Filing_Headers_FK, cascadeDelete: true)
                .ForeignKey("dbo.Pipeline_Inbound", t => t.Pipeline_Inbounds_FK, cascadeDelete: true)
                .Index(t => t.Filing_Headers_FK)
                .Index(t => t.Pipeline_Inbounds_FK);

            ExecuteSqlFile("201812290809103_cbdev_2224_add_pipeline_filing_headers_and_def_values.sql");

            // CBDEV-2227_Create_pipeline_results_tables
            ExecuteSqlFile("201812291258222_cbdev_2227_create_pipeline_results_tables.sql");

            // CBDEV-2229_add_pipeline_inbound_grid_view
            AddColumn("dbo.Pipeline_Inbound", "FDeleted", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Pipeline_Filing_Headers", "MappingStatus", false, "Idx_MappingStatus", false);
            CreateIndex("dbo.Pipeline_Filing_Headers", "FilingStatus", false, "Idx_FilingStatus", false);
            AddForeignKey("dbo.Pipeline_Filing_Headers", "MappingStatus", "dbo.MappingStatus");
            AddForeignKey("dbo.Pipeline_Filing_Headers", "FilingStatus", "dbo.FilingStatus");

            ExecuteSqlFile("201901141328433_cbdev_2229_add_pipeline_inbound_grid_view.sql");

            // CBDEV-2229_add_delete_procedure
            string sql = @"BEGIN
update dbo.Pipeline_Inbound set FDeleted=@FDeleted 
  where Id=@Id AND NOT EXISTS (
    select pfh.id
    from dbo.Pipeline_Filing_Details pfd 
    inner join dbo.Pipeline_Filing_Headers pfh on pfd.Filing_Headers_FK=pfh.id 
    where pfd.Pipeline_Inbounds_FK = @Id and (isnull(MappingStatus,0)>0 or isnull(FilingStatus,0)>0) );
end";

            CreateStoredProcedure("dbo.pipeline_inbound_del", x => new {
                Id = x.Int(),
                FDeleted = x.Boolean()
            }, sql);

            //cbdev_2247_pipeline_def_values
            ExecuteSqlFile("201901171241100_cbdev_2247_pipeline_def_values.sql");

            //cbdev_2228_add_pipeline_filing_procedures
            ExecuteSqlFile("201901181044447_cbdev_2228_add_pipeline_filing_procedures.sql");

            //cbdev_2231_add_pipeline_report_view
            ExecuteSqlFile("201901181255445_cbdev_2231_add_pipeline_report_view.sql");
        }

        public override void Down()
        {
            //cbdev_2231_add_pipeline_report_view
            ExecuteSqlFile("201901181255445_cbdev_2231_add_pipeline_report_view_down.sql");

            //cbdev_2228_add_pipeline_filing_procedures
            ExecuteSqlFile("201901181044447_cbdev_2228_add_pipeline_filing_procedures_down.sql");

            // CBDEV-2229_add_delete_procedure
            DropStoredProcedure("dbo.pipeline_inbound_del");

            // CBDEV-2229_add_pipeline_inbound_grid_view
            Sql(@"IF OBJECT_ID(N'dbo.Pipeline_Inbound_Grid', 'V') IS NOT NULL DROP VIEW dbo.Pipeline_Inbound_Grid");
            DropForeignKey("dbo.Pipeline_Filing_Headers", "MappingStatus", "dbo.MappingStatus");
            DropForeignKey("dbo.Pipeline_Filing_Headers", "FilingStatus", "dbo.FilingStatus");
            DropIndex("dbo.Pipeline_Filing_Headers", "Idx_MappingStatus");
            DropIndex("dbo.Pipeline_Filing_Headers", "Idx_FilingStatus");
            DropColumn("dbo.Pipeline_Inbound", "FDeleted");

            // CBDEV-2227_create_pipeline_results_tables
            ExecuteSqlFile("201812291258222_cbdev_2227_create_pipeline_results_tables_down.sql");

            // CBDEV-2224_add_pipeline_filing_headers_and_def_values
            ExecuteSqlFile("201812290809103_cbdev_2224_add_pipeline_filing_headers_and_def_values_down.sql");

            // Drop pipeline API rule table
            DropTable("dbo.Pipeline_Rule_API");
        }
    }
}
