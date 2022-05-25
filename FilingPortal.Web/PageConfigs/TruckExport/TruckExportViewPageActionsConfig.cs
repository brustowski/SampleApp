using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    /// <summary>
    /// Represents the Truck Export View Page Actions Configuration
    /// </summary>
    public class TruckExportViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportViewPageActionsConfig"/> class.
        /// </summary>
        public TruckExportViewPageActionsConfig()
        {
            PageName = PageConfigNames.TruckExportViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Import").AvailabilityRulesFrom<TruckExportViewPageImportRule>();
            AddAction("Template").AvailabilityRulesFrom<TruckExportViewPageImportTemplateRule>();
        }
    }
}