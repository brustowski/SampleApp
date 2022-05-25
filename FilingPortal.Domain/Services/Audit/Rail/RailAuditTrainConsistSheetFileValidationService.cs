using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Audit.Rail;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services.Audit.Rail
{
    /// <summary>
    /// Validates Rail audit train consist sheet import file
    /// </summary>
    internal class RailAuditTrainConsistSheetFileValidationService : IParsingDataValidationService<RailAuditTrainConsistSheetImportParsingModel>
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
        /// Initializes a new instance of the <see cref="RailAuditTrainConsistSheetFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public RailAuditTrainConsistSheetFileValidationService(
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
        /// <typeparam name="T">Parsing data model type</typeparam>
        /// <param name="records">Collection of records to validate</param>
        public ParsedDataValidationResult<RailAuditTrainConsistSheetImportParsingModel> Validate(IEnumerable<RailAuditTrainConsistSheetImportParsingModel> records)
        {
            var validationResult = new ParsedDataValidationResult<RailAuditTrainConsistSheetImportParsingModel>();
            var validRecords = new List<RailAuditTrainConsistSheetImportParsingModel>();
            IValidator<RailAuditTrainConsistSheetImportParsingModel> validator = _validatorFactory.Create<RailAuditTrainConsistSheetImportParsingModel>();
            if (validator == null)
            {
                throw new FileParserException("Validator not found");
            }

            IParseModelMap map = _parseModelMapRegistry.Get<RailAuditTrainConsistSheetImportParsingModel>();
            if (map == null)
            {
                throw new FileParserException("Parsing model not found");
            }

            foreach (RailAuditTrainConsistSheetImportParsingModel record in records)
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
