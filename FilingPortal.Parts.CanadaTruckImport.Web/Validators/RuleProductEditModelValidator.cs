using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleProduct;
using FluentValidation;
using System;
using FilingPortal.Parts.Common.Domain.Repositories;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="RuleProductEditModel"/>
    /// </summary>
    public class RuleProductEditModelValidator : AbstractValidator<RuleProductEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleProductEditModelValidator"/> class
        /// </summary>
        /// <param name="productCodeRepository">the Product Code repository</param>
        public RuleProductEditModelValidator(IDataProviderRepository<ProductCode, Guid> productCodeRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.ProductCodeId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Product"))
                .Must(x => x.IsGuidFormat()).WithMessage(VM.IdFormatMismatch)
                .Must(x => Guid.TryParse(x, out Guid id) && productCodeRepository.Get(id) != null).WithMessage("Unknown Product");

            RuleFor(x => x.GrossWeightUnit)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Gross Weight Unit"))
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));

                RuleFor(x => x.PackagesUnit)
                    .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Packages Unit"))
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));

            RuleFor(x => x.InvoiceUQ)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Invoice UQ"))
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
        }

    }
}