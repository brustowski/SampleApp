using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.TruckExport
{
    /// <summary>
    /// Defines the Export Truck Exporter-Consignee Rule View model
    /// </summary>
    public class TruckExportRuleExporterConsigneeViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the Exporter
        /// </summary>
        public string Exporter { get; set; }

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public string Address { get; set; }

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