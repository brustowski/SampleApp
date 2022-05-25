using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class for Vessel Rule Record Configuration
    /// </summary>
    public class VesselExportRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRuleActionsConfig"/> class.
        /// </summary>
        public VesselExportRuleActionsConfig()
        {
            PageName = PageConfigNames.VesselExportRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<VesselExportRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<VesselExportRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<VesselExportRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<VesselExportRuleDeleteRule>();
        }
    }
}