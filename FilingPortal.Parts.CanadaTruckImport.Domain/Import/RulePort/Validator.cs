using FilingPortal.Cargowise.Domain.Repositories;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RulePort
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator(IPortsOfClearanceRepository portsOfClearanceRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.PortOfClearance)
                .NotEmpty().WithMessage("Port Of Clearance is required")
                .Length(4).WithMessage(string.Format(VM.ValueNotEqualSpecifiedLength, 4))
                .Must(portsOfClearanceRepository.IsExist).WithMessage("Unknown Port Of Clearance");

            RuleFor(x => x.FirstPortOfArrival)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));

            RuleFor(x => x.FinalDestination)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));

            RuleFor(x => x.SubLocation)
                .MaximumLength(50).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 50));
        }

    }
}