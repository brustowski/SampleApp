using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides DefValues Manual Read Model entity type configuration
    /// </summary>
    public class DefValueReadModelConfiguration : EntityTypeConfiguration<DefValueReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValueReadModelConfiguration"/> class.
        /// </summary>
        public DefValueReadModelConfiguration() : this("zones_entry")
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValueReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema <see cref="string"/></param>
        public DefValueReadModelConfiguration(string schema)
        {
            ToTable("v_form_configuration", schema);
        }
    }
}