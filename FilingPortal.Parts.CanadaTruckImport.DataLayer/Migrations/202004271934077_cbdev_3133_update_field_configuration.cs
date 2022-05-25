// Generated Time: 04/27/2020 22:34:07
// Generated By: AIKravchenko

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3133_update_field_configuration : FpMigration
    {
        public override void Up()
        {
            AddColumn("canada_imp_truck.form_configuration", "depends_on_id", c => c.Int());

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            DropColumn("canada_imp_truck.form_configuration", "depends_on_id");

            ExecuteSqlFileDown();
        }
    }
}
