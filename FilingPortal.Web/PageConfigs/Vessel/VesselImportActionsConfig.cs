using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class for Vessel Inbound View Configuration
    /// </summary>
    public class VesselImportActionsConfig : PageConfiguration<VesselImportReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportActionsConfig"/> class.
        /// </summary>
        public VesselImportActionsConfig()
        {
            PageName = PageConfigNames.VesselImportActions;
        }

        /// <summary>
        /// Configures actions for Vessel Inbound View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<VesselImportDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<VesselImportSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<VesselImportEditRule>();
            AddAction("EditInitial").AvailabilityRulesFrom<VesselImportEditInboundRule>();
        }
    }
}