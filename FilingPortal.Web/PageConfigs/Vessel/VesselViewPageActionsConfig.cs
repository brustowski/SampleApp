using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Represents the Vessel View Page Actions Configuration
    /// </summary>
    public class VesselViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselViewPageActionsConfig"/> class.
        /// </summary>
        public VesselViewPageActionsConfig()
        {
            PageName = PageConfigNames.VesselViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselViewPageAddImportRecord>();
        }
    }
}