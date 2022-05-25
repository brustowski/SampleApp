using System.Collections.Generic;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Service building configuration of Inbound Record fields
    /// </summary>
    public class InboundRecordFieldConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFieldConfiguration"/> class
        /// </summary>
        public InboundRecordFieldConfiguration()
        {
            AdditionalParameters = new List<InboundRecordField>();
            CommonData = new List<InboundRecordFieldSection>();
            Documents = new List<InboundRecordDocumentViewModel>();
        }

        /// <summary>
        /// Gets or sets the fields for Additional Parameters section
        /// </summary>
        public IEnumerable<BaseInboundRecordField> AdditionalParameters { get; set; }

        /// <summary>
        /// Gets or sets the fields for Common Data section
        /// </summary>
        public List<InboundRecordFieldSection> CommonData { get; set; }

        /// <summary>
        /// Gets or sets the list of documents
        /// </summary>
        public List<InboundRecordDocumentViewModel> Documents { get; set; }
    }

}