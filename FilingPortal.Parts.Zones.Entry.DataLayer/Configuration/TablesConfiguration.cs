using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Tables entity type configuration
    /// </summary>
    internal class TablesConfiguration : EntityTypeConfiguration<Tables>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesConfiguration"/> class.
        /// </summary>
        public TablesConfiguration() : this("zones_entry") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TablesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TablesConfiguration(string schema)
        {
            ToTable("v_field_configuration", schema);
        }
    }
}
