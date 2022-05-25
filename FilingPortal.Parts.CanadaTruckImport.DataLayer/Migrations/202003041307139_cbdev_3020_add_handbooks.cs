// Generated Time: 03/04/2020 16:07:13
// Generated By: aikravchenko

using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3020_add_handbooks : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "canada_imp_truck.handbook_carrier",
                c => new
                {
                    code = c.String(nullable: false, maxLength: 4, unicode: false),
                    name = c.String(nullable: false, maxLength: 255, unicode: false),
                    transport_mode = c.String(maxLength: 3, unicode: false),
                })
                .PrimaryKey(t => t.code);

            CreateTable(
                "canada_imp_truck.handbook_product_code",
                c => new
                {
                    id = c.Guid(nullable: false),
                    code = c.String(nullable: false, maxLength: 35, unicode: false),
                    description = c.String(nullable: false, maxLength: 80, unicode: false),
                    updated = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "canada_imp_truck.handbook_service_option",
                c => new
                {
                    code = c.String(nullable: false, maxLength: 3, unicode: false),
                    description = c.String(nullable: false, maxLength: 200, unicode: false),
                })
                .PrimaryKey(t => t.code);

            AddColumn("canada_imp_truck.rule_importer", "product_code_id", c => c.Guid());
            AlterColumn("canada_imp_truck.filing_header", "job_link", c => c.String(maxLength: 8000, unicode: false));
            CreateIndex("canada_imp_truck.rule_importer", "product_code_id");
            AddForeignKey("canada_imp_truck.rule_importer", "product_code_id", "canada_imp_truck.handbook_product_code", "id");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            this.AddColumnIfNotExists("canada_imp_truck.rule_importer", "product_code", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileDown();

            DropForeignKey("canada_imp_truck.rule_importer", "product_code_id", "canada_imp_truck.handbook_product_code");
            DropIndex("canada_imp_truck.rule_importer", new[] { "product_code_id" });
            AlterColumn("canada_imp_truck.filing_header", "job_link", c => c.String(maxLength: 128, unicode: false));
            DropColumn("canada_imp_truck.rule_importer", "product_code_id");
            DropTable("canada_imp_truck.handbook_service_option");
            DropTable("canada_imp_truck.handbook_product_code");
            DropTable("canada_imp_truck.handbook_carrier");
        }
    }
}