using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Rail.Export.Web.Models
{
    /// <summary>
    /// Defines the Exporter-Consignee Rule View model
    /// </summary>
    public class RuleExporterConsigneeViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Exporter
        /// </summary>
        public string Exporter { get; set; }

        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets the contact name
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the transactions related
        /// </summary>
        public string TranRelated { get; set; }
    }
}