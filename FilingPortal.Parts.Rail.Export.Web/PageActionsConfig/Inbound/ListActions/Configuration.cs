using System.Collections.Generic;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Rail.Export.Web.PageActionsConfig.Inbound.ListActions
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class Configuration : PageConfiguration<List<InboundReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.InboundListActions;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<CancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<ReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<ViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<DeleteRule>();
        }
    }
}