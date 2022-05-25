using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides DefValues Manual Read Model entity type configuration
    /// </summary>
    public class DefValuesReadModelConfiguration : EntityTypeConfiguration<DefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesReadModelConfiguration"/> class.
        /// </summary>
        public DefValuesReadModelConfiguration() : this("us_exp_rail")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema <see cref="string"/></param>
        public DefValuesReadModelConfiguration(string schema)
        {
            ToTable("v_form_configuration", schema);
        }
    }
}