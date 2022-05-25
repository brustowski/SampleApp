using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Admin.RecordActions
{
    /// <summary>
    /// Class for Rule Record Configuration
    /// </summary>
    public class Configuration : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.AdminRulesPageActions;
        }

        /// <summary>
        /// Configures actions for rule
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<RuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<RuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<RuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<RuleDeleteRule>();
        }
    }
}