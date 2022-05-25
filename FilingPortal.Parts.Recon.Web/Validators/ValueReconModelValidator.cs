using System;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Web.Models;
using FluentValidation;
using FluentValidation.Results;

namespace FilingPortal.Parts.Recon.Web.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="ValueReconViewModel"/> record validation
    /// </summary>
    public class ValueReconModelValidator : AbstractValidator<ValueReconViewModel>
    {
        private const decimal WarningThreshold = 0.2M;
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconModelValidator"/> class
        /// </summary>
        public ValueReconModelValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.FinalTotalValue)
                .Must((model, value) =>
                {
                    var enteredValue = model.LineEnteredValue ?? 0;
                    var totalValue = model.FinalTotalValue ?? 0;
                    return enteredValue == 0 || totalValue == 0 ||
                           WarningThreshold > Math.Abs(1 - enteredValue / totalValue);
                })
                .WithMessage("Large value change")
                .WithSeverity(Severity.Warning);
            RuleFor(x => x.PsaReason)
                .Must((model, value) => string.IsNullOrWhiteSpace(model.PsaReason) || model.PsaFiledDate.HasValue)
                .WithMessage("PSA Reason is flagged and not filed")
                .WithSeverity(Severity.Warning);
            RuleFor(x => x.PsaReason520d)
                .Must((model, value) => string.IsNullOrWhiteSpace(model.PsaReason520d) || model.PsaFiledDate520d.HasValue)
                .WithMessage("PSA reason 520D is flagged and not filed")
                .WithSeverity(Severity.Warning);
        }
    }
}