using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2502_add_complex_and_dropdown_fields_support : FpMigration
    {
        public override void Up()
        {
            ExecuteSqlFile("201905231354078_cbdev_2494_add_handbooks.sql");

            AddColumn("dbo.Pipeline_DEFValues_Manual", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_DEFValues_Manual", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_DEFValues_Manual", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_DEFValues", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_DEFValues", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_DEFValues", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues_Manual", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues_Manual", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Rail_DEFValues_Manual", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues_Manual", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues_Manual", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Truck_DEFValues_Manual", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values", "handbook_name", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values_manual", "paired_field_table", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values_manual", "paired_field_column", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.truck_export_def_values_manual", "handbook_name", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            ExecuteSqlFileDown();

            DropColumn("dbo.truck_export_def_values_manual", "handbook_name");
            DropColumn("dbo.truck_export_def_values_manual", "paired_field_column");
            DropColumn("dbo.truck_export_def_values_manual", "paired_field_table");
            DropColumn("dbo.truck_export_def_values", "handbook_name");
            DropColumn("dbo.truck_export_def_values", "paired_field_column");
            DropColumn("dbo.truck_export_def_values", "paired_field_table");
            DropColumn("dbo.Truck_DEFValues_Manual", "handbook_name");
            DropColumn("dbo.Truck_DEFValues_Manual", "paired_field_column");
            DropColumn("dbo.Truck_DEFValues_Manual", "paired_field_table");
            DropColumn("dbo.Truck_DEFValues", "handbook_name");
            DropColumn("dbo.Truck_DEFValues", "paired_field_column");
            DropColumn("dbo.Truck_DEFValues", "paired_field_table");
            DropColumn("dbo.Rail_DEFValues_Manual", "handbook_name");
            DropColumn("dbo.Rail_DEFValues_Manual", "paired_field_column");
            DropColumn("dbo.Rail_DEFValues_Manual", "paired_field_table");
            DropColumn("dbo.Rail_DEFValues", "handbook_name");
            DropColumn("dbo.Rail_DEFValues", "paired_field_column");
            DropColumn("dbo.Rail_DEFValues", "paired_field_table");
            DropColumn("dbo.Pipeline_DEFValues", "handbook_name");
            DropColumn("dbo.Pipeline_DEFValues", "paired_field_column");
            DropColumn("dbo.Pipeline_DEFValues", "paired_field_table");
            DropColumn("dbo.Pipeline_DEFValues_Manual", "handbook_name");
            DropColumn("dbo.Pipeline_DEFValues_Manual", "paired_field_column");
            DropColumn("dbo.Pipeline_DEFValues_Manual", "paired_field_table");

            ExecuteSqlFile("201905231354078_cbdev_2494_add_handbooks_down.sql");
        }
    }
}
