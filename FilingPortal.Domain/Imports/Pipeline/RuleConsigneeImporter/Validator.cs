using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Pipeline.RuleConsigneeImporter
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// <param name="clientRepository">Clients repository</param>
        /// </summary>
        public Validator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterFromTicket)
                .NotEmpty().WithMessage(VM.ImporterNameIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(clientRepository.IsImporterValid).WithMessage(VM.InvalidImporter);
        }
    }
}