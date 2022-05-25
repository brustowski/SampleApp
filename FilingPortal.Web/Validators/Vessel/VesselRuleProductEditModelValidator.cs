using FilingPortal.Web.Models.Vessel;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Vessel
{
    /// <summary>
    /// Validator for <see cref="VesselRuleProductEditModel"/>
    /// </summary>
    public class VesselRuleProductEditModelValidator : AbstractValidator<VesselRuleProductEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleProductEditModelValidator"/> class
        /// </summary>
        public VesselRuleProductEditModelValidator()
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(VM.TariffIsRequired)
                .MaximumLength(10).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 10));

            RuleFor(x => x.GoodsDescription)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomsAttribute1)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomsAttribute2)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.InvoiceUQ)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.TSCARequirement)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}