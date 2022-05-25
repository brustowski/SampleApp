namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using System.Data.Entity.ModelConfiguration;
    using FilingPortal.Domain.Entities.Pipeline;

    /// <summary>
    /// Provides Truck Rule Port entity type configuration
    /// </summary>
    internal class PipelineRulePriceConfiguration : EntityTypeConfiguration<PipelineRulePrice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulePriceConfiguration"/> class.
        /// </summary>
        public PipelineRulePriceConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulePriceConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineRulePriceConfiguration(string schema)
        {
            ToTable("imp_pipeline_rule_price", schema);

            HasKey(x => x.Id);

            Property(x => x.Pricing).HasPrecision(28,15);

            HasRequired(x => x.Importer)
                .WithMany()
                .HasForeignKey(x => x.ImporterId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.CrudeType)
                .WithMany(x => x.PipelinePriceRules)
                .HasForeignKey(x => x.CrudeTypeId).WillCascadeOnDelete(false);

            HasIndex(x => x.CrudeTypeId).HasName("Idx__crude_type_id");
            HasIndex(x => x.ImporterId).HasName("Idx__importer_id");
        }
    }
}
