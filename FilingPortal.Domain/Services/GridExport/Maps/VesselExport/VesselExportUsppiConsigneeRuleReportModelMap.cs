using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.VesselExport;

namespace FilingPortal.Domain.Services.GridExport.Maps.VesselExport
{
    /// <summary>
    /// Class describing report model mapping for the Vessel Export USPPI-Consignee rule grid
    /// </summary>
    internal class VesselExportUsppiConsigneeRuleReportModelMap : ReportModelMap<VesselExportUsppiConsigneeReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportUsppiConsigneeRuleReportModelMap"/> class.
        /// </summary>
        public VesselExportUsppiConsigneeRuleReportModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}