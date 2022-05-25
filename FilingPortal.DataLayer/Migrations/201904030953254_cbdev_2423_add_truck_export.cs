using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2423_add_truck_export : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.truck_export_filing_headers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        created_date = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "SUSER_NAME()"),
                        error_description = c.String(maxLength: 255, unicode: false),
                        filing_number = c.String(maxLength: 255, unicode: false),
                        mapping_status = c.Byte(),
                        filing_status = c.Byte(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.truck_exports",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        exporter_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        usppi_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        consignee_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        carrier = c.String(nullable: false, maxLength: 128, unicode: false),
                        scac = c.String(nullable: false, maxLength: 128, unicode: false),
                        license = c.String(nullable: false, maxLength: 128, unicode: false),
                        license_type = c.String(nullable: false, maxLength: 3, unicode: false),
                        tariff = c.String(nullable: false, maxLength: 35, unicode: false),
                        routed_tran = c.String(nullable: false, maxLength: 1, unicode: false),
                        container = c.String(nullable: false, maxLength: 128, unicode: false),
                        eccn = c.String(nullable: false, maxLength: 128, unicode: false),
                        goods_description = c.String(nullable: false, maxLength: 128, unicode: false),
                        deleted = c.Boolean(nullable: false, defaultValue: false),
                        created_date = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "SUSER_NAME()"),
                    })
                .PrimaryKey(t => t.id);
           
            CreateTable(
                "dbo.truck_export_filing_details",
                c => new
                    {
                        filing_header_id = c.Int(nullable: false),
                        truck_export_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.filing_header_id, t.truck_export_id })
                .ForeignKey("dbo.truck_export_filing_headers", t => t.filing_header_id, cascadeDelete: true)
                .ForeignKey("dbo.truck_exports", t => t.truck_export_id, cascadeDelete: true)
                .Index(t => t.filing_header_id)
                .Index(t => t.truck_export_id);

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();

            DropForeignKey("dbo.truck_export_filing_details", "truck_export_id", "dbo.truck_exports");
            DropForeignKey("dbo.truck_export_filing_details", "filing_header_id", "dbo.truck_export_filing_headers");
            DropIndex("dbo.truck_export_filing_details", new[] { "truck_export_id" });
            DropIndex("dbo.truck_export_filing_details", new[] { "filing_header_id" });
            DropTable("dbo.truck_export_filing_details");
            DropTable("dbo.truck_exports");
            DropTable("dbo.truck_export_filing_headers");
        }
    }
}
