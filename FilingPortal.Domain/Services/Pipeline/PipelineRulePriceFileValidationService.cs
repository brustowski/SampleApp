using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Imports.Pipeline.RulePrice;
using FluentValidation;
using FluentValidation.Results;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Validates Pipeline Price Rule import file
    /// </summary>
    internal class PipelineRulePriceFileValidationService : IParsingDataValidationService<ImportModel>
    {
        /// <summary>
        /// Dictionary for unique data store
        /// </summary>
        private readonly Dictionary<string, int> _hashSet;
        /// <summary>
        /// Factory for Parsing Data Model Validators
        /// </summary>
        private readonly IParsingDataModelValidatorFactory _validatorFactory;
        /// <summary>
        /// Register of parsing models
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRulePriceFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public PipelineRulePriceFileValidationService(
            IParsingDataModelValidatorFactory validatorFactory,
            IParseModelMapRegistry parseModelMapRegistry)
        {
            _hashSet = new Dictionary<string, int>(1000);
            _validatorFactory = validatorFactory;
            _parseModelMapRegistry = parseModelMapRegistry;
        }

        /// <summary>
        /// Validate all parsed data from file
        /// </summary>
        /// <param name="records">Collection of records to validate</param>
        public ParsedDataValidationResult<ImportModel> Validate(IEnumerable<ImportModel> records)
        {
            var validationResult = new ParsedDataValidationResult<ImportModel>();
            var validRecords = new List<ImportModel>();
            IValidator<ImportModel> validator = _validatorFactory.Create<ImportModel>();
            if (validator == null)
            {
                throw new FileParserException("Validator not found");
            }

            IParseModelMap map = _parseModelMapRegistry.Get<ImportModel>();
            if (map == null)
            {
                throw new FileParserException("Parsing model not found");
            }

            foreach (ImportModel record in records)
            {
                if (record == null)
                {
                    continue;
                }

                var hash = record.ToString().ToMD5HashString();
                if (_hashSet.ContainsKey(hash))
                {
                    validationResult.AddError(new RowError(
                            record.RowNumberInFile,
                            map.SheetName,
                            ErrorLevel.Critical,
                            "",
                            string.Format(ValidationMessages.DuplicatesRecordInFile, _hashSet[hash])
                    ));
                    continue;
                }

                _hashSet.Add(hash, record.RowNumberInFile);

                ValidationResult result = validator.Validate(record);
                if (!result.IsValid)
                {
                    IEnumerable<RowError> errors = result.Errors
                        .Select(error => new RowError(
                            record.RowNumberInFile,
                            map.SheetName,
                            ErrorLevel.Critical,
                            map.GetColumnName(error.PropertyName),
                            error.ErrorMessage)
                        );
                    validationResult.AddError(errors);
                    continue;
                }
                validRecords.Add(record);
            }

            validationResult.AddData(validRecords);

            return validationResult;
        }
    }
}
