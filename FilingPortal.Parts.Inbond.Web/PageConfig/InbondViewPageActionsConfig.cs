using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig
{
    /// <summary>
    /// Represents the Inbond View Page Actions Configuration
    /// </summary>
    public class InbondViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondViewPageActionsConfig"/> class.
        /// </summary>
        public InbondViewPageActionsConfig()
        {
            PageName = PageConfigNames.InbondViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Import").AvailabilityRulesFrom<InbondViewPageImportRule>();
            AddAction("Template").AvailabilityRulesFrom<InbondViewPageImportTemplateRule>();
        }
    }
}