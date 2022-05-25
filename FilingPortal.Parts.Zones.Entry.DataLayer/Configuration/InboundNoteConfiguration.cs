using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Notes entity type configuration
    /// </summary>
    public class InboundNoteConfiguration : EntityTypeConfiguration<InboundNote>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundNoteConfiguration"/> class.
        /// </summary>
        public InboundNoteConfiguration()
            : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundNoteConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundNoteConfiguration(string schema)
        {
            ToTable("inbound_notes", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).HasMaxLength(35);
            Property(x => x.Message).HasMaxLength(350);

            HasRequired(x => x.InboundRecord)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete();
        }
    }
}
