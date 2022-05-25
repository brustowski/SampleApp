using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_truck_export_table_structure : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.truck_exports", "exporter", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "importer", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "tariff_type", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AddColumn("dbo.truck_exports", "sold_en_route", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "master_bill", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "origin", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "export", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "export_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.truck_exports", "customs_qty", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.truck_exports", "price", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.truck_exports", "gross_weight", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.truck_exports", "gross_weight_uom", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.truck_exports", "routed_tran", c => c.String(nullable: false, maxLength: 10, unicode: false));
            DropColumn("dbo.truck_exports", "exporter_code");
            DropColumn("dbo.truck_exports", "usppi_code");
            DropColumn("dbo.truck_exports", "consignee_code");
            DropColumn("dbo.truck_exports", "carrier");
            DropColumn("dbo.truck_exports", "scac");
            DropColumn("dbo.truck_exports", "license");
            DropColumn("dbo.truck_exports", "license_type");
            DropColumn("dbo.truck_exports", "container");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("dbo.truck_exports", "container", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "license_type", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AddColumn("dbo.truck_exports", "license", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "scac", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "carrier", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "consignee_code", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "usppi_code", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.truck_exports", "exporter_code", c => c.String(nullable: false, maxLength: 128, unicode: false));
            Sql("UPDATE dbo.truck_exports SET routed_tran = LEFT(routed_tran, 1)");
            AlterColumn("dbo.truck_exports", "routed_tran", c => c.String(nullable: false, maxLength: 1, unicode: false));
            DropColumn("dbo.truck_exports", "gross_weight_uom");
            DropColumn("dbo.truck_exports", "gross_weight");
            DropColumn("dbo.truck_exports", "price");
            DropColumn("dbo.truck_exports", "customs_qty");
            DropColumn("dbo.truck_exports", "export_date");
            DropColumn("dbo.truck_exports", "export");
            DropColumn("dbo.truck_exports", "origin");
            DropColumn("dbo.truck_exports", "master_bill");
            DropColumn("dbo.truck_exports", "sold_en_route");
            DropColumn("dbo.truck_exports", "tariff_type");
            DropColumn("dbo.truck_exports", "importer");
            DropColumn("dbo.truck_exports", "exporter");

            ExecuteSqlFileDown();
        }
    }
}
