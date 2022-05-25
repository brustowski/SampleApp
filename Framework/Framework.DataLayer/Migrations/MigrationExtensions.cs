using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Builders;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;

namespace Framework.DataLayer.Migrations
{
    public static class MigrationExtensions 
    {
        public static void AddColumnIfNotExists(this DbMigration migration, string table, string name, Func<ColumnBuilder, ColumnModel> columnAction, object anonymousArguments = null)
        {
            ((IDbMigration)migration)
              .AddOperation(new AddColumnIfNotExistsOperation(table, name, columnAction, anonymousArguments));
        }
    }
}
