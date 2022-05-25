using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Common.DataLayer.Conventions;
using FilingPortal.Parts.Common.Domain.Entities;
using Framework.DataLayer;

namespace FilingPortal.Parts.Common.DataLayer
{
    public class CommonContext : FpContext
    {
        public DbSet<HeaderJobStatus> JobStatuses { get; set; }

        /// <summary>
        /// The Schema
        /// </summary>
        public override string DefaultSchema => "common";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            modelBuilder.Conventions.Add(new KeyConvention());
            modelBuilder.Conventions.Add(new ColumnNameConvention());
            modelBuilder.Conventions.Add(new DecimalConvention());
            modelBuilder.Conventions.Add(new StringConvention());
            modelBuilder.Conventions.Add(new DatetimeConvention());
            modelBuilder.Configurations.AddFromAssembly(typeof(CommonContext).Assembly);
        }

        #region Constructor
        static CommonContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CommonContext, Migrations.Configuration>());
        }
    }
    #endregion
}
