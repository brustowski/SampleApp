using System.Collections.Generic;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.PageConfigs.Common;

namespace FilingPortal.Web.PageConfigs.Vessel
{
    /// <summary>
    /// Class for page configuration of list of Inbound Records
    /// </summary>
    public class VesselImportListActionsConfig : PageConfiguration<List<VesselImportReadModel>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportListActionsConfig"/> class.
        /// </summary>
        public VesselImportListActionsConfig()
        {
            PageName = PageConfigNames.VesselListImportActions;
        }

        /// <summary>
        /// Configures actions available for list of Inbound Records
        /// </summary>
        public override void Configure()
        {
            AddAction("Undo").AvailabilityRulesFrom<VesselImportListCancelRule>();
            AddAction("ReviewFile").AvailabilityRulesFrom<VesselImportListReviewFileRule>();
            AddAction("View").AvailabilityRulesFrom<VesselImportListViewRule>();
            AddAction("Delete").AvailabilityRulesFrom<VesselImportListDeleteRule>();
        }
    }
}