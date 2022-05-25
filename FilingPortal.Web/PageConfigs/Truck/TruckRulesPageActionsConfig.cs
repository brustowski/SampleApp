using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class for Truck Rules Page Actions Configuration
    /// </summary>
    public class TruckRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRulesPageActionsConfig"/> class.
        /// </summary>
        public TruckRulesPageActionsConfig()
        {
            PageName = PageConfigNames.TruckRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<TruckRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<TruckRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<TruckRulesPageAddRule>();
        }
    }
}