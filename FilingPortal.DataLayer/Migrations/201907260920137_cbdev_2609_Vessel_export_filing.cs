using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2609_Vessel_export_filing : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Vessel_Exports",
                    c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usppi_id = c.Guid(nullable: false),
                        importer_id = c.Guid(nullable: false),
                        vessel_id = c.Int(nullable: false),
                        export_date = c.DateTime(nullable: false),
                        load_port = c.String(nullable: false, maxLength: 128, unicode: false),
                        discharge_port = c.String(nullable: false, maxLength: 128, unicode: false),
                        country_of_destination = c.String(nullable: false, maxLength: 128, unicode: false),
                        tariff_type = c.String(nullable: false, maxLength: 4, unicode: false),
                        tariff = c.String(nullable: false, maxLength: 128, unicode: false),
                        goods_description = c.String(nullable: false, maxLength: 512, unicode: false),
                        origin_indicator = c.String(nullable: false, maxLength: 1, unicode: false),
                        quantity = c.Decimal(nullable: false, precision: 18, scale: 6),
                        weight = c.Decimal(precision: 18, scale: 6),
                        value = c.Decimal(nullable: false, precision: 18, scale: 6),
                        invoice_total = c.Decimal(nullable: false, precision: 18, scale: 6),
                        scheduler = c.String(nullable: false, maxLength: 128, unicode: false),
                        transport_ref = c.String(nullable: false, maxLength: 128, unicode: false),
                        description = c.String(nullable: false, maxLength: 128, unicode: false),
                        container = c.String(nullable: false, maxLength: 128, unicode: false),
                        in_bond = c.String(nullable: false, maxLength: 2, unicode: false),
                        sold_en_route = c.String(nullable: false, maxLength: 128, unicode: false),
                        export_adjustment_value = c.String(nullable: false, maxLength: 128, unicode: false),
                        original_itn = c.String(maxLength: 128, unicode: false),
                        routed_transaction = c.String(nullable: false, maxLength: 1, unicode: false),
                        reference_number = c.String(nullable: false, maxLength: 128, unicode: false),
                        deleted = c.Boolean(nullable: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Clients", t => t.importer_id)
                .ForeignKey("dbo.Clients", t => t.usppi_id)
                .ForeignKey("dbo.Vessels", t => t.vessel_id)
                .Index(t => t.usppi_id)
                .Index(t => t.importer_id)
                .Index(t => t.vessel_id);
            
            CreateTable(
                "dbo.Vessel_Export_Filing_Headers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                        error_description = c.String(maxLength: 8000, unicode: false),
                        filing_number = c.String(maxLength: 255, unicode: false),
                        mapping_status = c.Byte(),
                        filing_status = c.Byte(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Vessel_Export_Documents",
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
                .ForeignKey("dbo.Vessel_Export_Filing_Headers", t => t.filing_header_id, cascadeDelete: true)
                .Index(t => t.filing_header_id);
            
            CreateTable(
                "dbo.Vessel_Export_Rule_USPPI_Consignee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usppi_id = c.Guid(nullable: false),
                        consignee_id = c.Guid(nullable: false),
                        transaction_related = c.String(maxLength: 128, unicode: false),
                        ultimate_consignee_type = c.String(maxLength: 10, unicode: false),
                        consignee_address = c.String(maxLength: 128, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Clients", t => t.consignee_id)
                .ForeignKey("dbo.Clients", t => t.usppi_id)
                .Index(t => new { t.usppi_id, t.consignee_id }, unique: true);
            
            CreateTable(
                "dbo.Vessel_Export_Rule_USPPI",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        usppi_id = c.Guid(nullable: false),
                        address = c.String(maxLength: 128, unicode: false),
                        contact = c.String(maxLength: 128, unicode: false),
                        phone = c.String(maxLength: 128, unicode: false),
                        last_ref_number = c.Int(nullable: false),
                        last_ref_year = c.String(maxLength: 2, unicode: false),
                        last_ref = c.String(maxLength: 128, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Clients", t => t.usppi_id)
                .Index(t => t.usppi_id, unique: true);
            
            CreateTable(
                "dbo.Vessel_Export_Def_Values",
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
                .ForeignKey("dbo.Vessel_Export_Sections", t => t.section_id)
                .Index(t => t.section_id);
            
            CreateTable(
                "dbo.Vessel_Export_Sections",
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
                .ForeignKey("dbo.Vessel_Export_Sections", t => t.parent_id)
                .Index(t => t.name, unique: true)
                .Index(t => t.parent_id);
            
            CreateTable(
                "dbo.Vessel_Export_Def_Values_Manual",
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
                .Index(t => t.filing_header_id, name: "Idx_FilingHeaderId")
                .Index(t => new { t.record_id, t.table_name, t.column_name }, name: "Idx_recordId_tableName_columnName");
            
            CreateTable(
                "dbo.Vessel_Export_Rule_LoadPort",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        load_port = c.String(nullable: false, maxLength: 128, unicode: false),
                        unloco_code = c.String(maxLength: 10, unicode: false),
                        state_of_origin = c.String(maxLength: 10, unicode: false),
                        created_date = c.DateTime(),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.load_port, unique: true);
            
            CreateTable(
                "dbo.Vessel_Export_Filing_Details",
                c => new
                    {
                        filing_header_id = c.Int(nullable: false),
                        vessel_export_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.filing_header_id, t.vessel_export_id })
                .ForeignKey("dbo.Vessel_Export_Filing_Headers", t => t.filing_header_id, cascadeDelete: true)
                .ForeignKey("dbo.Vessel_Exports", t => t.vessel_export_id, cascadeDelete: true)
                .Index(t => t.filing_header_id)
                .Index(t => t.vessel_export_id);
            
            ExecuteSqlFileUp();
            ExecuteSqlFile("201907311149354_cbdev_2616_add_vessel_export_mapping_procedures.sql");

            CreateTable(
                    "dbo.Countries",
                    c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.name, unique: true, name: "Idx_Name");

            CreateTable(
                    "dbo.Discharge_Ports",
                    c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        port = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.port, unique: true, name: "Idx_Port");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Discharge_Ports", "Idx_Port");
            DropIndex("dbo.Countries", "Idx_Name");
            DropTable("dbo.Discharge_Ports");
            DropTable("dbo.Countries");

            ExecuteSqlFile("201907311149354_cbdev_2616_add_vessel_export_mapping_procedures_down.sql");
            ExecuteSqlFileDown();

            DropForeignKey("dbo.Vessel_Export_Def_Values", "section_id", "dbo.Vessel_Export_Sections");
            DropForeignKey("dbo.Vessel_Export_Sections", "parent_id", "dbo.Vessel_Export_Sections");
            DropForeignKey("dbo.Vessel_Exports", "vessel_id", "dbo.Vessels");
            DropForeignKey("dbo.Vessel_Exports", "usppi_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Exports", "importer_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI", "usppi_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI_Consignee", "usppi_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Export_Rule_USPPI_Consignee", "consignee_id", "dbo.Clients");
            DropForeignKey("dbo.Vessel_Export_Filing_Details", "vessel_export_id", "dbo.Vessel_Exports");
            DropForeignKey("dbo.Vessel_Export_Filing_Details", "filing_header_id", "dbo.Vessel_Export_Filing_Headers");
            DropForeignKey("dbo.Vessel_Export_Documents", "filing_header_id", "dbo.Vessel_Export_Filing_Headers");
            DropIndex("dbo.Vessel_Export_Filing_Details", new[] { "vessel_export_id" });
            DropIndex("dbo.Vessel_Export_Filing_Details", new[] { "filing_header_id" });
            DropIndex("dbo.Vessel_Export_Rule_LoadPort", new[] { "load_port" });
            DropIndex("dbo.Vessel_Export_Def_Values_Manual", "Idx_recordId_tableName_columnName");
            DropIndex("dbo.Vessel_Export_Def_Values_Manual", "Idx_FilingHeaderId");
            DropIndex("dbo.Vessel_Export_Sections", new[] { "parent_id" });
            DropIndex("dbo.Vessel_Export_Sections", new[] { "name" });
            DropIndex("dbo.Vessel_Export_Def_Values", new[] { "section_id" });
            DropIndex("dbo.Vessel_Export_Rule_USPPI", new[] { "usppi_id" });
            DropIndex("dbo.Vessel_Export_Rule_USPPI_Consignee", new[] { "usppi_id", "consignee_id" });
            DropIndex("dbo.Vessel_Export_Documents", new[] { "filing_header_id" });
            DropIndex("dbo.Vessel_Exports", new[] { "vessel_id" });
            DropIndex("dbo.Vessel_Exports", new[] { "importer_id" });
            DropIndex("dbo.Vessel_Exports", new[] { "usppi_id" });
            DropTable("dbo.Vessel_Export_Filing_Details");
            DropTable("dbo.Vessel_Export_Rule_LoadPort");
            DropTable("dbo.Vessel_Export_Def_Values_Manual");
            DropTable("dbo.Vessel_Export_Sections");
            DropTable("dbo.Vessel_Export_Def_Values");
            DropTable("dbo.Vessel_Export_Rule_USPPI");
            DropTable("dbo.Vessel_Export_Rule_USPPI_Consignee");
            DropTable("dbo.Vessel_Export_Documents");
            DropTable("dbo.Vessel_Export_Filing_Headers");
            DropTable("dbo.Vessel_Exports");
        }
    }
}
