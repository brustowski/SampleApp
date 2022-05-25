using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    /// <summary>
    /// Provides Model Configuration for <see cref="PipelineFilingData" />
    /// </summary>
    public class PipelineFilingDataConfiguration : EntityTypeConfiguration<PipelineFilingData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingDataConfiguration"/> class.
        /// </summary>
        public PipelineFilingDataConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingDataConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineFilingDataConfiguration(string schema)
        {
            ToTable("v_imp_pipeline_review_grid", schema);

            Property(x => x.Batch).HasMaxLength(20);
            Property(x => x.TicketNumber).HasMaxLength(20);
        }
    }
}
