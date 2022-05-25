using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Admin.AutoCreate
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ShipmentType)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.EntryType)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.TransportMode)
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.ImporterExporter)
                .MaximumLength(24).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 24));
        }
    }
}