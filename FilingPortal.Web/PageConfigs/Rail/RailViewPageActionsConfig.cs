using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Represents the Vessel View Page Actions Configuration
    /// </summary>
    public class RailViewPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailViewPageActionsConfig"/> class.
        /// </summary>
        public RailViewPageActionsConfig()
        {
            PageName = PageConfigNames.RailViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<InboundRecordListAddNewRule>();
        }
    }
}