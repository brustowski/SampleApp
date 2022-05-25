namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Rule Port entity type configuration
    /// </summary>
    internal class PipelineRuleBatchCodeConfiguration : EntityTypeConfiguration<PipelineRuleBatchCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleBatchCodeConfiguration"/> class.
        /// </summary>
        public PipelineRuleBatchCodeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleBatchCodeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineRuleBatchCodeConfiguration(string schema)
        {
            ToTable("imp_pipeline_rule_batch_code", schema);

            HasKey(x => x.Id);

            HasIndex(x => x.BatchCode).HasName("Idx__batch_code").IsUnique();

            Property(x => x.BatchCode).IsRequired();
        }
    }
}
