namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides DB configuration for the <see cref="PipelineDefValueReadModel"/>
    /// </summary>
    class PipelineDefValuesReadModelConfiguration : EntityTypeConfiguration<PipelineDefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public PipelineDefValuesReadModelConfiguration() : this("dbo")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_imp_pipeline_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}