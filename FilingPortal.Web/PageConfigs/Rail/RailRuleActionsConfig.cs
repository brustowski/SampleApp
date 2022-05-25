using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class for Rail Rule records Configuration
    /// </summary>
    public class RailRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailRuleActionsConfig"/> class.
        /// </summary>
        public RailRuleActionsConfig()
        {
            PageName = PageConfigNames.RailRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<RailRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<RailRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<RailRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<RailRuleDeleteRule>();
        }
    }
}