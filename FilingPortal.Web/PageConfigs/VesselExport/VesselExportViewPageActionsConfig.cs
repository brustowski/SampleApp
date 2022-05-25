using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Represents the Vessel View Page Actions Configuration
    /// </summary>
    public class VesselExportViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportViewPageActionsConfig"/> class.
        /// </summary>
        public VesselExportViewPageActionsConfig()
        {
            PageName = PageConfigNames.VesselExportViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselExportViewPageAddImportRecord>();
        }
    }
}