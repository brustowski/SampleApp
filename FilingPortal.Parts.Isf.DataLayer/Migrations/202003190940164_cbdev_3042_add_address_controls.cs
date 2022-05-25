// Generated Time: 03/19/2020 15:55:01
// Generated By: IAPetrov

namespace FilingPortal.Parts.Isf.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_3042_add_address_controls : FpMigration
    {
        public override void Up()
        {
            DropForeignKey("isf.inbound", "buyer_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "consignee_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "consolidator_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "container_stuffing_location_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "importer_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound_manufacturers", "manufacturer_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "seller_address_id", "dbo.Client_Addresses");
            DropForeignKey("isf.inbound", "ship_to_address_id", "dbo.Client_Addresses");
            DropIndex("isf.inbound", new[] { "importer_address_id" });
            DropIndex("isf.inbound", new[] { "buyer_address_id" });
            DropIndex("isf.inbound", new[] { "consignee_address_id" });
            DropIndex("isf.inbound", new[] { "seller_address_id" });
            DropIndex("isf.inbound", new[] { "ship_to_address_id" });
            DropIndex("isf.inbound", new[] { "container_stuffing_location_address_id" });
            DropIndex("isf.inbound", new[] { "consolidator_address_id" });
            DropIndex("isf.inbound_manufacturers", new[] { "manufacturer_address_id" });
            AddColumn("isf.inbound", "importer_app_address_id", c => c.Int(nullable: false));
            AddColumn("isf.inbound", "buyer_app_address_id", c => c.Int());
            AddColumn("isf.inbound", "consignee_app_address_id", c => c.Int());
            AddColumn("isf.inbound", "seller_app_address_id", c => c.Int());
            AddColumn("isf.inbound", "ship_to_app_address_id", c => c.Int());
            AddColumn("isf.inbound", "container_stuffing_location_app_address_id", c => c.Int());
            AddColumn("isf.inbound", "consolidator_app_address_id", c => c.Int());
            AddColumn("isf.inbound_manufacturers", "manufacturer_app_address_id", c => c.Int());
            CreateIndex("isf.inbound", "importer_app_address_id");
            CreateIndex("isf.inbound", "buyer_app_address_id");
            CreateIndex("isf.inbound", "consignee_app_address_id");
            CreateIndex("isf.inbound", "seller_app_address_id");
            CreateIndex("isf.inbound", "ship_to_app_address_id");
            CreateIndex("isf.inbound", "container_stuffing_location_app_address_id");
            CreateIndex("isf.inbound", "consolidator_app_address_id");
            CreateIndex("isf.inbound_manufacturers", "manufacturer_app_address_id");
            AddForeignKey("isf.inbound", "buyer_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "consignee_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "consolidator_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "container_stuffing_location_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "importer_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound_manufacturers", "manufacturer_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "seller_app_address_id", "dbo.app_addresses", "id");
            AddForeignKey("isf.inbound", "ship_to_app_address_id", "dbo.app_addresses", "id");
            DropColumn("isf.inbound", "importer_address_id");
            DropColumn("isf.inbound", "buyer_address_id");
            DropColumn("isf.inbound", "consignee_address_id");
            DropColumn("isf.inbound", "seller_address_id");
            DropColumn("isf.inbound", "ship_to_address_id");
            DropColumn("isf.inbound", "container_stuffing_location_address_id");
            DropColumn("isf.inbound", "consolidator_address_id");
            DropColumn("isf.inbound_manufacturers", "manufacturer_address_id");

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            AddColumn("isf.inbound_manufacturers", "manufacturer_address_id", c => c.Guid());
            AddColumn("isf.inbound", "consolidator_address_id", c => c.Guid());
            AddColumn("isf.inbound", "container_stuffing_location_address_id", c => c.Guid());
            AddColumn("isf.inbound", "ship_to_address_id", c => c.Guid());
            AddColumn("isf.inbound", "seller_address_id", c => c.Guid());
            AddColumn("isf.inbound", "consignee_address_id", c => c.Guid());
            AddColumn("isf.inbound", "buyer_address_id", c => c.Guid());
            AddColumn("isf.inbound", "importer_address_id", c => c.Guid(nullable: false));
            DropForeignKey("isf.inbound", "ship_to_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "seller_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound_manufacturers", "manufacturer_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "importer_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "container_stuffing_location_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "consolidator_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "consignee_app_address_id", "dbo.app_addresses");
            DropForeignKey("isf.inbound", "buyer_app_address_id", "dbo.app_addresses");
            DropIndex("isf.inbound_manufacturers", new[] { "manufacturer_app_address_id" });
            DropIndex("isf.inbound", new[] { "consolidator_app_address_id" });
            DropIndex("isf.inbound", new[] { "container_stuffing_location_app_address_id" });
            DropIndex("isf.inbound", new[] { "ship_to_app_address_id" });
            DropIndex("isf.inbound", new[] { "seller_app_address_id" });
            DropIndex("isf.inbound", new[] { "consignee_app_address_id" });
            DropIndex("isf.inbound", new[] { "buyer_app_address_id" });
            DropIndex("isf.inbound", new[] { "importer_app_address_id" });
            DropColumn("isf.inbound_manufacturers", "manufacturer_app_address_id");
            DropColumn("isf.inbound", "consolidator_app_address_id");
            DropColumn("isf.inbound", "container_stuffing_location_app_address_id");
            DropColumn("isf.inbound", "ship_to_app_address_id");
            DropColumn("isf.inbound", "seller_app_address_id");
            DropColumn("isf.inbound", "consignee_app_address_id");
            DropColumn("isf.inbound", "buyer_app_address_id");
            DropColumn("isf.inbound", "importer_app_address_id");
            CreateIndex("isf.inbound_manufacturers", "manufacturer_address_id");
            CreateIndex("isf.inbound", "consolidator_address_id");
            CreateIndex("isf.inbound", "container_stuffing_location_address_id");
            CreateIndex("isf.inbound", "ship_to_address_id");
            CreateIndex("isf.inbound", "seller_address_id");
            CreateIndex("isf.inbound", "consignee_address_id");
            CreateIndex("isf.inbound", "buyer_address_id");
            CreateIndex("isf.inbound", "importer_address_id");
            AddForeignKey("isf.inbound", "ship_to_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "seller_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound_manufacturers", "manufacturer_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "importer_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "container_stuffing_location_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "consolidator_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "consignee_address_id", "dbo.Client_Addresses", "id");
            AddForeignKey("isf.inbound", "buyer_address_id", "dbo.Client_Addresses", "id");

            ExecuteSqlFileDown();
        }
    }
}