using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Audit.Rule
{
    /// <summary>
    /// Validator for <see cref="DailyAuditRuleImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<DailyAuditRuleImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        public Validator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ImporterCode).MaximumLength(24).NotEmpty();
            RuleFor(x => x.Tariff).MaximumLength(50).NotEmpty();
            RuleFor(x => x.GoodsDescription).MaximumLength(525).NotEmpty();
            RuleFor(x => x.ConsigneeCode).MaximumLength(24);
            RuleFor(x => x.SupplierCode).MaximumLength(24);
            RuleFor(x => x.DestinationState).MaximumLength(2);
            RuleFor(x => x.PortCode).MaximumLength(5);
            RuleFor(x => x.Carrier).MaximumLength(4);
            RuleFor(x => x.CountryOfOrigin).MaximumLength(2);
            RuleFor(x => x.SupplierMid).MaximumLength(16);
            RuleFor(x => x.ManufacturerMid).MaximumLength(16);
            RuleFor(x => x.ExportingCountry).MaximumLength(2);
            RuleFor(x => x.UltimateConsigneeName).MaximumLength(500);
            RuleFor(x => x.InvoiceQtyUnit).MaximumLength(3);
            RuleFor(x => x.InvoiceQtyUnit).MaximumLength(3);
            RuleFor(x => x.CustomsQtyUnit).MaximumLength(3);
            RuleFor(x => x.GrossWeightUq).MaximumLength(3);
            RuleFor(x => x.CustomsAttrib1).MaximumLength(50);
            RuleFor(x => x.ValueRecon).MaximumLength(2);
            RuleFor(x => x.NaftaRecon).MaximumLength(1);
        }
    }
}