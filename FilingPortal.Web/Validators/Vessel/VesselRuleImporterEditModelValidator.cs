using FilingPortal.Web.Models.Vessel;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Vessel
{
    /// <summary>
    /// Validator for <see cref="VesselRuleImporterEditModel"/>
    /// </summary>
    public class VesselRuleImporterEditModelValidator : AbstractValidator<VesselRuleImporterEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleImporterEditModelValidator"/> class
        /// </summary>
        public VesselRuleImporterEditModelValidator()
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CWImporter)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}