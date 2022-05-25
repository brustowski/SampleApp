using FilingPortal.Domain.Imports.Audit.Rule;
using FluentValidation;

namespace FilingPortal.Domain.Imports.Audit.RuleSpi
{
    /// <summary>
    /// Validator for <see cref="DailyAuditRuleImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<DailyAuditSpiRuleImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode).MaximumLength(24).NotEmpty();
            RuleFor(x => x.ImporterCode).MaximumLength(24);
            RuleFor(x => x.GoodsDescription).MaximumLength(525).NotEmpty();
            RuleFor(x => x.DestinationState).MaximumLength(2);
            RuleFor(x => x.DateFrom).NotEmpty();
            RuleFor(x => x.DateTo).NotEmpty();
            RuleFor(x => x.Spi).MaximumLength(3).NotEmpty();
        }
    }
}