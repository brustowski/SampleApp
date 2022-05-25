using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Defines the Document model
    /// </summary>
    public class Document : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding FilingHeader
        /// </summary>
        public virtual FilingHeader FilingHeader { get; set; }
    }
}
