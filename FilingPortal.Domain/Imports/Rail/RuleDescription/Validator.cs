using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Rail.RuleDescription
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="portRepository">The Port Repository to check values</param>
        public Validator(ILookupMasterRepository<LookupMaster> portRepository)
        {
            
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Description1)
                .NotEmpty().WithMessage(VM.Description1IsRequired)
                .MaximumLength(500).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 500));

            RuleFor(x => x.Importer)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .NotEmpty().When(record =>
                    !string.IsNullOrWhiteSpace(record.Supplier) || !string.IsNullOrWhiteSpace(record.Port) ||
                    !string.IsNullOrWhiteSpace(record.Destination))
                .WithMessage(VM.ImporterIsRequired)
                ;
            RuleFor(x => x.Supplier)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .NotEmpty().When(record =>
                    !string.IsNullOrWhiteSpace(record.Importer) || !string.IsNullOrWhiteSpace(record.Port) ||
                    !string.IsNullOrWhiteSpace(record.Destination))
                .WithMessage(VM.SupplierIsRequired)
                ;
            RuleFor(x => x.Port)
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4))
                .NotEmpty().When(record => !string.IsNullOrWhiteSpace(record.Destination))
                .Must(x => string.IsNullOrWhiteSpace(x) || portRepository.IsExist(x)).WithMessage(VM.InvalidPort)
                ;

            RuleFor(x => x.Destination)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2))
                ;

            RuleFor(x => x.ProductID)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Attribute1)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Tariff)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Tariff"))
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GoodsDescription)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.InvoiceUOM)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.TemplateHTSQuantity)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.TemplateInvoiceQuantity)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

        }

    }
}