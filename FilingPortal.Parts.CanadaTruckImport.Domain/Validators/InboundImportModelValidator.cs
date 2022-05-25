using FilingPortal.Domain;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Import.Inbound;
using FluentValidation;
using System;
using System.Linq;
using FilingPortal.Parts.CanadaTruckImport.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Validators
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
        /// <param name="productCodeRepository">the product Code repository</param>
        public InboundImportModelValidator(IClientRepository clientRepository,
            IProductCodeRepository productCodeRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Vendor)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Vendor"))
                .MaximumLength(12).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 12))
                .Must(clientRepository.IsExist).WithMessage("Invalid Vendor");

            RuleFor(x => x.Port)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Port"))
                .MaximumLength(4).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 4));

            RuleFor(x => x.ParsNumber)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "PARS #"))
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Eta)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "ETA"));

            RuleFor(x => x.OwnersReference)
                .MaximumLength(128).WithMessage(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GrossWeight)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Gross Weight"));

            RuleFor(x => x.DirectShipDate)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Direct ship date"));

            RuleFor(x => x.Consignee)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Consignee"))
                .Must(clientRepository.IsExist).WithMessage("Unknown Consignee");

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Product"))
                .Must(productCodeRepository.IsExist).WithMessage("Unknown Product Code");

            RuleFor(x => x.InvoiceQty)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Invoice Qty"));

            RuleFor(x => x.LinePrice)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.PropertyRequired, "Line Price"));
        }
    }
}
