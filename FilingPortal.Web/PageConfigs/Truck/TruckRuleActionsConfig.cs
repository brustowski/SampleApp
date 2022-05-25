using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class for Truck Rule Record Configuration
    /// </summary>
    public class TruckRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleActionsConfig"/> class.
        /// </summary>
        public TruckRuleActionsConfig()
        {
            PageName = PageConfigNames.TruckRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<TruckRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<TruckRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<TruckRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<TruckRuleDeleteRule>();
        }
    }
}