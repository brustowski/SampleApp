// Generated Time: 12/26/2019 15:59:59
// Generated By: IAPetrov

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2905_add_canada_truck_imports : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "canada_imp_truck.filing_header",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        mapping_status = c.Byte(),
                        filing_status = c.Byte(),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                        filing_number = c.String(maxLength: 255, unicode: false),
                        job_link = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "canada_imp_truck.documents",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                        document_type = c.String(maxLength: 128, unicode: false),
                        description = c.String(maxLength: 1000, unicode: false),
                        extension = c.String(nullable: false, maxLength: 128, unicode: false),
                        file_name = c.String(nullable: false, maxLength: 255, unicode: false),
                        filing_header_id = c.Int(nullable: false),
                        inbound_record_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("canada_imp_truck.filing_header", t => t.filing_header_id, cascadeDelete: true)
                .Index(t => t.filing_header_id);
            
            CreateTable(
                "canada_imp_truck.inbound",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        importer_id = c.Guid(nullable: false),
                        carrier_at_import = c.String(maxLength: 4, unicode: false),
                        port = c.String(maxLength: 4, unicode: false),
                        pars_number = c.String(maxLength: 128, unicode: false),
                        eta = c.DateTime(),
                        owners_reference = c.String(maxLength: 128, unicode: false),
                        direct_ship_date = c.DateTime(),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                        deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Clients", t => t.importer_id)
                .Index(t => t.importer_id);
            
            CreateTable(
                "canada_imp_truck.form_section_configuration",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128, unicode: false),
                        title = c.String(nullable: false, maxLength: 128, unicode: false),
                        table_name = c.String(maxLength: 128, unicode: false),
                        procedure_name = c.String(maxLength: 128, unicode: false),
                        is_array = c.Boolean(nullable: false),
                        is_hidden = c.Boolean(nullable: false),
                        parent_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("canada_imp_truck.form_section_configuration", t => t.parent_id)
                .Index(t => t.name, unique: true)
                .Index(t => t.parent_id);
            
            CreateTable(
                "canada_imp_truck.form_configuration",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        section_id = c.Int(nullable: false),
                        has_default_value = c.Boolean(nullable: false),
                        editable = c.Boolean(nullable: false),
                        mandatory = c.Boolean(nullable: false),
                        column_name = c.String(nullable: false, maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                        value = c.String(maxLength: 512, unicode: false),
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
                .ForeignKey("canada_imp_truck.form_section_configuration", t => t.section_id)
                .Index(t => t.section_id);
            
            CreateTable(
                "canada_imp_truck.filing_detail",
                c => new
                    {
                        filing_header_id = c.Int(nullable: false),
                        inbound_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.filing_header_id, t.inbound_id })
                .ForeignKey("canada_imp_truck.filing_header", t => t.filing_header_id, cascadeDelete: true)
                .ForeignKey("canada_imp_truck.inbound", t => t.inbound_id, cascadeDelete: true)
                .Index(t => t.filing_header_id)
                .Index(t => t.inbound_id);
            
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropForeignKey("canada_imp_truck.form_section_configuration", "parent_id", "canada_imp_truck.form_section_configuration");
            DropForeignKey("canada_imp_truck.form_configuration", "section_id", "canada_imp_truck.form_section_configuration");
            DropForeignKey("canada_imp_truck.filing_detail", "inbound_id", "canada_imp_truck.inbound");
            DropForeignKey("canada_imp_truck.filing_detail", "filing_header_id", "canada_imp_truck.filing_header");
            DropForeignKey("canada_imp_truck.inbound", "importer_id", "dbo.Clients");
            DropForeignKey("canada_imp_truck.documents", "filing_header_id", "canada_imp_truck.filing_header");
            DropIndex("canada_imp_truck.filing_detail", new[] { "inbound_id" });
            DropIndex("canada_imp_truck.filing_detail", new[] { "filing_header_id" });
            DropIndex("canada_imp_truck.form_configuration", new[] { "section_id" });
            DropIndex("canada_imp_truck.form_section_configuration", new[] { "parent_id" });
            DropIndex("canada_imp_truck.form_section_configuration", new[] { "name" });
            DropIndex("canada_imp_truck.inbound", new[] { "importer_id" });
            DropIndex("canada_imp_truck.documents", new[] { "filing_header_id" });
            DropTable("canada_imp_truck.filing_detail");
            DropTable("canada_imp_truck.form_configuration");
            DropTable("canada_imp_truck.form_section_configuration");
            DropTable("canada_imp_truck.inbound");
            DropTable("canada_imp_truck.documents");
            DropTable("canada_imp_truck.filing_header");

            ExecuteSqlFileDown();
        }
    }
}