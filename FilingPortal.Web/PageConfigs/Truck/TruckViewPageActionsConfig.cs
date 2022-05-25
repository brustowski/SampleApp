using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Represents the Truck View Page Actions Configuration
    /// </summary>
    public class TruckViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckViewPageActionsConfig"/> class.
        /// </summary>
        public TruckViewPageActionsConfig()
        {
            PageName = PageConfigNames.TruckViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Import").AvailabilityRulesFrom<TruckViewPageImportRule>();
            AddAction("Template").AvailabilityRulesFrom<TruckViewPageImportTemplateRule>();
        }
    }
}