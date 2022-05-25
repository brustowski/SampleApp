using System.Collections.Generic;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.VesselExport
{
    /// <summary>
    /// Class for page configuration of list of Export Records
    /// </summary>
    public class VesselExportListActionsConfig : PageConfiguration<List<VesselExportReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportListActionsConfig"/> class.
        /// </summary>
        public VesselExportListActionsConfig()
        {
            PageName = PageConfigNames.VesselExportListActions;
        }

        /// <summary>
        /// Configures actions available for list of Export Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<VesselExportListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<VesselExportListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<VesselExportListViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<VesselExportListDeleteRule>();
        }
    }
}