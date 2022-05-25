using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Tables entity type configuration
    /// </summary>
    internal class VesselImportTablesConfiguration : EntityTypeConfiguration<VesselImportTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportTablesConfiguration"/> class.
        /// </summary>
        public VesselImportTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportTablesConfiguration(string schema)
        {
            ToTable("v_imp_vessel_field_configuration", schema);
        }
    }
}
