using FilingPortal.Domain;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Inbond.Domain.Import.Inbound;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Parts.Inbond.Domain.Services
{
    /// <summary>
    /// Validates Imported Inbond Import records
    /// </summary>
    internal class InbondImportFileValidationService : IParsingDataValidationService<ImportModel>
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
        /// Initializes a new instance of the <see cref="InbondImportFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public InbondImportFileValidationService(
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
            FluentValidation.IValidator<ImportModel> validator = _validatorFactory.Create<ImportModel>();
            IParseModelMap map = _parseModelMapRegistry.Get<ImportModel>();
            foreach (ImportModel record in records)
            {
                if (record == null)
                {
                    continue;
                }

                FluentValidation.Results.ValidationResult result = validator.Validate(record);
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
