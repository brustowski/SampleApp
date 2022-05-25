using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Audit.Rail;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Audit.Rail;
using FilingPortal.Parts.Common.Domain.Mapping;
using Framework.Infrastructure;

namespace FilingPortal.Domain.Services.Audit.Rail
{
    public class RailAuditExcelFileImportService : FileImportService<FileProcessingResult>, IRailAuditExcelFileImportService
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
        private readonly IParsingDataValidationService<RailAuditTrainConsistSheetImportParsingModel> _validationService;

        /// <summary>
        /// The repository of Truck Export records
        /// </summary>
        private readonly IAuditTrainConsistSheetRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="RailAuditExcelFileImportService"/>
        /// </summary>
        /// <param name="fileParser">Excel file parser</param>
        /// <param name="validationService">Excel data validation service</param>
        /// <param name="repository">Repository to write data into</param>
        public RailAuditExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<RailAuditTrainConsistSheetImportParsingModel> validationService,
            IAuditTrainConsistSheetRepository repository
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

                // Get data from excel
                var parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);

                var validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                processingResult.Count = Save(validationResult.ValidData.ToList(), userName);

                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        private ParsingResult<RailAuditTrainConsistSheetImportParsingModel> GetParsingResult(Stream fileStream)
        {
            ParsingResult<RailAuditTrainConsistSheetImportParsingModel> parseResult;
            try
            {
                parseResult = _fileParser.Parse<RailAuditTrainConsistSheetImportParsingModel>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<RailAuditTrainConsistSheetImportParsingModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<RailAuditTrainConsistSheetImportParsingModel> Validate(IEnumerable<RailAuditTrainConsistSheetImportParsingModel> items)
        {
            ParsedDataValidationResult<RailAuditTrainConsistSheetImportParsingModel> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<RailAuditTrainConsistSheetImportParsingModel>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">Current user name</param> 
        private int Save(IList<RailAuditTrainConsistSheetImportParsingModel> items, string userName)
        {
            if (items.Any())
            {
                var existingItems = _repository.GetAll().ToList();

                foreach (var item in items)
                {
                    var newRecord = item.Map<RailAuditTrainConsistSheetImportParsingModel, AuditRailTrainConsistSheet>();
                    newRecord.CreatedUser = userName;
                    _repository.Add(newRecord);
                }

                
                _repository.Save();

                _repository.Delete(existingItems.Select(x => x.Id));

            }

            return items.Count;
        }
    }
}
