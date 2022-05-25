using FilingPortal.Domain.Entities.Truck;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckTable"/>
    /// </summary>
    internal class TruckTablesConfiguration : EntityTypeConfiguration<TruckTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckTablesConfiguration"/> class.
        /// </summary>
        public TruckTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckTablesConfiguration(string schema)
        {
            ToTable("v_imp_truck_field_configuration", schema);
        }
    }
}
