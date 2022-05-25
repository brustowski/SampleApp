using System.Data.Entity;
using FilingPortal.Cargowise.DataLayer;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer
{
    public class InbondContext : FpContext
    {
        /// <summary>
        /// Inbond inbound table
        /// </summary>
        public DbSet<InboundRecord> Inbond { get; set; }
        /// <summary>
        /// Inbond main grid models
        /// </summary>
        public DbSet<InboundReadModel> InbondGrid { get; set; }
        /// <summary>
        /// Inbond filing headers
        /// </summary>
        public DbSet<FilingHeader> InbondFilingHeaders { get; set; }
        /// <summary>
        /// Configuration sections
        /// </summary>
        public DbSet<Section> Sections { get; set; }
        /// <summary>
        /// Marks and Remarks templates
        /// </summary>
        public DbSet<MarksRemarksTemplate> MarksRemarksTemplates { get; set; }
        /// <summary>
        /// Documents
        /// </summary>
        public DbSet<Document> Documents { get; set; }
        /// <summary>
        /// The Entry Rules
        /// </summary>
        public DbSet<RuleEntry> RuleEntries { get; set; }
        /// <summary>
        /// The In-Bond Carriers handbook 
        /// </summary>
        public DbSet<InBondCarrier> InBondCarriers { get; set; }
        /// <summary>
        /// The database schema name
        /// </summary>
        public override string DefaultSchema => "inbond";

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(typeof(CargoWiseContext).Assembly);
        }

        #region Constructor
        static InbondContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InbondContext, Migrations.Configuration>());
        }
        #endregion
    }
}
