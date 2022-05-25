namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Pipeline Sections Configuration
    /// </summary>
    internal class PipelineSectionsConfiguration : EntityTypeConfiguration<PipelineSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineSectionsConfiguration"/> class.
        /// </summary>
        public PipelineSectionsConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineSectionsConfiguration(string schema)
        {
            ToTable("imp_pipeline_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();

            HasIndex(x => x.Name).IsUnique().HasName("Idx__name");
            HasIndex(x => x.ParentId).HasName("Idx__parent_id");

            HasOptional(x => x.Parent)
                .WithMany(x => x.Descendants)
                .HasForeignKey(x => x.ParentId);

        }
    }
}
