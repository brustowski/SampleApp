using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2650_add_vessel_intake_form_missinig_columns : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Imports", "customs_qty", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Vessel_Imports", "unit_price", c => c.Decimal(nullable: false, precision: 18, scale: 6));
            AddColumn("dbo.Vessel_Imports", "country_of_origin_id", c => c.Int());
            AddColumn("dbo.Vessel_Imports", "owner_ref", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AddColumn("dbo.Countries", "code", c => c.String(maxLength: 2, unicode: false));
            CreateIndex("dbo.Vessel_Imports", "country_of_origin_id");
            AddForeignKey("dbo.Vessel_Imports", "country_of_origin_id", "dbo.Countries", "id");

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();

            DropForeignKey("dbo.Vessel_Imports", "country_of_origin_id", "dbo.Countries");
            DropIndex("dbo.Vessel_Imports", new[] { "country_of_origin_id" });
            DropColumn("dbo.Countries", "code");
            DropColumn("dbo.Vessel_Imports", "owner_ref");
            DropColumn("dbo.Vessel_Imports", "country_of_origin_id");
            DropColumn("dbo.Vessel_Imports", "unit_price");
            DropColumn("dbo.Vessel_Imports", "customs_qty");
        }
    }
}
