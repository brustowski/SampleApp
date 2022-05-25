using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Class for the Pipeline Rules Page Actions Configuration
    /// </summary>
    public class PipelineRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulesPageActionsConfig"/> class.
        /// </summary>
        public PipelineRulesPageActionsConfig()
        {
            PageName = PageConfigNames.PipelineRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<PipelineRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<PipelineRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<PipelineRulesPageAddRule>();
        }
    }
}