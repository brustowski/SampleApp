using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig
{
    /// <summary>
    /// Class for Inbond View Configuration
    /// </summary>
    public class InbondActionsConfig : PageConfiguration<InboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondActionsConfig"/> class.
        /// </summary>
        public InbondActionsConfig()
        {
            PageName = PageConfigNames.InbondActions;
        }

        /// <summary>
        /// Configures actions for Inbond View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<InbondDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<InbondSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<InbondEditRule>();
        }
    }
}