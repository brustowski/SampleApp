using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Domain.Imports.Truck.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        public Validator(IClientRepository clientRepository)
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(ValidationMessages.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsImporterValid(x)).WithMessage(ValidationMessages.InvalidImporter);

            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage(ValidationMessages.SupplierIsRequired)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsSupplierValid(x)).WithMessage(ValidationMessages.InvalidSupplier);

            RuleFor(x => x.PAPs)
                .NotEmpty().WithMessage(ValidationMessages.PAPsIsRequired)
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20));
        }
    }
}
