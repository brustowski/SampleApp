using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.FtaRecon.ListActions
{
    /// <summary>
    /// Actions configuration for the FTA recon models list
    /// </summary>
    public class Configuration : PageConfiguration<List<FtaReconViewModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.FtaListActions;
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