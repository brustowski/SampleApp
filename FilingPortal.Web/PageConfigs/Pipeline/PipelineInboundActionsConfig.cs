using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Class for Pipeline Inbound View Configuration
    /// </summary>
    public class PipelineInboundActionsConfig : PageConfiguration<PipelineInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundActionsConfig"/> class.
        /// </summary>
        public PipelineInboundActionsConfig()
        {
            PageName = PageConfigNames.PipelineInboundActions;
        }

        /// <summary>
        /// Configures actions for Pipeline Inbound View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<PipelineInboundDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<PipelineInboundSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<PipelineInboundEditRule>();
        }
    }
}