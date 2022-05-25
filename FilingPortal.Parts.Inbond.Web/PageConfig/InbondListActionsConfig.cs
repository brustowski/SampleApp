using System.Collections.Generic;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Inbond.Web.PageConfig
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class InbondListActionsConfig : PageConfiguration<List<InboundReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondListActionsConfig"/> class.
        /// </summary>
        public InbondListActionsConfig()
        {
            PageName = PageConfigNames.InbondListActions;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<InbondListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<InbondListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<InbondListViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<InbondListDeleteRule>();
        }
    }
}