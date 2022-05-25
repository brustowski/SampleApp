using System.Data.Entity.ModelConfiguration;
using FilingPortal.DataLayer.Models.Configurations.Rail;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rail DefValues Manual Read Model entity type configuration
    /// </summary>
    public class DefValuesReadModelConfiguration : EntityTypeConfiguration<DefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesReadModelConfiguration"/> class.
        /// </summary>
        public DefValuesReadModelConfiguration() : this("canada_imp_truck")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema <see cref="string"/></param>
        public DefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_form_configuration", schema);
        }
    }
}