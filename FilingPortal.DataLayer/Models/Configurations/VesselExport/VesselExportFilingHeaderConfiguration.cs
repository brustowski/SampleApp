using FilingPortal.Domain.Entities.VesselExport;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides the DB configuration for the Vessel Export Filing Header
    /// </summary>
    public class VesselExportFilingHeaderConfiguration : EntityTypeConfiguration<VesselExportFilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportFilingHeaderConfiguration"/> class.
        /// </summary>
        public VesselExportFilingHeaderConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportFilingHeaderConfiguration(string schema)
        {
            ToTable("exp_vessel_filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.FilingStatus).HasColumnType("tinyint");
            Property(x => x.MappingStatus).HasColumnType("tinyint");

            HasMany(t => t.VesselExports).WithMany(t => t.FilingHeaders)
                .Map(m =>
                {
                    m.ToTable("exp_vessel_filing_detail", "dbo");
                    m.MapLeftKey("filing_header_id");
                    m.MapRightKey("inbound_id");
                });
        }
    }
}
