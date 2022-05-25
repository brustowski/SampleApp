using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer.Configuration
{
    /// <summary>
    /// Provides Document type Configuration
    /// </summary>
    public class DocumentsConfiguration : EntityTypeConfiguration<Document>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsConfiguration"/> class.
        /// </summary>
        public DocumentsConfiguration() : this("isf") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public DocumentsConfiguration(string schema)
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
