using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Web.Models.Truck;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Truck
{
    /// <summary>
    /// Validator for <see cref="TruckRulePortEditModel"/>
    /// </summary>
    public class TruckRulePortEditModelValidator : AbstractValidator<TruckRulePortEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRulePortEditModelValidator"/> class
        /// </summary>
        public TruckRulePortEditModelValidator(ILookupMasterRepository<LookupMaster> repository)
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.EntryPort)
                .NotEmpty().WithMessage(VM.EntryPortIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.ArrivalPort)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.FIRMsCode)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidFIRMs); 
        }
    }
}