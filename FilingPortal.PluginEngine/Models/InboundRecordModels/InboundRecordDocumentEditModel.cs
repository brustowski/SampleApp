using System.Web;

namespace FilingPortal.PluginEngine.Models.InboundRecordModels
{
    /// <summary>
    /// Class describing model of Inbound Record Document used for edit
    /// </summary>
    public class InboundRecordDocumentEditModel : InboundRecordDocumentViewModel
    {
        /// <summary>
        /// Gets or sets the data
        /// </summary>
        public HttpPostedFileBase Data { get; set; }
    }
}