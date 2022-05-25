using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.FtaRecon.PageActions
{
    /// <summary>
    /// Actions configuration for the FTA recon page
    /// </summary>
    public class Configuration : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.FtaViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Upload").AvailabilityRulesFrom<UploadRule>();
            AddAction("FtaExport").AvailabilityRulesFrom<ExportRule>();
        }
    }
}