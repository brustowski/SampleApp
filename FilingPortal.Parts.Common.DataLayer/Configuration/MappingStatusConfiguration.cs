using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities;

namespace FilingPortal.Parts.Common.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rail Tables entity type configuration
    /// </summary>
    internal class MappingStatusConfiguration : EntityTypeConfiguration<HeaderMappingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStatusConfiguration"/> class.
        /// </summary>
        public MappingStatusConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingStatusConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public MappingStatusConfiguration(string schema)
        {
            ToTable("MappingStatus", schema);

            Property(x => x.Name).HasColumnType("varchar").HasMaxLength(20);
        }
    }
}
