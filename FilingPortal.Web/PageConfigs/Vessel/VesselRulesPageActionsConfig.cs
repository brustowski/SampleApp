using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class for Vessel Rules Page Actions Configuration
    /// </summary>
    public class VesselRulesPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulesPageActionsConfig"/> class.
        /// </summary>
        public VesselRulesPageActionsConfig()
        {
            PageName = PageConfigNames.VesselRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselRulesPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<VesselRulesPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<VesselRulesPageAddRule>();
        }
    }
}