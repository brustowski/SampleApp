// Generated Time: 05/19/2020 19:23:34
// Generated By: iapetrov

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3155_add_mappings : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "us_exp_rail.inbound_containers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        container_number = c.String(nullable: false, maxLength: 10, unicode: false),
                        customs_qty = c.Decimal(nullable: false, precision: 18, scale: 6),
                        price = c.Decimal(nullable: false, precision: 18, scale: 6),
                        gross_weight = c.Decimal(nullable: false, precision: 18, scale: 6),
                        inbound_record_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("us_exp_rail.inbound", t => t.inbound_record_id, cascadeDelete: true)
                .Index(t => t.inbound_record_id);
            
            CreateTable(
                "us_exp_rail.rule_consignee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        consignee_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        destination = c.String(maxLength: 5, unicode: false),
                        country = c.String(maxLength: 2, unicode: false),
                        ultimate_consignee_type = c.String(maxLength: 1, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.consignee_code, unique: true, name: "Idx__consignee_code");
            
            CreateTable(
                "us_exp_rail.rule_exporter_consignee",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        exporter = c.String(nullable: false, maxLength: 128, unicode: false),
                        consignee_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        address = c.String(maxLength: 128, unicode: false),
                        contact = c.String(maxLength: 128, unicode: false),
                        phone = c.String(maxLength: 128, unicode: false),
                        tran_related = c.String(maxLength: 1, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => new { t.exporter, t.consignee_code }, unique: true, name: "Idx__consignee_code__exporter");
            
            AddColumn("us_exp_rail.inbound", "exporter", c => c.String(nullable: false, maxLength: 12, unicode: false));
            AddColumn("us_exp_rail.inbound", "importer", c => c.String(nullable: false, maxLength: 12, unicode: false));
            AddColumn("us_exp_rail.inbound", "master_bill", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddColumn("us_exp_rail.inbound", "load_port", c => c.String(maxLength: 4, unicode: false));
            AddColumn("us_exp_rail.inbound", "carrier", c => c.String(maxLength: 128, unicode: false));
            AddColumn("us_exp_rail.inbound", "tariff_type", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AddColumn("us_exp_rail.inbound", "tariff", c => c.String(nullable: false, maxLength: 35, unicode: false));
            AddColumn("us_exp_rail.inbound", "goods_description", c => c.String(nullable: false, maxLength: 512, unicode: false));
            AddColumn("us_exp_rail.inbound", "gross_weight_uom", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("us_exp_rail.inbound", "created_user", c => c.String(nullable: false, maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropForeignKey("us_exp_rail.inbound_containers", "inbound_record_id", "us_exp_rail.inbound");
            DropIndex("us_exp_rail.rule_exporter_consignee", "Idx__consignee_code__exporter");
            DropIndex("us_exp_rail.rule_consignee", "Idx__consignee_code");
            DropIndex("us_exp_rail.inbound_containers", new[] { "inbound_record_id" });
            AlterColumn("us_exp_rail.inbound", "created_user", c => c.String(maxLength: 128, unicode: false));
            DropColumn("us_exp_rail.inbound", "gross_weight_uom");
            DropColumn("us_exp_rail.inbound", "goods_description");
            DropColumn("us_exp_rail.inbound", "tariff");
            DropColumn("us_exp_rail.inbound", "tariff_type");
            DropColumn("us_exp_rail.inbound", "carrier");
            DropColumn("us_exp_rail.inbound", "load_port");
            DropColumn("us_exp_rail.inbound", "master_bill");
            DropColumn("us_exp_rail.inbound", "importer");
            DropColumn("us_exp_rail.inbound", "exporter");
            DropTable("us_exp_rail.rule_exporter_consignee");
            DropTable("us_exp_rail.rule_consignee");
            DropTable("us_exp_rail.inbound_containers");

            ExecuteSqlFileDown();
        }
    }
}
