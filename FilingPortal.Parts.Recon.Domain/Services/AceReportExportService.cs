using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.Model;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.Domain.Paging;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilingPortal.Parts.Recon.Domain.Services
{
    /// <summary>
    /// Provides methods for creating ACE Comparison result report
    /// </summary>
    internal class AceReportExportService : IAceReportExportService
    {
        private readonly IReporterFactory _reporterFactory;
        private readonly IReportBodyBuilder _bodyBuilder;
        private readonly IInboundReadModelRepository _cwRepository;
        private readonly IAceReportRecordsRepository _aceRepository;
        private readonly ISearchRequestFactory _requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceReportExportService"/> class
        /// </summary>
        /// <param name="reporterFactory">The report builder factory</param>
        /// <param name="bodyBuilder">The body report builder</param>
        /// <param name="cwRepository">CW records repository</param>
        /// <param name="aceRepository">ACE records repository</param>
        /// <param name="requestFactory">The search request factory</param>
        public AceReportExportService(IReporterFactory reporterFactory,
            IReportBodyBuilder bodyBuilder,
            IInboundReadModelRepository cwRepository,
            IAceReportRecordsRepository aceRepository,
            ISearchRequestFactory requestFactory)
        {
            _reporterFactory = reporterFactory;
            _bodyBuilder = bodyBuilder;
            _cwRepository = cwRepository;
            _aceRepository = aceRepository;
            _requestFactory = requestFactory;
        }

        /// <summary>
        /// Creates report and save it to the file
        /// </summary>
        public async Task<FileExportResult> Export(SearchRequestModel searchRequestModel)
        {
            List<InboundRecordReadModel> matchedRecords = GetMatchedRecords(new SearchRequestModel(searchRequestModel));
            const string fileName = "ACE_comparison_result.xlsx";
            IReporter excelReporter = _reporterFactory.Create(fileName, "All Mismatches");
            WriteData(excelReporter, matchedRecords.Where(x => x.MismatchReconValueFlag || x.MismatchReconFtaFlag || x.MismatchEntryValue || x.MismatchDuty || x.MismatchMpf || x.MismatchPayableMpf || x.MismatchQuantity || x.MismatchHts)
                .Map<InboundRecordReadModel, AllMismatchesModel>());
            excelReporter.AddSection("Recon Value Flag");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, ValueFlagMismatchModel>());
            excelReporter.AddSection("Recon FTA Flag");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, FtaFlagMismatchModel>());
            excelReporter.AddSection("Entered Value");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, EntryValueMismatchModel>());
            excelReporter.AddSection("Duty");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, DutyMismatchModel>());
            excelReporter.AddSection("MPF");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, MpfMismatchModel>());
            excelReporter.AddSection("MPF Payable");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, PayableMpfMismatchModel>());
            excelReporter.AddSection("Quantity");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, QuantityMismatchModel>());
            excelReporter.AddSection("HTS");
            WriteData(excelReporter, matchedRecords.Map<InboundRecordReadModel, HtsMismatchModel>());
            List<InboundRecordReadModel> unmatchedRecords = GetUnmatchedRecords(new SearchRequestModel(searchRequestModel));
            excelReporter.AddSection("Could Not Locate in ACE");
            WriteData(excelReporter, unmatchedRecords.Map<InboundRecordReadModel, Reporting.CargoWiseInternal.Model>());
            excelReporter.AddSection("Could Not Locate in CW");
            WriteData(excelReporter, await _aceRepository.GetMissingRecords());

            var fullFilePath = excelReporter.SaveToFile();
            return new FileExportResult
            {
                DocumentExternalName = fileName,
                FileName = fullFilePath
            };
        }

        private List<InboundRecordReadModel> GetMatchedRecords(SearchRequestModel searchRequestModel)
        {
            Filter aceFound = FilterBuilder.CreateFor<InboundRecordReadModel>(x => x.AceFound).AddValue(true).Build();
            searchRequestModel.FilterSettings.Filters.Add(aceFound);
            searchRequestModel.PagingSettings = null;
            SearchRequest request = _requestFactory.Create<InboundRecordReadModel>(searchRequestModel);
            return _cwRepository.GetAll<InboundRecordReadModel>(request).Results.ToList();
        }

        private List<InboundRecordReadModel> GetUnmatchedRecords(SearchRequestModel searchRequestModel)
        {
            Filter aceNotFound = FilterBuilder.CreateFor<InboundRecordReadModel>(x => x.AceFound).AddValue(false).Build();
            searchRequestModel.FilterSettings.Filters.Add(aceNotFound);
            searchRequestModel.PagingSettings = null;
            SearchRequest request = _requestFactory.Create<InboundRecordReadModel>(searchRequestModel);
            return _cwRepository.GetAll<InboundRecordReadModel>(request).Results.ToList();
        }

        private void WriteData<TModel>(IReporter reporter, IEnumerable<TModel> dataSource)
        where TModel : class, new()
        {
            Row headerRow = _bodyBuilder.GetHeaderRow<TModel>();
            reporter.AddHeader(headerRow);
            try
            {
                IEnumerable<TModel> results = dataSource;
                IEnumerable<Row> rows = _bodyBuilder.GetRows(results);
                reporter.AddPartOfData(rows);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error processing report: {e.Message}";
                AppLogger.Error(e, errorMessage);
                throw new FilingPortalBusinnessException(errorMessage, e);
            }
        }
    }
}
