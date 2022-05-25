using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Defines the <see cref="AppDocument"/> entity to DB mapping configuration 
    /// </summary>
    internal class AppDocumentConfiguration : EntityTypeConfiguration<AppDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppAdminConfiguration"/> class.
        /// </summary>
        public AppDocumentConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDocumentConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppDocumentConfiguration(string schema)
        {
            ToTable("app_document", schema);

            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.FileContent).HasColumnName("file_content");
        }
    }
}
