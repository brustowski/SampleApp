using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundRecordsConfiguration : EntityTypeConfiguration<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        public InboundRecordsConfiguration()
            : this("us_exp_rail")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundRecordsConfiguration(string schema)
        {
            ToTable("inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.Exporter).IsRequired().HasMaxLength(12);
            Property(x => x.Importer).IsRequired().HasMaxLength(12);
            Property(x => x.TariffType).IsRequired().HasMaxLength(3);
            Property(x => x.Tariff).IsRequired().HasMaxLength(35);
            Property(x => x.MasterBill).IsRequired().HasMaxLength(20); ;
            Property(x => x.LoadPort).HasMaxLength(4);
            Property(x => x.ExportPort).HasMaxLength(4);
            Property(x => x.GoodsDescription).IsRequired().HasMaxLength(512);
            Property(x => x.GrossWeightUOM).IsRequired().HasMaxLength(3);
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
