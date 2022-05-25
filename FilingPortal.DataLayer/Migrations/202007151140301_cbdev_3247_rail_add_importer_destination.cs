// Generated Time: 07/16/2020 09:53:08
// Generated By: iapetrov

using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3247_rail_add_importer_destination : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.imp_rail_rule_product", "Idx__description_importer_supplier_port");
            AddColumn("dbo.imp_rail_inbound", "consignee", c => c.String(maxLength: 200));
            AddColumn("dbo.imp_rail_inbound", "destination", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.imp_rail_rule_product", "destination", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("dbo.imp_rail_rule_product", new[] { "description1", "importer", "supplier", "port", "destination" }, unique: true, name: "Idx__description_importer_supplier_port_dest");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropIndex("dbo.imp_rail_rule_product", "Idx__description_importer_supplier_port_dest");
            DropColumn("dbo.imp_rail_rule_product", "destination");
            DropColumn("dbo.imp_rail_inbound", "destination");
            DropColumn("dbo.imp_rail_inbound", "consignee");
            CreateIndex("dbo.imp_rail_rule_product", new[] { "description1", "importer", "supplier", "port" }, unique: true, name: "Idx__description_importer_supplier_port");

            ExecuteSqlFileDown();
        }
    }
}