using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class CBDEV_1980_add_filer_based_information_to_truck_filing : FpMigration
    {
        public override void Up()
        {
            CreateTable("dbo.app_users_data", t => new
            {
                UserAccount = t.String(name: "UserAccount", storeType: "nvarchar", maxLength: 255),
                Branch = t.String(name: "Branch", storeType: "varchar", maxLength: 128),
                Broker = t.String(name: "Broker", storeType: "varchar", maxLength: 128),
                Location = t.String(name: "Location", storeType: "varchar", maxLength: 128)
            })
            .PrimaryKey(t => t.UserAccount)
            .ForeignKey("dbo.app_users", t => t.UserAccount);

            // Change truck filing procedure
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            // Revert truck filing procedure
            ExecuteSqlFileDown();

            DropTable("dbo.app_users_data");
        }
    }
}
