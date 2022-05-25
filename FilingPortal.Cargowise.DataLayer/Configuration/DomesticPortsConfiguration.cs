using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides Domestic Port entity type configuration
    /// </summary>
    internal class DomesticPortsConfiguration : EntityTypeConfiguration<DomesticPort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPortsConfiguration"/> class.
        /// </summary>
        public DomesticPortsConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPortsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public DomesticPortsConfiguration(string schema)
        {
            ToTable("CW_Domestic_Ports", schema);
            HasKey(x => x.Id);
            Property(x => x.PortCode).IsRequired().HasMaxLength(10);
            Property(x => x.UNLOCO).IsRequired().HasMaxLength(5);
            Property(x => x.Country).HasMaxLength(2);

            HasIndex(x => new {x.PortCode, x.Country}).IsUnique();
        }
    }
}
