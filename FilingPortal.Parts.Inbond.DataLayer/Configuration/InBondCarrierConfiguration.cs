using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides the In-Bond Carrier entity type configuration
    /// </summary>
    public class InBondCarrierConfiguration : EntityTypeConfiguration<InBondCarrier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InBondCarrierConfiguration"/> class.
        /// </summary>
        public InBondCarrierConfiguration(): this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InBondCarrierConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InBondCarrierConfiguration(string schema)
        {
            ToTable("handbook_carrier", schema);
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("code").HasMaxLength(12);
            Property(x => x.Name).HasMaxLength(100).IsRequired();
        }
    }
}
