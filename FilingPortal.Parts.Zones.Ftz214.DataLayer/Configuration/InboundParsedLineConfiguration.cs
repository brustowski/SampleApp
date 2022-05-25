using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundParsedLineConfiguration : EntityTypeConfiguration<InboundParsedLine>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundParsedLineConfiguration"/> class.
        /// </summary>
        public InboundParsedLineConfiguration()
            : this("zones_ftz214")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundParsedLineConfiguration(string schema)
        {
            ToTable("inbound_parsed_line", schema);
            HasKey(x => x.Id);

            Property(x => x.SubItem).HasMaxLength(1);
            Property(x => x.Hts).HasMaxLength(10);
            Property(x => x.AdditionalHts).HasMaxLength(10);
            Property(x => x.Spi1).HasMaxLength(1);
            Property(x => x.Spi1Country).HasMaxLength(2);
            Property(x => x.Spi2).HasMaxLength(1);
            Property(x => x.Qty1uom).HasMaxLength(3);
            Property(x => x.Qty2uom).HasMaxLength(3);
            Property(x => x.CategoryNo).HasMaxLength(3);
            Property(x => x.Co).HasMaxLength(2);
            Property(x => x.Mid).HasMaxLength(15);
            Property(x => x.ZoneStatus).HasMaxLength(1);
            Property(x => x.MxCementLicenseNo).HasMaxLength(6);
            Property(x => x.PnDisclaimer).HasMaxLength(1);
            Property(x => x.ImporterAcctno).HasMaxLength(6);
            Property(x => x.ImporterAcct).HasMaxLength(6);
            Property(x => x.ImporterIrsno).HasMaxLength(12);
            Property(x => x.ProductName).HasMaxLength(35);
            Property(x => x.ProductCountry).HasMaxLength(2);
            Property(x => x.Description).IsMaxLength();
            Property(x => x.Remarks).IsMaxLength();

            HasRequired(x => x.InboundRecord)
                .WithMany(x => x.InboundParsedLines)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete(true);
        }
    }
}
