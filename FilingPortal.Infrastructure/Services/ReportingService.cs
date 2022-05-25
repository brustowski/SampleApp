using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using Framework.Domain.Paging;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Infrastructure.Common;
using Reporting = FilingPortal.Domain.Common.Reporting.Model;

namespace FilingPortal.Infrastructure.Services
{
    /// <summary>
    /// Provides method for creating report
    /// </summary>
    internal class ReportingService : IReportingService
    {
        private readonly IReporterFactory _reporterFactory;
        private readonly ISearchRequestFactory _requestFactory;
        private readonly IReportFiltersBuilder _filtersBuilder;
        private readonly IReportBodyBuilder _bodyBuilder;
        private readonly IReportDataSourceResolver _dataSourceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingService"/> class
        /// </summary>
        /// <param name="reporterFactory">The report builder factory</param>
        /// <param name="requestFactory">The search request factory</param>
        /// <param name="dataSourceResolver">The data source for report resolver</param>
        /// <param name="filtersBuilder">The filter report builder</param>
        /// <param name="bodyBuilder">The body report builder</param>
        public ReportingService(IReporterFactory reporterFactory,
            ISearchRequestFactory requestFactory,
            IReportDataSourceResolver dataSourceResolver,
            IReportFiltersBuilder filtersBuilder,
            IReportBodyBuilder bodyBuilder)
        {
            _reporterFactory = reporterFactory;
            _filtersBuilder = filtersBuilder;
            _requestFactory = requestFactory;
            _dataSourceResolver = dataSourceResolver;
            _bodyBuilder = bodyBuilder;
        }

        /// <summary>
        /// Creates report and save it to the file
        /// </summary>
        /// <typeparam name="TModel">The type of the filter model</typeparam>
        /// <typeparam name="TDataModel">The type of the data model</typeparam>
        /// <param name="reportConfig">The report configuration object</param>
        /// <param name="searchRequestModel">The <see cref="SearchRequestModel"/> object</param>
        public async Task<FileExportResult> ExportToFile<TModel, TDataModel>(IReportConfig reportConfig, SearchRequestModel searchRequestModel)
            where TModel : class, new()
            where TDataModel : class

        {
            SearchRequest request = _requestFactory.Create<TDataModel>(searchRequestModel);

            const int pageSize = 5000;
            var pageNumber = 1;

            var filename = reportConfig.FileName;
            IReporter excelReporter = _reporterFactory.Create(filename, reportConfig.DocumentTitle);

            if (reportConfig.IsFilterSettingsVisible)
            {
                IEnumerable<Reporting.Row> filtersData = _filtersBuilder.GetRows(searchRequestModel.FilterSettings.Filters);
                excelReporter.AddPartOfData(filtersData);
            }

            Reporting.Row headerRow = _bodyBuilder.GetHeaderRow<TModel>();
            excelReporter.AddHeader(headerRow, true);

            IReportDatasource dataSource = _dataSourceResolver.Resolve(reportConfig.Name);

            var totalRows = await dataSource.GetTotalMatches<TDataModel>(request);
            AppLogger.Info($"Total rows {totalRows}");
            var numberOfChunks = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1;
            AppLogger.Info($"Number of Chunks {numberOfChunks}");
            var currentChunk = 1;
            while (currentChunk <= numberOfChunks)
            {
                try
                {
                    request.PagingSettings = new PagingSettings { PageNumber = pageNumber, PageSize = pageSize };

                    Stopwatch stopWatch = Stopwatch.StartNew();
                    IEnumerable<TModel> results = await dataSource.GetAllAsync<TModel>(request);
                    stopWatch.Stop();
                    AppLogger.Info($"TimeMeasureInfo: Execution of .GetAllAsync() took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");


                    stopWatch = Stopwatch.StartNew();
                    IEnumerable<Reporting.Row> rows = _bodyBuilder.GetRows(results);
                    stopWatch.Stop();
                    AppLogger.Info($"TimeMeasureInfo: Execution of .GetRows() took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");

                    stopWatch = Stopwatch.StartNew();
                    excelReporter.AddPartOfData(rows);
                    stopWatch.Stop();
                    AppLogger.Info($"TimeMeasureInfo: Execution of .AddPartOfData() took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");

                }
                catch (Exception e)
                {
                    var errorMessage = $"Error processing page[{pageNumber}]: {e.Message}";
                    AppLogger.Error(e, errorMessage);
                    throw new FilingPortalBusinnessException(errorMessage, e);
                }

                pageNumber++;
                currentChunk++;
            }

            var fullFilePath = excelReporter.SaveToFile();

            return new FileExportResult
            {
                DocumentExternalName = reportConfig.FileName,
                FileName = fullFilePath
            };
        }

        /// <summary>
        /// Creates report and save it to the file
        /// </summary>
        /// <typeparam name="TModel">The type of the filter model</typeparam>
        /// <param name="reportConfig">The report configuration object</param>
        /// <param name="columnMapInfos">Columns definitions</param>
        /// <param name="model">Values</param>
        public FileExportResult ExportToFileFromStatic<TModel>(IReportConfig reportConfig, IList<IColumnMapInfo> columnMapInfos, IList<TModel> model) where TModel : class, new()
        {
            var filename = reportConfig.FileName;
            IReporter excelReporter = _reporterFactory.Create(filename, reportConfig.DocumentTitle);

            const int pageSize = 5000;
            var pageNumber = 1;

            
            
            Reporting.Row headerRow = _bodyBuilder.GetHeaderRow(columnMapInfos);
            excelReporter.AddHeader(headerRow, true);

            var totalRows = model.Count;
            AppLogger.Info($"Total rows {totalRows}");
            var numberOfChunks = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1;
            AppLogger.Info($"Number of Chunks {numberOfChunks}");
            var currentChunk = 1;
            while (currentChunk <= numberOfChunks)
            {
                try
                {
                    IEnumerable<TModel> results = model;

                    Stopwatch stopWatch = Stopwatch.StartNew();
                    IEnumerable<Reporting.Row> rows = _bodyBuilder.GetRows(columnMapInfos, results);
                    stopWatch.Stop();
                    AppLogger.Info($"TimeMeasureInfo: Execution of .GetRows() took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");

                    stopWatch = Stopwatch.StartNew();
                    excelReporter.AddPartOfData(rows);
                    stopWatch.Stop();
                    AppLogger.Info($"TimeMeasureInfo: Execution of .AddPartOfData() took {stopWatch.ElapsedMilliseconds} ms [{stopWatch.ElapsedMilliseconds / 1000} s]");

                }
                catch (Exception e)
                {
                    var errorMessage = $"Error processing page[{pageNumber}]: {e.Message}";
                    AppLogger.Error(e, errorMessage);
                    throw new FilingPortalBusinnessException(errorMessage, e);
                }

                pageNumber++;
                currentChunk++;
            }

            var fullFilePath = excelReporter.SaveToFile();

            return new FileExportResult
            {
                DocumentExternalName = reportConfig.FileName,
                FileName = fullFilePath
            };
        }
    }
}
