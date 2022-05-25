using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.Inbound.RecordActions
{
    /// <summary>
    /// Actions configuration for single inbound record
    /// </summary>
    public class Configuration : PageConfiguration<InboundRecord>
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
        }
    }
}