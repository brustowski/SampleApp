using FilingPortal.Web.Models.Pipeline;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Pipeline
{
    /// <summary>
    /// Validator for <see cref="PipelineRuleBatchCodeEditModelValidator"/>
    /// </summary>
    public class PipelineRuleBatchCodeEditModelValidator : AbstractValidator<PipelineRuleBatchCodeEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleBatchCodeEditModelValidator"/> class
        /// </summary>
        public PipelineRuleBatchCodeEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.BatchCode)
                .NotEmpty().WithMessage(VM.BatchIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Product)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}