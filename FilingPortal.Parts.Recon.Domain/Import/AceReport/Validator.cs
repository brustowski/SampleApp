using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Recon.Domain.Import.AceReport
{
    /// <summary>
    /// Provides validator for <see cref="AceReportImportModel"/>
    /// </summary>
    public class Validator : AbstractValidator<AceReportImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterName)
                .MaximumLength(255).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 255));
            RuleFor(x => x.ImporterNumber)
                .MaximumLength(20).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.BondNumber)
                .MaximumLength(20).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.SuretyCode)
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.EntryTypeCode)
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.EntrySummaryNumber)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Entry Summary Number"))
                .MaximumLength(11).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 11));
            RuleFor(x => x.EntrySummaryDate)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Entry Summary Date"));
            RuleFor(x => x.EntrySummaryLineNumber)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Entry Summary Line Number"));
            RuleFor(x => x.ReconciliationIndicator)
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1));
            RuleFor(x => x.ReconciliationIssueCode)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.ReconciliationOtherEntrySummaryNumber)
                .MaximumLength(11).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 11));
            RuleFor(x => x.ReconciliationFtaEntrySummaryNumber)
                .MaximumLength(11).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 11));
            RuleFor(x => x.NaftaReconciliationIndicator)
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1));
            RuleFor(x => x.ReviewTeamNumber)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.CountryOfOriginCode)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.LineSpiCode)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
            RuleFor(x => x.HtsNumberFull)
                .MaximumLength(10).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 10));

            RuleFor(x => x.LineTariffQuantity)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Line Tariff Quantity"));
            RuleFor(x => x.LineGoodsValueAmount)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Line Goods Value Amount"));
            RuleFor(x => x.LineDutyAmount)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Line Duty Amount"));
            RuleFor(x => x.TotalPaidMpfAmount)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Total Paid MPF Amount"));
            RuleFor(x => x.LineMpfAmount)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Line MPF Amount"));
            RuleFor(x => x.LineHmfAmount)
                .NotNull().WithMessage(string.Format(VM.PropertyRequired, "Line HMF Amount"));


        }
    }
}
