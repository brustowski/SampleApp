using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    /// <summary>
    /// Class for Truck Export Rules Page Actions Configuration
    /// </summary>
    public class TruckExportRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRulesPageActionsConfig"/> class.
        /// </summary>
        public TruckExportRulesPageActionsConfig()
        {
            PageName = PageConfigNames.TruckExportRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<TruckExportRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<TruckExportRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<TruckExportRulesPageAddRule>();
        }
    }
}