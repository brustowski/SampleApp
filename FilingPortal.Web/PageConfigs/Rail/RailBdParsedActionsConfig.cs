using FilingPortal.Domain.Entities.Rail;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Rail
{
    /// <summary>
    /// Class for RailBdParsed View Configuration
    /// </summary>
    public class RailBdParsedActionsConfig : PageConfiguration<RailInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailBdParsedActionsConfig"/> class.
        /// </summary>
        public RailBdParsedActionsConfig()
        {
            PageName = PageConfigNames.SingleInboundRecordConfigName;
        }

        /// <summary>
        /// Configures actions for Rail Parsed View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<RailBdParsedDeleteRule>();
            AddAction("Restore").AvailabilityRulesFrom<RailBdParsedRestoreRule>();
            AddAction("Select").AvailabilityRulesFrom<RailBdParsedSelectRule>();
            AddAction("ViewManifest").AvailabilityRulesFrom<RailBdParsedViewManifestRule>();
            AddAction("EditInitial").AvailabilityRulesFrom<RailBdParsedEditInboundRule>();
        }
    }
}