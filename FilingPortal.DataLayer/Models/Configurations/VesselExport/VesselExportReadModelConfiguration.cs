using FilingPortal.Domain.Entities.VesselExport;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides the DB configuration for the Vessel Export Read Model
    /// </summary>
    public class VesselExportReadModelConfiguration : EntityTypeConfiguration<VesselExportReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportReadModelConfiguration"/> class.
        /// </summary>
        public VesselExportReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportReadModelConfiguration(string schema)
        {
            ToTable("v_exp_vessel_inbound_grid", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.FilingStatusTitle).HasMaxLength(20);
            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.MappingStatusTitle).HasMaxLength(20);
            Property(x => x.MappingStatus).HasColumnType("int");

            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
