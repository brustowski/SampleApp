﻿using FilingPortal.Parts.Isf.Web.Configs;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;

namespace FilingPortal.Parts.Isf.Web.PageActionsConfig.Inbound.PageActions
{
    /// <summary>
    /// Represents the Page Actions Configuration
    /// </summary>
    public class Configuration : PageConfiguration<PageConfigurationModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            PageName = PageConfigNames.InboundViewPageActions;
        }

        /// <summary>
        /// Configures available actions
        /// </summary>
        public override void Configure()
        {
            AddAction("Add").AvailabilityRulesFrom<AddInboundRule>();
        }
    }
}