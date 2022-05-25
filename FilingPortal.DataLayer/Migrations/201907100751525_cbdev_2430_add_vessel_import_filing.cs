using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2430_add_vessel_import_filing : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vessel_Import_Documents",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    document_type = c.String(maxLength: 128, unicode: false),
                    content = c.Binary(),
                    description = c.String(maxLength: 1000, unicode: false),
                    extension = c.String(nullable: false, maxLength: 128, unicode: false),
                    file_name = c.String(nullable: false, maxLength: 255, unicode: false),
                    filing_header_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Vessel_Import_Filing_Headers", t => t.filing_header_id, cascadeDelete: true)
                .Index(t => t.filing_header_id);

            CreateTable(
                "dbo.Vessel_Import_Def_Values",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    section_id = c.Int(nullable: false),
                    editable = c.Boolean(nullable: false),
                    has_default_value = c.Boolean(nullable: false),
                    mandatory = c.Boolean(nullable: false),
                    column_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    default_value = c.String(maxLength: 512, unicode: false),
                    display_on_ui = c.Byte(nullable: false),
                    manual = c.Byte(nullable: false),
                    single_filing_order = c.Byte(),
                    description = c.String(maxLength: 128, unicode: false),
                    label = c.String(nullable: false, maxLength: 128, unicode: false),
                    paired_field_table = c.String(maxLength: 128, unicode: false),
                    paired_field_column = c.String(maxLength: 128, unicode: false),
                    handbook_name = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Vessel_Import_Sections", t => t.section_id)
                .Index(t => t.section_id);

            CreateTable(
                "dbo.Vessel_Import_Sections",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    name = c.String(nullable: false, maxLength: 128, unicode: false),
                    title = c.String(nullable: false, maxLength: 128, unicode: false),
                    table_name = c.String(maxLength: 128, unicode: false),
                    procedure_name = c.String(maxLength: 128, unicode: false),
                    is_array = c.Boolean(nullable: false),
                    parent_id = c.Int(),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Vessel_Import_Sections", t => t.parent_id)
                .Index(t => t.name, unique: true)
                .Index(t => t.parent_id);

            CreateTable(
                "dbo.Vessel_Import_Def_Values_Manual",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    editable = c.Boolean(nullable: false),
                    has_default_value = c.Boolean(nullable: false),
                    mandatory = c.Boolean(nullable: false),
                    filing_header_id = c.Int(nullable: false),
                    section_title = c.String(nullable: false, maxLength: 128, unicode: false),
                    table_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    column_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    modification_date = c.DateTime(nullable: false),
                    label = c.String(nullable: false, maxLength: 128, unicode: false),
                    value = c.String(maxLength: 512, unicode: false),
                    display_on_ui = c.Byte(nullable: false),
                    manual = c.Byte(nullable: false),
                    paired_field_table = c.String(maxLength: 128, unicode: false),
                    paired_field_column = c.String(maxLength: 128, unicode: false),
                    handbook_name = c.String(maxLength: 128, unicode: false),
                    parent_record_id = c.Int(nullable: false),
                    section_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    record_id = c.Int(nullable: false),
                    description = c.String(maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.filing_header_id, name: "Idx_FilingHeaderId");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropForeignKey("dbo.Vessel_Import_Def_Values", "section_id", "dbo.Vessel_Import_Sections");
            DropForeignKey("dbo.Vessel_Import_Sections", "parent_id", "dbo.Vessel_Import_Sections");
            DropForeignKey("dbo.Vessel_Import_Documents", "filing_header_id", "dbo.Vessel_Import_Filing_Headers");
            DropIndex("dbo.Vessel_Import_Def_Values_Manual", "Idx_FilingHeaderId");
            DropIndex("dbo.Vessel_Import_Sections", new[] { "parent_id" });
            DropIndex("dbo.Vessel_Import_Sections", new[] { "name" });
            DropIndex("dbo.Vessel_Import_Def_Values", new[] { "section_id" });
            DropIndex("dbo.Vessel_Import_Documents", new[] { "filing_header_id" });
            DropTable("dbo.Vessel_Import_Def_Values_Manual");
            DropTable("dbo.Vessel_Import_Sections");
            DropTable("dbo.Vessel_Import_Def_Values");
            DropTable("dbo.Vessel_Import_Documents");
        }
    }
}
