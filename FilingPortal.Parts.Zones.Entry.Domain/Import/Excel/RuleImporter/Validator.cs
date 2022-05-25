using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.RuleImporter
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage("Importer Code is required")
                .Must(clientRepository.IsExist).WithMessage("Unknown Importer");

            RuleFor(x => x.Rlf)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.F3461Box29)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Consignee)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Consignee");
            RuleFor(x => x.Manufacturer)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Manufacturer");
            RuleFor(x => x.Supplier)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Supplier");
            RuleFor(x => x.Seller)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Seller");
            RuleFor(x => x.SoldToParty)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Sold To Party");
            RuleFor(x => x.ShipToParty)
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage("Unknown Ship To Party");
            RuleFor(x => x.GoodsDescription)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.ReconIssue)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));
        }

    }
}