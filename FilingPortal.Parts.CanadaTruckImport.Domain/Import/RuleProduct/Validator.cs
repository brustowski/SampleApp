using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FluentValidation;
using System;
using System.Linq;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleProduct
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="productCodeRepository">the product Code repository</param>
        public Validator(IProductCodeRepository productCodeRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Product"))
                .Must(productCodeRepository.IsExist).WithMessage("Unknown Product Code");

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