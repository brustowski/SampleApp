// Generated Time: 05/22/2020 11:27:21
// Generated By: iapetrov

namespace FilingPortal.Parts.Rail.Export.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;

    public partial class cbdev_3175_update_mapping_attributes : FpMigration
    {
        public override void Up()
        {
            AlterColumn("us_exp_rail.declaration", "exp_date", x => x.DateTime(defaultValueSql: "(getdate())"));
            AlterColumn("us_exp_rail.containers", "type", x => x.String(defaultValue: "20GP"));

            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
