using System.Collections.Generic;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class InboundRecordListActionsConfig : PageConfiguration<List<RailInboundReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordListActionsConfig"/> class.
        /// </summary>
        public InboundRecordListActionsConfig()
        {
            PageName = PageConfigNames.InboundRecordListConfigName;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<InboundRecordListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<InboundRecordListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<InboundRecordListViewRule>();
            AddAction("SingleFiling").AvailabilityRulesFrom<InboundRecordListSingleFilingRule>();
            AddAction("Delete").AvailabilityRulesFrom<RailListDeleteRule>();
            AddAction("Restore").AvailabilityRulesFrom<RailListRestoreRule>();
        }
    }
}