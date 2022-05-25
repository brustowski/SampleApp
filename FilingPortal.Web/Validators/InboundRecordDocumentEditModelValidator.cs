using FilingPortal.Domain;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FluentValidation;

namespace FilingPortal.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="InboundRecordDocumentEditModel"/>
    /// </summary>
    public class InboundRecordDocumentEditModelValidator : AbstractValidator<InboundRecordDocumentEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordDocumentEditModelValidator"/> class
        /// </summary>
        public InboundRecordDocumentEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage(ValidationMessages.DocumentTypeIsRequired);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.DocumentNameIsRequired);
        }
    }
}