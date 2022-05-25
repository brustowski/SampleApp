using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Rail
{
    public class RailDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets whether this model is a manifest
        /// </summary>
        public byte IsManifest { get; set; }

        /// <summary>
        /// Gets or sets the link to corresponding RailFilingHeader
        /// </summary>
        public virtual RailFilingHeader RailFilingHeader { get; set; }

        /// <summary>
        /// Gets or sets the link to corresponding inbound record
        /// </summary>
        public virtual RailBdParsed RailInbound { get; set; }
    }

}
