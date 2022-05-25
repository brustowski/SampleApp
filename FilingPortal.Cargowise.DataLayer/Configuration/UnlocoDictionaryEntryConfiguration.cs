using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides UNLOCO dictionary entry type configuration
    /// </summary>
    internal class UnlocoDictionaryEntryConfiguration : EntityTypeConfiguration<UnlocoDictionaryEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnlocoDictionaryEntryConfiguration"/> class.
        /// </summary>
        public UnlocoDictionaryEntryConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnlocoDictionaryEntryConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public UnlocoDictionaryEntryConfiguration(string schema)
        {
            ToTable("cw_unloco_dictionary", schema);
            HasKey(x => x.Id);

            Property(x => x.Unloco).IsRequired().HasMaxLength(5);
            Property(x => x.CountryCode).HasMaxLength(2);
            
            HasIndex(x => new {x.Unloco}).IsUnique();
        }
    }
}
