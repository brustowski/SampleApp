using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Models.Pipeline;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Pipeline
{
    /// <summary>
    /// Validator for <see cref="PipelineRuleConsigneeImporterEditModel"/>
    /// </summary>
    public class PipelineRuleConsigneeImporterEditModelValidator : AbstractValidator<PipelineRuleConsigneeImporterEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRuleConsigneeImporterEditModelValidator"/> class
        /// <param name="clientRepository">Clients repository</param>
        /// </summary>
        public PipelineRuleConsigneeImporterEditModelValidator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.ImporterFromTicket)
                .NotEmpty().WithMessage(VM.ImporterNameIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(clientRepository.IsImporterValid).WithMessage(VM.InvalidImporter);
        }
    }
}