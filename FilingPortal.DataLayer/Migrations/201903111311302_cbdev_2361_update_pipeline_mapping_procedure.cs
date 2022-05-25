using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2361_update_pipeline_mapping_procedure : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
