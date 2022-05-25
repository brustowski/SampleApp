using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    public partial class cbdev_2334_update_rail_edi_message_parsing_procedure : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFileUp();
            AlterColumn("dbo.Rail_BD_Parsed", "EquipmentNumber", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rail_BD_Parsed", "EquipmentNumber", c => c.String(maxLength: 10));
        }
    }
}
