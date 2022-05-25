using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Framework.DataLayer.Migrations;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    public class FpMigrationConfiguration<TContext> : DbMigrationsConfiguration<TContext>
        where TContext : DbContext

    {
        protected FpMigrationConfiguration()
        {
            string[] ignoreSchemas = GetIgnoreSchemas();

            AutomaticMigrationsEnabled = false;
            var gen = new FpPluginMigrationCodeGenerator();

            gen.IgnoreSchema(ignoreSchemas.ToArray());

            CodeGenerator = gen;
            CommandTimeout = (int)TimeSpan.FromMinutes(10).TotalSeconds;
            SetSqlGenerator("System.Data.SqlClient", new AddColumnIfNotExistsSqlGenerator());
        }

        protected virtual string[] GetIgnoreSchemas()
        {
            return new[] { "common." };
        }
    }
}
