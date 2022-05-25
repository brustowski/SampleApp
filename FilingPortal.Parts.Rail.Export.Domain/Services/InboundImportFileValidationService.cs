using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Rail.Export.Domain.Services
{
    /// <summary>
    /// Validates Imported records
    /// </summary>
    internal class InboundImportFileValidationService : IParsingDataValidationService<InboundImportModel>
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
        /// Initializes a new instance of the <see cref="InboundImportFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public InboundImportFileValidationService(
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
        public ParsedDataValidationResult<InboundImportModel> Validate(IEnumerable<InboundImportModel> records)
        {
            var validationResult = new ParsedDataValidationResult<InboundImportModel>();
            var validRecords = new List<InboundImportModel>();
            FluentValidation.IValidator<InboundImportModel> validator = _validatorFactory.Create<InboundImportModel>();
            IParseModelMap map = _parseModelMapRegistry.Get<InboundImportModel>();
            foreach (InboundImportModel record in records)
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
