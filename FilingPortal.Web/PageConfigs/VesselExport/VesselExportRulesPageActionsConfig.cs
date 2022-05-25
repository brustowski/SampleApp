using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class for Vessel Export Rules Page Actions Configuration
    /// </summary>
    public class VesselExportRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRulesPageActionsConfig"/> class.
        /// </summary>
        public VesselExportRulesPageActionsConfig()
        {
            PageName = PageConfigNames.VesselExportRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselExportRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<VesselExportRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<VesselExportRulesPageAddRule>();
        }
    }
}