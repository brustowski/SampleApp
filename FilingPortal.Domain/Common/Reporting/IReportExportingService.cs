using System.Threading.Tasks;
using FilingPortal.Domain.DTOs.ReviewSectionModels;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting
{
    public interface IReportExportingService
    {
        Task<FileExportResult> GetExportingResult(string gridName, SearchRequestModel searchRequestModel);

        FileExportResult GetExportingResult(ReviewSectionExportModel exportModel);
    }
}
