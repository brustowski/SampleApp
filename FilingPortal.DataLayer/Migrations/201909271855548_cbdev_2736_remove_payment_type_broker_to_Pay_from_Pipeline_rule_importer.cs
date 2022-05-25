using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2736_remove_payment_type_broker_to_Pay_from_Pipeline_rule_importer : FpMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pipeline_Rule_Importer", "broker_to_pay");
            DropColumn("dbo.Pipeline_Rule_Importer", "payment_type");
            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pipeline_Rule_Importer", "payment_type", c => c.Int());
            AddColumn("dbo.Pipeline_Rule_Importer", "broker_to_pay", c => c.String(maxLength: 128, unicode: false));
            ExecuteSqlFileDown();
        }
    }
}
