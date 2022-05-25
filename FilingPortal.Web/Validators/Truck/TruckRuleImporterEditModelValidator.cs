using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Models.Truck;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Truck
{
    /// <summary>
    /// Validator for <see cref="TruckRuleImporterEditModel"/>
    /// </summary>
    public class TruckRuleImporterEditModelValidator : AbstractValidator<TruckRuleImporterEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterEditModelValidator"/> class
        /// </summary>
        public TruckRuleImporterEditModelValidator(ILookupMasterRepository<LookupMaster> repository,IClientRepository clientRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.CWIOR)
                .NotEmpty().WithMessage(VM.ImporterIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsImporterValid(x)).WithMessage(VM.InvalidImporter);

            RuleFor(x => x.CWSupplier)
                .NotEmpty().WithMessage(VM.SupplierIsRequired)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsSupplierValid(x)).WithMessage(VM.InvalidSupplier);

            RuleFor(x => x.ConsigneeCode)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || clientRepository.IsExist(x)).WithMessage(VM.InvalidConsignee);

            RuleFor(x => x.ArrivalPort)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.CE)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Charges)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);
                
            RuleFor(x => x.CO)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomAttrib1)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomAttrib2)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.CustomQuantity)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.CustomUQ)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.DestinationState)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.EntryPort)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || repository.IsExist(x)).WithMessage(VM.InvalidPort);

            RuleFor(x => x.GoodsDescription)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.GrossWeight)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.GrossWeightUQ)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.InvoiceQTY)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.InvoiceUQ)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.LinePrice)
                .Must(x => string.IsNullOrEmpty(x) || x.IsDecimalFormat()).WithMessage(VM.DecimalFormatMismatch);

            RuleFor(x => x.ManufacturerMID)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.NAFTARecon)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ProductID)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.ReconIssue)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.SPI)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.SupplierMID)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.Tariff)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));

            RuleFor(x => x.TransactionsRelated)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128));
        }
    }
}