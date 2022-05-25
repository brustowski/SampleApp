using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Common.DataLayer.Helpers;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.DataLayer.Migrations
{
    using FilingPortal.Domain.Enums;
    using Framework.Infrastructure.Extensions;
    using System;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Text;

    public partial class cbdev_1952_update_mapping_filing_status_fields : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilingStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            Sql(GetInsertStatmentFromEnum<FilingStatus>("dbo.FilingStatus"));

            CreateTable(
                "dbo.MappingStatus",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            Sql(GetInsertStatmentFromEnum<MappingStatus>("dbo.MappingStatus"));

            DropIndex("dbo.Rail_Filing_Headers", "Idx_MappingStatus");
            AlterColumn("dbo.Rail_Filing_Headers", "MappingStatus", c => c.Int());
            AlterColumn("dbo.Rail_Filing_Headers", "FilingStatus", c => c.Int());
            CreateIndex("dbo.Rail_Filing_Headers", "MappingStatus", false, "Idx_MappingStatus", false);
            CreateIndex("dbo.Rail_Filing_Headers", "FilingStatus", false, "Idx_FilingStatus", false);
            AddForeignKey("dbo.Rail_Filing_Headers", "MappingStatus", "dbo.MappingStatus");
            AddForeignKey("dbo.Rail_Filing_Headers", "FilingStatus", "dbo.FilingStatus");

            ExecuteSqlFileUp();
        }

        private string GetInsertStatmentFromEnum<TEnum>(string tableName)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"SET IDENTITY_INSERT {tableName} ON");

            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(t => $"Insert into {tableName} (id, name) values ({Enum.Format(typeof(TEnum), t, "d")}, '{(t as Enum).GetDescription()}')");
            builder.AppendLine(string.Join(Environment.NewLine, values));
            builder.Append($"SET IDENTITY_INSERT {tableName} OFF");
            return builder.ToString();
        }


        public override void Down()
        {
            ExecuteSqlFileDown();

            DropForeignKey("dbo.Rail_Filing_Headers", "MappingStatus", "dbo.MappingStatus");
            DropForeignKey("dbo.Rail_Filing_Headers", "FilingStatus", "dbo.FilingStatus");
            DropIndex("dbo.Rail_Filing_Headers", "Idx_MappingStatus");
            DropIndex("dbo.Rail_Filing_Headers", "Idx_FilingStatus");
            AlterColumn("dbo.Rail_Filing_Headers", "MappingStatus", c => c.Byte());
            AlterColumn("dbo.Rail_Filing_Headers", "FilingStatus", c => c.Byte());
            CreateIndex("dbo.Rail_Filing_Headers", "MappingStatus", false, "Idx_MappingStatus", false);
            DropTable("dbo.MappingStatus");
            DropTable("dbo.FilingStatus");
        }
    }
}
