using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2441_create_Tariff_State_Dictionary_Tables : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tariff",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        USC_Tariff = c.String(nullable: false, maxLength: 35),
                        Short_Description = c.String(maxLength: 128),
                        FromDateTime = c.DateTime(nullable: false),
                        ToDateTime = c.DateTime(nullable: false),
                        LastUpdatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.US_States",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        StateCode = c.String(maxLength: 2),
                        StateName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            ExecuteSqlFile("201904221739589_cbdev-2441-add-us_states_dictionary_table.sql");
        }
        
        public override void Down()
        {
            DropTable("dbo.US_States");
            DropTable("dbo.Tariff");
        }
    }
}
