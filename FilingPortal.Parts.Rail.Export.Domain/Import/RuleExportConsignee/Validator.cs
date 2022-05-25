using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.RuleExportConsignee
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        public Validator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "USPPI"))
                .Must(clientRepository.IsExist).WithMessage("Unknown USPPI client code");

            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Importer");

            RuleFor(x => x.TranRelated)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Tran Related"))
                .MaximumLength(1).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 1));
        }

    }
}