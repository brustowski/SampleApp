using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Vessel;

namespace FilingPortal.Domain.Services.GridExport.Maps.Vessel
{
    /// <summary>
    /// Class describing  report model mapping for the Vessel Port Rule Records grid
    /// </summary>
    internal class VesselRulePortsReportModelMap : ReportModelMap<VesselRulePortsReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModelMap{T}"/> class.
        /// </summary>
        public VesselRulePortsReportModelMap()
        {
            Ignore(x => x.DischargeName);
            Ignore(x => x.Id);
        }
    }
}