using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Domain.Services.GridExport.Maps.TruckExport
{
    /// <summary>
    /// Class describing  report model mapping for the Truck Export USPPI-Consignee Rule Records grid
    /// </summary>
    internal class TruckExportUsppiConsigneeRuleReportModelMap : ReportModelMap<TruckExportRuleExporterConsignee>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportUsppiConsigneeRuleReportModelMap"/> class.
        /// </summary>
        public TruckExportUsppiConsigneeRuleReportModelMap()
        {
            Map(x => x.Exporter).ColumnTitle("USPPI");
            Map(x => x.Address).ColumnTitle("USPPI Address");
            Ignore(x => x.Id);
        }
    }
}