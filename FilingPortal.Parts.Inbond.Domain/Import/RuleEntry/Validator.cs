using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.Inbond.Domain.Import.RuleEntry
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
        /// <param name="firmsCodesRepository">The FIRMs Code repository</param>
        /// <param name="carrierRepository">The In-Bond Carrier repository</param>
        /// <param name="entryTypeRepository">The Entry Type repository</param>
        /// <param name="domesticPortRepository">The Domestic Port repository</param>
        /// <param name="tariffRepository">The Tariff repository</param>
        /// <param name="transportModeRepository">The Transport Mode repository</param>
        /// <param name="addressRepository">The Client address repository</param>
        public Validator(IClientRepository clientRepository
            , IFirmsCodesRepository firmsCodesRepository
            , IDataProviderRepository<InBondCarrier, string> carrierRepository
            , IDataProviderRepository<EntryType, string> entryTypeRepository
            , IDataProviderRepository<DomesticPort, int> domesticPortRepository
            , ITariffRepository<HtsTariff> tariffRepository
            , ITransportModeRepository transportModeRepository
            , IClientAddressRepository addressRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.FirmsCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "FIRMs Code"))
                .Must(firmsCodesRepository.IsExist).WithMessage("Unknown FIRMs Code");

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Importer");

            RuleFor(x => x.ImporterAddress)
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.GetByCode(x) != null)
                .WithMessage("Unknown Importer Address");

            RuleFor(x => x.Carrier)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "In-Bond Carrier"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => carrierRepository.Get(x) != null).WithMessage("Unknown In-Bond Carrier");

            RuleFor(x => x.Consignee)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Consignee");

            RuleFor(x => x.ConsigneeAddress)
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.GetByCode(x) != null)
                .WithMessage("Unknown Consignee Address");

            RuleFor(x => x.UsPortOfDestination)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "US Port Of Destination"))
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4))
                .Must(x => string.IsNullOrWhiteSpace(x) || domesticPortRepository.Search(x, 2).Count == 1).WithMessage("Unknown US Port Of Destination");

            RuleFor(x => x.EntryType)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2))
                .Must(x => string.IsNullOrWhiteSpace(x) || entryTypeRepository.Get(x) != null).WithMessage("Unknown Entry Type");

            RuleFor(x => x.Tariff)
                .MaximumLength(12).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 12))
                .Must(x => string.IsNullOrWhiteSpace(x) || tariffRepository.IsExist(x)).WithMessage("Unknown Tariff");

            RuleFor(x => x.PortOfPresentation)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4))
                .Must(x => string.IsNullOrWhiteSpace(x) || domesticPortRepository.Search(x, 2).Count == 1).WithMessage("Unknown Port Of Presentation");

            RuleFor(x => x.ForeignDestination)
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));

            RuleFor(x => x.TransportMode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Transport Mode"))
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2))
                .Must(x => transportModeRepository.GetByNumber(x) != null).WithMessage("Unknown Transport Mode");

            RuleFor(x => x.ConfirmationNeeded)
                .Must(x => string.IsNullOrWhiteSpace(x) || (int.TryParse(x, out int y) && y >= 0)).WithMessage("Invalid format. Only 0, 1 or empty value is allowed.");
        }

    }
}