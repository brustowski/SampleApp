using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.CanadaTruckImport.Domain.Import.Inbound;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Services
{
    /// <summary>
    /// Represents service for importing Canada Truck Import records from Excel file
    /// </summary>
    internal class InboundImportExcelFileImportService : DefaultFileImportService, IInboundImportExcelFileImportService
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
        private readonly IParsingDataValidationService<InboundImportModel> _validationService;

        /// <summary>
        /// The repository of Canada Truck Import records
        /// </summary>
        private readonly IInboundRecordsRepository _repository;

        /// <summary>
        /// Current user's name
        /// </summary>
        private string _userName;

        public InboundImportExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<InboundImportModel> validationService,
            IInboundRecordsRepository repository
            )
        {
            _fileParser = fileParser;
            _validationService = validationService;
            _repository = repository;
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="stream">File stream</param>
        /// <param name="userName">Current user name</param>
        public override FileProcessingResult Process(string fileName, Stream stream, string userName = null)
        {
            _userName = userName;

            lock (Locker)
            {
                var processingResult = new FileProcessingResult(fileName);

                List<InboundImportModel> parsedData = GetParsingResult(stream, processingResult);
                if (!parsedData.Any())
                {
                    return processingResult;
                }

                List<InboundImportModel> validParsedData = ValidateParsedModel(parsedData, processingResult);
                if (!validParsedData.Any())
                {
                    return processingResult;
                }

                List<InboundRecord> validData = ConvertAndValidateOnDb(validParsedData, processingResult);

                processingResult.Count = Save(validData);
                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        /// <param name="processingResult">The processing result</param>
        private List<InboundImportModel> GetParsingResult(Stream fileStream, FileProcessingResult processingResult)
        {
            try
            {
                ParsingResult<InboundImportModel> result = _fileParser.Parse<InboundImportModel>(fileStream);
                processingResult.AddCommonErrors(result.Errors);
                processingResult.AddParsingErrors(result.RowErrors);
                return result.ParsedData;
            }
            catch (FileParserException)
            {
                processingResult.AddCommonError(ErrorMessages.ExcelFileParsingError);
                return Enumerable.Empty<InboundImportModel>().ToList();
            }
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        /// <param name="items">List of items to validate</param>
        /// <param name="processingResult">The processing result</param>
        private List<InboundImportModel> ValidateParsedModel(IEnumerable<InboundImportModel> items, FileProcessingResult processingResult)
        {
            try
            {
                ParsedDataValidationResult<InboundImportModel> result = _validationService.Validate(items);
                processingResult.AddValidationErrors(result.Errors);
                return result.ValidData.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                processingResult.AddValidationErrors(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
                return Enumerable.Empty<InboundImportModel>().ToList();
            }
        }

        /// <summary>
        /// Convert Parsed data to the entity model and validate them on DB
        /// </summary>
        /// <param name="items">List of items to convert and validate</param>
        /// <param name="processingResult">The processing result</param>
        private List<InboundRecord> ConvertAndValidateOnDb(IEnumerable<InboundImportModel> items, FileProcessingResult processingResult)
        {
            var result = new List<InboundRecord>();
            var errors = new List<RowError>();
            foreach (InboundImportModel item in items)
            {
                InboundRecord record = item.Map<InboundImportModel, InboundRecord>();
                try
                {
                    if (_repository.IsDuplicated(record))
                    {
                        errors.Add(new RowError(item.RowNumberInFile, string.Empty, ErrorLevel.Critical,
                            string.Empty, ValidationMessages.DuplicatesRecordInDB));
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                    processingResult.AddValidationErrors(new[] {
                        new RowError(item.RowNumberInFile,string.Empty, ErrorLevel.Critical,string.Empty, ErrorMessages.UnexpectedValidationError)
                    });
                    continue;
                }

                result.Add(record);
            }

            if (errors.Any())
            {
                processingResult.AddValidationErrors(errors);
            }

            return result;
        }


        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items">The list of the <see cref="InboundImportModel"/> to save </param>
        private int Save(IList<InboundRecord> items)
        {
            if (!items.Any())
            {
                return items.Count;
            }

            foreach (InboundRecord item in items)
            {
                item.CreatedUser = _userName;
                _repository.Add(item);
            }
            _repository.Save();

            return items.Count;
        }
    }
}
