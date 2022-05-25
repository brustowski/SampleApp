using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundReadModelConfiguration : EntityTypeConfiguration<InboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelConfiguration"/> class.
        /// </summary>
        public InboundReadModelConfiguration()
            : this("zones_ftz214")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundReadModelConfiguration(string schema)
        {
            ToTable("v_inbound_grid", schema);

            Property(x => x.JobStatus).HasColumnType("int");
        }
    }
}
