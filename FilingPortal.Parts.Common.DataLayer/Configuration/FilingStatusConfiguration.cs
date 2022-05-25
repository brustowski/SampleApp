using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities;

namespace FilingPortal.Parts.Common.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rail Tables entity type configuration
    /// </summary>
    internal class FilingStatusConfiguration : EntityTypeConfiguration<HeaderFilingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingStatusConfiguration"/> class.
        /// </summary>
        public FilingStatusConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="FilingStatusConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FilingStatusConfiguration(string schema)
        {
            ToTable("FilingStatus", schema);

            Property(x => x.Name).HasColumnType("varchar").HasMaxLength(20);
        }
    }
}
