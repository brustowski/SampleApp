namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Rule Port entity type configuration
    /// </summary>
    internal class PipelineRuleImporterConfiguration : EntityTypeConfiguration<PipelineRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleImporterConfiguration"/> class.
        /// </summary>
        public PipelineRuleImporterConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineRuleImporterConfiguration(string schema)
        {
            ToTable("imp_pipeline_rule_importer", schema);

            HasKey(x => x.Id);

            HasIndex(x => x.Importer).HasName("Idx__importer").IsUnique();

            Property(x => x.Importer).IsRequired();
        }
    }
}
