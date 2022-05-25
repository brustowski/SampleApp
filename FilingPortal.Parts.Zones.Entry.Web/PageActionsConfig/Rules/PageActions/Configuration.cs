using FilingPortal.Parts.Zones.Entry.Web.Configs;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Zones.Entry.Web.PageActionsConfig.Rules.PageActions
{
    /// <summary>
    /// Class for Rules Page Actions Configuration
    /// </summary>
    public class Configuration : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.RulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<RulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<RulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<RulesPageAddRule>();
        }
    }
}