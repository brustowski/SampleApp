using FilingPortal.Cargowise.Domain.Repositories;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Vessel.RulePort
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="firmsCodesRepository">The FIRMs Code repository</param>
        public Validator(IFirmsCodesRepository firmsCodesRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.EntryPort)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.DischargePort)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.FirmsCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "FIRMs Code"))
                .Must(firmsCodesRepository.IsExist).WithMessage(string.Format(VM.PropertyInvalid, "FIRMs Code"));

            RuleFor(x => x.HMF)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}