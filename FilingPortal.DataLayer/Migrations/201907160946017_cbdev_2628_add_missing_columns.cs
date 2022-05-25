using FilingPortal.Parts.Common.DataLayer.Base;
using Framework.DataLayer.Migrations;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2628_add_missing_columns : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("dbo.Pipeline_Filing_Headers", "RequestXML", c => c.String());
            this.AddColumnIfNotExists("dbo.Pipeline_Filing_Headers", "ResponseXML", c => c.String());

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
