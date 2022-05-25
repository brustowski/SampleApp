using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Web.Models.Pipeline;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Pipeline
{
    /// <summary>
    /// Validator for <see cref="PipelineRuleFacilityEditModelValidator"/>
    /// </summary>
    public class PipelineRuleFacilityEditModelValidator : AbstractValidator<PipelineRuleFacilityEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleFacilityEditModelValidator"/> class
        /// </summary>
        public PipelineRuleFacilityEditModelValidator(ILookupMasterRepository<LookupMaster> repository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.Facility)
                .NotEmpty().WithMessage(VM.FacilityIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Port)
                .NotEmpty().WithMessage(VM.PortIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);
            RuleFor(x => x.Issuer)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Origin)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidOrigin);
            RuleFor(x => x.Pipeline)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Destination)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidDestination);
            RuleFor(x => x.DestinationState)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.FIRMsCode)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidFIRMs);
        }
    }
}