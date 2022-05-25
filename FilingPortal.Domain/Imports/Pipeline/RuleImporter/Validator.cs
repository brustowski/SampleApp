using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Domain.Imports.Pipeline.RuleImporter
{
    /// <summary>
    /// Validator for <see cref="ImportModel"/>
    /// </summary>
    internal class Validator : AbstractValidator<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Validator"/> class
        /// </summary>
        /// <param name="repository">The repository of the common lookup data</param>
        /// <param name="clientRepository">The Clients repository </param>
        public Validator(ILookupMasterRepository<LookupMaster> repository, IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsImporterValid(x)).WithMessage(VM.InvalidImporter); 
            RuleFor(x => x.Consignee)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.CountryOfExport)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidExport); 
            RuleFor(x => x.Freight)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);
            RuleFor(x => x.FTARecon)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Manufacturer)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.ManufacturerAddress)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.MID)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Origin)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.ReconIssue)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Seller)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.SPI)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Supplier)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsSupplierValid(x)).WithMessage(VM.InvalidSupplier);
            RuleFor(x => x.TransactionRelated)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
            RuleFor(x => x.Value)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);
        }
    }
}