using FilingPortal.Domain.Entities.Pipeline;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    /// <summary>
    /// Defines the Pipeline Inbound DB mapping configuration
    /// </summary>
    public class PipelineInboundReadModelConfiguration : EntityTypeConfiguration<PipelineInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundConfiguration"/> class.
        /// </summary>
        public PipelineInboundReadModelConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema name</param>
        public PipelineInboundReadModelConfiguration(string schema)
        {
            ToTable("v_imp_pipeline_inbound_grid", schema);
            HasKey(x => x.Id);

            Property(x => x.Importer).HasColumnType("nvarchar").IsRequired().HasMaxLength(200);
            Property(x => x.Batch).HasMaxLength(20);
            Property(x => x.TicketNumber).HasMaxLength(20);
            Property(x => x.API).HasPrecision(18,4);

            Property(x => x.FilingHeaderId).IsOptional();
            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.MappingStatusTitle).HasMaxLength(20);
            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.FilingStatusTitle).HasMaxLength(20);
            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
