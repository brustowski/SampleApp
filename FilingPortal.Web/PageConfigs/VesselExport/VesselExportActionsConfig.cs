using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class for Vessel Export View Configuration
    /// </summary>
    public class VesselExportActionsConfig : PageConfiguration<VesselExportReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportActionsConfig"/> class.
        /// </summary>
        public VesselExportActionsConfig()
        {
            PageName = PageConfigNames.VesselExportActions;
        }

        /// <summary>
        /// Configures actions for Vessel Export View Item
        /// </summary>
        public override void Configure()
        {
            AddAction("Delete").AvailabilityRulesFrom<VesselExportDeleteRule>();
            AddAction("Select").AvailabilityRulesFrom<VesselExportSelectRule>();
            AddAction("Edit").AvailabilityRulesFrom<VesselExportEditRule>();
            AddAction("EditInitial").AvailabilityRulesFrom<VesselExportEditInboundRule>();
        }
    }
}