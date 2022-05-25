namespace FilingPortal.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cbdev_2354_update_defvalues_tables : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Rail_DEFValues SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");
            Sql("UPDATE Rail_DEFValues_Manual SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");
            Sql("UPDATE Truck_DEFValues SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");
            Sql("UPDATE Truck_DEFValues_Manual SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");
            Sql("UPDATE Pipeline_DEFValues SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");
            Sql("UPDATE Pipeline_DEFValues_Manual SET Display_on_UI = 0 WHERE Display_on_UI IS NULL");

            AlterColumn("dbo.Rail_DEFValues", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Rail_DEFValues_Manual", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Pipeline_DEFValues", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Pipeline_DEFValues_Manual", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Truck_DEFValues", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
            AlterColumn("dbo.Truck_DEFValues_Manual", "Display_on_UI", c => c.Byte(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rail_DEFValues", "Display_on_UI", c => c.Byte());
            AlterColumn("dbo.Rail_DEFValues_Manual", "Display_on_UI", c => c.Byte());
            AlterColumn("dbo.Truck_DEFValues", "Display_on_UI", c => c.Byte());
            AlterColumn("dbo.Truck_DEFValues_Manual", "Display_on_UI", c => c.Byte());
            AlterColumn("dbo.Pipeline_DEFValues", "Display_on_UI", c => c.Byte());
            AlterColumn("dbo.Pipeline_DEFValues_Manual", "Display_on_UI", c => c.Byte());
        }
    }
}
