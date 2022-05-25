using FilingPortal.PluginEngine.Models;
using FluentValidation;
using Framework.Infrastructure.Extensions;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="DefValuesEditModel"/>
    /// </summary>
    public class DefValuesEditModelValidator : AbstractValidator<DefValuesEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesEditModelValidator"/> class
        /// </summary>
        public DefValuesEditModelValidator()
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.ValueLabel)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ValueDesc)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.DefaultValue)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.UISection)
                .MaximumLength(32).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 32));

            RuleFor(x => x.DisplayOnUI)
                .NotEmpty().WithMessage(VM.DisplayOnUIIsRequired)
                .Must(x => x.ToByte() != null).WithMessage(VM.ProvidedValueNotByte);

            RuleFor(x => x.Manual)
                .NotEmpty().WithMessage(VM.ManualIsRequired)
                .Must(x => x.ToByte() != null).WithMessage(VM.ProvidedValueNotByte);
        }

    }
}