using System.Collections.Generic;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Pipeline
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class PipelineInboundListActionsConfig : PageConfiguration<List<PipelineInboundReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundListActionsConfig"/> class.
        /// </summary>
        public PipelineInboundListActionsConfig()
        {
            PageName = PageConfigNames.PipelineListInboundActions;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<PipelineInboundListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<PipelineInboundListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<PipelineInboundListViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<PipelineListDeleteRule>();
            AddAction("DocumentsUpload").AvailabilityRulesFrom<PipelineInboundListReviewFileRule>();
        }
    }
}