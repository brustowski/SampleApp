using System.ComponentModel.DataAnnotations.Schema;
using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{

    public class RailFilingHeaderConfiguration : EntityTypeConfiguration<RailFilingHeader>
    {
        public RailFilingHeaderConfiguration() : this("dbo") { }

        public RailFilingHeaderConfiguration(string schema)
        {
            ToTable("imp_rail_filing_header", schema);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).HasColumnName("job_hyperlink").IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.GrossWeightSummary).HasColumnType("decimal").IsOptional().HasPrecision(18, 9).
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            Property(x => x.GrossWeightSummaryUnit).HasColumnType("nvarchar").HasMaxLength(2).
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasMany(t => t.RailBdParseds).WithMany(t => t.FilingHeaders).Map(m =>
            {
                m.ToTable("imp_rail_filing_detail", "dbo");
                m.MapLeftKey("filing_header_id");
                m.MapRightKey("inbound_id");
            });
        }
    }

}
