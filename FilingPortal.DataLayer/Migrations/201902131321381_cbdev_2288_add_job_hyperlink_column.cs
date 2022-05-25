using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using Framework.DataLayer.Migrations;
    
    public partial class cbdev_2288_add_job_hyperlink_column : FpMigration
    {
        public override void Up()
        {
            this.AddColumnIfNotExists("dbo.Rail_Filing_Headers", "JobPKHyperlink", c => c.String(maxLength: 8000, unicode: false));
            this.AddColumnIfNotExists("dbo.Rail_Filing_Headers", "RequestXML", c => c.String(maxLength: 4000));
            this.AddColumnIfNotExists("dbo.Rail_Filing_Headers", "ResponseXML", c => c.String(maxLength: 4000));

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
