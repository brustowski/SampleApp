namespace FilingPortal.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cbdev_2094_clients_table : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Clients", t => new
            {
                Id = t.Guid(name: "id", identity: true),
                ClientCode = t.String(name: "ClientCode", nullable: false, storeType: "nvarchar", maxLength: 12),
                Status = t.Boolean(name: "Status"),
                ClientName = t.String(name: "ClientName", storeType: "nvarchar", maxLength: 100),
                Importer = t.Boolean(name: "Importer"),
                Supplier = t.Boolean(name: "Supplier"),
                LastUpdatedTime = t.DateTime(name: "LastUpdatedTime", storeType: "datetime", nullable: false)
            }).PrimaryKey(t => t.Id).Index(t=>t.ClientCode, name: "IDX_Clients_ClientCode", unique: true);
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
