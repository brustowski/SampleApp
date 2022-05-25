using FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="RulePortEditModel"/>
    /// </summary>
    public class RulePortEditModelValidator : AbstractValidator<RulePortEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RulePortEditModelValidator"/> class
        /// </summary>
        public RulePortEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.PortOfClearance)
                .NotEmpty().WithMessage("Port Of Clearance is required")
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.FirstPortOfArrival)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));

            RuleFor(x => x.FinalDestination)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));

            RuleFor(x => x.SubLocation)
                .MaximumLength(50).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 50));
        }

    }
}