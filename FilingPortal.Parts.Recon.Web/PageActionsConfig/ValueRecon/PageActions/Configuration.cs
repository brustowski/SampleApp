using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.PageActions
{
    /// <summary>
    ///  Actions configuration for the value recon page
    /// </summary>
    public class Configuration : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.ValueViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Upload").AvailabilityRulesFrom<UploadRule>();
            AddAction("ValueExport").AvailabilityRulesFrom<ExportRule>();
        }
    }
}