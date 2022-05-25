using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    /// <summary>
    /// Class for Truck Export View Configuration
    /// </summary>
    public class TruckExportActionsConfig : PageConfiguration<TruckExportReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportActionsConfig"/> class.
        /// </summary>
        public TruckExportActionsConfig()
        {
            PageName = PageConfigNames.TruckExportActions;
        }

        /// <summary>
        /// Configures actions for Truck Export View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<TruckExportDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<TruckExportSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<TruckExportEditRule>();
            AddAction("View").AvailabilityRulesFrom<TruckExportViewRule>();
        }
    }
}