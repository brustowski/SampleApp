using FilingPortal.Parts.Zones.Entry.Web.Models;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Zones.Entry.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="RuleImporterEditModel"/>
    /// </summary>
    public class RuleImporterEditModelValidator : AbstractValidator<RuleImporterEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleImporterEditModelValidator"/> class
        /// </summary>
        public RuleImporterEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.ImporterId)
                .NotEmpty().WithMessage(string.Format(VM.ImporterIsRequired));

            RuleFor(x => x.F3461Box29)
                .MaximumLength(800).WithMessage(@"The field 3461 Box 29 exceeds 800 characters long");


        }

    }
}