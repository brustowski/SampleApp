namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides DB configuration for the <see cref="PipelineDefValue"/>
    /// </summary>
    internal class PipelineDefValuesConfiguration : EntityTypeConfiguration<PipelineDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesConfiguration"/> class.
        /// </summary>
        public PipelineDefValuesConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineDefValuesConfiguration(string schema)
        {
            ToTable("imp_pipeline_form_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Label).IsRequired();
            Property(x => x.ColumnName).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Section)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.SectionId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.DependsOn).WithMany().HasForeignKey(x => x.DependsOnId);
        }
    }
}
