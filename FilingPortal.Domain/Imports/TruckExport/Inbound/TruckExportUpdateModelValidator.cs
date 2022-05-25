using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Domain.Imports.TruckExport.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="TruckExportUpdateModel"/>
    /// </summary>
    internal class TruckExportUpdateModelValidator : AbstractValidator<TruckExportUpdateModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportUpdateModelValidator"/> class
        /// </summary>
        public TruckExportUpdateModelValidator()
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Exporter"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.MasterBill)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "CharterRefNum"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Importer)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.TariffType)
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.Tariff)
                .MaximumLength(35).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 35));

            RuleFor(x => x.Export)
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.RoutedTran)
                .MaximumLength(10).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10));

            RuleFor(x => x.SoldEnRoute)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Origin)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ECCN)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GoodsDescription)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GrossWeightUOM)
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.Hazardous)
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.OriginIndicator)
                .MaximumLength(1).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 1));

            RuleFor(x => x.GoodsOrigin)
                .MaximumLength(10).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10));
        }
    }
}
