using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Parts.Common.Domain.Validators;
using FluentValidation;

namespace FilingPortal.Domain.Imports.TruckExport.Inbound
{
    /// <summary>
    /// Provides validator for <see cref="TruckExportImportModel"/>
    /// </summary>
    internal class TruckExportImportModelValidator : AbstractValidator<TruckExportImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportImportModelValidator"/> class
        /// </summary>
        public TruckExportImportModelValidator()
        {

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Exporter)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Exporter"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Receiver Consignee Name"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.TariffType)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "TariffType"))
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Tariff"))
                .MaximumLength(35).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 35));

            RuleFor(x => x.RoutedTran)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "RoutedTran"))
                .MaximumLength(10).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10));

            RuleFor(x => x.SoldEnRoute)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "SoldEnRoute"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.MasterBill)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "CharterRefNum"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Origin)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Port Code"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Export)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "ExportPort"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ExportDate)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Actual Border Crossing Date"));

            RuleFor(x => x.ECCN)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "ECCN"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Product"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomsQty)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Final Product Qty BBL"));

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Final Value"));

            RuleFor(x => x.GrossWeight)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Final KG"));

            RuleFor(x => x.GrossWeightUOM)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "GrossWghtUOM"))
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.Hazardous)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Hazardous"))
                .MaximumLength(3).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.OriginIndicator)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "OriginIndicator"))
                .MaximumLength(1).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 1));

            RuleFor(x => x.GoodsOrigin)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "GoodsOrigin"))
                .MaximumLength(10).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10));

        }
    }
}
