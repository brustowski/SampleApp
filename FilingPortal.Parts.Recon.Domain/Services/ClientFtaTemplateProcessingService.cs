using FilingPortal.Domain;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Import.FtaRecon;
using FilingPortal.Parts.Recon.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilingPortal.Parts.Recon.Domain.Services
{
    /// <summary>
    /// Represents the Client records template processing service
    /// </summary>
    public class ClientFtaTemplateProcessingService : ITemplateProcessingService<Model, FtaRecon>
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
        private readonly IParsingDataValidationService<Model> _validationService;

        /// <summary>
        /// The FTA Recon repository
        /// </summary>
        private readonly IInboundRepository _repository;

        /// <summary>
        /// Creates a new instance of <see cref="TemplateProcessingService{TParsingDataModel, TRuleEntity}"/>
        /// </summary>
        /// <param name="fileParser">Excel file parser</param>
        /// <param name="validationService">Pipeline price rule validation service</param>
        /// <param name="repository">The repository</param>
        public ClientFtaTemplateProcessingService(IFileParser fileParser, IParsingDataValidationService<Model> validationService, IInboundRepository repository)
        {
            _fileParser = fileParser;
            _validationService = validationService;
            _repository = repository;
        }


        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="path">Fully qualified file path</param>
        /// <param name="userName">Current user name</param>
        public FileProcessingResult Process(string fileName, string path, string userName = null)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File not found", Path.GetFileName(path));
            }

            using (FileStream fileStream = File.OpenRead(path))
            {
                return Process(fileName, fileStream, userName);
            }
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="userName">Current user name</param>
        public FileProcessingResult Process(string fileName, Stream fileStream, string userName = null)
        {
            lock (Locker)
            {
                var processingResult = new FileProcessingResult(fileName);

                // Get data from excel
                ParsingResult<Model> parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<Model> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                processingResult.Count = Save(validationResult.ValidData, processingResult, userName);

                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        private ParsingResult<Model> GetParsingResult(Stream fileStream)
        {
            ParsingResult<Model> parseResult;
            try
            {
                parseResult = _fileParser.Parse<Model>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<Model>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<Model> Validate(IEnumerable<Model> items)
        {
            ParsedDataValidationResult<Model> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<Model>(new[] {
                    new RowError(-1,"", ErrorLevel.Critical,"", ErrorMessages.UnexpectedValidationError)
                });
            }
            return validationResult;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="items">List of items to convert and validate</param>
        /// <param name="processingResult">The processing result</param>
        private int Save(IEnumerable<Model> items, FileProcessingResult processingResult, string userName)
        {
            var errors = new List<RowError>();
            var count = 0;
            foreach (Model item in items)
            {
                try
                {
                    InboundRecord record = _repository.Get(item.Filer, item.EntryNo, item.LineNumber7501);
                    if (record == null)
                    {
                        errors.Add(new RowError(item.RowNumberInFile, string.Empty, ErrorLevel.Critical,
                            string.Empty, "Corresponding record not found."));
                        continue;
                    }

                    FtaRecon ftaRecon = item.Map<Model, FtaRecon>();
                    if (record.FtaRecon == null)
                    {
                        record.FtaRecon = ftaRecon;
                        record.FtaRecon.CreatedUser = userName;
                    }
                    else
                    {
                        record.FtaRecon.CreatedDate = DateTime.Now;
                        record.FtaRecon.ClientNote = ftaRecon.ClientNote;
                        record.FtaRecon.FtaEligibility = ftaRecon.FtaEligibility;
                    }

                    _repository.Update(record);
                    count++;
                }
                catch (Exception ex)
                {
                    AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                    processingResult.AddValidationErrors(new[] {
                        new RowError(item.RowNumberInFile,string.Empty, ErrorLevel.Critical,string.Empty, ErrorMessages.UnexpectedValidationError)
                    });
                }
            }

            if (errors.Any())
            {
                processingResult.AddValidationErrors(errors);
            }

            if (count > 0)
            {
                _repository.Save();
            }

            return count;
        }

        /// <summary>
        /// Verify the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        public FileProcessingDetailedResult Verify(AppDocument document)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Import data from the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        /// <param name="userName">Current user name</param>
        public FileProcessingDetailedResult Import(AppDocument document, string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
