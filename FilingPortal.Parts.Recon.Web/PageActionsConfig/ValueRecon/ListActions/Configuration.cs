using FilingPortal.Parts.Recon.Web.Configs;
using FilingPortal.Parts.Recon.Web.Models;
using FilingPortal.PluginEngine.PageConfigs;
using System.Collections.Generic;

namespace FilingPortal.Parts.Recon.Web.PageActionsConfig.ValueRecon.ListActions
{
    /// <summary>
    /// Actions configuration for the value recon models list
    /// </summary>
    public class Configuration : PageConfiguration<List<ValueReconViewModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.ValueListActions;
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