using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Truck.RulePort
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="repository">The common lookup data repository</param>
        public Validator(ILookupMasterRepository<LookupMaster> repository)
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.EntryPort)
                .NotEmpty().WithMessage(VM.EntryPortIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.ArrivalPort)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.FIRMsCode)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidFIRMs); 
        }
    }
}