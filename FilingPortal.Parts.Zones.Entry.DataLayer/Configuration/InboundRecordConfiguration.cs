using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
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
            : this("zones_entry")
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

            Property(x => x.EntryNo).HasMaxLength(11);
            Property(x => x.EntryPort).HasMaxLength(5);
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.Ein).HasMaxLength(15).IsRequired();

            HasOptional(x => x.ParsedData)
                .WithRequired(x => x.InboundRecord);

            HasOptional(x => x.Importer)
                .WithMany()
                .HasForeignKey(x => x.ImporterId)
                .WillCascadeOnDelete(false);
        }
    }
}
