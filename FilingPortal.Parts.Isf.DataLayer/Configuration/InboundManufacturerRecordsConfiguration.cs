using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundManufacturerRecordsConfiguration : EntityTypeConfiguration<InboundManufacturerRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundManufacturerRecordsConfiguration"/> class.
        /// </summary>
        public InboundManufacturerRecordsConfiguration()
            : this("isf")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundManufacturerRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundManufacturerRecordsConfiguration(string schema)
        {
            ToTable("inbound_manufacturers", schema);
            HasKey(x => x.Id);
            Property(x => x.InboundRecordId).IsRequired();
            Property(x => x.CountryOfOrigin).HasMaxLength(2);

            HasRequired(x => x.Inbound).WithMany(x => x.Manufacturers).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(true);
            HasOptional(x => x.Manufacturer).WithMany().HasForeignKey(x => x.ManufacturerId).WillCascadeOnDelete(false);
            HasOptional(x => x.ManufacturerAppAddress).WithMany().HasForeignKey(x => x.ManufacturerAppAddressId).WillCascadeOnDelete(false);
        }
    }
}
