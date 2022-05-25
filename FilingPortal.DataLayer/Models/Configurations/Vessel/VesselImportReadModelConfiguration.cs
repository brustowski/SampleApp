using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Vessel;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Read Model type Configuration
    /// </summary>
    public class VesselInboundReadModelConfiguration : EntityTypeConfiguration<VesselImportReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselInboundReadModelConfiguration"/> class.
        /// </summary>
        public VesselInboundReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselInboundReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselInboundReadModelConfiguration(string schema)
        {
            ToTable("v_imp_vessel_inbound_grid", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.MappingStatusTitle).HasMaxLength(20);
            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.FilingStatusTitle).HasMaxLength(20);
            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
