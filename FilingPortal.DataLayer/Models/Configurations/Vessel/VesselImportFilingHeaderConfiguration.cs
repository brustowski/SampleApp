using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Filing Header type Configuration
    /// </summary>
    public class VesselImportFilingHeaderConfiguration : EntityTypeConfiguration<VesselImportFilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportFilingHeaderConfiguration"/> class.
        /// </summary>
        public VesselImportFilingHeaderConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportFilingHeaderConfiguration(string schema)
        {
            ToTable("imp_vessel_filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasColumnType("varchar").HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.MappingStatus).HasColumnType("int").IsOptional();
            Property(x => x.FilingStatus).HasColumnType("int").IsOptional();

            HasMany(t => t.VesselInbounds).WithMany(t => t.FilingHeaders)
                .Map(m =>
                {
                    m.ToTable("imp_vessel_filing_detail", "dbo");
                    m.MapLeftKey("filing_header_id");
                    m.MapRightKey("inbound_id");
                });
        }
    }
}
