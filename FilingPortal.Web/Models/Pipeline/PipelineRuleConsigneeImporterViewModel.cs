using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Consignee-Importer View Model
    /// </summary>
    public class PipelineRuleConsigneeImporterViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Importer name
        /// </summary>
        public string ImporterFromTicket { get; set; }

        /// <summary>
        /// Cargowise importer code
        /// </summary>
        public string ImporterCode { get; set; }
    }
}
