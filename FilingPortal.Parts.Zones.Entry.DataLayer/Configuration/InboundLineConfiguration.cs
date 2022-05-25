using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundLineConfiguration : EntityTypeConfiguration<InboundLine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundLineConfiguration"/> class.
        /// </summary>
        public InboundLineConfiguration()
            : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundLineConfiguration(string schema)
        {
            ToTable("inbound_lines", schema);
            HasKey(x => x.Id);

            Property(x => x.TransactionRelated).HasMaxLength(1);

            HasRequired(x => x.InboundRecord)
                .WithMany(x => x.InboundLines)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete();
        }
    }
}
