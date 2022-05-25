using FilingPortal.Domain.Entities.Truck;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Truck
{
    /// <summary>
    /// Class for Truck Inbound View Configuration
    /// </summary>
    public class TruckInboundActionsConfig : PageConfiguration<TruckInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundActionsConfig"/> class.
        /// </summary>
        public TruckInboundActionsConfig()
        {
            PageName = PageConfigNames.TruckInboundActions;
        }

        /// <summary>
        /// Configures actions for Truck Inbound View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<TruckInboundDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<TruckInboundSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<TruckInboundEditRule>();
        }
    }
}