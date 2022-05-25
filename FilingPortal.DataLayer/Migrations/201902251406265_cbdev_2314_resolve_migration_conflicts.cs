using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2314_resolve_migration_conflicts : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFile("201902251053470_cbdev_2314_add_issuer_code.sql");
        }
        
        public override void Down()
        {
            ExecuteSqlFile("201902251053470_cbdev_2314_add_issuer_code_down.sql");
        }
    }
}
