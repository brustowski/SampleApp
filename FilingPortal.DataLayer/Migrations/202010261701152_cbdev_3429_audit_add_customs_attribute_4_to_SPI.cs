// Generated Time: 10/26/2020 20:01:15
// Generated By: iapetrov

namespace FilingPortal.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3429_audit_add_customs_attribute_4_to_SPI : FpMigration
    {
        public override void Up()
        {
            DropIndex("dbo.imp_rail_audit_daily_spi_rules", new[] { "importer_code", "supplier_code", "goods_description", "destination_state", "date_from", "date_to" });
            AddColumn("dbo.imp_rail_audit_daily_spi_rules", "customs_attrib4", c => c.String(maxLength: 50, unicode: false));
            CreateIndex("dbo.imp_rail_audit_daily_spi_rules", new[] { "importer_code", "supplier_code", "goods_description", "destination_state", "date_from", "date_to", "customs_attrib4" }, unique: true);
        }

        public override void Down()
        {
            DropIndex("dbo.imp_rail_audit_daily_spi_rules", new[] { "importer_code", "supplier_code", "goods_description", "destination_state", "date_from", "date_to", "customs_attrib4" });
            DropColumn("dbo.imp_rail_audit_daily_spi_rules", "customs_attrib4");
            CreateIndex("dbo.imp_rail_audit_daily_spi_rules", new[] { "importer_code", "supplier_code", "goods_description", "destination_state", "date_from", "date_to" }, unique: true);
        }
    }
}
