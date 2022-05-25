using FilingPortal.Domain.Entities.Pipeline;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    /// <summary>
    /// Provides DB configuration for the <see cref="PipelineFilingHeader"/>
    /// </summary>
    public class PipelineFilingHeaderConfiguration : EntityTypeConfiguration<PipelineFilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingHeaderConfiguration"/> class.
        /// </summary>
        public PipelineFilingHeaderConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineFilingHeaderConfiguration(string schema)
        {
            ToTable("imp_pipeline_filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.FilingStatus).HasColumnType("int");

            HasMany(x => x.PipelineInbounds).WithMany(t => t.FilingHeaders).Map(m =>
            {
                m.ToTable("imp_pipeline_filing_detail", "dbo");
                m.MapLeftKey("filing_header_id");
                m.MapRightKey("inbound_id");
            });
        }
    }
}
