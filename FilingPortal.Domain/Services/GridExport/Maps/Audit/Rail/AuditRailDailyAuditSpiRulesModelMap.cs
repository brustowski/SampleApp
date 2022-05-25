using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;

namespace FilingPortal.Domain.Services.GridExport.Maps.Audit.Rail
{
    /// <summary>
    /// Class describing report model mapping for the Audit Rail Daily Audit SPI rules
    /// </summary>
    internal class AuditRailDailyAuditSpiRulesModelMap : ReportModelMap<AuditRailDailyAuditSpiRulesReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyAuditSpiRulesModelMap"/> class.
        /// </summary>
        public AuditRailDailyAuditSpiRulesModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}