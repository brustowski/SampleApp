using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.TruckExport
{
    /// <summary>
    /// Class for TruckExport Rule Record Configuration
    /// </summary>
    public class TruckExportRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleActionsConfig"/> class.
        /// </summary>
        public TruckExportRuleActionsConfig()
        {
            PageName = PageConfigNames.TruckExportRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<TruckExportRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<TruckExportRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<TruckExportRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<TruckExportRuleDeleteRule>();
        }
    }
}