using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    public class PipelineDocument : BaseDocumentWithContent
    {
        /// <summary>
        /// Gets or sets the link to corresponding PipelineFilingHeader
        /// </summary>
        public virtual PipelineFilingHeader PipelineFilingHeader { get; set; }
        /// <summary>
        /// Gets or sets the link to corresponding pipeline inbound record
        /// </summary>
        public virtual PipelineInbound PipelineInbound { get; set; }
    }

}
