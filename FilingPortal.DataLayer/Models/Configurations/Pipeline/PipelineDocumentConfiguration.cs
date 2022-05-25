namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides DB configuration for the <see cref="PipelineDocument"/>
    /// </summary>
    public class PipelineDocumentConfiguration : EntityTypeConfiguration<PipelineDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDocumentConfiguration"/> class.
        /// </summary>
        public PipelineDocumentConfiguration() : this("dbo")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDocumentConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineDocumentConfiguration(string schema)
        {
            ToTable("imp_pipeline_document", schema);

            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255);
            Property(x => x.Extension).HasColumnName(@"file_extension");
            Property(x => x.Description).HasColumnName(@"file_description").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName(@"file_content").HasColumnType("varbinary(max)").IsOptional();
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(a => a.PipelineFilingHeader)
                .WithMany(b => b.PipelineDocuments)
                .HasForeignKey(c => c.FilingHeaderId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.PipelineInbound)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete(false);

            HasIndex(x => x.FilingHeaderId).HasName("Idx__filing_header_id");
            HasIndex(x => x.InboundRecordId).HasName("Idx__inbound_record_id");
        }
    }
}
