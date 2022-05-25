using FilingPortal.Web.Models.Vessel;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Vessel
{
    /// <summary>
    /// Validator for <see cref="VesselRulePortEditModel"/>
    /// </summary>
    public class VesselRulePortEditModelValidator : AbstractValidator<VesselRulePortEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortEditModelValidator"/> class
        /// </summary>
        public VesselRulePortEditModelValidator()
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.EntryPort)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.DischargePort)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.FirmsCodeId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "FIRMs Code"));

            RuleFor(x => x.HMF)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}