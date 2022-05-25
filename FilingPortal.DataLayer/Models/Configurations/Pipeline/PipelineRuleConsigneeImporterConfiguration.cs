namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Pipeline Consignee-Importer Rule configuration
    /// </summary>
    internal class PipelineRuleConsigneeImporterConfiguration : EntityTypeConfiguration<PipelineRuleConsigneeImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleConsigneeImporterConfiguration"/> class.
        /// </summary>
        public PipelineRuleConsigneeImporterConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleConsigneeImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineRuleConsigneeImporterConfiguration(string schema)
        {
            ToTable("imp_pipeline_rule_consignee_importer", schema);

            HasKey(x => x.Id);

            HasIndex(x => x.ImporterFromTicket).HasName("Idx__ticket_importer").IsUnique();

            Property(x => x.ImporterFromTicket).HasColumnName("ticket_importer").IsRequired();
            Property(x => x.ImporterCode).IsRequired();
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
