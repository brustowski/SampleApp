using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Defines the Inbond Document model
    /// </summary>
    public class Document : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding InbondFilingHeader
        /// </summary>
        public virtual FilingHeader FilingHeader { get; set; }
    }
}
