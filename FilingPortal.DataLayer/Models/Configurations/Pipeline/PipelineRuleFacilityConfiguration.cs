namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Rule Port entity type configuration
    /// </summary>
    internal class PipelineRuleFacilityConfiguration : EntityTypeConfiguration<PipelineRuleFacility>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleFacilityConfiguration"/> class.
        /// </summary>
        public PipelineRuleFacilityConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleFacilityConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineRuleFacilityConfiguration(string schema)
        {
            ToTable("imp_pipeline_rule_facility", schema);

            HasKey(x => x.Id);

            HasIndex(x => x.Facility).HasName("Idx__facility").IsUnique();

            Property(x => x.Facility).IsRequired();
            Property(x => x.Port).IsRequired();
            Property(x => x.FIRMsCode).HasColumnName(@"firms_code");
        }
    }
}
