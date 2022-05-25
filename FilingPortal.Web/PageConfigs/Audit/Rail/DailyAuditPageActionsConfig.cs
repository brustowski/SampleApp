using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Audit.Rail
{
    /// <summary>
    /// Actions for Rail daily audit rules page
    /// </summary>
    public class DailyAuditPageActionsConfig : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditPageActionsConfig"/> class.
        /// </summary>
        public DailyAuditPageActionsConfig()
        {
            PageName = PageConfigNames.AuditRailDailyAuditRulesPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<DailyAuditPageAddRule>();
            AddAction("Import").AvailabilityRulesFrom<DailyAuditPageAddRule>();
            AddAction("Template").AvailabilityRulesFrom<DailyAuditPageAddRule>();
        }
    }
}