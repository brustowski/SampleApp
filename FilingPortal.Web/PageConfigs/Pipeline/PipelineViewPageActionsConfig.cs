using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Represents the Pipeline View Page Actions Configuration
    /// </summary>
    public class PipelineViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineViewPageActionsConfig"/> class.
        /// </summary>
        public PipelineViewPageActionsConfig()
        {
            PageName = PageConfigNames.PipelineViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Import").AvailabilityRulesFrom<PipelineViewPageImportRule>(); 
            AddAction("Template").AvailabilityRulesFrom<PipelineViewPageImportTemplateRule>();
        }
    }
}