using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class for Rail Rules Page Actions Configuration
    /// </summary>
    public class RailRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRulesPageActionsConfig"/> class.
        /// </summary>
        public RailRulesPageActionsConfig()
        {
            PageName = PageConfigNames.RailRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<RailRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<RailRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<RailRulesPageAddRule>();
        }
    }
}