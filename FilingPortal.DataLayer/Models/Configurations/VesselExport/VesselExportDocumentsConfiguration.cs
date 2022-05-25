using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.VesselExport;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides Vessel Export Document type Configuration
    /// </summary>
    public class VesselExportDocumentsConfiguration : EntityTypeConfiguration<VesselExportDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDocumentsConfiguration"/> class.
        /// </summary>
        public VesselExportDocumentsConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDocumentsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportDocumentsConfiguration(string schema)
        {
            ToTable("exp_vessel_document", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.Extension).HasColumnName("file_extension").IsRequired();
            Property(x => x.Description).HasColumnName("file_description").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName("file_content");
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(a => a.FilingHeader).WithMany(b => b.Documents).HasForeignKey(c => c.FilingHeaderId).WillCascadeOnDelete(false);
            HasOptional(x => x.InboundRecord).WithMany(x => x.Documents).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(false);
        }
    }

}
