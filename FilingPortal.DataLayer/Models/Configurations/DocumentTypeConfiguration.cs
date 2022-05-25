using FilingPortal.Domain.DocumentTypes.Entities;

namespace FilingPortal.DataLayer.Models.Configurations
{
    /// <summary>
    /// Provides Document Type entity type configuration
    /// </summary>
    public class DocumentTypeConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DocumentType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeConfiguration"/> class.
        /// </summary>
        public DocumentTypeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public DocumentTypeConfiguration(string schema)
        {
            ToTable("DocumentTypes", schema);
            Property(x => x.TypeName).HasColumnName("TypeName").HasColumnType("varchar").HasMaxLength(120).IsRequired().IsUnicode(false);
            Property(x => x.Description).HasColumnType("varchar").IsOptional().IsUnicode(false).HasMaxLength(1000);
        }
    }
}