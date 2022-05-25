using FilingPortal.Domain.Entities.VesselExport;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides DB configuration for the <see cref="VesselExportDefValueReadModel"/>
    /// </summary>
    internal class VesselExportDefValuesReadModelConfiguration : EntityTypeConfiguration<VesselExportDefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public VesselExportDefValuesReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public VesselExportDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_exp_vessel_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}