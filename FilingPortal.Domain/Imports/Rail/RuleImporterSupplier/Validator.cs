using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using Framework.Infrastructure.Extensions;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Rail.RuleImporterSupplier
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="repository">The client repository to check values</param>
        /// <param name="addressRepository">The Client Address repository</param>
        /// <param name="portRepository">Ports repository</param>
        public Validator(IClientRepository repository, IClientAddressRepository addressRepository, ILookupMasterRepository<LookupMaster> portRepository)
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterName)
                .NotEmpty().WithMessage(VM.ImporterNameIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.SupplierName)
                .NotEmpty().WithMessage(VM.SupplierNameIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Port)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4))
                .Must(x => string.IsNullOrWhiteSpace(x) || portRepository.IsExist(x)).WithMessage(VM.InvalidPort)
                ;

            RuleFor(x => x.Destination)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2))
                ;

            RuleFor(x => x.MainSupplier)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Main Supplier"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsSupplierValid(x)).WithMessage(VM.InvalidSupplier);

            RuleFor(x => x.MainSupplierAddress)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.GetByCode(x) != null).WithMessage(VM.InvalidSupplierAddress);

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsImporterValid(x)).WithMessage(VM.InvalidImporter);

            RuleFor(x => x.DestinationState)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Consignee)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsImporterValid(x)).WithMessage(VM.InvalidConsignee);

            RuleFor(x => x.Manufacturer)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsSupplierValid(x)).WithMessage(VM.InvalidManufacturer);

            RuleFor(x => x.ManufacturerAddress)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || addressRepository.GetByCode(x) != null).WithMessage(VM.InvalidManufacturerAddress);

            RuleFor(x => x.Seller)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsSupplierValid(x)).WithMessage(VM.InvalidSeller);

            RuleFor(x => x.SoldToParty)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsImporterValid(x)).WithMessage(VM.InvalidSoldToParty);

            RuleFor(x => x.ShipToParty)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsImporterValid(x)).WithMessage(VM.InvalidShipToParty);

            RuleFor(x => x.CountryOfOrigin)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Relationship)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.DFT)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ValueRecon)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.FTARecon)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.BrokerToPay)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Value)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.PaymentType)
                .Must(x => string.IsNullOrEmpty(x) || x.ToInt() != null).WithMessage(VM.ProvidedValueNotInteger);

            RuleFor(x => x.Freight)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);
            RuleFor(x => x.FreightType)
                .Must(x => string.IsNullOrEmpty(x) || x == "0" || x == "1").WithMessage(VM.ProvidedValueNotInteger);

        }

    }
}