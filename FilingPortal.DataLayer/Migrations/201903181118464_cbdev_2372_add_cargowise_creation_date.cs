using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2372_add_cargowise_creation_date : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rail_BD_Parsed", "CW_CreatedDateUTC", c => c.DateTime(nullable: false, defaultValueSql: "getdate()"));
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
            DropColumn("dbo.Rail_BD_Parsed", "CW_CreatedDateUTC");
        }
    }
}
