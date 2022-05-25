using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Facility View Model
    /// </summary>
    public class PipelineRuleFacilityViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Facility
        /// </summary>
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sets the Destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the Destination State
        /// </summary>
        public string DestinationState { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FIRMsCode { get; set; }

        /// <summary>
        /// Gets or sets the Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the Pipeline
        /// </summary>
        public string Pipeline { get; set; }
    }
}
