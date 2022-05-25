using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rail DefValues Manual Read Model entity type configuration
    /// </summary>
    public class DefValuesReadModelConfiguration : EntityTypeConfiguration<DefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesReadModelConfiguration"/> class.
        /// </summary>
        public DefValuesReadModelConfiguration() : this("inbond")
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