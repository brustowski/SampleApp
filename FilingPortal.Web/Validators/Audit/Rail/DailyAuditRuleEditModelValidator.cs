using FilingPortal.Web.Models.Audit.Rail;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Audit.Rail
{
    /// <summary>
    /// Rail Daily audit rule validator
    /// </summary>
    public class DailyAuditRuleEditModelValidator : AbstractValidator<DailyAuditRuleEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditRuleEditModelValidator"/> class
        /// </summary>
        public DailyAuditRuleEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .MaximumLength(24).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 24));
            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Goods Description"))
                .MaximumLength(525).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 525));
            RuleFor(x => x.DestinationState)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.PortCode)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.Carrier)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));
            RuleFor(x => x.ApiFrom)
                .NotEmpty()
                .When(x => x.Tariff != null && x.Tariff.StartsWith("27"))
                .WithMessage(string.Format(VM.PropertyRequired, "API from"));
            RuleFor(x => x.ApiTo)
                .NotEmpty()
                .When(x => x.Tariff != null && x.Tariff.StartsWith("27"))
                .WithMessage(string.Format(VM.PropertyRequired, "API to"));
            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Tariff"))
                .MaximumLength(50).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 50));
        }
    }
}