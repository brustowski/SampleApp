namespace FilingPortal.DataLayer.Models.Configurations.Pipeline
{
    using FilingPortal.Domain.Entities.Pipeline;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Defines the Pipeline Inbound DB mapping configuration
    /// </summary>
    public class PipelineInboundConfiguration: EntityTypeConfiguration<PipelineInbound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundConfiguration"/> class.
        /// </summary>
        public PipelineInboundConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PipelineInboundConfiguration(string schema)
        {
            ToTable("imp_pipeline_inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.Importer).HasColumnType("nvarchar").IsRequired().HasMaxLength(200);
            Property(x => x.Batch).IsRequired().HasMaxLength(20);
            Property(x => x.TicketNumber).IsRequired().HasMaxLength(20);
            Property(x => x.API).HasColumnType("numeric").HasPrecision(18, 4);
            Property(x => x.EntryNumber).HasMaxLength(11);
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
