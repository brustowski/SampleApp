using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.VesselExport
{
    public class VesselExportDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding VesselExportFilingHeader
        /// </summary>
        public virtual VesselExportFilingHeader FilingHeader { get; set; }
        /// <summary>
        /// Gets or sets the link to corresponding inbound record
        /// </summary>
        public virtual VesselExportRecord InboundRecord { get; set; }
    }

}
