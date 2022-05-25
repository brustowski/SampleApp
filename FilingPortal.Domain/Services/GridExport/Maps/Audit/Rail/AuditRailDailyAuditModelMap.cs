using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;

namespace FilingPortal.Domain.Services.GridExport.Maps.Audit.Rail
{
    /// <summary>
    /// Class describing report model mapping for the Audit Rail Daily Audit
    /// </summary>
    internal class AuditRailDailyAuditModelMap : ReportModelMap<AuditRailDailyAuditReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyAuditModelMap"/> class.
        /// </summary>
        public AuditRailDailyAuditModelMap()
        {
            Ignore(x => x.Id);

            Map(x => x.Chgs).ColumnTitle("Freight");
        }
    }
}