using FilingPortal.Domain.Common.Validation;
using FilingPortal.Web.Models.VesselExport;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.VesselExport
{
    /// <summary>
    /// Vessel inbound form validator
    /// </summary>
    public class VesselExportEditModelValidator : AbstractValidator<VesselExportEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportEditModelValidator"/> class
        /// </summary>
        public VesselExportEditModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.UsppiId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "USPPI"))
                .Must(y => y.IsGuidFormat()).WithMessage(VM.IdFormatMismatch);
            RuleFor(x => x.AddressId)
                .Must(y => string.IsNullOrWhiteSpace(y) || y.IsGuidFormat()).WithMessage(VM.IdFormatMismatch);
            RuleFor(x => x.ImporterId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .Must(y => y.IsGuidFormat()).WithMessage(VM.IdFormatMismatch);
            RuleFor(x => x.VesselId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Vessel"));
            RuleFor(x => x.ExportDate)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Export Date"));
            RuleFor(x => x.LoadPort)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Load Port"));
            RuleFor(x => x.DischargePort)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Discharge Port"));
            RuleFor(x => x.CountryOfDestinationId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Country Of Destination"));
            RuleFor(x => x.TariffType)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Tariff Type"));
            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Tariff"));
            RuleFor(x => x.GoodsDescription)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Goods Description"));
            RuleFor(x => x.OriginIndicator)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Origin Indicator"))
                .Must(x => x == "D" || x == "F").WithMessage("Origin Indicator must be D of F");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Quantity"));
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Value"));
            RuleFor(x => x.TransportRef)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Transport Reference"));
            RuleFor(x => x.Container)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Container"));
            RuleFor(x => x.InBond)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "In-Bond"))
                .Must(x => x == "67" || x == "70").WithMessage("In-Bond must be 67 of 70");
            RuleFor(x => x.SoldEnRoute)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Sold En Route"));
            RuleFor(x => x.ExportAdjustmentValue)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Export Adjustment Value"));
            RuleFor(x => x.RoutedTransaction)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Routed Transaction"))
                .Must(x => x == "Y" || x == "N").WithMessage("Routed Transaction must be Y of N");
            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Reference Number"));
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Description"));
        }
    }
}