using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FluentValidation;

namespace FilingPortal.Parts.Inbond.Domain.Import.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="firmsCodesRepository">The FIRMs code repository</param>
        /// <param name="clientRepository">The Client repository</param>
        /// <param name="carrierRepository">The In-Bond Carrier repository</param>
        public Validator(IFirmsCodesRepository firmsCodesRepository
            , IClientRepository clientRepository
            , IDataProviderRepository<InBondCarrier, string> carrierRepository)
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FirmsCode)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "FIRMs Code"))
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4))
                .Must(firmsCodesRepository.IsExist).WithMessage("Unknown FIRMs Code");

            RuleFor(x => x.ImporterCode)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Importer Code"))
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 12))
                .Must(clientRepository.IsExist).WithMessage("Unknown Importer Code");

            RuleFor(x => x.PortOfArrival)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Port of Arrival"))
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.PortOfDestination)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Port of Destination"))
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.ExportConveyance)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ConsigneeCode)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Consignee Code"))
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 12))
                .Must(clientRepository.IsExist).WithMessage("Unknown Consignee Code");

            RuleFor(x => x.Carrier)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "In-Bond Carrier"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128))
                .Must(x => carrierRepository.Get(x) != null).WithMessage("Unknown In-Bond Carrier");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Value"));

            RuleFor(x => x.ManifestQty)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Manifest Quantity"));

            RuleFor(x => x.ManifestQtyUnit)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Manifest Quantity Unit"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Weight"));
        }
    }
}
