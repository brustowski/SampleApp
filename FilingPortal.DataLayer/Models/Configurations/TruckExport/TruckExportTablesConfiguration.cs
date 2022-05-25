using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    /// <summary>
    /// Provides Truck Tables entity type configuration
    /// </summary>
    internal class TruckExportTablesConfiguration : EntityTypeConfiguration<TruckExportTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportTablesConfiguration"/> class.
        /// </summary>
        public TruckExportTablesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportTablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportTablesConfiguration(string schema)
        {
            ToTable("v_exp_truck_field_configuration", schema);
        }
    }
}
