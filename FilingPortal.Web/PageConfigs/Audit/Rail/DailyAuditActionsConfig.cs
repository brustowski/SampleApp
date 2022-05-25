using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Audit.Rail
{
    /// <summary>
    /// Class for Rail Rule records Configuration
    /// </summary>
    public class DailyAuditActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditActionsConfig"/> class.
        /// </summary>
        public DailyAuditActionsConfig()
        {
            PageName = PageConfigNames.AuditRailDailyAuditRulesConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<DailyAuditDeleteRule>();
            AddAction("Copy").AvailabilityRulesFrom<DailyAuditDeleteRule>();
            AddAction("Edit").AvailabilityRulesFrom<DailyAuditDeleteRule>();
            AddAction("Delete").AvailabilityRulesFrom<DailyAuditDeleteRule>();
        }
    }
}