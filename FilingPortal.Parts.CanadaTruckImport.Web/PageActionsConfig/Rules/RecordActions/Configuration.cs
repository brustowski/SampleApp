using FilingPortal.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.CanadaTruckImport.Web.PageActionsConfig.Rules.RecordActions
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
            PageName = PageConfigNames.RulesRecordActions;
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