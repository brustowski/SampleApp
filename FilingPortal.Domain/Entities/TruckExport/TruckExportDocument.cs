using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.TruckExport
{
    /// <summary>
    /// Represents truck export document
    /// </summary>
    public class TruckExportDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding TruckExportFilingHeader
        /// </summary>
        public virtual TruckExportFilingHeader FilingHeader { get; set; }

        /// <summary>
        /// Gets or sets the link to corresponding inbound record
        /// </summary>
        public virtual TruckExportRecord InboundRecord { get; set; }
    }

}
