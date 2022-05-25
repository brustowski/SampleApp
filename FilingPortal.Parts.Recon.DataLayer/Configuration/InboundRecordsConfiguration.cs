using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundRecordsConfiguration : PropertiesConfigurator<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        public InboundRecordsConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundRecordsConfiguration(string schema)
        {
            ToTable("inbound", schema);

            ConfigureProperties();

            HasOptional(x=>x.FtaRecon).WithRequired(x => x.Inbound).WillCascadeOnDelete(true);
            HasOptional(x=>x.ValueRecon).WithRequired(x => x.Inbound).WillCascadeOnDelete(true);

            HasIndex(x => new {x.Filer, x.EntryNo, x.LineNumber7501}).HasName("Idx__filer_entry_line");
        }
    }
}
