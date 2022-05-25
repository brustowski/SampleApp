using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Configuration
{
    /// <summary>
    /// Provides Action Configuration for DefValues record
    /// </summary>
    public class DefValuesActionsConfig : PageConfiguration<DefValuesViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesActionsConfig"/> class.
        /// </summary>
        public DefValuesActionsConfig()
        {
            PageName = PageConfigNames.DefValueActionsConfigName;
        }

        /// <summary>
        /// Configures actions for model
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<DefValuesEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<DefValuesEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<DefValuesEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<DefValuesDeleteRule>();
        }
    }
}