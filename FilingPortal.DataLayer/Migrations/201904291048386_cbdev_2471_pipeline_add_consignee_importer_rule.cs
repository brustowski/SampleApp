namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2471_pipeline_add_consignee_importer_rule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pipeline_Rule_Consignee_Importer",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ticket_importer = c.String(nullable: false, maxLength: 128, unicode: false),
                        importer_code = c.String(nullable: false, maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => t.ticket_importer, unique: true, name: "Idx_TicketImporter");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pipeline_Rule_Consignee_Importer", "Idx_TicketImporter");
            DropTable("dbo.Pipeline_Rule_Consignee_Importer");
        }
    }
}
