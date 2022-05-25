using FilingPortal.Web.Models.Audit.Rail;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Audit.Rail
{
    /// <summary>
    /// Rail Daily audit SPI rule validator
    /// </summary>
    public class DailyAuditSpiRuleEditModelValidator : AbstractValidator<DailyAuditSpiRuleEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditSpiRuleEditModelValidator"/> class
        /// </summary>
        public DailyAuditSpiRuleEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .MaximumLength(24).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 24));
            RuleFor(x => x.SupplierCode)
                .MaximumLength(24).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 24));
            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Goods Description"))
                .MaximumLength(525).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 525));
            RuleFor(x => x.DateFrom)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Date From"))
                .LessThanOrEqualTo(x => x.DateTo);
            RuleFor(x => x.DateTo)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Date To"));
            RuleFor(x => x.Spi)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "SPI"));
            RuleFor(x => x.DestinationState)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
        }
    }
}