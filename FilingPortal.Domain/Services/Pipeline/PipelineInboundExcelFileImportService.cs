using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Parts.Common.Domain.Mapping;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Service for import Pipeline Inbound records from Excel file
    /// </summary>
    public class PipelineInboundExcelFileImportService : FileImportService<FileProcessingResult>, IPipelineInboundExcelFileImportService
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
        private readonly IParsingDataValidationService<PipelineInboundImportParsingModel> _fileValidator;

        /// <summary>
        /// The repository of Rail Inbound records
        /// </summary>
        private readonly IPipelineInboundRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundExcelFileImportService" /> class
        /// </summary>
        /// <param name="fileParser">Excel file parser</param>
        /// <param name="fileValidator">Parsed file data validator</param>
        /// <param name="repository">The repository of Rail Inbound records</param>
        public PipelineInboundExcelFileImportService(
            IFileParser fileParser,
            IParsingDataValidationService<PipelineInboundImportParsingModel> fileValidator,
            IPipelineInboundRepository repository)
        {
            _fileParser = fileParser;
            _fileValidator = fileValidator;
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

                ParsingResult<PipelineInboundImportParsingModel> parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<PipelineInboundImportParsingModel> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                processingResult.Count = Save(validationResult.ValidData.ToList(), userName);


                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">Fully qualified file path</param>
        private ParsingResult<PipelineInboundImportParsingModel> GetParsingResult(Stream fileStream)
        {
            ParsingResult<PipelineInboundImportParsingModel> parseResult;
            try
            {
                parseResult = _fileParser.Parse<PipelineInboundImportParsingModel>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<PipelineInboundImportParsingModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<PipelineInboundImportParsingModel> Validate(IEnumerable<PipelineInboundImportParsingModel> items)
        {
            ParsedDataValidationResult<PipelineInboundImportParsingModel> validationResult;
            try
            {
                validationResult = _fileValidator.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<PipelineInboundImportParsingModel>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">User Name</param>
        private int Save(IList<PipelineInboundImportParsingModel> items, string userName)
        {
            if (!items.Any())
            {
                return 0;
            }

            foreach (PipelineInboundImportParsingModel item in items)
            {
                PipelineInbound newRecord = item.Map<PipelineInboundImportParsingModel, PipelineInbound>();
                newRecord.CreatedUser = userName;
                _repository.Add(newRecord);
            }
            _repository.Save();

            return items.Count;
        }
    }
}
