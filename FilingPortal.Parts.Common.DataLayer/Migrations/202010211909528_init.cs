// Generated Time: 10/21/2020 22:20:43
// Generated By: iapetrov

namespace FilingPortal.Parts.Common.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "common.job_statuses",
                c => new
                    {
                        id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("common.job_statuses");
        }
    }
}
