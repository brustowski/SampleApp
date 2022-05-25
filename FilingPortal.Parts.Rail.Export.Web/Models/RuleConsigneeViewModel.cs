using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Rail.Export.Web.Models
{
    /// <summary>
    /// Defines the Consignee Rule View model
    /// </summary>
    public class RuleConsigneeViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
    }
}