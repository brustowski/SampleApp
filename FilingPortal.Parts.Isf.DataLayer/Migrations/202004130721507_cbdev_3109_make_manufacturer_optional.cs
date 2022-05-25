// Generated Time: 04/13/2020 11:40:58
// Generated By: iapetrov

namespace FilingPortal.Parts.Isf.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3109_make_manufacturer_optional : FpMigration
    {
        public override void Up()
        {
            DropIndex("isf.inbound_manufacturers", new[] { "manufacturer_id" });
            AlterColumn("isf.inbound_manufacturers", "manufacturer_id", c => c.Guid());
            CreateIndex("isf.inbound_manufacturers", "manufacturer_id");
        }
        
        public override void Down()
        {
            DropIndex("isf.inbound_manufacturers", new[] { "manufacturer_id" });
            AlterColumn("isf.inbound_manufacturers", "manufacturer_id", c => c.Guid(nullable: false));
            CreateIndex("isf.inbound_manufacturers", "manufacturer_id");
        }
    }
}
