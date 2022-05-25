using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Parsed Data entity type configuration
    /// </summary>
    public class InboundParsedDataConfiguration : EntityTypeConfiguration<InboundParsedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundParsedDataConfiguration"/> class.
        /// </summary>
        public InboundParsedDataConfiguration()
            : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundParsedDataConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundParsedDataConfiguration(string schema)
        {
            ToTable("inbound_parsed_data", schema);
            HasKey(x => x.Id);

            Property(x => x.EntryNumber).HasMaxLength(7);
            Property(x => x.CheckDigit).HasMaxLength(1);

            HasRequired(x => x.InboundRecord);
        }
    }
}
