using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2704_add_port_tables : FpMigration
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
