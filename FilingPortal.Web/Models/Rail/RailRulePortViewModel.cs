using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Describes Port Rail Rule
    /// </summary>
    public class RailRulePortViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Origin
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// Gets or sets Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets FIRMs Code
        /// </summary>
        public string FIRMsCode { get; set; }
        /// <summary>
        /// Gets or sets Export
        /// </summary>
        public string Export { get; set; }
    }
}