using System;
using System.Web.WebPages;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleVendor;
using FluentValidation;
using VM = FilingPortal.Parts.Common.Domain.Validators.ValidationMessages;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Validators
{
    /// <summary>
    /// Validator for <see cref="RuleVendorEditModel"/>
    /// </summary>
    public class RuleVendorEditModelValidator : AbstractValidator<RuleVendorEditModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleVendorEditModelValidator"/> class
        /// </summary>
        /// <param name="clientRepository">The Client repository</param>
        /// <param name="unlocoRepository">The UNLOCO repository</param>
        public RuleVendorEditModelValidator(IClientRepository clientRepository
            , IUnlocoDictionaryRepository unlocoRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage(VM.IdIsRequired);

            RuleFor(x => x.VendorId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Vendor"))
                .Must(x => x.IsGuidFormat()).WithMessage(VM.IdFormatMismatch)
                .Must(x => Guid.TryParse(x, out Guid id) && clientRepository.Get(id) != null).WithMessage("Unknown Vendor");

            RuleFor(x => x.ImporterId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Importer"))
                .Must(x => x.IsGuidFormat()).WithMessage(VM.IdFormatMismatch)
                .Must(x => Guid.TryParse(x, out Guid id) && clientRepository.Get(id) != null).WithMessage("Unknown Importer");


            RuleFor(x => x.ExporterId)
                .NotEmpty().WithMessage(string.Format(VM.PropertyRequired, "Exporter"))
                .Must(x => x.IsGuidFormat()).WithMessage(VM.IdFormatMismatch)
                .Must(x => Guid.TryParse(x, out Guid id) && clientRepository.Get(id) != null).WithMessage("Unknown Exporter");

            RuleFor(x => x.ExportState)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));

            RuleFor(x => x.DirectShipPlace)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must(x => string.IsNullOrWhiteSpace(x) || unlocoRepository.GetByCode(x) != null).WithMessage("Unknown Direct Shipment Place");

            RuleFor(x => x.NoPackages)
                .Must(y => string.IsNullOrWhiteSpace(y) || y.IsInt()).WithMessage(VM.IntegerFormatMismatch);

            RuleFor(x => x.CountryOfOrigin)
                .MaximumLength(2).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 2));

            RuleFor(x => x.OrgState)
                .MaximumLength(128).WithMessage(string.Format(VM.ValueExceedsSpecifiedLength, 128))
                .Must((rule, orgState) =>
                    string.IsNullOrWhiteSpace(rule.CountryOfOrigin)
                    || !rule.CountryOfOrigin.Equals("US", StringComparison.InvariantCultureIgnoreCase)
                    || (rule.CountryOfOrigin.Equals("US", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(orgState)))
                .WithMessage(string.Format(VM.PropertyRequired, "Org State"));
        }
    }
}