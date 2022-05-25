using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records Read Model entity type configuration
    /// </summary>
    public class InboundReadModelRecordsConfiguration : PropertiesConfigurator<InboundRecordReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        public InboundReadModelRecordsConfiguration()
            : this("recon")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundReadModelRecordsConfiguration(string schema)
        {
            ToTable("v_inbound_grid", schema);
        }
    }
}
