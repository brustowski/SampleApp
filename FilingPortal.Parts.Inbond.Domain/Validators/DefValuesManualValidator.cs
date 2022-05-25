using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Inbond.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Parts.Inbond.Domain.Validators
{
    /// <summary>
    /// Provides field validation for DefValues_Manual models
    /// </summary>
    public class DefValuesManualValidator : DefValuesManualValidator<DefValueManualReadModel>
    {
        /// <summary>
        /// The Filing headers repository
        /// </summary>
        private readonly IFilingHeaderRepository<FilingHeader> _filingHeadersRepository;

        /// <summary>
        /// Creates a new instance of field-by-field validator
        /// </summary>
        /// <param name="repository">DefValues_Manual repository</param>
        /// <param name="filingHeadersRepository">Filing headers repository</param>
        public DefValuesManualValidator(IDefValuesManualReadModelRepository<DefValueManualReadModel> repository,
            IFilingHeaderRepository<FilingHeader> filingHeadersRepository) :
            base(repository)
        {
            _filingHeadersRepository = filingHeadersRepository;
        }

        /// <summary>
        /// Validates database model
        /// </summary>
        /// <param name="models">Database model</param>
        public override IDictionary<DefValueManualReadModel, DetailedValidationResult> ValidateDatabaseModels(IEnumerable<DefValueManualReadModel> models)
        {
            IEnumerable<DefValueManualReadModel> values = models as DefValueManualReadModel[] ?? models.ToArray();
            IDictionary<DefValueManualReadModel, DetailedValidationResult> result = base.ValidateDatabaseModels(values);

            var filingHeaderId = values.First().FilingHeaderId;
            FilingHeader filingHeader = _filingHeadersRepository.Get(filingHeaderId);


            if (!filingHeader.ConfirmationNeeded)
            {
                return result;
            }
            IEnumerable<KeyValuePair<DefValueManualReadModel, DetailedValidationResult>> confirmationRecords =
                result.Where(x => x.Key.ConfirmationNeeded);

            foreach (KeyValuePair<DefValueManualReadModel, DetailedValidationResult> defValue in confirmationRecords)
            {
                DefValueManualReadModel confirmationField = result.Keys.FirstOrDefault(x =>
                    x.TableName == defValue.Key.PairedFieldTable &&
                    x.ColumnName == defValue.Key.PairedFieldColumn);

                if (confirmationField != null && confirmationField.Value != "1")
                {
                    defValue.Value.AddError($"Confirmation required for field {defValue.Key.Label}");
                }
            }

            return result;
        }
    }
}
