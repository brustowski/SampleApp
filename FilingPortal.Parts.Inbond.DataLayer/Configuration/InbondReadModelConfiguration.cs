using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbond Records entity type configuration
    /// </summary>
    public class InbondReadModelConfiguration : EntityTypeConfiguration<InboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondReadModelConfiguration"/> class.
        /// </summary>
        public InbondReadModelConfiguration()
            : this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InbondReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InbondReadModelConfiguration(string schema)
        {
            ToTable("v_inbound_grid", schema);

            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.MappingStatus).HasColumnType("int");
        }
    }
}
