using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using Framework.Domain;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Represents the template processing service for parsing data model and result entity
    /// </summary>
    /// <typeparam name="TParsingDataModel">Parsing data model</typeparam>
    /// <typeparam name="TRuleEntity">Rule entity</typeparam>
    public class TemplateProcessingService<TParsingDataModel, TRuleEntity> : ITemplateProcessingService<TParsingDataModel, TRuleEntity>
        where TParsingDataModel : IParsingDataModel, new()
        where TRuleEntity : AuditableEntity, new()
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
        private readonly IParsingDataValidationService<TParsingDataModel> _validationService;

        /// <summary>
        /// The rule repository
        /// </summary>
        protected readonly IRuleRepository<TRuleEntity> Repository;

        /// <summary>
        /// Creates a new instance of <see cref="TemplateProcessingService{TParsingDataModel, TRuleEntity}"/>
        /// </summary>
        /// <param name="fileParser">Excel file parser</param>
        /// <param name="repository">Pipeline rule price repository</param>
        /// <param name="validationService">Pipeline price rule validation service</param>
        public TemplateProcessingService(
            IFileParser fileParser,
            IRuleRepository<TRuleEntity> repository,
            IParsingDataValidationService<TParsingDataModel> validationService = null
            )
        {
            _fileParser = fileParser;
            _validationService = validationService;
            Repository = repository;
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
                ParsingResult<TParsingDataModel> parsingResult = GetParsingResult(fileStream);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);

                ParsedDataValidationResult<TParsingDataModel> validationResult = Validate(parsingResult.ParsedData);
                processingResult.AddValidationErrors(validationResult.Errors);

                List<TRuleEntity> validData = ConvertAndValidateOnDb(validationResult.ValidData, processingResult);

                processingResult.Count = Save(validData, userName);

                return processingResult;
            }
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="fileStream">File stream</param>
        private ParsingResult<TParsingDataModel> GetParsingResult(Stream fileStream)
        {
            ParsingResult<TParsingDataModel> parseResult;
            try
            {
                parseResult = _fileParser.Parse<TParsingDataModel>(fileStream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<TParsingDataModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Validate parsed data
        /// </summary>
        private ParsedDataValidationResult<TParsingDataModel> Validate(IEnumerable<TParsingDataModel> items)
        {
            ParsedDataValidationResult<TParsingDataModel> validationResult;
            try
            {
                validationResult = _validationService.Validate(items);
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, ErrorMessages.UnexpectedValidationError);
                validationResult = new ParsedDataValidationResult<TParsingDataModel>(new[] {
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
        private List<TRuleEntity> ConvertAndValidateOnDb(IEnumerable<TParsingDataModel> items, FileProcessingResult processingResult)
        {
            var result = new List<TRuleEntity>();
            var errors = new List<RowError>();
            foreach (TParsingDataModel item in items)
            {
                TRuleEntity record = item.Map<TParsingDataModel, TRuleEntity>();
                try
                {
                    if (IsDuplicate(record))
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
        /// Validates whether the record is duplicate
        /// </summary>
        /// <param name="record">The record to validate</param>
        protected virtual bool IsDuplicate(TRuleEntity record)
        {
            return Repository.IsDuplicate(record);
        }

        /// <summary>
        /// Verify the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        public FileProcessingDetailedResult Verify(AppDocument document)
        {
            var processingResult = new FileProcessingDetailedResult(document.FileName);

            // Get data from excel
            ParsingResult<TParsingDataModel> parsingResult = GetParsingResult(document);
            processingResult.Count = parsingResult.ParsedData.Count;
            processingResult.AddCommonErrors(parsingResult.Errors);

            ParsedDataValidationResult<TParsingDataModel> validationResult = Validate(parsingResult.ParsedData);
            processingResult.AddValidationErrors(validationResult.Errors);

            List<TRuleEntity> entities = ConvertToEntity(validationResult.ValidData);
            (var insert, var update) = CountByAction(entities);

            processingResult.Inserted = insert;
            processingResult.Updated = update;

            return processingResult;
        }

        /// <summary>
        /// Parse excel file stream
        /// </summary>
        /// <param name="document">The document to validate</param>
        private ParsingResult<TParsingDataModel> GetParsingResult(AppDocument document)
        {
            ParsingResult<TParsingDataModel> parseResult;
            try
            {
                var stream = new MemoryStream(document.FileContent);
                parseResult = _fileParser.Parse<TParsingDataModel>(stream);
            }
            catch (FileParserException)
            {
                parseResult = new ParsingResult<TParsingDataModel>(new[] { ErrorMessages.ExcelFileParsingError });
            }

            return parseResult;
        }

        /// <summary>
        /// Counts separately the number of records for insert and update 
        /// </summary>
        /// <param name="items">The collection of the records to count</param>
        private (int Insert, int Update) CountByAction(List<TRuleEntity> items)
        {
            var insert = 0;
            var update = 0;
            foreach (TRuleEntity item in items)
            {
                if (IsDuplicate(item))
                {
                    update++;
                }
                else
                {
                    insert++;
                }
            }

            return (insert, update);
        }

        /// <summary>
        /// Converts the collection of the import models to the collection of the entity models
        /// </summary>
        /// <param name="items">The collection to convert</param>
        private static List<TRuleEntity> ConvertToEntity(IEnumerable<TParsingDataModel> items)
        {
            return items.Map<TParsingDataModel, TRuleEntity>().ToList();
        }

        /// <summary>
        /// Import data from the uploaded file
        /// </summary>
        /// <param name="document">The document to validate</param>
        /// <param name="userName">Current user name</param>
        public FileProcessingDetailedResult Import(AppDocument document, string userName)
        {
            var processingResult = new FileProcessingDetailedResult(document.FileName);

            // Get data from excel
            ParsingResult<TParsingDataModel> parsingResult = GetParsingResult(document);
            processingResult.Count = parsingResult.ParsedData.Count;
            processingResult.AddCommonErrors(parsingResult.Errors);

            ParsedDataValidationResult<TParsingDataModel> validationResult = Validate(parsingResult.ParsedData);
            processingResult.AddValidationErrors(validationResult.Errors);

            List<TRuleEntity> entities = ConvertToEntity(validationResult.ValidData);
            (var inserted, var updated) = Process(entities, userName);

            processingResult.Inserted = inserted;
            processingResult.Updated = updated;

            return processingResult;
        }

        /// <summary>
        /// Add new or Update existing rule
        /// </summary>
        /// <param name="items">The Collection of the rules to process</param>
        /// <param name="userName">The user name</param>
        private (int inserted, int updated) Process(IEnumerable<TRuleEntity> items, string userName)
        {
            var insert = 0;
            var update = 0;
            foreach (TRuleEntity item in items)
            {
                item.CreatedUser = userName;
                var recordId = Repository.GetId(item);
                if (recordId == default(int))
                {
                    Repository.Add(item);
                    insert++;
                }
                else
                {
                    item.Id = recordId;
                    Repository.Update(item);
                    update++;
                }
            }

            Repository.Save();

            return (insert, update);
        }


        /// <summary>
        /// Save parsed and valid data
        /// </summary>
        /// <param name="items"><see cref="ParsingResult{T}"/></param>
        /// <param name="userName">User name</param>
        private int Save(ICollection<TRuleEntity> items, string userName)
        {
            if (!items.Any())
            {
                return items.Count;
            }

            BeforeSave(items);

            foreach (TRuleEntity item in items)
            {
                item.CreatedUser = userName;
                ProcessEntity(item);
            }

            Repository.Save();

            return items.Count;
        }

        /// <summary>
        /// Override this method to modify collection before saving to repository
        /// </summary>
        /// <param name="items">Parsed and valid data</param>
        protected virtual void BeforeSave(ICollection<TRuleEntity> items)
        {

        }

        /// <summary>
        /// Override this method to modify saving to repository
        /// </summary>
        /// <param name="entity">Mapped entity</param>

        protected virtual void ProcessEntity(TRuleEntity entity)
        {
            Repository.Add(entity);
        }
    }
}
