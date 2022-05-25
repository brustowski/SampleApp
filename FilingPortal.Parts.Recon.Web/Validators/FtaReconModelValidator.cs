using FilingPortal.Parts.Recon.Web.Models;
using FluentValidation;

namespace FilingPortal.Parts.Recon.Web.Validators
{
    /// <summary>
    /// Provides methods for single <see cref="FtaReconViewModel"/> record validation
    /// </summary>
    public class FtaReconModelValidator : AbstractValidator<FtaReconViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconModelValidator"/> class
        /// </summary>
        public FtaReconModelValidator()
        {
            CascadeMode = CascadeMode.Continue;

            RuleFor(x => x.PsaReason)
                .Must((model, value) => string.IsNullOrWhiteSpace(model.PsaReason) || model.PsaFiledDate.HasValue)
                .WithMessage("PSA Reason is flagged and not filed")
                .WithSeverity(Severity.Warning);
        }
    }
}