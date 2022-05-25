using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;
using FluentValidation;

namespace FilingPortal.Parts.Rail.Export.Domain.Validators
{
    /// <summary>
    /// Provides validator for <see cref="InboundImportModel"/>
    /// </summary>
    internal class InboundImportModelValidator : AbstractValidator<InboundImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundImportModelValidator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        public InboundImportModelValidator(IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage("Exporter is required")
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 24))
                .Must(clientRepository.IsExist).WithMessage("Invalid Exporter Code");

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage("Importer is required")
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 24))
                .Must(clientRepository.IsExist).WithMessage("Invalid Importer Code");

            RuleFor(x => x.MasterBill)
                .NotEmpty().WithMessage("Master Bill is required")
                .MaximumLength(20).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 20));

            RuleFor(x => x.ContainerNumber)
                .NotEmpty().WithMessage("Container Number is required")
                .MaximumLength(10).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10));

            RuleFor(x => x.LoadPort)
                .NotEmpty().WithMessage("Load Port is required")
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.ExportPort)
                .NotEmpty().WithMessage("Export Port is required")
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.TariffType)
                .NotEmpty().WithMessage("Tariff Type is required")
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage("Tariff is required")
                .MaximumLength(35).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 35));

            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage("Goods Description is required")
                .MaximumLength(512).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 512));

            RuleFor(x => x.GrossWeightUOM)
                .NotEmpty().WithMessage("Gross Weight UOM is required")
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));
        }
    }
}
