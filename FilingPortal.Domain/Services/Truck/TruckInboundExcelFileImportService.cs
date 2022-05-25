using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Imports.Truck.Inbound;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Truck;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Services.Truck
{
    internal class TruckInboundExcelFileImportService : FileImportService<FileProcessingResult>, ITruckInboundExcelFileImportService
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
        /// The repository of Truck Inbound records
        /// </summary>
        private readonly ITruckInboundRepository _repository;

        public TruckInboundExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<ImportModel> validationService,
            ITruckInboundRepository repository
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

                ParsingResult<ImportModel> parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<ImportModel> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                List<TruckInbound> validData = ConvertAndValidateOnDb(validationResult.ValidData, processingResult);

                processingResult.Count = Save(validData, userName);

                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        private ParsingResult<ImportModel> GetParsingResult(Stream fileStream)
        {
            ParsingResult<ImportModel> parseResult;
            try
            {
                parseResult = _fileParser.Parse<ImportModel>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<ImportModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<ImportModel> Validate(IEnumerable<ImportModel> items)
        {
            ParsedDataValidationResult<ImportModel> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<ImportModel>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        /// <summary>
        /// Convert Parsed data to the entity model and validate them on DB
        /// </summary>
        /// <param name="items">List of items to convert and validate</param>
        /// <param name="processingResult">The processing result</param>
        private List<TruckInbound> ConvertAndValidateOnDb(IEnumerable<ImportModel> items, FileProcessingResult processingResult)
        {
            var result = new List<TruckInbound>();
            var errors = new List<RowError>();
            foreach (ImportModel item in items)
            {
                TruckInbound record = item.Map<ImportModel, TruckInbound>();
                try
                {
                    if (_repository.IsDuplicate(record))
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
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">User name</param>
        private int Save(ICollection<TruckInbound> items, string userName)
        {
            if (!items.Any()) return items.Count;

            foreach (TruckInbound item in items)
            {
                item.CreatedUser = userName;
                _repository.Add(item);
            }
            _repository.Save();

            return items.Count;
        }
    }
}
