using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Framework.DataLayer.Migrations;


    public partial class cbdev_2676_add_missing_columnsto_filingheader_document_table : FpMigration
    {
        public override void Up()
        {

            this.AddColumnIfNotExists("dbo.Vessel_Import_Filing_Headers", "request_xml", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Import_Filing_Headers", "response_xml", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Export_Filing_Headers", "request_xml", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Export_Filing_Headers", "response_xml", c => c.String( unicode: false));
            this.AddColumnIfNotExists("dbo.Truck_Filing_Headers", "RequestXML", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Truck_Filing_Headers", "ResponseXML", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Rail_Filing_Headers", "RequestXML", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Rail_Filing_Headers", "ResponseXML", c => c.String(unicode: false));
            this.AddColumnIfNotExists("dbo.Rail_Documents", "Status", c => c.String(maxLength:50,unicode: false));
            this.AddColumnIfNotExists("dbo.Truck_Documents", "Status", c => c.String(maxLength: 50, unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Import_Documents", "Status", c => c.String(maxLength: 50, unicode: false));
            this.AddColumnIfNotExists("dbo.Vessel_Export_Documents", "Status", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vessel_Import_Filing_Headers", "request_xml");
            DropColumn("dbo.Vessel_Export_Filing_Headers", "response_xml");
            DropColumn("dbo.Vessel_Export_Filing_Headers", "request_xml");
            DropColumn("dbo.Vessel_Import_Filing_Headers", "response_xml");
            DropColumn("dbo.Truck_Filing_Headers", "RequestXML");
            DropColumn("dbo.Truck_Filing_Headers", "ResponseXML");
            DropColumn("dbo.Rail_Filing_Headers", "RequestXML");
            DropColumn("dbo.Rail_Filing_Headers", "ResponseXML");
            DropColumn("dbo.Rail_Documents", "Status");
            DropColumn("dbo.Truck_Documents", "Status");
            DropColumn("dbo.Vessel_Import_Documents", "Status");
            DropColumn("dbo.Vessel_Export_Documents", "Status");
        }

    }
    
}
