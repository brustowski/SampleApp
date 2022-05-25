using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Carrier entity configuration
    /// </summary>
    internal class CarrierConfiguration : EntityTypeConfiguration<Carrier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierConfiguration"/> class.
        /// </summary>
        public CarrierConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarrierConfiguration"/> class.
        /// </summary>
        /// <param name="schema">Database schema</param>
        public CarrierConfiguration(string schema)
        {
            ToTable("handbook_carrier", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("code").HasMaxLength(4);
            Property(x => x.Name).IsRequired().HasMaxLength(255);
            Property(x => x.TransportMode).HasMaxLength(3);
        }
    }
}
