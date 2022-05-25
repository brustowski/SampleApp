namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides DB configuration for the <see cref="PipelineTable"/>
    /// </summary>
    internal class PipelineTablesConfiguration : EntityTypeConfiguration<PipelineTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTablesConfiguration"/> class.
        /// </summary>
        public PipelineTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public PipelineTablesConfiguration(string schema)
        {
            ToTable("v_imp_pipeline_field_configuration", schema);
           
        }
    }
}
