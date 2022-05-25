using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides Discharge Port entity type configuration
    /// </summary>
    class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public CountryConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public CountryConfiguration(string schema)
        {
            ToTable("Countries", schema);

            Property(x => x.Code).HasMaxLength(2);

            HasIndex(x => x.Name).IsUnique().HasName("Idx_Name");
        }
    }
}
