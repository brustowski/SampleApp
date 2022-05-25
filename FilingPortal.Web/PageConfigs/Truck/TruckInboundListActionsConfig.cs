using System.Collections.Generic;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class TruckInboundListActionsConfig : PageConfiguration<List<TruckInboundReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundListActionsConfig"/> class.
        /// </summary>
        public TruckInboundListActionsConfig()
        {
            PageName = PageConfigNames.TruckListInboundActions;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<TruckInboundListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<TruckInboundListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<TruckInboundListViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<TruckInboundListDeleteRule>();
        }
    }
}