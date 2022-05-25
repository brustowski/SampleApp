using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class for Vessel Rule Record Configuration
    /// </summary>
    public class VesselRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleActionsConfig"/> class.
        /// </summary>
        public VesselRuleActionsConfig()
        {
            PageName = PageConfigNames.VesselRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<VesselRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<VesselRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<VesselRuleDeleteRule>();
        }
    }
}