namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Rail Document entity type configuration
    /// </summary>
    public class RailDocumentConfiguration : EntityTypeConfiguration<RailDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDocumentConfiguration"/> class.
        /// </summary>
        public RailDocumentConfiguration() : this("dbo")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDocumentConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public RailDocumentConfiguration(string schema)
        {
            ToTable("imp_rail_document", schema);

            Property(x => x.FileName).HasColumnName(@"filename").HasMaxLength(255);
            Property(x => x.Extension).HasColumnName(@"file_extension").HasMaxLength(128);
            Property(x => x.Description).HasColumnName(@"file_desc").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName(@"file_content").HasColumnType("varbinary(max)").IsOptional();
            Property(x => x.IsManifest).HasColumnType("tinyint").IsRequired();
            Property(x => x.CreatedDate).HasColumnType("datetime").IsOptional();
            Property(x => x.DocumentType).HasMaxLength(120);

            HasOptional(a => a.RailFilingHeader).WithMany(b => b.RailDocuments).HasForeignKey(c => c.FilingHeaderId).WillCascadeOnDelete(false);
            HasOptional(x => x.RailInbound).WithMany(x => x.Documents).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(false);
        }
    }
}
