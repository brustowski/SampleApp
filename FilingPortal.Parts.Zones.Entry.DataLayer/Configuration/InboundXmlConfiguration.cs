using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound XML configuration
    /// </summary>
    public class InboundXmlConfiguration : EntityTypeConfiguration<InboundXml>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundXmlConfiguration"/> class.
        /// </summary>
        public InboundXmlConfiguration()
            : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundXmlConfiguration(string schema)
        {
            ToTable("inbound_xmls", schema);

            Property(x => x.FileName).IsRequired();
            Property(x => x.Content).IsRequired();
            Property(x => x.ValidationResult).HasMaxLength(1000);

            HasKey(x => x.Id);
        }
    }
}
