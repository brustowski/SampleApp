using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines the Vessel Import Document model
    /// </summary>
    public class VesselImportDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding VesselImportFilingHeader
        /// </summary>
        public virtual VesselImportFilingHeader FilingHeader { get; set; }
        /// <summary>
        /// Gets or sets the link to corresponding inbound record
        /// </summary>
        public virtual VesselImportRecord InboundRecord { get; set; }
    }
}
