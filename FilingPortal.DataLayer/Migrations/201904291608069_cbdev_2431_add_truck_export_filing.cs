using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2431_add_truck_export_filing : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.truck_export_def_values_manual",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    filing_header_id = c.Int(nullable: false),
                    parent_record_id = c.Int(nullable: false),
                    section_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    section_title = c.String(nullable: false, maxLength: 128, unicode: false),
                    table_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    column_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    record_id = c.Int(nullable: false),
                    modification_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    label = c.String(nullable: false, maxLength: 128, unicode: false),
                    description = c.String(maxLength: 128, unicode: false),
                    value = c.String(maxLength: 512, unicode: false),
                    editable = c.Boolean(nullable: false),
                    has_default_value = c.Boolean(nullable: false),
                    mandatory = c.Boolean(nullable: false),
                    display_on_ui = c.Byte(nullable: false),
                    manual = c.Byte(nullable: false),
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.filing_header_id, name: "Idx_FilingHeaderId")
                .Index(t => new { t.record_id, t.table_name, t.column_name }, name: "Idx_recordId_tableName_columnName");

            CreateTable(
                "dbo.truck_export_def_values",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    section_id = c.Int(nullable: false),
                    column_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "suser_name()"),
                    default_value = c.String(maxLength: 512, unicode: false),
                    editable = c.Boolean(nullable: false),
                    display_on_ui = c.Byte(nullable: false),
                    has_default_value = c.Boolean(nullable: false),
                    mandatory = c.Boolean(nullable: false),
                    manual = c.Byte(nullable: false),
                    description = c.String(maxLength: 128, unicode: false),
                    label = c.String(nullable: false, maxLength: 128, unicode: false),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.truck_export_sections", t => t.section_id)
                .Index(t => t.section_id);

            CreateTable(
                "dbo.truck_export_sections",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    name = c.String(maxLength: 128, unicode: false),
                    title = c.String(nullable: false, maxLength: 128, unicode: false),
                    table_name = c.String(maxLength: 128, unicode: false),
                    is_array = c.Boolean(nullable: false),
                    parent_id = c.Int(),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.truck_export_sections", t => t.parent_id)
                .Index(t => t.parent_id);

            AlterColumn("dbo.truck_exports", "goods_description", c => c.String(nullable: false, maxLength: 512, unicode: false));

            AlterColumn("dbo.Truck_DEFValues", "CreatedDate", c => c.DateTime(nullable: true, defaultValueSql: "getdate()"));
            AlterColumn("dbo.Truck_DEFValues", "CreatedUser", c => c.String(nullable: true, defaultValueSql: "suser_name()"));

            ExecuteSqlFile("201904101005084_cbdev_2434_add_fields_configuration.sql");
            ExecuteSqlFile("201904150950449_cbdev_2436_add_truck_export_procedures.sql");
            ExecuteSqlFile("201904171010241_cbdev_2442_add_truck_export_table_view.sql");

            CreateTable(
                    "dbo.truck_export_documents",
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
                .ForeignKey("dbo.truck_export_filing_headers", t => t.filing_header_id, cascadeDelete: true)
                .Index(t => t.filing_header_id);

            AddColumn("dbo.truck_export_def_values", "single_filing_order", c => c.Byte());

            ExecuteSqlFile("cbdev_2447_add_truck_export_documents_update_grid_view.sql");

            ExecuteSqlFile("cbdev_2449_add_truck_export_defvalue_manual_readmodel.sql");

            AddColumn("dbo.truck_export_sections", "procedure_name", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.truck_export_sections", "name", c => c.String(nullable: false, maxLength: 128, unicode: false));
            CreateIndex("dbo.truck_export_sections", "name", unique: true);

            ExecuteSqlFile("cbdev_2462_add_procedure_name_column_to_section_table.sql");
        }
        
        public override void Down()
        {
            ExecuteSqlFile("cbdev_2462_add_procedure_name_column_to_section_table_down.sql");
            DropIndex("dbo.truck_export_sections", new[] { "name" });
            AlterColumn("dbo.truck_export_sections", "name", c => c.String(maxLength: 128, unicode: false));
            DropColumn("dbo.truck_export_sections", "procedure_name");

            Sql("IF OBJECT_ID('dbo.v_truck_export_def_values_manual', 'V') IS NOT NULL DROP VIEW dbo.v_truck_export_def_values_manual");

            ExecuteSqlFile("cbdev_2447_add_truck_export_documents_update_grid_view_down.sql");

            DropForeignKey("dbo.truck_export_documents", "filing_header_id", "dbo.truck_export_filing_headers");
            DropIndex("dbo.truck_export_documents", new[] { "filing_header_id" });
            DropColumn("dbo.truck_export_def_values", "single_filing_order");
            DropTable("dbo.truck_export_documents");

            ExecuteSqlFile("201904171010241_cbdev_2442_add_truck_export_table_view_down.sql");

            ExecuteSqlFile("201904150950449_cbdev_2436_add_truck_export_procedures_down.sql");

            AlterColumn("dbo.truck_exports", "goods_description", c => c.String(nullable: false, maxLength: 128, unicode: false));

            DropForeignKey("dbo.truck_export_def_values", "section_id", "dbo.truck_export_sections");
            DropForeignKey("dbo.truck_export_sections", "parent_id", "dbo.truck_export_sections");
            DropIndex("dbo.truck_export_sections", new[] { "parent_id" });
            DropIndex("dbo.truck_export_def_values", new[] { "section_id" });
            DropIndex("dbo.truck_export_def_values_manual", "Idx_recordId_tableName_columnName");
            DropIndex("dbo.truck_export_def_values_manual", "Idx_FilingHeaderId");
            DropTable("dbo.truck_export_sections");
            DropTable("dbo.truck_export_def_values");
            DropTable("dbo.truck_export_def_values_manual");
        }
    }
}
