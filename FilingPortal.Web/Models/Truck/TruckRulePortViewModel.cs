using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Truck
{
    /// <summary>
    /// Defines the Truck Rule Port View model
    /// </summary>
    public class TruckRulePortViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Arrival Port
        /// </summary>
        public string ArrivalPort { get; set; }

        /// <summary>
        /// Gets or sets the FIRMsCode
        /// </summary>
        public string FIRMsCode { get; set; }
    }
}