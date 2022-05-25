using FilingPortal.Domain.Common.Parsing;
using FluentValidation;
using FluentValidation.Results;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Validates Excel import file
    /// </summary>
    internal class TemplateValidationService<TParsingDataModel> : IParsingDataValidationService<TParsingDataModel>
    where TParsingDataModel : IParsingDataModel
    {
        /// <summary>
        /// Dictionary for unique data store
        /// </summary>
        private readonly Dictionary<string, int> _hashSet = new Dictionary<string, int>(1000);
        /// <summary>
        /// Factory for Parsing Data Model Validators
        /// </summary>
        private readonly IParsingDataModelValidatorFactory _validatorFactory;
        /// <summary>
        /// Register of parsing models
        /// </summary>
        private readonly IParseModelMapRegistry _parseModelMapRegistry;
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateValidationService{TParsingDataModel}" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public TemplateValidationService(
            IParsingDataModelValidatorFactory validatorFactory,
            IParseModelMapRegistry parseModelMapRegistry)
        {
            _validatorFactory = validatorFactory;
            _parseModelMapRegistry = parseModelMapRegistry;
        }

        /// <summary>
        /// Validate all parsed data from file
        /// </summary>
        /// <param name="records">Collection of records to validate</param>
        public ParsedDataValidationResult<TParsingDataModel> Validate(IEnumerable<TParsingDataModel> records)
        {
            var validationResult = new ParsedDataValidationResult<TParsingDataModel>();
            var validRecords = new List<TParsingDataModel>();

            IValidator<TParsingDataModel> validator = _validatorFactory.Create<TParsingDataModel>();
            if (validator == null)
            {
                throw new FileParserException("Validator not found");
            }

            IParseModelMap map = _parseModelMapRegistry.Get<TParsingDataModel>();
            if (map == null)
            {
                throw new FileParserException("Parsing model not found");
            }

            foreach (TParsingDataModel record in records)
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
