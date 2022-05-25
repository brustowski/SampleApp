namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2754_add_mass_upload_button : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vessel_Import_Documents", "filing_header_id", "dbo.Vessel_Import_Filing_Headers");
            DropForeignKey("dbo.Vessel_Export_Documents", "filing_header_id", "dbo.Vessel_Export_Filing_Headers");
            DropForeignKey("dbo.truck_export_documents", "filing_header_id", "dbo.truck_export_filing_headers");
            DropIndex("dbo.Vessel_Import_Documents", new[] { "filing_header_id" });
            DropIndex("dbo.Vessel_Export_Documents", new[] { "filing_header_id" });
            DropIndex("dbo.Pipeline_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.Rail_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.Truck_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.truck_export_documents", new[] { "filing_header_id" });
            AddColumn("dbo.Vessel_Import_Documents", "inbound_record_id", c => c.Int());
            AddColumn("dbo.Vessel_Export_Documents", "inbound_record_id", c => c.Int());
            AddColumn("dbo.Pipeline_Documents", "inbound_record_id", c => c.Int());
            AddColumn("dbo.Rail_Documents", "inbound_record_id", c => c.Int());
            AddColumn("dbo.Truck_Documents", "inbound_record_id", c => c.Int());
            AddColumn("dbo.truck_export_documents", "inbound_record_id", c => c.Int());
            AlterColumn("dbo.Vessel_Import_Documents", "filing_header_id", c => c.Int());
            AlterColumn("dbo.Vessel_Export_Documents", "filing_header_id", c => c.Int());
            AlterColumn("dbo.Pipeline_Documents", "Filing_Headers_FK", c => c.Int());
            AlterColumn("dbo.Rail_Documents", "Filing_Headers_FK", c => c.Int());
            AlterColumn("dbo.Truck_Documents", "Filing_Headers_FK", c => c.Int());
            AlterColumn("dbo.truck_export_documents", "filing_header_id", c => c.Int());
            CreateIndex("dbo.Vessel_Import_Documents", "filing_header_id");
            CreateIndex("dbo.Vessel_Import_Documents", "inbound_record_id");
            CreateIndex("dbo.Vessel_Export_Documents", "filing_header_id");
            CreateIndex("dbo.Vessel_Export_Documents", "inbound_record_id");
            CreateIndex("dbo.Pipeline_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Pipeline_Documents", "inbound_record_id");
            CreateIndex("dbo.Rail_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Rail_Documents", "inbound_record_id");
            CreateIndex("dbo.Truck_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Truck_Documents", "inbound_record_id");
            CreateIndex("dbo.truck_export_documents", "filing_header_id");
            CreateIndex("dbo.truck_export_documents", "inbound_record_id");
            AddForeignKey("dbo.Vessel_Import_Documents", "inbound_record_id", "dbo.Vessel_Imports", "id");
            AddForeignKey("dbo.Vessel_Export_Documents", "inbound_record_id", "dbo.Vessel_Exports", "id");
            AddForeignKey("dbo.Pipeline_Documents", "inbound_record_id", "dbo.Pipeline_Inbound", "id");
            AddForeignKey("dbo.Rail_Documents", "inbound_record_id", "dbo.Rail_BD_Parsed", "BDP_PK");
            AddForeignKey("dbo.Truck_Documents", "inbound_record_id", "dbo.Truck_Inbound", "id");
            AddForeignKey("dbo.truck_export_documents", "inbound_record_id", "dbo.truck_exports", "id");
            AddForeignKey("dbo.Vessel_Import_Documents", "filing_header_id", "dbo.Vessel_Import_Filing_Headers", "id");
            AddForeignKey("dbo.Vessel_Export_Documents", "filing_header_id", "dbo.Vessel_Export_Filing_Headers", "id");
            AddForeignKey("dbo.truck_export_documents", "filing_header_id", "dbo.truck_export_filing_headers", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.truck_export_documents", "filing_header_id", "dbo.truck_export_filing_headers");
            DropForeignKey("dbo.Vessel_Export_Documents", "filing_header_id", "dbo.Vessel_Export_Filing_Headers");
            DropForeignKey("dbo.Vessel_Import_Documents", "filing_header_id", "dbo.Vessel_Import_Filing_Headers");
            DropForeignKey("dbo.truck_export_documents", "inbound_record_id", "dbo.truck_exports");
            DropForeignKey("dbo.Truck_Documents", "inbound_record_id", "dbo.Truck_Inbound");
            DropForeignKey("dbo.Rail_Documents", "inbound_record_id", "dbo.Rail_BD_Parsed");
            DropForeignKey("dbo.Pipeline_Documents", "inbound_record_id", "dbo.Pipeline_Inbound");
            DropForeignKey("dbo.Vessel_Export_Documents", "inbound_record_id", "dbo.Vessel_Exports");
            DropForeignKey("dbo.Vessel_Import_Documents", "inbound_record_id", "dbo.Vessel_Imports");
            DropIndex("dbo.truck_export_documents", new[] { "inbound_record_id" });
            DropIndex("dbo.truck_export_documents", new[] { "filing_header_id" });
            DropIndex("dbo.Truck_Documents", new[] { "inbound_record_id" });
            DropIndex("dbo.Truck_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.Rail_Documents", new[] { "inbound_record_id" });
            DropIndex("dbo.Rail_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.Pipeline_Documents", new[] { "inbound_record_id" });
            DropIndex("dbo.Pipeline_Documents", new[] { "Filing_Headers_FK" });
            DropIndex("dbo.Vessel_Export_Documents", new[] { "inbound_record_id" });
            DropIndex("dbo.Vessel_Export_Documents", new[] { "filing_header_id" });
            DropIndex("dbo.Vessel_Import_Documents", new[] { "inbound_record_id" });
            DropIndex("dbo.Vessel_Import_Documents", new[] { "filing_header_id" });
            AlterColumn("dbo.truck_export_documents", "filing_header_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Truck_Documents", "Filing_Headers_FK", c => c.Int(nullable: false));
            AlterColumn("dbo.Rail_Documents", "Filing_Headers_FK", c => c.Int(nullable: false));
            AlterColumn("dbo.Pipeline_Documents", "Filing_Headers_FK", c => c.Int(nullable: false));
            AlterColumn("dbo.Vessel_Export_Documents", "filing_header_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Vessel_Import_Documents", "filing_header_id", c => c.Int(nullable: false));
            DropColumn("dbo.truck_export_documents", "inbound_record_id");
            DropColumn("dbo.Truck_Documents", "inbound_record_id");
            DropColumn("dbo.Rail_Documents", "inbound_record_id");
            DropColumn("dbo.Pipeline_Documents", "inbound_record_id");
            DropColumn("dbo.Vessel_Export_Documents", "inbound_record_id");
            DropColumn("dbo.Vessel_Import_Documents", "inbound_record_id");
            CreateIndex("dbo.truck_export_documents", "filing_header_id");
            CreateIndex("dbo.Truck_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Rail_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Pipeline_Documents", "Filing_Headers_FK");
            CreateIndex("dbo.Vessel_Export_Documents", "filing_header_id");
            CreateIndex("dbo.Vessel_Import_Documents", "filing_header_id");
            AddForeignKey("dbo.truck_export_documents", "filing_header_id", "dbo.truck_export_filing_headers", "id", cascadeDelete: true);
            AddForeignKey("dbo.Vessel_Export_Documents", "filing_header_id", "dbo.Vessel_Export_Filing_Headers", "id", cascadeDelete: true);
            AddForeignKey("dbo.Vessel_Import_Documents", "filing_header_id", "dbo.Vessel_Import_Filing_Headers", "id", cascadeDelete: true);
        }
    }
}
