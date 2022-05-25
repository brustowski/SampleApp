using System.Collections.Generic;
using Framework.Domain.Paging;
using System.Threading.Tasks;
using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Domain.Common.Reporting
{
    public interface IReportingService
    {
        Task<FileExportResult> ExportToFile<TModel, TDataModel>(IReportConfig reportConfig, SearchRequestModel searchRequestModel)
            where TModel : class, new()
            where TDataModel : class;

        FileExportResult ExportToFileFromStatic<TModel>(IReportConfig reportConfig, IList<IColumnMapInfo> columnMapInfos, IList<TModel> model)
            where TModel : class, new();
    }
}
