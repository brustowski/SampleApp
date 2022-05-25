using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Configuration
{
    /// <summary>
    /// Class for Configuration Page Actions Configuration
    /// </summary>
    public class ConfigurationPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationPageActionsConfig"/> class.
        /// </summary>
        public ConfigurationPageActionsConfig()
        {
            PageName = PageConfigNames.ConfigurationPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<ConfigurationPageAddRule>();
        }
    }
}