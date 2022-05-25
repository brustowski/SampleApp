using FilingPortal.Domain.Entities.Truck;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckDefValueReadModel"/>
    /// </summary>
    internal class TruckDefValuesReadModelConfiguration : EntityTypeConfiguration<TruckDefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public TruckDefValuesReadModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public TruckDefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_imp_truck_form_configuration", schema);
            HasKey(x => x.Id);
        }
    }
}