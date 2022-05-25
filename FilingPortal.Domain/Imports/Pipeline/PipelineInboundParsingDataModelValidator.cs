using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Domain.Imports.Pipeline
{
    /// <summary>
    /// Provides validator for <see cref="PipelineInboundImportParsingModel"/>
    /// </summary>
    public class PipelineInboundParsingDataModelValidator : AbstractValidator<PipelineInboundImportParsingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundParsingDataModelValidator"/> class
        /// </summary>
        public PipelineInboundParsingDataModelValidator()
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(ValidationMessages.ImporterIsRequired)
                .MaximumLength(200).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 200));
            RuleFor(x => x.Batch).NotEmpty().WithMessage(ValidationMessages.BatchIsRequired)
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.TicketNumber).NotEmpty().WithMessage(ValidationMessages.TicketNumberIsRequired)
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage(ValidationMessages.QuantityIsRequired);
            RuleFor(x => x.API)
                .NotEmpty().WithMessage(ValidationMessages.APIIsRequired);
            RuleFor(x => x.ExportDate)
                .NotEmpty().WithMessage(ValidationMessages.ExportDateIsRequired);
            RuleFor(x => x.ImportDate)
                .NotEmpty().WithMessage(ValidationMessages.ImportDateIsRequired);
            RuleFor(x => x.EntryNumber)
                .MaximumLength(11).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 11));
        }
    }
}
