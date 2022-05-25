using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents the Inbound Document
    /// </summary>
    public class Document : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the corresponding Inbound Record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }
        /// <summary>
        /// Gets or sets the link to corresponding FilingHeader
        /// </summary>
        public virtual FilingHeader FilingHeader { get; set; }
    }
}
