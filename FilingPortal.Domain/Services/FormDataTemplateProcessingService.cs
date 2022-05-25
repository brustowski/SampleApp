using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Represents the form data template processing service
    /// </summary>
    internal class FormDataTemplateProcessingService<TParsingDataModel, TEntity> : IFormDataTemplateProcessingService<TParsingDataModel, TEntity>
        where TParsingDataModel : ParsingDataModelDynamic, new()
        where TEntity : BaseDefValuesManual, new()
    {
        /// <summary>
        /// Locker is used to prevent multiple file processing at the same time
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// The file parser
        /// </summary>
        private readonly IFileParser _fileParser;

        /// <summary>
        /// The model map registry
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;

        /// <summary>
        /// Data repository
        /// </summary>
        private readonly IDefValuesManualRepository<TEntity> _defValuesManualRepository;

        /// <summary>
        /// Parsed model validation service
        /// </summary>
        private readonly IParsingDataValidationService<TParsingDataModel> _validationService;

        /// <summary>
        /// Initialize a new instance of the <see cref="FormDataTemplateProcessingService{TParsingDataModel,TEntity}"/> class.
        /// </summary>
        /// <param name="fileParser">The file parser</param>
        /// <param name="parseModelMapRegistry">The model map registry</param>
        /// <param name="defValuesManualRepository">The def values manual repository</param>
        /// <param name="validationService">The validation service</param>
        public FormDataTemplateProcessingService(IFileParser fileParser
            , IParseModelMapRegistry parseModelMapRegistry
            , IDefValuesManualRepository<TEntity> defValuesManualRepository
            , IParsingDataValidationService<TParsingDataModel> validationService)
        {
            _fileParser = fileParser;
            _parseModelMapRegistry = parseModelMapRegistry;
            _defValuesManualRepository = defValuesManualRepository;
            _validationService = validationService;
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="filePath">File path</param>
        /// <param name="configuration">Import configuration</param>
        public FileProcessingDetailedResult Process(string fileName, string filePath, IFormImportConfiguration configuration)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", Path.GetFileName(filePath));
            }

            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return Process(fileName, fileStream, configuration);
            }
        }

        /// <summary>
        /// Processing the file
        /// </summary>
        /// <param name="fileName">Import file Name</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="configuration">Import configuration</param>
        public FileProcessingDetailedResult Process(string fileName, Stream fileStream, IFormImportConfiguration configuration)
        {
            lock (Locker)
            {
                var processingResult = new FileProcessingDetailedResult(fileName);

                var modelMap = (IParseModelMapFormData)_parseModelMapRegistry.Get<TParsingDataModel>();
                modelMap.Section = configuration.Section;

                // Get data from excel
                ParsingResult<TParsingDataModel> parsingResult = GetParsingResult(fileStream, modelMap);
                processingResult.AddCommonErrors(parsingResult.Errors);
                processingResult.AddParsingErrors(parsingResult.RowErrors);
                AddSectionName(parsingResult, configuration.Section);
                ParsedDataValidationResult<TParsingDataModel> validationResult = Validate(parsingResult.ParsedData.ToArray());
                processingResult.AddValidationErrors(validationResult.Errors);
                
                List<ImportFormParameter> filingParams = Convert(validationResult.ValidData, configuration.ParentRecordId, configuration.FilingHeaderId);
                (var inserted, var updated) = Save(filingParams);
                processingResult.Inserted = inserted;
                processingResult.Updated = updated;
                processingResult.Count = validationResult.ValidData.Count;

                return processingResult;
            }
        }

        private ParsingResult<TParsingDataModel> GetParsingResult(Stream fileStream, IParseModelMapFormData modelMap)
        {
            _fileParser.SetModelMap(modelMap);
            return _fileParser.Parse<TParsingDataModel>(fileStream);
        }

        private void AddSectionName(ParsingResult<TParsingDataModel> parsingResult, string sectionName)
        {
            foreach (TParsingDataModel model in parsingResult.ParsedData)
            {
                model.Section = sectionName;
            }
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

        private List<ImportFormParameter> Convert(IEnumerable<TParsingDataModel> parsingResult, int parentRecordId, int filingHeaderId)
        {
            var filingParams = new List<ImportFormParameter>();
            foreach (TParsingDataModel model in parsingResult)
            {
                IDictionary<string, object> properties = model.GetProperties();
                filingParams.AddRange(properties.Where(x=>x.Value != null).Select(x => new ImportFormParameter
                {
                    Section = model.Section,
                    Column = x.Key,
                    Value = x.Value.ToString(),
                    RowNumber = model.RowNumberInFile,
                    ParentRecordId = parentRecordId,
                    FilingHeaderId = filingHeaderId
                }));
            }

            return filingParams;
        }

        private (int inserted, int updated) Save(IEnumerable<ImportFormParameter> filingParameters)
        {
            return _defValuesManualRepository.ImportValues(filingParameters);
        }
    }
}
