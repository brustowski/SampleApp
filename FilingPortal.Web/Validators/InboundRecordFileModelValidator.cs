using FilingPortal.Domain;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FluentValidation;

namespace FilingPortal.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="InboundRecordFileModel"/>
    /// </summary>
    public class InboundRecordFileModelValidator : AbstractValidator<InboundRecordFileModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFileModelValidator"/> class
        /// </summary>
        public InboundRecordFileModelValidator()
        { 
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Documents)
                .SetCollectionValidator(new InboundRecordDocumentEditModelValidator());
            RuleFor(x => x.FilingHeaderId)
                .NotEmpty().WithMessage(ValidationMessages.FilingHeaderIdIsRequired);
        }
    }
}