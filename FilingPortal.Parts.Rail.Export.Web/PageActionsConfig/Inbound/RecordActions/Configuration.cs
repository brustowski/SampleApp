using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Rail.Export.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Actions configuration for single inbound record
    /// </summary>
    public class Configuration : PageConfiguration<InboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.InboundActions;
        }

        /// <summary>
        /// Configures actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<DeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<SelectRule>();
        }
    }
}