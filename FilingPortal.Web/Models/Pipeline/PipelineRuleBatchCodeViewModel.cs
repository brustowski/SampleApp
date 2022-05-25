using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Batch Code View Model
    /// </summary>
    public class PipelineRuleBatchCodeViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Batch Code
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public string Product { get; set; }
    }
}
