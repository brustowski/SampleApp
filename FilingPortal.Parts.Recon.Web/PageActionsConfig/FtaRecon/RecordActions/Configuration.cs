using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.FtaRecon.RecordActions
{
    /// <summary>
    /// Actions configuration for single FTA recon record
    /// </summary>
    public class Configuration : PageConfiguration<FtaReconViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.FtaActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Process").AvailabilityRulesFrom<ProcessRule>();
        }
    }
}