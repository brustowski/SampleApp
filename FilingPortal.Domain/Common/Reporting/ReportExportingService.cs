using Framework.Domain.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FilingPortal.Domain.DTOs.ReviewSectionModels;
using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Service for exporting reports
    /// </summary>
    public class ReportExportingService : IReportExportingService
    {
        /// <summary>
        /// Reporting service
        /// </summary>
        private readonly IReportingService _reportingService;
        /// <summary>
        /// Report configurations register
        /// </summary>
        private readonly IReportConfigRegistry _reportConfigRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportExportingService"/> class
        /// </summary>
        /// <param name="reportingService">Reporting service</param>
        /// <param name="reportConfigRegistry">Report configurations register</param>
        public ReportExportingService(IReportingService reportingService, IReportConfigRegistry reportConfigRegistry)
        {
            _reportingService = reportingService;
            _reportConfigRegistry = reportConfigRegistry;
        }

        /// <summary>
        /// Creates report as file and return information about it
        /// </summary>
        /// <param name="gridName">Name of the grid for report</param>
        /// <param name="searchRequestModel">The <see cref="SearchRequestModel"/> object</param>
        public async Task<FileExportResult> GetExportingResult(string gridName, SearchRequestModel searchRequestModel)
        {
            IReportConfig reportConfig = _reportConfigRegistry.GetConfig(gridName);

            // This line ensures that method is available in reporting service
            var genMethod = new Func<IReportConfig, SearchRequestModel, Task<FileExportResult>>(_reportingService.ExportToFile<object, object>);

            MethodInfo method = typeof(IReportingService).GetMethod(genMethod.Method.Name);
            MethodInfo generic = method.MakeGenericMethod(reportConfig.ModelType, reportConfig.ModelDataType);
            var task = (Task<FileExportResult>)generic.Invoke(_reportingService,
                new object[] { reportConfig, searchRequestModel });

            var exportResult = await task;

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(exportResult.DocumentExternalName);
            var filenameExtension = Path.GetExtension(exportResult.DocumentExternalName);
            var filename = $"{filenameWithoutExtension} - {DateTime.Now:yyyyMMdd-HHmm}{filenameExtension}";
            exportResult.DocumentExternalName = filename;

            return exportResult;
        }

        public FileExportResult GetExportingResult(ReviewSectionExportModel exportModel)
        {
            IReportConfig reportConfig = new DynamicReportConfig(GridNames.AutoCreateRecords)
                {FileName = $"{exportModel.SectionName}.xlsx", DocumentTitle = exportModel.SectionName};

            // This line ensures that method is available in reporting service
            var genMethod = new Func<IReportConfig, IList<IColumnMapInfo>, IList<object>, FileExportResult>(_reportingService.ExportToFileFromStatic);

            MethodInfo method = typeof(IReportingService).GetMethod(genMethod.Method.Name);
            MethodInfo generic = method.MakeGenericMethod(reportConfig.ModelType);
            var task = (FileExportResult)generic.Invoke(_reportingService,
                new object[] { reportConfig, exportModel.GetColumnInfos(), exportModel.GetDynamicData() });

            var exportResult = task;

            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(exportResult.DocumentExternalName);
            var filenameExtension = Path.GetExtension(exportResult.DocumentExternalName);
            var filename = $"{filenameWithoutExtension} - {DateTime.Now:yyyyMMdd-HHmm}{filenameExtension}";
            exportResult.DocumentExternalName = filename;

            return exportResult;
        }
    }
}