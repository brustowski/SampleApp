using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort
{
    /// <summary>
    /// Describes Port Rule View Model
    /// </summary>
    public class RulePortViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Port of Clearance
        /// </summary>
        public string PortOfClearance { get; set; }
        /// <summary>
        /// Gets or sets sub-location
        /// </summary>
        public string SubLocation { get; set; }
        /// <summary>
        /// Gets or sets First port of arrival
        /// </summary>
        public string FirstPortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets final destination
        /// </summary>
        public string FinalDestination { get; set; }
    }
}