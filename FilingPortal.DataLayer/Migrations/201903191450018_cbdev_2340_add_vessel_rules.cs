using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2340_add_vessel_rules : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vessel_Rule_Importer",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    ior = c.String(nullable: false, maxLength: 128, unicode: false),
                    cw_ior = c.String(maxLength: 128, unicode: false),
                    cw_supplier = c.String(maxLength: 128, unicode: false),
                    relationship = c.String(maxLength: 128, unicode: false),
                    value_recon = c.String(maxLength: 128, unicode: false),
                    fta_recon = c.String(maxLength: 128, unicode: false),
                    spi = c.String(maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "suser_name()")
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.ior, unique: true, name: "Idx_IOR");

            CreateTable(
                "dbo.Vessel_Rule_Port",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    discharge_name = c.String(nullable: false, maxLength: 128, unicode: false),
                    entry_port = c.String(maxLength: 4, unicode: false),
                    arrival_port = c.String(maxLength: 4, unicode: false),
                    destination_state = c.String(maxLength: 128, unicode: false),
                    firms_code = c.String(maxLength: 128, unicode: false),
                    hmf = c.String(maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "suser_name()")
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.discharge_name, unique: true, name: "Idx_DischargeName");

            CreateTable(
                "dbo.Vessel_Rule_Product",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    tariff = c.String(nullable: false, maxLength: 10, unicode: false),
                    goods_description = c.String(maxLength: 128, unicode: false),
                    customs_attribute1 = c.String(maxLength: 128, unicode: false),
                    customs_attribute2 = c.String(maxLength: 128, unicode: false),
                    invoice_uq = c.String(maxLength: 128, unicode: false),
                    product_id = c.String(maxLength: 128, unicode: false),
                    tsca_requirement = c.String(maxLength: 128, unicode: false),
                    created_date = c.DateTime(nullable: false, defaultValueSql: "getdate()"),
                    created_user = c.String(nullable: false, maxLength: 128, unicode: false, defaultValueSql: "suser_name()")
                })
                .PrimaryKey(t => t.id)
                .Index(t => t.tariff, unique: true, name: "Idx_Tariff");

            ExecuteSqlFile("201903181609022_cbdev_2345_add_vessel_rules_permissions.sql");
            ExecuteSqlFile("201903191429042_cbdev_2348_add_vessel_rule_data.sql");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vessel_Rule_Product", "Idx_Tariff");
            DropIndex("dbo.Vessel_Rule_Port", "Idx_DischargeName");
            DropIndex("dbo.Vessel_Rule_Importer", "Idx_IOR");
            DropTable("dbo.Vessel_Rule_Product");
            DropTable("dbo.Vessel_Rule_Port");
            DropTable("dbo.Vessel_Rule_Importer");

            ExecuteSqlFile("201903181609022_cbdev_2345_add_vessel_rules_permissions_down.sql");
        }
    }
}
