using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckExportDefValueReadModel"/>
    /// </summary>
    internal class TruckExportDefValuesReadModelConfiguration : EntityTypeConfiguration<TruckExportDefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public TruckExportDefValuesReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public TruckExportDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_exp_truck_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}