using FilingPortal.Domain.Common;
using System.Threading.Tasks;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Recon.Domain.Services
{
    /// <summary>
    /// Describes methods for creating ACE Comparison result report
    /// </summary>
    public interface IAceReportExportService
    {
        /// <summary>
        /// Creates the ACE Comparison result report
        /// </summary>
        /// <param name="searchRequest">The search request</param>
        Task<FileExportResult> Export(SearchRequestModel searchRequest);
    }
}
