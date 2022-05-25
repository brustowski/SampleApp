// Generated Time: 12/27/2019 17:41:16
// Generated By: IAPetrov

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2905_add_rules : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "canada_imp_truck.rule_importer",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        importer_id = c.Guid(nullable: false),
                        vendor_id = c.Guid(),
                        transport = c.String(maxLength: 128, unicode: false),
                        service_option = c.String(maxLength: 128, unicode: false),
                        gross_weight_unit = c.String(maxLength: 128, unicode: false),
                        no_packages = c.Int(),
                        packages_unit = c.String(maxLength: 128, unicode: false),
                        inv_number = c.Int(),
                        direct_ship_place = c.String(maxLength: 128, unicode: false),
                        packs = c.Int(),
                        consignee_id = c.Guid(),
                        exporter_id = c.Guid(),
                        org_state = c.String(maxLength: 128, unicode: false),
                        product_code = c.String(maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Clients", t => t.consignee_id)
                .ForeignKey("dbo.Clients", t => t.exporter_id)
                .ForeignKey("dbo.Clients", t => t.importer_id)
                .ForeignKey("dbo.Clients", t => t.vendor_id)
                .Index(t => t.importer_id)
                .Index(t => t.vendor_id)
                .Index(t => t.consignee_id)
                .Index(t => t.exporter_id);
            
            CreateTable(
                "canada_imp_truck.rule_carrier",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        carrier = c.String(nullable: false, maxLength: 128, unicode: false),
                        gross_weight = c.Decimal(precision: 18, scale: 6),
                        invoice_qty = c.Decimal(precision: 18, scale: 6),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "canada_imp_truck.rule_port",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        port_of_clearance = c.String(nullable: false, maxLength: 128, unicode: false),
                        sub_location = c.String(maxLength: 128, unicode: false),
                        first_port_of_arrival = c.String(maxLength: 128, unicode: false),
                        final_destination = c.String(maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(nullable: false, maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);

            ExecuteSqlFileUp();
            
        }
        
        public override void Down()
        {
            DropForeignKey("canada_imp_truck.rule_importer", "vendor_id", "dbo.Clients");
            DropForeignKey("canada_imp_truck.rule_importer", "importer_id", "dbo.Clients");
            DropForeignKey("canada_imp_truck.rule_importer", "exporter_id", "dbo.Clients");
            DropForeignKey("canada_imp_truck.rule_importer", "consignee_id", "dbo.Clients");
            DropIndex("canada_imp_truck.rule_importer", new[] { "exporter_id" });
            DropIndex("canada_imp_truck.rule_importer", new[] { "consignee_id" });
            DropIndex("canada_imp_truck.rule_importer", new[] { "vendor_id" });
            DropIndex("canada_imp_truck.rule_importer", new[] { "importer_id" });
            DropTable("canada_imp_truck.rule_port");
            DropTable("canada_imp_truck.rule_carrier");
            DropTable("canada_imp_truck.rule_importer");

            ExecuteSqlFileDown();
        }
    }
}
