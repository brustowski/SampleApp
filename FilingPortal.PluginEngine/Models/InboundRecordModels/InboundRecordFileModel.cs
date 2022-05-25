using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Models.InboundRecordModels
{
    /// <summary>
    /// Class describing model for Inbound Record File Process
    /// </summary>
    public class InboundRecordFileModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFileModel"/> class
        /// </summary>
        public InboundRecordFileModel()
        {
            Parameters = new List<InboundRecordParameterModel>();
            Documents = new List<InboundRecordDocumentEditModel>();
        }

        /// <summary>
        /// Gets or sets the filing header identifier
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Parameters
        /// </summary>
        public IEnumerable<InboundRecordParameterModel> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the List of Documents
        /// </summary>
        public List<InboundRecordDocumentEditModel> Documents { get; set; }
    }
}