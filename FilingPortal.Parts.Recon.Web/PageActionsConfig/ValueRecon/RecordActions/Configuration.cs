using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.RecordActions
{
    /// <summary>
    /// Actions configuration for single value recon model
    /// </summary>
    public class Configuration : PageConfiguration<ValueReconViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.ValueActions;
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