using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    
    public partial class cbdev_2266_update_rail_inbound_view_table_models : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rail_BD_Parsed", "DuplicateOf", c => c.Int());
            CreateIndex("dbo.Rail_BD_Parsed", "DuplicateOf", name: "Idx_DuplicateOf");
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();
            DropIndex("dbo.Rail_BD_Parsed", "Idx_DuplicateOf");
            DropColumn("dbo.Rail_BD_Parsed", "DuplicateOf");
        }
    }
}
