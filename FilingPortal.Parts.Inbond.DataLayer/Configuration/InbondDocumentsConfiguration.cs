using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Vessel Import Document type Configuration
    /// </summary>
    public class VesselImportDocumentsConfiguration : EntityTypeConfiguration<Document>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDocumentsConfiguration"/> class.
        /// </summary>
        public VesselImportDocumentsConfiguration() : this("inbond") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDocumentsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportDocumentsConfiguration(string schema)
        {
            ToTable("documents", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.Extension).IsRequired();
            Property(x => x.Description).HasMaxLength(1000);
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.FilingHeaderId).IsRequired();
        }
    }

}
