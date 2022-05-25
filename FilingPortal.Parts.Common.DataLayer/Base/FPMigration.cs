using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.IO;
using System.Linq;
using FilingPortal.Parts.Common.DataLayer.Helpers;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    public abstract class FpMigration: DbMigration
    {
        private string path = MigrationHelper.GetResourcePath();

        protected void ExecuteSqlFileUp(bool suppressTransaction = false)
        {
            var defaultFileName = $"{((IMigrationMetadata)this).Id}.sql";
            SqlFile(GetMigrationFileName(defaultFileName), suppressTransaction);
        }

        protected void ExecuteSqlFile(string fileName, bool suppressTransaction = false)
        {
            SqlFile(GetMigrationFileName(fileName), suppressTransaction);
        }

        protected void ExecuteSqlFileDown(bool suppressTransaction = false)
        {
            var defaultFileName = $"{((IMigrationMetadata)this).Id}_down.sql";
            SqlFile(GetMigrationFileName(defaultFileName), suppressTransaction);
        }

        private string GetMigrationFileName(string filename)
        {
            return Directory.GetFiles(Path.Combine(path, "Migrations"), "*.sql", SearchOption.TopDirectoryOnly)
                .FirstOrDefault(x => x.EndsWith(filename));
        }

        public override void Up()
        {
            ExecuteSqlFileUp();
        }

        public override void Down()
        {
            ExecuteSqlFileDown();
        }
    }
}
