using FilingPortal.Domain.Entities;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Class for Pipeline Rule Record Configuration
    /// </summary>
    public class PipelineRuleActionsConfig : PageConfiguration<IRuleEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleActionsConfig"/> class.
        /// </summary>
        public PipelineRuleActionsConfig()
        {
            PageName = PageConfigNames.PipelineRuleConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<PipelineRuleEditRule>();
            AddAction("Copy").AvailabilityRulesFrom<PipelineRuleEditRule>();
            AddAction("Edit").AvailabilityRulesFrom<PipelineRuleEditRule>();
            AddAction("Delete").AvailabilityRulesFrom<PipelineRuleDeleteRule>();
        }
    }
}