using System.Linq;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Web.Models.Rail;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Web.Validators.Rail
{
    /// <summary>
    /// Rail inbound form validator
    /// </summary>
    public class RailInboundEditModelValidator : AbstractValidator<RailInboundEditModel>
    {
        private readonly IRailFilingHeadersRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundEditModelValidator"/> class
        /// </summary>
        public RailInboundEditModelValidator(IRailFilingHeadersRepository repository)
        {
            _repository = repository;
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id)
                .Must(x => !x.HasValue || IsEditable(x)).WithMessage("Only records in Open status can be edited");

            RuleFor(x => x.Consignee)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Consignee"))
                .MaximumLength(200).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 200));
            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Supplier"))
                .MaximumLength(200).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 200));
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Description"))
                .MaximumLength(500).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 500));
            RuleFor(x => x.EquipmentInitial)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Equipment Initial"))
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));
            RuleFor(x => x.EquipmentNumber)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Equipment Number"))
                .MaximumLength(6).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 6));
            RuleFor(x => x.IssuerCode)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Issuer Code"))
                .MaximumLength(5).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 5));
            RuleFor(x => x.BillOfLading)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Bill Of Lading"))
                .MaximumLength(20).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 20));
            RuleFor(x => x.PortOfUnlading)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Port Of Unlading"))
                .MaximumLength(4).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 4));
            RuleFor(x => x.ManifestUnits)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Manifest Units"))
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.Weight)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Weight"))
                .Must(x => x.IsDecimalFormat());
            RuleFor(x => x.WeightUnit)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Weight Unit"))
                .MaximumLength(3).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 3));
            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Reference Number"))
                .MaximumLength(50).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 50));
            RuleFor(x => x.Importer)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .MaximumLength(200).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 200));
            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Destination"))
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));

        }

        private bool IsEditable(int? id)
        {
            if (!id.HasValue) return true;

            var header = _repository.FindByInboundRecordIds(new[] { id.Value }).SingleOrDefault();
            if (header == null) return true;
            if (header.MappingStatus == MappingStatus.Open) return true;
            return false;
        }
    }
}