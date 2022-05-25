using FilingPortal.Web.Models.Vessel;
using FluentValidation;
using Framework.Infrastructure.Extensions;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Vessel
{
    /// <summary>
    /// Validator for <see cref="VesselDefValuesEditModel"/>
    /// </summary>
    public class VesselDefValuesEditModelValidator : AbstractValidator<VesselDefValuesEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselDefValuesEditModelValidator"/> class
        /// </summary>
        public VesselDefValuesEditModelValidator()
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);
            
            RuleFor(x => x.ValueLabel)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Value Label"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ValueDesc)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.DefaultValue)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128)); 

            RuleFor(x => x.TableName)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Table Name"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ColumnName)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Column Name"))
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