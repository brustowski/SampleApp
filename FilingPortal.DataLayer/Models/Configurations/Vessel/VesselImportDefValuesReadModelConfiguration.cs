using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides DB configuration for the <see cref="VesselImportDefValueReadModel"/>
    /// </summary>
    internal class VesselImportDefValuesReadModelConfiguration : EntityTypeConfiguration<VesselImportDefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public VesselImportDefValuesReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public VesselImportDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_imp_vessel_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}