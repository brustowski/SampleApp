using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides Foreign Port entity type configuration
    /// </summary>
    internal class ForeignPortsConfiguration : EntityTypeConfiguration<ForeignPort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignPortsConfiguration"/> class.
        /// </summary>
        public ForeignPortsConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignPortsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public ForeignPortsConfiguration(string schema)
        {
            ToTable("CW_Foreign_Ports", schema);
            HasKey(x => x.Id);
            Property(x => x.PortCode).IsRequired().HasMaxLength(10);
            Property(x => x.UNLOCO).IsRequired().HasMaxLength(5);
            Property(x => x.Country).HasMaxLength(2);

            HasIndex(x => x.PortCode).IsUnique();
        }
    }
}
