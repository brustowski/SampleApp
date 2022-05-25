using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class init : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFile("Init.sql");
            ExecuteSqlFile("Required_data.sql");
            ExecuteSqlFile("Required_script.sql");
        }

        public override void Down()
        {

        }
    }
}
