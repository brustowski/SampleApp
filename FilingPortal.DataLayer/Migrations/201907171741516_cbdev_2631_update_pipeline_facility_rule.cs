using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2631_update_pipeline_facility_rule : FpMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pipeline_Rule_Facility", "destination", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_Rule_Facility", "destination_state", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_Rule_Facility", "firms_code", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_Rule_Facility", "issuer", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_Rule_Facility", "origin", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.Pipeline_Rule_Facility", "pipeline", c => c.String(maxLength: 128, unicode: false));

            ExecuteSqlFileUp();

            DropIndex("dbo.Pipeline_Rule_PortImporter", "Idx_Port_Importer");
            DropTable("dbo.Pipeline_Rule_PortImporter");

        }
        
        public override void Down()
        {
            DropColumn("dbo.Pipeline_Rule_Facility", "pipeline");
            DropColumn("dbo.Pipeline_Rule_Facility", "origin");
            DropColumn("dbo.Pipeline_Rule_Facility", "issuer");
            DropColumn("dbo.Pipeline_Rule_Facility", "firms_code");
            DropColumn("dbo.Pipeline_Rule_Facility", "destination_state");
            DropColumn("dbo.Pipeline_Rule_Facility", "destination");
            ExecuteSqlFileDown();
            CreateTable(
                    "dbo.Pipeline_Rule_PortImporter",
                    c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        customs_attribute3 = c.String(maxLength: 128, unicode: false),
                        description = c.String(maxLength: 128, unicode: false),
                        destination = c.String(maxLength: 128, unicode: false),
                        destination_state = c.String(maxLength: 128, unicode: false),
                        firms_code = c.String(maxLength: 128, unicode: false),
                        importer = c.String(maxLength: 128, unicode: false),
                        issuer = c.String(maxLength: 128, unicode: false),
                        origin = c.String(maxLength: 128, unicode: false),
                        pipeline = c.String(maxLength: 128, unicode: false),
                        port = c.String(nullable: false, maxLength: 128, unicode: false),
                        created_date = c.DateTime(nullable: false),
                        created_user = c.String(maxLength: 128, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            CreateIndex("dbo.Pipeline_Rule_PortImporter", new[] { "port", "importer" }, unique: true, name: "Idx_Port_Importer");

        }
    }
}
