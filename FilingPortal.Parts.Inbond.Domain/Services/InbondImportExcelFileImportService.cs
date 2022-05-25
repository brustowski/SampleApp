using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Mapping;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Import.Inbound;
using FilingPortal.Parts.Inbond.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Parts.Inbond.Domain.Services
{
    /// <summary>
    /// Represents service for importing Inbond Import records from Excel file
    /// </summary>
    internal class InbondImportExcelFileImportService : DefaultFileImportService, IInbondImportExcelFileImportService
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
        private readonly IParsingDataValidationService<ImportModel> _validationService;

        /// <summary>
        /// The repository of the Inbond records
        /// </summary>
        private readonly IInboundRecordsRepository _repository;

        public InbondImportExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<ImportModel> validationService,
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
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        public override FileProcessingResult Process(string fileName, Stream fileStream, string userName = null)
        {
            lock (Locker)
            {
                var processingResult = new FileProcessingResult(fileName);

                List<ImportModel> parsedData = GetParsingResult(fileStream, processingResult);
                if (!parsedData.Any())
                {
                    return processingResult;
                }

                List<ImportModel> validParsedData = ValidateParsedModel(parsedData, processingResult);
                if (!validParsedData.Any())
                {
                    return processingResult;
                }

                List<InboundRecord> validData = ConvertAndValidateOnDb(validParsedData, processingResult);

                processingResult.Count = Save(validData, userName);
                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="stream">File stream</param>
        /// <param name="processingResult">The processing result</param>
        private List<ImportModel> GetParsingResult(Stream stream, FileProcessingResult processingResult)
        {
            try
            {
                ParsingResult<ImportModel> result = _fileParser.Parse<ImportModel>(stream);
                processingResult.AddCommonErrors(result.Errors);
                processingResult.AddParsingErrors(result.RowErrors);
                return result.ParsedData;
            }
            catch (FileParserException)
            {
                processingResult.AddCommonError(ErrorMessages.ExcelFileParsingError);
                return Enumerable.Empty<ImportModel>().ToList();
            }
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        /// <param name="items">List of items to validate</param>
        /// <param name="processingResult">The processing result</param>
        private List<ImportModel> ValidateParsedModel(IEnumerable<ImportModel> items, FileProcessingResult processingResult)
        {
            try
            {
                ParsedDataValidationResult<ImportModel> result = _validationService.Validate(items);
                processingResult.AddValidationErrors(result.Errors);
                return result.ValidData.ToList();
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                processingResult.AddValidationErrors(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
                return Enumerable.Empty<ImportModel>().ToList();
            }
        }

        /// <summary>
        /// Convert Parsed data to the entity model and validate them on DB
        /// </summary>
        /// <param name="items">List of items to convert and validate</param>
        /// <param name="processingResult">The processing result</param>
        private List<InboundRecord> ConvertAndValidateOnDb(IEnumerable<ImportModel> items, FileProcessingResult processingResult)
        {
            var result = new List<InboundRecord>();
            var errors = new List<RowError>();
            foreach (ImportModel item in items)
            {
                InboundRecord record = item.Map<ImportModel, InboundRecord>();
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
        /// <param name="items">The list of the <see cref="ImportModel"/> to save </param>
        /// <param name="userName">User name</param>
        private int Save(IList<InboundRecord> items, string userName)
        {
            if (!items.Any())
            {
                return items.Count;
            }

            foreach (InboundRecord item in items)
            {
                item.CreatedUser = userName;
                _repository.Add(item);
            }
            _repository.Save();

            return items.Count;
        }
    }
}
