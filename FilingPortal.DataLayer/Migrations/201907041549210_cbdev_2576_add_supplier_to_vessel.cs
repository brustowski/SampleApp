using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2576_add_supplier_to_vessel : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vessel_Imports", "supplier_id", c => c.Guid());
            CreateIndex("dbo.Vessel_Imports", "supplier_id");
            AddForeignKey("dbo.Vessel_Imports", "supplier_id", "dbo.Clients", "id");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();

            DropForeignKey("dbo.Vessel_Imports", "supplier_id", "dbo.Clients");
            DropIndex("dbo.Vessel_Imports", new[] { "supplier_id" });
            DropColumn("dbo.Vessel_Imports", "supplier_id");
        }
    }
}
