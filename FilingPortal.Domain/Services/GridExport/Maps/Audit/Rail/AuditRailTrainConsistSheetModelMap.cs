using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;

namespace FilingPortal.Domain.Services.GridExport.Maps.Audit.Rail
{
    /// <summary>
    /// Class describing report model mapping for the Audit Rail Train Consist Sheet
    /// </summary>
    internal class AuditRailTrainConsistSheetModelMap : ReportModelMap<AuditRailTrainConsistSheetReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailTrainConsistSheetModelMap"/> class.
        /// </summary>
        public AuditRailTrainConsistSheetModelMap()
        {
            Ignore(x => x.Id);
        }
    }
}