using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Domain.Imports.Pipeline.RulePrice
{
    /// <summary>
    /// Provides validator for <see cref="ImportModel"/>
    /// </summary>
    public class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// /// <param name="clientRepository">The Clients repository </param>
        /// <param name="batchRepository">The Batch code repository</param>
        /// <param name="facilityRepository">The facility repository</param>
        public Validator(
            IClientRepository clientRepository,
            IPipelineRuleBatchCodeRepository batchRepository,
            IPipelineRuleFacilityRepository facilityRepository
            )
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(ValidationMessages.ImporterIsRequired)
                .MaximumLength(200).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 200))
                .Must(x => clientRepository.GetClientByCode(x) != null).WithMessage(x => $"Importer {x.ImporterCode} not found");

            RuleFor(x => x.BatchCode)
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20))
                .Must(x => string.IsNullOrWhiteSpace(x) || batchRepository.GetByBatchCode(x) != null).WithMessage(x => $"Batch Rule with code ${x.BatchCode} not found");

            RuleFor(x => x.Facility)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || facilityRepository.GetByFacilityName(x) != null)
                .WithMessage(x => $"Facility Rule with name {x.Facility} not found");

            RuleFor(x => x.Pricing).NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Pricing"))
                .Must(x => x.IsDecimalFormat()).WithMessage(ValidationMessages.DecimalFormatMismatch);

            RuleFor(x => x.Freight)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Freight"))
                .Must(x => x.IsDecimalFormat()).WithMessage(ValidationMessages.DecimalFormatMismatch);
        }
    }
}
