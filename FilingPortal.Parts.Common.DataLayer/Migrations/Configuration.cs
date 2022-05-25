using System;
using System.Data.Entity.Migrations;
using FilingPortal.Parts.Common.DataLayer.Base;
using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.Common.DataLayer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CommonContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            CodeGenerator = new FpMigrationCodeGenerator();
            CommandTimeout = (int)TimeSpan.FromMinutes(10).TotalSeconds;
            SetSqlGenerator("System.Data.SqlClient", new AddColumnIfNotExistsSqlGenerator());
        }
    }
}
