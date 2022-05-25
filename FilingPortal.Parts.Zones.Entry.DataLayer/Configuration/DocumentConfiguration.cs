using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Document type Configuration
    /// </summary>
    public class DocumentConfiguration : EntityTypeConfiguration<Document>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentConfiguration"/> class.
        /// </summary>
        public DocumentConfiguration() : this("zones_entry") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public DocumentConfiguration(string schema)
        {
            ToTable("document", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.Extension).IsRequired();
            Property(x => x.Description).HasMaxLength(1000);
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(x=>x.FilingHeader)
                .WithMany(x=>x.Documents)
                .HasForeignKey(x=>x.FilingHeaderId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.InboundRecord)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete(true);
        }
    }

}
