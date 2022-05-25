using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundRecordConfiguration : EntityTypeConfiguration<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        public InboundRecordConfiguration()
            : this("zones_ftz214")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundRecordConfiguration(string schema)
        {
            ToTable("inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
