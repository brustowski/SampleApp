using System.Collections.Generic;
using System.Data;
using System.Linq;
using FilingPortal.Domain.Common.Parsing;
using FluentValidation;
using FluentValidation.Results;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Services
{
    /// <summary>
    /// Validates Pipeline Inbound records
    /// </summary>
    internal class InboundXmlFileValidationService : IParsingDataValidationService<FTZ_214FTZ_ADMISSION>
    {
        private IParsingDataModelValidatorFactory _validatorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundXmlFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        public InboundXmlFileValidationService(
            IParsingDataModelValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        /// <summary>
        /// Validate all parsed data from file
        /// </summary>
        /// <typeparam name="T">Parsing data model type</typeparam>
        /// <param name="records">Collection of records to validate</param>
        public ParsedDataValidationResult<FTZ_214FTZ_ADMISSION> Validate(IEnumerable<FTZ_214FTZ_ADMISSION> records)
        {
            var validationResult = new ParsedDataValidationResult<FTZ_214FTZ_ADMISSION>();
            var validRecords = new List<FTZ_214FTZ_ADMISSION>();
            IValidator<FTZ_214FTZ_ADMISSION> validator = _validatorFactory.Create<FTZ_214FTZ_ADMISSION>();
            if (validator == null)
            {
                throw new FileParserException("Validator not found");
            }

            foreach (FTZ_214FTZ_ADMISSION record in records)
            {
                if (record == null)
                {
                    continue;
                }

                ValidationResult result = validator.Validate(record);
                if (!result.IsValid)
                {
                    IEnumerable<RowError> errors = result.Errors
                        .Select(error => new RowError(
                            record.RowNumberInFile,
                            string.Empty,
                            ErrorLevel.Critical,
                            error.PropertyName,
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
