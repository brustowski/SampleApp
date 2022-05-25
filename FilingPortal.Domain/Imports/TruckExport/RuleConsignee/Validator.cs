using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.TruckExport.RuleConsignee
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

            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(VM.ConsigneeCodeIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Country)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.Destination)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.UltimateConsigneeType)
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1));
        }
    }
}