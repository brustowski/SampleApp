using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.TruckExport;
using FluentValidation.Results;
using Framework.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Validates Imported Truck Export records
    /// </summary>
    internal class TruckExportFileValidationService : IParsingDataValidationService<TruckExportImportModel>
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
        /// Initializes a new instance of the <see cref="TruckExportFileValidationService" /> class
        /// </summary>
        /// <param name="validatorFactory">Factory for Parsing Data Model Validators</param>
        /// <param name="parseModelMapRegistry">Register of parsing models</param>
        public TruckExportFileValidationService(
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
        public ParsedDataValidationResult<TruckExportImportModel> Validate(IEnumerable<TruckExportImportModel> records)
        {
            var validationResult = new ParsedDataValidationResult<TruckExportImportModel>();
            var validRecords = new List<TruckExportImportModel>();
            FluentValidation.IValidator<TruckExportImportModel> validator = _validatorFactory.Create<TruckExportImportModel>();
            IParseModelMap map = _parseModelMapRegistry.Get<TruckExportImportModel>();
            foreach (TruckExportImportModel record in records)
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
