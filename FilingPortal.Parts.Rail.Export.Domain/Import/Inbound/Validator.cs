using System.Linq;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.Inbound
{
    /// <summary>
    /// Validator for <see cref="InboundImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<InboundImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="portRepository">The Port Repository to check values</param>
        /// <param name="clientAddressRepository">Client Addresses repository</param>
        /// <param name="clientsRepository">Clients repository</param>
        public Validator(IDomesticPortsRepository portRepository, IClientAddressRepository clientAddressRepository, IClientRepository clientsRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Exporter"))
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 12))
                .Must(clientsRepository.IsExist)
                .WithMessage(x => $"Unknown Exporter Code: {x.Exporter}");
            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Importer"))
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 12))
                .Must(clientsRepository.IsExist)
                .WithMessage(x => $"Unknown Importer Code: {x.Importer}");
            RuleFor(x => x.TariffType)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Tariff Type"))
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Tariff"))
                .MaximumLength(35).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 35));
            RuleFor(x => x.MasterBill)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Master Bill"))
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.LoadPort)
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4))
                .Must(x => string.IsNullOrWhiteSpace(x) || portRepository.IsExist(x)).WithMessage(x => $"Load Port ({x.LoadPort}) not recognized");
            RuleFor(x => x.ExportPort)
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4))
                .Must(x => string.IsNullOrWhiteSpace(x) || portRepository.IsExist(x)).WithMessage(x => $"Export Port ({x.ExportPort}) not recognized");
            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Goods Description"))
                .MaximumLength(512).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 512));

            RuleFor(x => x.GrossWeightUOM)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Gross Weight UOM"))
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.TerminalAddress)
                .Must((model, value) => string.IsNullOrWhiteSpace(value) ||
                                        clientAddressRepository.Search(value, 1, model.Exporter).Any())
                .WithMessage(x => $"Unknown address {x.TerminalAddress} for exporter {x.Exporter}");
        }

    }
}