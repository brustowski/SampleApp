using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Mapping;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Services.TruckExport
{
    internal class TruckExportExcelFileImportService : FileImportService<FileProcessingDetailedResult>, ITruckExportExcelFileImportService
    {
        /// <summary>
        /// Locker is used to prevent multiple file processing at the same time
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// Excel file parser
        /// </summary>
        private readonly IFileParser _fileParser;

        /// <summary>
        /// Parsed data validator
        /// </summary>
        private readonly IParsingDataValidationService<TruckExportImportModel> _validationService;

        /// <summary>
        /// The repository of Truck Export records
        /// </summary>
        private readonly ITruckExportRepository _repository;

        /// <summary>
        /// The Filing header repository
        /// </summary>
        private readonly ITruckExportFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Initialize a new instance of the <see cref="TruckExportExcelFileImportService"/> class.
        /// </summary>
        /// <param name="fileParser">The File parser</param>
        /// <param name="validationService">The validation service</param>
        /// <param name="repository">The Entity repository</param>
        /// <param name="filingHeadersRepository">The filing Header repository</param>
        public TruckExportExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<TruckExportImportModel> validationService,
            ITruckExportRepository repository,
            ITruckExportFilingHeadersRepository filingHeadersRepository)
        {
            _fileParser = fileParser;
            _validationService = validationService;
            _repository = repository;
            _filingHeadersRepository = filingHeadersRepository;
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        public override FileProcessingDetailedResult Process(string fileName, Stream fileStream, string userName = null)
        {
            lock (Locker)
            {
                var processingResult = new FileProcessingDetailedResult(fileName);

                ParsingResult<TruckExportImportModel> parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<TruckExportImportModel> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                (List<TruckExportRecord> created, List<TruckExportRecord> updated) = Save(validationResult.ValidData.ToList(), userName);

                UpdateFilingHeaders(updated, userName);

                processingResult.Inserted = created.Count;
                processingResult.Updated = updated.Count;
                processingResult.Count = created.Count + updated.Count;

                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        private ParsingResult<TruckExportImportModel> GetParsingResult(Stream fileStream)
        {
            ParsingResult<TruckExportImportModel> parseResult;
            try
            {
                parseResult = _fileParser.Parse<TruckExportImportModel>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<TruckExportImportModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<TruckExportImportModel> Validate(IEnumerable<TruckExportImportModel> items)
        {
            ParsedDataValidationResult<TruckExportImportModel> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<TruckExportImportModel>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">User name</param>
        private (List<TruckExportRecord> created, List<TruckExportRecord> updated) Save(IReadOnlyCollection<TruckExportImportModel> items, string userName)
        {
            var created = new List<TruckExportRecord>(items.Count);
            var updated = new List<TruckExportRecord>(items.Count);

            if (!items.Any())
            {
                return (created, updated);
            }

            DateTime currentDate = DateTime.Now;

            foreach (TruckExportImportModel item in items)
            {
                TruckExportRecord matchedEntity = _repository.GetMatchedEntity(item);
                if (matchedEntity == null)
                {
                    TruckExportRecord entity = item.Map<TruckExportImportModel, TruckExportRecord>();
                    entity.CreatedUser = userName;
                    entity.CreatedDate = currentDate;
                    entity.ModifiedUser = userName;
                    entity.ModifiedDate = currentDate;
                    _repository.Add(entity);
                    created.Add(entity);
                }
                else
                {
                    matchedEntity.Importer = item.Importer;
                    matchedEntity.TariffType = item.TariffType;
                    matchedEntity.Tariff = item.Tariff;
                    matchedEntity.RoutedTran = item.RoutedTran;
                    matchedEntity.SoldEnRoute = item.SoldEnRoute;
                    matchedEntity.Export = item.Export;
                    matchedEntity.ExportDate = item.ExportDate;
                    matchedEntity.CustomsQty = item.CustomsQty;
                    matchedEntity.Price = item.Price;
                    matchedEntity.GrossWeight = item.GrossWeight;
                    matchedEntity.GrossWeightUOM = item.GrossWeightUOM;
                    matchedEntity.Origin = item.Origin;
                    matchedEntity.GoodsDescription = item.GoodsDescription;
                    matchedEntity.ECCN = item.ECCN;
                    matchedEntity.GoodsOrigin = item.GoodsOrigin;
                    matchedEntity.Hazardous = item.Hazardous;
                    matchedEntity.OriginIndicator = item.OriginIndicator;
                    matchedEntity.ModifiedUser = userName;
                    matchedEntity.ModifiedDate = currentDate;
                    matchedEntity.IsUpdate = true;
                    matchedEntity.IsAuto = false;
                    matchedEntity.IsAutoProcessed = false;
                    _repository.Update(matchedEntity);
                    updated.Add(matchedEntity);
                }
            }

            _repository.Save();

            _repository.Validate(created.Union(updated).ToList());

            return (created, updated);
        }

        private void UpdateFilingHeaders(List<TruckExportRecord> updated, string user)
        {
            if (!updated.Any())
            {
                return;
            }

            IEnumerable<TruckExportFilingHeader> filingHeaders = _filingHeadersRepository.FindByInboundRecordIds(updated.Select(x => x.Id))
                .Where(x => x.JobStatus != JobStatus.Open);

            DateTime lastModifiedDate = DateTime.Now;
            foreach (TruckExportFilingHeader filingHeader in filingHeaders)
            {
                filingHeader.LastModifiedDate = lastModifiedDate;
                filingHeader.LastModifiedUser = user;
                filingHeader.IsUpdated = true;
                filingHeader.JobStatus = JobStatus.WaitingUpdate;

                _filingHeadersRepository.Update(filingHeader);
            }

            _filingHeadersRepository.Save();
        }

    }
}
