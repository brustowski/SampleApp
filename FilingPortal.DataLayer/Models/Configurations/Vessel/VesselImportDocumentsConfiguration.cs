using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Document type Configuration
    /// </summary>
    public class VesselImportDocumentsConfiguration : EntityTypeConfiguration<VesselImportDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDocumentsConfiguration"/> class.
        /// </summary>
        public VesselImportDocumentsConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDocumentsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportDocumentsConfiguration(string schema)
        {
            ToTable("imp_vessel_document", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.Extension).HasColumnName("file_extension").IsRequired();
            Property(x => x.Description).HasColumnName("file_description").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName("file_content");
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(a => a.FilingHeader).WithMany(b => b.Documents).HasForeignKey(c => c.FilingHeaderId).WillCascadeOnDelete(false);
            HasOptional(x => x.InboundRecord).WithMany(x => x.Documents).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(false);

            HasIndex(x => x.FilingHeaderId).HasName("Idx__filing_header_id");
            HasIndex(x => x.InboundRecordId).HasName("Idx__inbound_record_id");
        }
    }

}
