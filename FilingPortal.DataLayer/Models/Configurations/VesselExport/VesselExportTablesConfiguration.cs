using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.VesselExport;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides Vessel Tables entity type configuration
    /// </summary>
    internal class VesselExportTablesConfiguration : EntityTypeConfiguration<VesselExportTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportTablesConfiguration"/> class.
        /// </summary>
        public VesselExportTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportTablesConfiguration(string schema)
        {
            ToTable("v_exp_vessel_field_configuration", schema);
        }
    }
}
