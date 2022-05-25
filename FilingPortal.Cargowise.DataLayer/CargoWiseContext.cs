using System.Data.Entity;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Common.DataLayer.Conventions;

namespace FilingPortal.Cargowise.DataLayer
{
    public class CargoWiseContext : FpContext
    {
        /// <summary>
        /// Inbound records table
        /// </summary>
        public DbSet<USStates> UsStatesess { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DomesticPort> DomesticPorts { get; set; }
        public DbSet<ForeignPort> ForeignPorts { get; set; }

        public override string DefaultSchema => "cw";

        #region Constructor
        static CargoWiseContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CargoWiseContext, Migrations.Configuration>());
        }
        #endregion

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        /// <param name="modelBuilder"> The builder that defines the model for the context being created. </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new KeyConvention());
            modelBuilder.Conventions.Add(new ColumnNameConvention());
            modelBuilder.Conventions.Add(new DecimalConvention());
            modelBuilder.Conventions.Add(new StringConvention());
            modelBuilder.Conventions.Add(new DatetimeConvention());
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}
